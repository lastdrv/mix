using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: Fiddler.RequiredVersion("2.3.5.0")]

namespace GoWFiddler
{
    public class Class1 : IAutoTamper
    {
        public void OnLoad()
        {
            File.AppendAllText("gow.txt", DateTime.Now + " " + "Start" + '\n');
        }
        
        public void OnBeforeUnload() { }

        public void AutoTamperRequestBefore(Session oSession)
        {
            if (oSession.host=="pcmob.parse.gemsofwar.com")
            {
                // сюда будем писать парсер и обработчик ответа серверу
                //File.AppendAllText("gow.txt", DateTime.Now + " request before " + oSession.host + '\n');
            }
        }
        public void AutoTamperRequestAfter(Session oSession)         {        }
        public void AutoTamperResponseBefore(Session oSession) 
        {
            if (oSession.host == "pcmob.parse.gemsofwar.com")
            {
                string json = oSession.GetResponseBodyAsString();
                try
                {
                    JObject o1 = JObject.Parse(json);
                    if (o1["result"] != null)
                    {
                        if (o1["result"]["MapTurnBonus"] != null)
                         {
                            //File.AppendAllText("gow.txt", DateTime.Now + " " + json + '\n');
                            File.AppendAllText("gow.txt", DateTime.Now + " MapTurnBonus exist " + o1["result"]["MapTurnBonus"].ToString() + '\n');
                            int res = o1["result"]["MapTurnBonus"].ToObject<int>() + 5;
                            o1["result"]["MapTurnBonus"] = res;
                            File.AppendAllText("gow.txt", DateTime.Now + " MapTurnBonus update to " + res.ToString() + '\n');
                            //File.AppendAllText("gow.txt", DateTime.Now + " " + json + '\n');
                            oSession.utilSetResponseBody(o1.ToString(Formatting.None));
                        }
                         else
                         {
                             File.AppendAllText("gow.txt", DateTime.Now + " value MapTurnBonus not found"  + '\n');
                         }
                    }
                    else
                    {
                        File.AppendAllText("gow.txt", DateTime.Now + " no result in reply" + '\n');
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText("gow.txt", DateTime.Now + " EXCEPT " + e + '\n');
                }

                
            }
        }
        public void AutoTamperResponseAfter(Session oSession)         {        }
        public void OnBeforeReturningError(Session oSession) { }
    }
}
