using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSim
{
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
    public Component comp;
    public bool value;
    public int inputId;

    public UserInputEvent(int t, Component comp, int inputId, bool value) : base(t)
    {
      this.comp = comp;
      this.inputId = inputId;
      this.value = value;
    }
  }
}
