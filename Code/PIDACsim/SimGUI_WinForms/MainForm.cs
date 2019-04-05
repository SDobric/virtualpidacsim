using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Graph;
using Graph.Compatibility;
using Graph.Items;
//using GateSim;
using System.Net.Sockets;
using System.Threading;

namespace SimGUI
{
  public partial class MainForm : Form
  {
    private delegate void RefreshDelegate();

    void RefreshControl()
    {
      if (this.InvokeRequired)
        this.Invoke(new RefreshDelegate(this.RefreshControl));
      else
        graphControl.Refresh();
    }

    public MainForm()
    {
      InitializeComponent();

      graphControl.CompatibilityStrategy = new AlwaysCompatible();

      Thread sendThread = new Thread(SendThread);
      Thread GUIManipThread = new Thread(GUIManipulationThread);

      sendThread.Start();
      GUIManipThread.Start();

      graphControl.NodeAdded += new EventHandler<AcceptNodeEventArgs>(NodeAdded);
      graphControl.NodeRemoved += new EventHandler<NodeEventArgs>(NodeRemoved);
      graphControl.ConnectionAdded	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdded);
      graphControl.ConnectionAdding	+= new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionAdding);
      graphControl.ConnectionRemoving += new EventHandler<AcceptNodeConnectionEventArgs>(OnConnectionRemoved);
      graphControl.ShowElementMenu	+= new EventHandler<AcceptElementLocationEventArgs>(OnShowElementMenu);
    }

    void SendThread(object obj)
    {
      Socket sock = GetSocket.ConnectSocket("127.0.0.1", 28999);

      while (true)
      {
        UserInputEvent userEvent;
        while (Globals.userInputEventQueue.TryDequeue(out userEvent))
        {
          sock.Send(Json.jsonEncode(userEvent.jsonValues));
        }
        Thread.Sleep(10);
      }
    }

    void GUIManipulationThread(object obj)
    {
      byte[] bytes;
      string bytesStr;

      Socket sock = GetSocket.ListenSocket(27999);

      while (true)
      {
        bytes = new byte[1024];
        sock.Receive(bytes);

        bytesStr = Encoding.UTF8.GetString(bytes);

        List<ResultMsg> msgs = Json.jsonDecode(bytesStr);

        foreach(ResultMsg msg in msgs)
        {
          Component comp = Globals.components[msg.compId];

          Node node = comp.uiNode;
          List<NodeItem> labelItems = new List<NodeItem>();

          foreach (NodeItem nodeItem in node.Items)
            if (nodeItem is NodeLabelItem)
              labelItems.Add(nodeItem);

          foreach (NodeLabelItem nodeLabelItem in labelItems)
          {
            Tag tag = (Tag)(nodeLabelItem.Tag);

            if (tag.type == PortType.Input)
            {
              nodeLabelItem.Text = msg.input[tag.id].ToString();
            }
            else if (tag.type == PortType.Output)
            {
              nodeLabelItem.Text = msg.output[tag.id].ToString();
            }
          }
        }
        RefreshControl();
      }
    }

    void NodeAdded(object sender, AcceptNodeEventArgs args)
    {
      Node node = args.Node;
      Component comp = (Component)node.Comp;
      comp.setId(Globals.lastCompId++);

      UserInputEvent newEvent = new UserInputEvent(UserInputEvent.EType.ADDCOMP, comp);

      Globals.userInputEventQueue.Enqueue(newEvent);
      Globals.components.TryAdd(comp.getId(), comp);

      /*
      Console.WriteLine("Num components in comp dict " + Globals.components.Count);
      Console.WriteLine("Num user input events in queue " + Globals.userInputEventQueue.Count);
      */
      //Console.WriteLine(Json.jsonEncode(newEvent.jsonValues));
    }

    void NodeRemoved(object sender, NodeEventArgs args)
    {
      Node node = args.Node;
      Component comp = (Component)node.Comp;

      UserInputEvent newEvent = new UserInputEvent(UserInputEvent.EType.DELCOMP, comp);

      Globals.userInputEventQueue.Enqueue(newEvent);
      Globals.components.TryRemove(comp.getId(), out comp);

      //Console.WriteLine(Json.jsonEncode(newEvent.jsonValues));
    }

    void ToggleClicked(object sender, NodeItemEventArgs args)
    {
      NodeImageItem imgItem = (NodeImageItem)sender;

      if ((int)imgItem.Image.Tag == 0)
      {
        imgItem.Image = Properties.Resources.TGL2;
        imgItem.Image.Tag = 1;
      }
      else
      {
        imgItem.Image = Properties.Resources.TGL1;
        imgItem.Image.Tag = 0;
      }
    }

    void ClkClicked(object sender, NodeItemEventArgs args)
    {
      NodeImageItem imgItem = (NodeImageItem)sender;

      if ((int)imgItem.Image.Tag == 0)
      {
        imgItem.Image = Properties.Resources.CLK;
        imgItem.Image.Tag = 1;
      }
      else
      {
        imgItem.Image = Properties.Resources.CLK;
        imgItem.Image.Tag = 0;
      }
    }

    void CreateNewAndNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new LogicalComp(CompType.AND), CompType.AND);
    }

    void CreateNewOrNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new LogicalComp(CompType.OR), CompType.OR);
    }

    void CreateNewNotNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new LogicalComp(CompType.NOT), CompType.NOT);
    }

    void CreateNewXorNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new LogicalComp(CompType.XOR), CompType.XOR);
    }

    void CreateNewClkNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new InteractionComp(CompType.Clk), CompType.Clk);
    }

    void CreateNewTglNode(object sender, MouseEventArgs e)
    {
      CreateNewNode(new InteractionComp(CompType.Toggle), CompType.Toggle);
    }

    void CreateNewNode(Component comp, CompType type)
    {
      NodeLabelItem labelItem;
      Image imgBitmap = null;
      EventHandler<NodeItemEventArgs> imgClickedDelegate = null;

      if (comp is LogicalComp)
      {
        LogicalComp primComp = (LogicalComp)comp;
        switch (primComp.type)
        {
          case CompType.AND:
            imgBitmap = Properties.Resources.AND; break;
          case CompType.OR:
            imgBitmap = Properties.Resources.OR; break;
          case CompType.NOT:
            imgBitmap = Properties.Resources.NOT; break;
          case CompType.XOR:
            imgBitmap = Properties.Resources.XOR; break;
        }
      }
      else if (comp is InteractionComp)
      {
        InteractionComp inputComp = (InteractionComp)comp;
        switch (inputComp.type)
        {
          case CompType.Toggle:
            imgBitmap = Properties.Resources.TGL1;
            imgBitmap.Tag = 0;
            imgClickedDelegate = ToggleClicked; break;
          case CompType.Clk:
            imgBitmap = Properties.Resources.CLK;
            imgBitmap.Tag = 0;
            imgClickedDelegate = ClkClicked; break;
        }
      }
      else
      {
        //TODO: throw exception
      }

      Node node = new Node(Globals.lastCompId.ToString());

      NodeImageItem imgItem = new NodeImageItem(imgBitmap, 50, 25, false, false);

      imgItem.Clicked += imgClickedDelegate;

      node.AddItem(imgItem);


      PortType portType = PortType.Input;

      for (int i = 0; i < comp.inputsLen; i++)
      {
        labelItem = new NodeLabelItem("Input " + i, true, false);
        Tag tag = new Tag(portType, i);
        labelItem.Tag = tag;
        node.AddItem(labelItem);
      }

      portType = PortType.Output;

      for (int i = 0; i < comp.outputsLen; i++)
      {
        labelItem = new NodeLabelItem("Output " + i, false, true);
        Tag tag = new Tag(portType, i);
        labelItem.Tag = tag;
        node.AddItem(labelItem);
      }

      //node.AddItem(new NodeLabelItem("Entry 1", true, false));
      //node.AddItem(new NodeLabelItem("Entry 2", true, false));
      //node.AddItem(new NodeLabelItem("Entry 3", false, true));
      //node.AddItem(new NodeTextBoxItem("TEXTTEXT", false, true));
      //node.AddItem(new NodeDropDownItem(new string[] { "1", "2", "3", "4" }, 0, false, false));

      node.Comp = comp;

      comp.uiNode = node;

      this.DoDragDrop(node, DragDropEffects.Copy);
    }

    void OnShowElementMenu(object sender, AcceptElementLocationEventArgs e)
    {
      /*
      if (e.Element == null)
      {
        // Show a test menu for when you click on nothing
        testMenuItem.Text = "(clicked on nothing)";
        nodeMenu.Show(e.Position);
        e.Cancel = false;
      } else
      if (e.Element is Node)
      {
        // Show a test menu for a node
        testMenuItem.Text = ((Node)e.Element).Title;
        nodeMenu.Show(e.Position);
        e.Cancel = false;
      } else
      if (e.Element is NodeItem)
      {
        // Show a test menu for a nodeItem
        testMenuItem.Text = e.Element.GetType().Name;
        nodeMenu.Show(e.Position);
        e.Cancel = false;
      } else
      {
        // if you don't want to show a menu for this item (but perhaps show a menu for something more higher up) 
        // then you can cancel the event
        e.Cancel = true;
      }
      */
    }

    void OnConnectionAdding(object sender, AcceptNodeConnectionEventArgs e)
    {
      //e.Cancel = true;
    }

    void OnConnectionAdded(object sender, AcceptNodeConnectionEventArgs e)
    {
      //Wire wire = new Wire();
      char[] seperator = " ".ToCharArray();
      Component outComp = (Component)e.Connection.From.Node.Comp;
      Component inComp = (Component)e.Connection.To.Node.Comp;
      string fromLabel = ((NodeLabelItem)(e.Connection.From.Item)).Text;
      string toLabel = ((NodeLabelItem)(e.Connection.To.Item)).Text;
      int connOutId = Convert.ToInt32(fromLabel.Split(seperator)[1]);
      int connInId = Convert.ToInt32(toLabel.Split(seperator)[1]);
      int wireId = Globals.lastConnId;

      Connector connOut = outComp.outputs[connOutId];
      Connector connIn = inComp.inputs[connInId];

      Wire wire = new Wire(wireId, connIn, connOut);

      Globals.wires.TryAdd(wireId, wire);

      UserInputEvent newEvent = new UserInputEvent(UserInputEvent.EType.CONNECT, wire);

      Globals.userInputEventQueue.Enqueue(newEvent);

      Console.WriteLine("Connection added");
      //e.Cancel = true;
      Globals.lastConnId++;
      //e.Connection.Name = "Connection " + counter ++;
      e.Connection.DoubleClick += new EventHandler<NodeConnectionEventArgs>(OnConnectionDoubleClick);

      //lastConnId++;
    }

    void OnConnectionRemoved(object sender, AcceptNodeConnectionEventArgs e)
    {
      //e.Cancel = true;
    }

    void OnConnectionDoubleClick(object sender, NodeConnectionEventArgs e)
    {
      //e.Connection.Name = "Connection " + counter++;
    }

    /*
    private void SomeNode_MouseDown(object sender, MouseEventArgs e)
    {
      var node = new Node("Some node");
      node.AddItem(new NodeLabelItem("Entry 1", true, false));
      node.AddItem(new NodeLabelItem("Entry 2", true, false));
      node.AddItem(new NodeLabelItem("Entry 3", false, true));
      node.AddItem(new NodeTextBoxItem("TEXTTEXT", false, true));
      node.AddItem(new NodeDropDownItem(new string[] { "1", "2", "3", "4" }, 0, false, false));
      this.DoDragDrop(node, DragDropEffects.Copy);
    }

    private void ColorNode_MouseDown(object sender, MouseEventArgs e)
    {
      var colorNode = new Node("Color");
      colorNode.Location = new Point(200, 50);
      var redChannel = new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
      var greenChannel = new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
      var blueChannel = new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f, false, false);
      var colorItem = new NodeColorItem("Color", Color.Black, false, true);

      EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object s, NodeItemEventArgs args)
      {
        var red = redChannel.Value;
        var green = blueChannel.Value;
        var blue = greenChannel.Value;
        colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
      };
      redChannel.ValueChanged += channelChangedDelegate;
      greenChannel.ValueChanged += channelChangedDelegate;
      blueChannel.ValueChanged += channelChangedDelegate;


      colorNode.AddItem(redChannel);
      colorNode.AddItem(greenChannel);
      colorNode.AddItem(blueChannel);

      colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
      colorNode.AddItem(colorItem);

      this.DoDragDrop(colorNode, DragDropEffects.Copy);
    }
    */

    private void OnShowLabelsChanged(object sender, EventArgs e)
    {
      graphControl.ShowLabels = showLabelsCheckBox.Checked;
    }

    private void compToolstrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {

    }

    private void compToolStripLabel_Click(object sender, EventArgs e)
    {

    }

    private void StopSimulation(object sender, MouseEventArgs e)
    {
      if (Globals.simRunning)
        Globals.simStopRequested = true;

      UserInputEvent stopEvent = new UserInputEvent(UserInputEvent.EType.SIMSTOP);
      Globals.userInputEventQueue.Enqueue(stopEvent);
    }

    private void StartSimulation(object sender, MouseEventArgs e)
    {
      if (!Globals.simRunning)
        Globals.simStartRequested = true;

      UserInputEvent startEvent = new UserInputEvent(UserInputEvent.EType.SIMSTART);
      Globals.userInputEventQueue.Enqueue(startEvent);
    }
  }
}
