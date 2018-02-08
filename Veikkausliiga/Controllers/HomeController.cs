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
        /// <summary>
        /// Index view
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public ActionResult Index(string a, string b)
        {
            //Otetaan päivämäärät stringiin.
            var s = String.Format("{0}", Request.Form["starting-day"]);
            var e = String.Format("{0}", Request.Form["last-day"]);

            //Splitataan päivät molemmista päivämääräfilttereistä jos mahdollista
            string[] splitted = s.Split('-');
            string[] splittedE = e.Split('-');
            string parseS = s;
            string parseE = e;

            //Tarkistetaan ettei päivämäärä ole tyhjä, jotta voidaan parsia päivämäärä oikeaan muotoon
            if (!String.IsNullOrEmpty(s))
            {
                parseS = string.Format("{0}/{1}/{2}", splitted[2], splitted[1], splitted[0]);
            }
            if (!String.IsNullOrEmpty(e))
            {
                parseE = string.Format("{0}/{1}/{2}", splittedE[2], splittedE[1], splittedE[0]);
            }

            List<Games> list = Teams(parseS,parseE);
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
            DateTime sd = new DateTime(); //alkaen päivä
            DateTime ed = new DateTime(); //päättyen päivä

            //Tarkistetaan voidaanko parsia DateTime-muotoon.
            var parsedS = DateTime.TryParseExact(s,"d/M/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out sd);
            var parsedE = DateTime.TryParseExact(e, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ed);

            //Parsitaan DateTimeksi mikäli parsiminen onnistuu ja alkupäivämäärä ei ole tyhjä.
            if (!String.IsNullOrEmpty(s) && parsedS == true)
            {
                sd = DateTime.ParseExact(s, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            
            if (!String.IsNullOrEmpty(e) && parsedE == true)
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
            return list; //Palautetaan lista jossa kaikki ottelut tietyltä aikaväliltä
        }
    }
}
