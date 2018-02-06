using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Veikkausliiga.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Veikkausliiga.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Haetaan tiedot tiedostosta, ja deserialisoidaan se.
            string file = Server.MapPath("~/App_Data/matches.json");
            string JsonF = System.IO.File.ReadAllText(file);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var rootObj = ser.Deserialize<List<RootObject>>(JsonF);

            //Luodaan Lista Games objektista
            List<Games> list = new List<Games>();

            //Loopilla haetaan kaikki tarvittavat tiedot tauluun.
            foreach (var item in rootObj)
            {
                int i = 0;

                var homeName = item.HomeTeam.Name;
                var awayName = item.AwayTeam.Name;
                var homeGoals = item.HomeGoals;
                var awayGoals = item.AwayGoals;
                var date = item.MatchDate;
                list.Add(new Games { GameN = i, HomeT = homeName, AwayT = awayName, HomeG = homeGoals, AwayG = awayGoals, DateT = date });
                i++;
            }
            return View(list);
        }
    }
}
