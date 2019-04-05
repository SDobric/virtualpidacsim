using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimGUI
{
  public enum PortType { Input, Output };

  public class Tag
  {
    public PortType type;
    public int id;

    public Tag(PortType type, int id)
    {
      this.type = type;
      this.id = id;
    }
  }

  static class Globals
  {
    public static bool simRunning = false;
    public static bool simStartRequested = false;
    public static bool simStopRequested = false;
    public static ConcurrentQueue<UserInputEvent> userInputEventQueue = new ConcurrentQueue<UserInputEvent>();
    public static ConcurrentQueue<ResultMsg> resultMsgQueue = new ConcurrentQueue<ResultMsg>();
    public static ConcurrentDictionary<int, Component> components =  new ConcurrentDictionary<int, Component>();
    public static ConcurrentDictionary<int, Wire> wires = new ConcurrentDictionary<int, Wire>();
    public static int lastCompId = 0;
    public static int lastConnId = 0;
    // TODO: Create event dictionary class and incorporate last comp id
  }
}
