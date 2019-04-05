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
    public enum EType { SIMSTART, SIMSTOP, ADDCOMP, DELCOMP, CONNECT, DISCONNECT, INTERACT };

    public struct JsonValues
    {
      public EType eventType;
      public CompType compType;
      public int compId;
      public int compDepth;
      public int wireId;
      public int fromCompId;
      public int toCompId;
      public int outputId;
      public int inputId;
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

    public UserInputEvent(EType type, Wire wire)
    {
      jsonValues.wireId = wire.id;
      jsonValues.eventType = type;

      jsonValues.fromCompId = wire.cOut.belongsTo.getId();
      jsonValues.toCompId = wire.cIn.belongsTo.getId();
      jsonValues.outputId = wire.cOut.id;
      jsonValues.inputId = wire.cIn.id;

      switch (type)
      {
        case EType.CONNECT:
          break;
        case EType.DISCONNECT:
          break;
      }

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
          this.type = type;
          break;
        case EType.DELCOMP:
          this.type = type;
          break;
        case EType.INTERACT:
          break;
      }

      this.type = type;
    }
  }
}
