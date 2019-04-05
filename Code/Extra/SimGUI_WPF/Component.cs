using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using GateSim;

namespace SimGUI_WPF
{
  public class CompThumb : Thumb
  {
    public List<LineGeometry> EndLines { get; private set; }
    public List<LineGeometry> StartLines { get; private set; }

    public CompThumb()
    {
      StartLines = new List<LineGeometry>();
      EndLines = new List<LineGeometry>();
    }
  }

  class CompGeometry
  {
    String imgLocation;
    List<Ellipse> connEllipses;

  }

  class UiComp
  {
    public String text;
    //double leftPos, topPos;
    Component simComp;

    public UiComp(Component comp)
    {
      simComp = comp;
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
