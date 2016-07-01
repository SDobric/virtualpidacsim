using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GateSim;

namespace SimTester
{
  class Program
  {
    static void Main(string[] args)
    {
      ComponentDict comps = new ComponentDict();
      comps.add(new SingleComp(CompType.NOT, 1));
      comps.add(new SingleComp(CompType.NOT, 1));
      comps[0].outputs[0].connectTo(comps[1].inputs[0], 0);
      comps[1].outputs[0].connectTo(comps[0].inputs[0], 0);
      Simulation simul = new Simulation(comps);
      simul.addInputCompById(0);
      simul.addUserInputEvent(0,0,true);

      for (int i = 0; i < 5; i++)
      {
        Console.WriteLine("Simulation step: " + i);
        Console.WriteLine(comps[0].ToString());
        Console.WriteLine(comps[1].ToString());
        simul.simStep();
      }

      Console.ReadLine();
    }
  }
}
