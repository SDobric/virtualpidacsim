using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;

namespace SimGUI
{
  class UserInputEvent
  {
    public enum EType { SIMSTART, SIMSTOP, ADDCOMP, DELCOMP, ADDWIRE, DELWIRE, INTERACT };

    public struct JsonValues
    {
      public EType eventType;
      public CompType compType;
      public int compId;
      public int compDepth;
      public int wireFromCompId;
      public int wireToCompId;
      public int wireFromInputId;
      public int wireToOutputId;
      public int interactVal;
    }

    public int interactVal;
    public EType type;
    public Component comp;
    public JsonValues jsonValues;

    public UserInputEvent(EType type)
    {
      jsonValues.eventType = type;
    }

    public UserInputEvent(EType type,  Component comp)
    {
      jsonValues.eventType = type;
      jsonValues.compType = comp.type;
      jsonValues.compId = comp.getId();
      jsonValues.compDepth = comp.depth;

      switch (type)
      {
        case EType.ADDCOMP:
          jsonValues.compDepth = comp.depth;
          this.type = type;
          break;
        case EType.DELCOMP:
          jsonValues.compDepth = comp.depth;
          this.type = type;
          break;
        case EType.ADDWIRE:
          break;
        case EType.DELWIRE:
          break;
        case EType.INTERACT:
          break;
      }

      this.type = type;
    }
  }
}
