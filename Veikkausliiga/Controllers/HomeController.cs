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

        /// <summary>
        /// Listaa joukkueet päivämäärien mukaan. Luodaan lista jossa Games objekteja.
        /// </summary>
        /// <param name="start">Alkupäivämäärä</param>
        /// <param name="end">Loppupäivämäärä</param>
        /// <returns>Palauttaa listan Games-objekteista</returns>
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
            
            //otteluiden lukumäärä
            int i = 0; 

            //Loopilla haetaan kaikki tarvittavat tiedot tauluun.
            foreach (var item in rootObj)
            {
                var homeName = item.HomeTeam.Name;
                var awayName = item.AwayTeam.Name;
                var homeGoals = item.HomeGoals;
                var awayGoals = item.AwayGoals;
                var date = item.MatchDate;

                //Listaa ottelut jos vain alkaen input täytetty
                if (!(s == "")&&(e == ""))
                {
                    if (sd > date  )
                    {
                    }
                    else list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                }

                //Listaa ottelut jos vain loppuen input täytetty
                if (!(e == "")&&(s == ""))
                {
                    if (ed < date)
                    {
                    }
                    else list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                }

                //Listaa ottelut jos molemmat inputit täytetty. Listaa ottelut päivämäärien väliltä.
                if (!(e == "") && !(s == ""))
                {
                    if ((sd < date) && (ed > date))
                    {
                        list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                    }
                }

                //Listaa kaikki ottelut jos inputit ovat tyhjiä
                if ((e == "") && (s == ""))
                {
                    list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                }

                i++;
            }
            return list;
        }
    }
}
