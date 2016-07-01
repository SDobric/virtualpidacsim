using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSim
{
  public delegate bool EvalFunc();

  public enum ConnType { Input, Output };
  //public enum CompType { Clk, Toggle, AND, OR, NOT, XOR, Led, Number };
  public enum PrimCompType { AND, OR, NOT, XOR };
  public enum InputGenCompType { Clk, Toggle };
  //public enum InCompType { Clock, Switch, Toggle };
  //public enum OutCompType { Led, Number };

  public abstract class Component
  {
    protected int id;
    public int insertedAtT;
    public Component parent;
    public List<Connector> inputs;
    public List<Connector> outputs;

    // TODO: Create all logic for "inserted at t"
    public Component()
    {
      insertedAtT = 0;
      inputs = new List<Connector>();
      outputs = new List<Connector>();
    }

    public Component(Component parent) : this()
    {
      this.parent = parent;
    }

    /*
    public void connect()
    {

    }
    */
    public string getIdStr()
    {
      string resultStr = id.ToString();
      Component tempParent = this;

      while (tempParent != null)
      {
        resultStr = tempParent.getId().ToString() + ", " + resultStr;
        tempParent = tempParent.parent;
      }

      return resultStr;
    }

    public int getId()
    {
      return id;
    }

    public void setId(int id)
    {
      this.id = id;
    }

    // Add "connect to method"

    //public abstract void addConn(Connector conn, ConnType type, int id);
    //public abstract void delConn(Connector conn);


    public int inputsLen { get { return inputs.Count; } }
    public int outputsLen { get { return outputs.Count; } }
  }

  public class SimpleComp : Component
  {
    public int propDelay;

    public SimpleComp(int propDelay) : base()
    {
      this.propDelay = propDelay;
    }
  }

  public class PrimitiveComp : SimpleComp
  {
    protected EvalFunc evalFunc;
    public PrimCompType type;

    public PrimitiveComp(PrimCompType type, int propDelay) : base(propDelay)
    {
      int numInputs = 2, numOutputs = 1;
      this.propDelay = propDelay;
      this.type = type;

      switch (type)
      {
        /*
        case CompType.Clk:
          evalFunc = clkFunc;
          numInputs = 0;
          numOutputs = 1; break;
        case PrimCompType.Toggle:
          evalFunc = tglFunc;
          numInputs = 1;
          numOutputs = 1; break;
        */
        case PrimCompType.AND:
          evalFunc = andFunc; break;
        case PrimCompType.OR:
          evalFunc = orFunc; break;
        case PrimCompType.NOT:
          evalFunc = notFunc;
          numInputs = 1; break;
        case PrimCompType.XOR:
          evalFunc = xorFunc; break;
      }

      for (int i = 0; i < numInputs; i++)
        inputs.Add(new Connector(this));

      for (int i = 0; i < numOutputs; i++)
        outputs.Add(new Connector(this));

      outputs[0].changeVal(0, evaluate());
    }

    public bool evaluate()
    {
      return evalFunc();
    }

    /*
    bool clkFunc()
    {
      return !inputs[0].currVal;
    }
    */

    bool tglFunc()
    {
      return inputs[0].currVal;
    }

    bool orFunc()
    {
      return inputs[0].currVal || inputs[1].currVal;
    }

    bool andFunc()
    {
      return inputs[0].currVal && inputs[1].currVal;
    }

    bool notFunc()
    {
      return !inputs[0].currVal;
    }

    bool xorFunc()
    {

      return inputs[0].currVal ^ inputs[1].currVal;
    }

    public override string ToString()
    {
      string inputsStr = "";
      foreach (Connector input in inputs)
        inputsStr += input.currVal + " ";

      string outputsStr = "";
      foreach (Connector output in outputs)
        outputsStr += output.currVal + " ";

      return "Component id: " + id + ", input values: " + inputsStr + ", output values:" + outputsStr;
    }
  }

  public class CompiledComp : SimpleComp
  {
    String logicStr;

    public CompiledComp(int propDelay) : base(propDelay)
    {

    }
  }

  public class ClockComp : SimpleComp
  {
    public int period;

    public ClockComp(int period, int propDelay) : base(propDelay)
    {
      this.period = period;
      outputs.Add(new Connector(this));
    }
  }



  /*
  public class BoolComp : PrimitiveComp
  {

  }
  */

  public class HierarchicalComp : Component
  {
    ComponentDict comps;

    public HierarchicalComp() : base()
    {
      comps = new ComponentDict();
    }

    void addSubComp(Component comp)
    {
      comps.add(comp);
    }

    void removeComp()
    {

    }
  }

  public class Connector
  {
    public bool currVal;
    public LinkedList<Value> prevVals;
    public Component belongsTo;
    public List<Wire> connections;

    public Connector(Component belongsTo)
    {
      prevVals = new LinkedList<Value>();
      connections = new List<Wire>();
      this.belongsTo = belongsTo;
    }

    public void changeVal(int t, bool value)
    {
      prevVals.AddLast(new Value(t, value));
      currVal = value;
    }

    public void connectTo(Connector conn)
    {
      Component c1Parent = belongsTo.parent;
      Component c2Parent = conn.belongsTo.parent;

      if (c1Parent == c2Parent)
      {
        Wire wire = new Wire(this, conn);
        connections.Add(wire);
      }
      else
      {
        // TODO: generate exception because components can not be connected
      }
    }
  }

  public class Value
  {
    public int t;
    public bool value;

    public Value(int t, bool value)
    {
      this.t = t;
      this.value = value;
    }
  }

  public abstract class SimEvent : IComparable<SimEvent>
  {
    public int t;

    public SimEvent(int t)
    {
      this.t = t;
    }

    public int CompareTo(SimEvent Other)
    {
      return t.CompareTo(Other.t);
    }
  }

  public class PropEvent : SimEvent
  {
    public bool value;
    public SimpleComp comp;

    public PropEvent(int t, bool value, SimpleComp comp) : base(t)
    {
      this.value = value;
      this.comp = comp;
    }
  }
  /*
  public class CompPropEvent : PropEvent
  {
    public PrimitiveComp comp;

    public CompPropEvent(int t, bool value, PrimitiveComp comp) : base(t, value)
    {
      this.comp = comp;
    }
  }
  */

  public class ClkEvent : SimEvent
  {
    public Component comp;
    public bool nextValue;

    public ClkEvent(int t, Component comp, bool nextValue) : base(t)
    {
      this.comp = comp;
      this.nextValue = nextValue;
    }
  }

  public class UserInputEvent : SimEvent
  {
    public bool value;
    public int compId;
    public int inputId;

    public UserInputEvent(int t, int compId, int inputId, bool value) : base(t)
    {
      this.compId = compId;
      this.inputId = inputId;
      this.value = value;
    }
  }

  public class Wire
  {
    public Connector cIn;
    public Connector cOut;

    public Wire(Connector cIn, Connector cOut)
    {
      this.cIn = cIn;
      this.cOut = cOut;
    }
  }

  public class ComponentDict
  {
    Dictionary<int, Component> components;

    public ComponentDict()
    {
      components = new Dictionary<int, Component>();
    }

    public Component this[int id]
    {
      get
      {
        return components[id];
      }
    }

    public int add(Component comp)
    {
      int id = components.Count;
      comp.setId(id);
      components.Add(id, comp);

      return id;
    }

    public void remove(int id)
    {
      components.Remove(id);
    }

    public int count()
    {
      return components.Count;
    }
  }
}
