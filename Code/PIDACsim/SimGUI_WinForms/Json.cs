using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimGUI
{
  public class ResultMsg
  {
    public int compId;
    public int time;
    public string input;
    public string output;
  }

  class Json
  {
    public static byte[] jsonEncode(object input)
    {
      String encString = JsonConvert.SerializeObject(input);
      return Encoding.ASCII.GetBytes(encString);
    }

    public static List<ResultMsg> jsonDecode(string input)
    {
      input = input.TrimEnd("\0".ToCharArray());

      String inputStr = input.Replace("}{", "}x{");

      string[] inputs = inputStr.Split("x".ToCharArray());

      List<ResultMsg> msgs = new List<ResultMsg>();

      foreach(string jsonStr in inputs)
         msgs.Add(JsonConvert.DeserializeObject<ResultMsg>(jsonStr, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.All }));

      return msgs;
    }
  }
}
