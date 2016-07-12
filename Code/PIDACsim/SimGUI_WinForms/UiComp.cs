using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using GateSim;

namespace SimGUI_WinForms
{
  class UiComp
  {
    public String text;
    //double leftPos, topPos;
    public Component simComp;
    public Node uiNode;

    public UiComp()
    {
    }

    public int numInputs()
    {
      return simComp.inputsLen;
    }

    public int numOutputs()
    {
      return simComp.outputsLen;
    }
  }
}
