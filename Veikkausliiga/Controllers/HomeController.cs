using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Veikkausliiga.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Globalization;

namespace Veikkausliiga.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index(string a, string b)
        {
            var s = String.Format("{0}", Request.Form["starting-day"]);
            var e = String.Format("{0}", Request.Form["last-day"]);
            List<Games> list = Teams(s,e);
            return View(list);
        }

        public List<Games> Teams(string start, string end)
        {
            string s = start;
            string e = end;
            DateTime sd = new DateTime();
            DateTime ed = new DateTime();

            if (!String.IsNullOrEmpty(s))
            {
                
                sd = DateTime.ParseExact(s, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            if (!String.IsNullOrEmpty(e))
            {
                ed = DateTime.ParseExact(e, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            //Haetaan tiedot tiedostosta, ja deserialisoidaan se.
            string file = Server.MapPath("~/App_Data/matches.json");
            string JsonF = System.IO.File.ReadAllText(file);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var rootObj = ser.Deserialize<List<RootObject>>(JsonF);



            //Luodaan Lista Games objektista
            List<Games> list = new List<Games>();
            int i = 0;
            //Loopilla haetaan kaikki tarvittavat tiedot tauluun.
            foreach (var item in rootObj)
            {


                var homeName = item.HomeTeam.Name;
                var awayName = item.AwayTeam.Name;
                var homeGoals = item.HomeGoals;
                var awayGoals = item.AwayGoals;
                var date = item.MatchDate;
                var date2 =date.ToShortDateString();
                var dt = date2.ToString();
                //var edt = String.Format("{0}",dt);

                //string[] dateString = dt.Split('/',' ');
                //DateTime d = Convert.ToDateTime(dateString[1] + "/" + dateString[0] + "/" + dateString[2]);

                //var d = DateTime.ParseExact(dt, "dd/MM/yy", CultureInfo.InvariantCulture);
                //DateTime d = DateTime.ParseExact(dt,"d/M/yy",CultureInfo.GetCultureInfo("fi-FI").DateTimeFormat);

                if (!(s == "") || !(e == ""))
                {

                    if (sd > date  )
                    {
                        //list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                    }
                    else list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                
                    if (ed < date )
                    {
                    }
                    else list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                }
                else list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });


                i++;
            }
            return list;
        }
    }
}
