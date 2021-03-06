// GateSimSystemC.cpp : Defines the entry point for the application.
//


#define WIN32_LEAN_AND_MEAN

#include "stdafx.h"
#include "GateSimSystemC.h"

#pragma comment(lib, "Ws2_32.lib")

#define DEFAULT_BUFLEN 100000
#define LOCALHOST "127.0.0.1"

enum { SIMSTART, SIMSTOP, ADDCOMP, DELCOMP, CONNECT, DISCONNECT, INTERACT };
enum { CLK, TOGGLE, AND, OR, NOT};

using json = nlohmann::json;

std::map<int, Component*> compMap;
std::map<int, WireInfo*> wireInfoMap;


bool simRunning = false;


int RecvThread()
{
  WSADATA wsaData;
  int iResult;
  const char *port = "28999";

  SOCKET ListenSocket = INVALID_SOCKET;
  SOCKET ClientSocket = INVALID_SOCKET;

  struct addrinfo *result = NULL;
  struct addrinfo hints;

  char recvbuf[DEFAULT_BUFLEN];
  int recvbuflen = DEFAULT_BUFLEN;

  // Initialize Winsock
  iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
  if (iResult != 0) {
    printf("WSAStartup failed with error: %d\n", iResult);
    return 1;
  }

  ZeroMemory(&hints, sizeof(hints));
  hints.ai_family = AF_INET;
  hints.ai_socktype = SOCK_STREAM;
  hints.ai_protocol = IPPROTO_TCP;
  hints.ai_flags = AI_PASSIVE;

  // Resolve the server address and port
  iResult = getaddrinfo(NULL, port, &hints, &result);
  if (iResult != 0) {
    printf("getaddrinfo failed with error: %d\n", iResult);
    WSACleanup();
    return 1;
  }

  // Create a SOCKET for connecting to server
  ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
  if (ListenSocket == INVALID_SOCKET) {
    printf("socket failed with error: %ld\n", WSAGetLastError());
    freeaddrinfo(result);
    WSACleanup();
    return 1;
  }

  // Setup the TCP listening socket
  iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
  if (iResult == SOCKET_ERROR) {
    printf("bind failed with error: %d\n", WSAGetLastError());
    freeaddrinfo(result);
    closesocket(ListenSocket);
    WSACleanup();
    return 1;
  }

  freeaddrinfo(result);

  iResult = listen(ListenSocket, SOMAXCONN);
  if (iResult == SOCKET_ERROR) {
    printf("listen failed with error: %d\n", WSAGetLastError());
    closesocket(ListenSocket);
    WSACleanup();
    return 1;
  }

  // Accept a client socket
  ClientSocket = accept(ListenSocket, NULL, NULL);
  if (ClientSocket == INVALID_SOCKET) {
    printf("accept failed with error: %d\n", WSAGetLastError());
    closesocket(ListenSocket);
    WSACleanup();
    return 1;
  }

  // No longer need server socket
  closesocket(ListenSocket);

  // Receive until the peer shuts down the connection
  do {

    iResult = recv(ClientSocket, recvbuf, recvbuflen, 0);
    if (iResult > 0) {
      std::string temp = recvbuf;
      recvbuf[iResult] = '\0';
      json decodedStr = json::parse(recvbuf);

      //cout << decodedStr.dump(4) << "\n";

      userInputEvents.push(decodedStr);
    }
    else if (iResult == 0)
      printf("Connection closing...\n");
    else {
      printf("recv failed with error: %d\n", WSAGetLastError());
      closesocket(ClientSocket);
      WSACleanup();
      return 1;
    }

  } while (iResult > 0);

  // shutdown the connection since we're done
  iResult = shutdown(ClientSocket, SD_SEND);
  if (iResult == SOCKET_ERROR) {
    printf("shutdown failed with error: %d\n", WSAGetLastError());
    closesocket(ClientSocket);
    WSACleanup();
    return 1;
  }

  // cleanup
  closesocket(ClientSocket);
  WSACleanup();

  return 0;
}

int SendThread()
{
  WSADATA wsaData;
  SOCKET ConnectSocket = INVALID_SOCKET;
  struct addrinfo *result = NULL, *ptr = NULL, hints;
  char recvbuf[DEFAULT_BUFLEN];
  int iResult;
  int recvbuflen = DEFAULT_BUFLEN;
  const char *port = "27999";

  //Sleep(2000);

  // Initialize Winsock
  iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
  if (iResult != 0) {
    printf("WSAStartup failed with error: %d\n", iResult);
    return 1;
  }

  ZeroMemory(&hints, sizeof(hints));
  hints.ai_family = AF_UNSPEC;
  hints.ai_socktype = SOCK_STREAM;
  hints.ai_protocol = IPPROTO_TCP;

  // Resolve the server address and port
  iResult = getaddrinfo(LOCALHOST, port, &hints, &result);
  if (iResult != 0) {
    printf("getaddrinfo failed with error: %d\n", iResult);
    WSACleanup();
    return 1;
  }

  // Attempt to connect to an address until one succeeds
  for (ptr = result; ptr != NULL; ptr = ptr->ai_next) {

    // Create a SOCKET for connecting to server
    ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype, ptr->ai_protocol);
    if (ConnectSocket == INVALID_SOCKET) {
      printf("socket failed with error: %ld\n", WSAGetLastError());
      WSACleanup();
      return 1;
    }

    // Connect to server.
    iResult = connect(ConnectSocket, ptr->ai_addr, (int)ptr->ai_addrlen);
    if (iResult == SOCKET_ERROR) {
      closesocket(ConnectSocket);
      ConnectSocket = INVALID_SOCKET;
      continue;
    }
    break;
  }

  freeaddrinfo(result);

  if (ConnectSocket == INVALID_SOCKET) {
    printf("Unable to connect to server!\n");
    WSACleanup();
    return 1;
  }

  // Send an initial buffer
  /*
  iResult = send(ConnectSocket, sendbuf, (int)strlen(sendbuf), 0);
  if (iResult == SOCKET_ERROR) {
    printf("send failed with error: %d\n", WSAGetLastError());
    closesocket(ConnectSocket);
    WSACleanup();
    return 1;
  }
  */

  printf("Bytes Sent: %ld\n", iResult);

  /*
  // shutdown the connection since no more data will be sent
  iResult = shutdown(ConnectSocket, SD_SEND);
  if (iResult == SOCKET_ERROR) {
    printf("shutdown failed with error: %d\n", WSAGetLastError());
    closesocket(ConnectSocket);
    WSACleanup();
    return 1;
  }
  */

  // Receive until the peer closes the connection
  do {
    if (resultQueue.size() > 0)
    {
      std::string encoded;
      resultMsg msg = resultQueue.front();
      resultQueue.pop();

      json test = { { "compId",  msg.compId },
                    { "time", msg.time },
                    { "input", msg.input },
                    { "output", msg.output } };

      encoded = test.dump();

      cout << encoded << "\n";

      iResult = send(ConnectSocket, encoded.c_str(), encoded.length(), 0);
      printf("Bytes Sent: %ld\n", iResult);
    }
    else
      iResult = 1;

    Sleep(10);
  } while (iResult > 0);

  // cleanup
  closesocket(ConnectSocket);
  WSACleanup();

  return 0;
}

int Simulation()
{
  return 0;
}

int sc_main(int argc, char* argv[])
{
  sc_report_handler::set_log_file_name("log.txt");

  std::thread recvThread(RecvThread);
  std::thread sendThread(SendThread);

  while (1)
  {
    while (userInputEvents.size() > 0)
    {
      json eventDict = userInputEvents.front();
      userInputEvents.pop();
      int eventType = eventDict["eventType"];

      if (eventType == SIMSTART)
      {
        cout << "SIMSTART event received\n";

        for (auto const& x : wireInfoMap)
        {
          std::cout << x.first  // string (key)
            << ':'
            << x.second // string's value 
            << std::endl;
        }

        /* Connect all wires. */
        for (std::pair<const int, WireInfo *> kvPair : wireInfoMap)
        {
          WireInfo *wireInfo = kvPair.second;
          Component *from = compMap[wireInfo->fromCompId];
          Component *to = compMap[wireInfo->toCompId];
          from->connPort(to, wireInfo->outPortId, wireInfo->inPortId);
        }

        /* Finalize all components. */
        for (std::pair<const int, Component *> kvPair : compMap)
        {
          Component *comp = kvPair.second;
          comp->finalize();
        }

        sc_start(0, SC_NS);
        simRunning = true;
      }
      else if(eventType == SIMSTOP)
      {
        cout << "SIMSTOP event received\n";
        sc_stop();
      }
      else if (eventType == ADDCOMP)
      {
        int compId = eventDict["compId"];
        int compType = eventDict["compType"];
        int depth = eventDict["compDepth"];
        std::pair<int, Component*> kvPair;

        std::string idString = std::to_string(compId);
        const char *idStr = idString.c_str();

        cout << "ADDCOMP event received\n";

        Component *newComp;

        if (compType == AND)
          newComp = new AndComp(compId, depth);
        else if (compType == OR)
          newComp = new OrComp(compId, depth);
        else if (compType == NOT)
          newComp = new NotComp(compId, depth);
        else if (compType == CLK)
          newComp = new ClkComp(compId, depth, 1);
        else
          break;

        kvPair = std::make_pair(compId, (Component*)(newComp));
        compMap.insert(kvPair);
      }
      else if (eventType == DELCOMP)
      {
        cout << "DELCOMP event received\n";
        int compId = eventDict["compId"];
        int compType = eventDict["compType"];
        int depth = eventDict["compDepth"];
        compMap.erase(compId);
      }
      else if (eventType == CONNECT)
      {
        cout << "CONNECT event received\n";
        std::pair<int, WireInfo*> kvPair;
        int wireId = eventDict["wireId"];
        int fromCompId = eventDict["fromCompId"];
        int toCompId = eventDict["toCompId"];
        int outputId = eventDict["outputId"];
        int inputId = eventDict["inputId"];

        WireInfo *newWireInfo = new WireInfo(wireId, fromCompId, toCompId, outputId, inputId);

        kvPair = std::make_pair(wireId, (WireInfo*)(newWireInfo));

        wireInfoMap.insert(kvPair);

      }
      else if (eventType == DISCONNECT)
      {
        cout << "DISCONNECT event received\n";
      }
    }

    if (simRunning)
    {
      for (std::pair<const int, Component *> kvPair : compMap)
      {
        resultMsg msg;
        Component *comp = kvPair.second;

        msg.compId = comp->id;
        msg.time = std::atoi(sc_time_stamp().to_string().c_str());

        for (unsigned int i = 0; i < comp->inPorts->size(); i++)
          if (comp->readInPort(i))
            msg.input += "1";
          else
            msg.input += "0";

        for (unsigned int i = 0; i < comp->outPorts->size(); i++)
          if (comp->readOutPort(i))
            msg.output += "1";
          else
            msg.output += "0";

        resultQueue.push(msg);

        cout << "Comp id: " << comp->name << ", in values: (";

        for (unsigned int i = 0; i < comp->inPorts->size(); i++)
          cout << comp->readInPort(i);

        cout << "), out values: (";

        for (unsigned int i = 0; i < comp->outPorts->size(); i++)
          cout << comp->readOutPort(i);

        cout << ")\n";
      }
    }

    cout << sc_time_stamp() << "\n";
    Sleep(1000);

    /*
    for (auto const& x : compMap)
    {
      std::cout << x.first  // string (key)
        << ':'
        << x.second // string's value 
        << std::endl;
    }
    */

    if (simRunning)
    {
      cout << "Result Queue size: " << resultQueue.size() << "\n";
      sc_start(1, SC_NS);
    }
  }

  return 0;
}