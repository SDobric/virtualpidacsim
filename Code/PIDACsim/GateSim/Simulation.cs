using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSim
{
  public class Simulation
  {
    int t;
    public ComponentDict comps;
    //List<PrimitiveComp> inputComps;
    Dictionary<int, LinkedList<SimEvent>> eventList;
    //PriorityQueue<SimEvent> eventQueue;


    public Simulation(ComponentDict comps)
    {
      this.comps = comps;

      t = 0;
      //inputComps = new List<PrimitiveComp>();
      eventList = new Dictionary<int, LinkedList<SimEvent>>();
      //eventQueue = new PriorityQueue<SimEvent>();
    }

    /*
    public void addInputCompById(int id)
    {
      Component comp = comps[id];
      if (comp is PrimitiveComp)
        inputComps.Add((PrimitiveComp)comp);
      // TODO: adding input of hierarchical comp to input comp list
    }
    */
    public void addComponent()
    {

    }

    // Advance simulation time by number of cycles.
    public void simStep(int cycles)
    {
      for(int i = 0; i < cycles; i++)
        simStep();
    }

    public void simStep()
    {
      if(!eventList.ContainsKey(t))
        eventList[t] = new LinkedList<SimEvent>();

      List<PrimitiveComp> inModComps = new List<PrimitiveComp>();
      List<Wire> inModWires = new List<Wire>();

      if(t == 0)
      {
        for (int i = 0; i < comps.count(); i++)
        {
          Component comp = comps[i];
          //if (comp is PrimitiveComp && ((PrimitiveComp)comp).type == PrimCompType.Clk)
          if (comp is ClockComp)
          {
            ClockComp clkComp = (ClockComp)comp;

            // Check whether it is time to schedule clock tick, if so schedule clock tick.
            if ((clkComp.insertedAtT + t) % clkComp.period == 0)
              queueEvent(new ClkEvent(t, comp, true));
            //eventQueue.Enqueue(new ClkEvent(t, comp, true));
          }
        }
      }


      /* For each event that is scheduled for this cycle. */
      //while (eventQueue.Count != 0 && eventQueue.Peek().t == t)
      while(eventList[t].Count != 0)
      {
        //Console.WriteLine("test");
        //Console.WriteLine(eventQueue.);
        //SimEvent currEvent = eventQueue.Dequeue();
        SimEvent currEvent = dequeueEvent();

        if (currEvent is UserInputEvent)
        {
          UserInputEvent inputEvent = ((UserInputEvent)currEvent);
          comps[inputEvent.compId].inputs[inputEvent.inputId].currVal = inputEvent.value;
          Console.WriteLine("User input event handled");
        }

        if (currEvent is ClkEvent)
        {
          ClkEvent clkEvent = ((ClkEvent)currEvent);
          clkEvent.comp.outputs[0].changeVal(t, clkEvent.nextValue);
          queueEvent(new ClkEvent(t + 1, clkEvent.comp, !clkEvent.nextValue));
          //eventQueue.Enqueue(new ClkEvent(t + 1, clkEvent.comp, !clkEvent.nextValue));

          ClockComp comp = (ClockComp)(clkEvent.comp);

          /* Create resulting prop event */
          int timeStamp = t + comp.propDelay;
          PropEvent propEvent = new PropEvent(timeStamp, !clkEvent.nextValue, (SimpleComp)(clkEvent.comp));
          queueEvent(propEvent);
          //eventQueue.Enqueue(propEvent);

          Console.WriteLine("Clkevent handled");
        }

        /* Propagate wire value to it's output (component input).
        if (currEvent is WirePropEvent)
        {
          WirePropEvent propEvent = ((WirePropEvent)currEvent);
          if (propEvent.wire.cOut.value != propEvent.value)
          {
            propEvent.wire.cOut.value = propEvent.value;
            inModComps.Add((PrimitiveComp)(propEvent.wire.cOut.belongsTo));
          }
        }
        */

        // TODO: FIX entirely
        /* Propagate component input to it's output. */
        if (currEvent is PropEvent)
        {
          PropEvent propEvent = ((PropEvent)currEvent);
          //Console.WriteLine("test." + propEvent.comp.outputs[0].value);
          // TODO: Check correctness of if statement
          //if (propEvent.comp.outputs[0].value != propEvent.value)
          {
            //Console.WriteLine("test.");
            propEvent.comp.outputs[0].currVal = propEvent.value;
            if (propEvent.comp.outputs[0].connections.Count > 0)
            {
                propEvent.comp.outputs[0].connections[0].cOut.currVal = propEvent.value;
                //inModWires.Concat(propEvent.comp.outputs[0].connections);
                inModComps.Add((PrimitiveComp)(propEvent.comp.outputs[0].connections[0].cOut.belongsTo));
            }
          }

          //PropEvent propEvent2 = new PropEvent(t+1, propEvent.value, (PrimitiveComp)(propEvent.comp.outputs[0].connections[0].cOut.belongsTo));
          //eventQueue.Enqueue(propEvent);

          Console.WriteLine("Propagation event handled for component: " + propEvent.comp.getId() + ".");
        }
      }

      while(inModComps.Count != 0 || inModWires.Count != 0)
      {
        /* For each component that has had it's input modified this cycle, 
          * create new component propagation event. */
        foreach (PrimitiveComp comp in inModComps)
        {
          bool value = comp.evaluate();
          //if (comp.propDelay == 0)
          //  comp.outputs[0].value = value;
          //else
          //{
          int timeStamp = t + comp.propDelay;
          PropEvent propEvent = new PropEvent(timeStamp, value, comp);
          queueEvent(propEvent);
          //eventQueue.Enqueue(propEvent);
          //}
        }
        inModComps.Clear();

        /* For each wire that has had it's input modified this cycle, 
          * create new wire propagation event.
        foreach (Wire wire in inModWires)
        {
          bool value = wire.cIn.value;
          if (wire.propDelay == 0)
          {
            wire.cOut.value = value;
            inModComps.Add((PrimitiveComp)(wire.cOut.belongsTo));
          }
          else
          {
            int timeStamp = t + wire.propDelay;
            WirePropEvent propEvent = new WirePropEvent(timeStamp, value, wire);
            eventQueue.Enqueue(propEvent);
          }
        }
        inModWires.Clear();
        */
      }
      eventList.Remove(t);
      t++;
    }

    private void queueEvent(SimEvent e)
    {
      if(!eventList.ContainsKey(e.t))
        eventList[e.t] = new LinkedList<SimEvent>();

      eventList[e.t].AddLast(e);
    }

    private SimEvent dequeueEvent()
    {
      SimEvent e = eventList[t].First();
      eventList[t].RemoveFirst();
      return e;
    }

    public void addUserInputEvent(int compId, int inputId, bool value)
    {
      UserInputEvent userInputEvent = new UserInputEvent(t + 1, compId, inputId, value);
      queueEvent(userInputEvent);
      //eventQueue.Enqueue(userInputEvent);
    }

    void addCompHierarchy()
    {

    }

    void delCompHierarchy()
    {

    }
  }
}
