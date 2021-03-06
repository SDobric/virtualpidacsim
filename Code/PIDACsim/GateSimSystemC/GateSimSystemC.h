#pragma once

#include "resource.h"
#include <systemc.h>
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <io.h>
#include <stdio.h> 
#include <stdlib.h> 
#include <string.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <thread>
#include <nlohmann/json.hpp>
#include <queue>
#include <iostream>
#include <iomanip>
#include <sstream>
#include <iterator>
#include <map>
#include <vector>

struct resultMsg {
  int compId;
  int time;
  std::string input = "";
  std::string output = "";
};

using json = nlohmann::json;
std::queue<json> userInputEvents;
std::queue<resultMsg> resultQueue;

SC_MODULE(clk)
{
  sc_clock clockSignal{ "TestClock", 2, SC_NS };
  sc_in_clk clockIn;

  sc_vector<sc_in<bool>> inPorts{ "inPorts", 0 };
  sc_vector<sc_out<bool>> outPorts{ "outPorts", 1 };
  sc_vector<sc_signal<bool>> inSignals{ "inSignals", 0 };
  sc_vector<sc_signal<bool>> outSignals{ "outSignals", 1 };

  SC_CTOR(clk)
  {
    clockIn(clockSignal);
  }
};

SC_MODULE(andGate)
{
  int id;
  sc_vector<sc_in<bool>> inPorts{ "inPorts", 2 };
  sc_vector<sc_out<bool>> outPorts{ "outPorts", 1 };
  sc_vector<sc_signal<bool>> inSignals{ "inSignals", 2 };
  sc_vector<sc_signal<bool>> outSignals{ "outSignals", 1 };

  void body()
  {
    outPorts[0].write(inPorts[0].read() && inPorts[1].read());

    /*
    resultMsg result;
    result.compId = id;
    result.time = (int)(sc_time_stamp().to_double());
    result.in = inPorts[0].read() + inPorts[1].read();
    result.out = outPorts[0].read();
    resultQueue.push(result);
    */
  }

  SC_CTOR(andGate)
  {
    SC_HAS_PROCESS(andGate);
    SC_METHOD(body);
    sensitive << inPorts[0] << inPorts[1];
  }
};

SC_MODULE(orGate)
{
  sc_vector<sc_in<bool>> inPorts{ "inPorts", 2 };
  sc_vector<sc_out<bool>> outPorts{ "outPorts", 1 };
  sc_vector<sc_signal<bool>> inSignals{ "inSignals", 2 };
  sc_vector<sc_signal<bool>> outSignals{ "outSignals", 1 };

  void body()
  {
    outPorts[0].write(inPorts[0].read() || inPorts[1].read());
  }

  SC_CTOR(orGate)
  {
    SC_HAS_PROCESS(orGate);
    SC_METHOD(body);
    
    sensitive << inPorts[0] << inPorts[1];
  }
};

SC_MODULE(notGate)
{
  sc_vector<sc_in<bool>> inPorts{ "inPorts", 1 };
  sc_vector<sc_out<bool>> outPorts{ "outPorts", 1 };
  sc_vector<sc_signal<bool>> inSignals{ "inSignals", 1 };
  sc_vector<sc_signal<bool>> outSignals{ "outSignals", 1 };

  void body()
  {
    outPorts[0].write(!inPorts[0].read());
  }

  SC_CTOR(notGate)
  {
    SC_HAS_PROCESS(notGate);
    SC_METHOD(body);

    sensitive << inPorts[0];
  }
};

class WireInfo
{
public:
  int id;
  int fromCompId;
  int toCompId;
  int outPortId;
  int inPortId;

  WireInfo(int wireId, int fromCompId, int toCompId, int outputId, int inputId)
  {
    this->id = wireId;
    this->fromCompId = fromCompId;
    this->toCompId = toCompId;
    this->outPortId = outputId;
    this->inPortId = inputId;
  }
};

class Component
{
public:
  int id;
  int depth;
  int compType;
  std::string name;
  sc_vector<sc_in<bool>> *inPorts;
  sc_vector<sc_out<bool>> *outPorts;
  sc_vector<sc_signal<bool>> *inSignals;
  sc_vector<sc_signal<bool>> *outSignals;

  Component()
  {
  }

  Component(int id, int depth)
  {
    this->id = id;
    this->depth = depth;
  }

  /* Connects a component from out port with a certain id to another components in port with id */
  void virtual connPort(Component *toComp, int outPortId, int inPortId)
  {
    sc_in<bool> *inPort = &(toComp->inPorts->at(inPortId));
    sc_out<bool> *outPort = &(outPorts->at(outPortId));
    sc_signal<bool> *outSignal = &(outSignals->at(outPortId));

    //outPort->bind(*outSignal);
    inPort->bind(*outSignal);
  }

  void virtual finalize()
  {
    for (unsigned int i = 0; i < inPorts->size(); i++)
    {
      sc_in<bool> *inPort = &(inPorts->at(i));
      if (inPort->bind_count() == 0)
      {
        inPort->bind(inSignals->at(i));
      }
    }
    for (unsigned int i = 0; i < outPorts->size(); i++)
    {
      sc_out<bool> *outPort = &(outPorts->at(i));
      if (outPort->bind_count() == 0)
      {
        outPort->bind(outSignals->at(i));
      }
    }
  }

  bool virtual readInPort(int portId)
  {
    return (*inPorts)[portId].read();
  }

  bool virtual readOutPort(int portId)
  {
    return (*outPorts)[portId].read();
  }
};

class ClkComp : public Component
{
public:
  clk *clock;

  ClkComp() : Component()
  {

  }

  ClkComp(int id, int depth, int period) : Component(id, depth)
  {
    name = std::to_string(id) + "_clk";
    const char *idStr = name.c_str();

    clock = new clk(idStr);

    inPorts = &(clock->inPorts);
    outPorts = &(clock->outPorts);
    inSignals = &(clock->inSignals);
    outSignals = &(clock->outSignals);
  }

  bool virtual readInPort(int portId)
  {
    return false;
    //return clock->clockIn.read();
  }

  bool readOutPort(int portId)
  {
    return clock->clockIn.read();
  }

  
  void connPort(Component *toComp, int outPortId, int inPortId)
  {
    sc_in<bool> *inPort = &(toComp->inPorts->at(inPortId));

    inPort->bind(clock->clockSignal);
  }
};

class AndComp : public Component
{
public:
  andGate *gate;

  AndComp() : Component()
  {
  }

  AndComp(int id, int depth) : Component(id, depth)
  {
    name = std::to_string(id) + "_and";
    const char *idStr = name.c_str();

    gate = new andGate(idStr);
    gate->id = id;
    inPorts = &(gate->inPorts);
    outPorts = &(gate->outPorts);
    inSignals = &(gate->inSignals);
    outSignals = &(gate->outSignals);
  }
};

class OrComp : public Component
{
public:
  orGate *gate;

  OrComp() : Component()
  {
  }

  OrComp(int id, int depth) : Component(id, depth)
  {
    name = std::to_string(id) + "_or_";
    const char *idStr = name.c_str();

    gate = new orGate(idStr);
    inPorts = &(gate->inPorts);
    outPorts = &(gate->outPorts);
    inSignals = &(gate->inSignals);
    outSignals = &(gate->outSignals);
  }
};

class NotComp : public Component
{
public:
  notGate *gate;

  NotComp() : Component()
  {
  }

  NotComp(int id, int depth) : Component(id, depth)
  {
    name = std::to_string(id) + "_not";
    const char *idStr = name.c_str();

    gate = new notGate(idStr);
    inPorts = &(gate->inPorts);
    outPorts = &(gate->outPorts);
    inSignals = &(gate->inSignals);
    outSignals = &(gate->outSignals);
  }
};

class HierarchicalComp : public Component
{
public:
  std::map<int, Component*> comps;
  std::map<int, Component*> wires;

  HierarchicalComp() : Component()
  {
  }

  HierarchicalComp(int id, int depth) : Component(id, depth)
  {
  }

  void addComp(int compId, Component* comp)
  {
    std::pair<int, Component*> kvPair = std::make_pair(compId, (comp));
    comps.insert(kvPair);
  }

  void delComp(int compId)
  {
    comps.erase(compId);
  }
};

/*
SC_MODULE(nand)          // declare nand2 sc_module
{
  sc_in<bool> A, B;       // input signal ports
  sc_out<bool> F;         // output signal ports

  void do_nand()         // a C++ function
  {
    F.write(!(A.read() && B.read()));
  }

  SC_CTOR(nand)          // constructor for nand2
  {
    SC_METHOD(do_nand);  // register do_nand2 with kernel
    sensitive << A << B;  // sensitivity list
  }
};

SC_MODULE(exor)
{
  sc_in<bool> A, B;
  sc_out<bool> F;

  nand n1, n2, n3, n4;

  sc_signal<bool> S1, S2, S3;

  SC_CTOR(exor) : n1("N1"), n2("N2"), n3("N3"), n4("N4")
  {
    n1.A(A);
    n1.B(B);
    n1.F(S1);

    n2.A(A);
    n2.B(S1);
    n2.F(S2);

    n3.A(S1);
    n3.B(B);
    n3.F(S3);

    n4.A(S2);
    n4.B(S3);
    n4.F(F);
  }
};

*/
