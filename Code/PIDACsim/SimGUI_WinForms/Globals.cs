using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SimGUI
{
  static class Globals
  {
    public static bool simRunning = false;
    public static bool simStartRequested = false;
    public static bool simStopRequested = false;
    public static ConcurrentQueue<UserInputEvent> userInputEventQueue = new ConcurrentQueue<UserInputEvent>();
    public static ConcurrentDictionary<int, Component> components =  new ConcurrentDictionary<int, Component>();
    public static int lastCompId = 0;
    // TODO: Create event dictionary class and incorporate last comp id
  }
}
