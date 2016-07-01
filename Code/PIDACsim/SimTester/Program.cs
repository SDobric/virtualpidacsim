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
      comps.add(new ClockComp(1, 1));
      comps.add(new PrimitiveComp(PrimCompType.NOT, 1));
      comps.add(new PrimitiveComp(PrimCompType.NOT, 1));
      comps[0].outputs[0].connectTo(comps[1].inputs[0]);
      comps[1].outputs[0].connectTo(comps[2].inputs[0]);
      Simulation sim = new Simulation(comps);
      //sim.addUserInputEvent(0,0,true);

      for (int i = 0; i < 5; i++)
      {
        sim.simStep();
        Console.WriteLine("Simulation step: " + i);
        Console.WriteLine(comps[0].ToString());
        Console.WriteLine(comps[1].ToString());
        Console.WriteLine(comps[2].ToString());
      }

      Console.ReadLine();
    }
  }
}
