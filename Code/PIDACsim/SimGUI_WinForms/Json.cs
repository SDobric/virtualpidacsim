using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimGUI
{
  class Json
  {
    public static byte[] jsonEncode(object input)
    {
      String encString = JsonConvert.SerializeObject(input);
      return Encoding.ASCII.GetBytes(encString);
    }

    public static String jsonDecode(String input)
    {
      return "";
    }
  }
}
