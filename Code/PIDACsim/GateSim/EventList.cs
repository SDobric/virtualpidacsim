using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSim
{
  /* The event list class
   * 
   */
  class EventList
  {
    private int t;
    private Dictionary<LinkedList<SimEvent>, int> eventList;

    public EventList()
    {
      t = 0;
      eventList = new Dictionary<LinkedList<SimEvent>, int>();
    }

    public void schedule(SimEvent e)
    {
      //eventList[t];
    }

    public void deSchedule(SimEvent e)
    {

    }

    public void dequeue(int t)
    {

    }

    /*
    public void advanceTime()
    {
      t++;
    }
    */
  }

  class EventListItem
  {
  }
}
