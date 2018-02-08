using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Veikkausliiga.Models
{
    /// <summary>
    /// Kotijoukkueen luokka
    /// </summary>
    public class HomeTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public object Logo { get; set; }
        public string LogoUrl { get; set; }
        public int Ranking { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Vastustajajoukkueen luokka
    /// </summary>
    public class AwayTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public object Logo { get; set; }
        public string LogoUrl { get; set; }
        public int Ranking { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// RootObject.
    /// </summary>
    public class RootObject
    {
        public int Id { get; set; }
        public object Round { get; set; }
        public int RoundNumber { get; set; }
        public DateTime MatchDate { get; set; }
        public HomeTeam HomeTeam { get; set; }
        public AwayTeam AwayTeam { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int Status { get; set; }
        public int PlayedMinutes { get; set; }
        public object SecondHalfStarted { get; set; }
        public DateTime? GameStarted { get; set; }
        public List<object> MatchEvents { get; set; }
        public List<object> PeriodResults { get; set; }
        public bool OnlyResultAvailable { get; set; }
        public int Season { get; set; }
        public string Country { get; set; }
        public string League { get; set; }
    }

    /// <summary>
    /// Luokka peli-objektien luomiseen, jotta saadaan tiedot taulukkoon.
    /// </summary>
    public class Games
    {
        //Kertoo monesko ottelu taulukossa
        public int GameN { get; set; } 
        //Kotijoukkue
        public string HomeT { get; set; }
        //Vierasjoukkue
        public string AwayT { get; set; }
        //Kotijoukkueen pisteet
        public int HomeG { get; set; }
        //Vierailijajoukkueen pisteet
        public int AwayG { get; set; }
        
        //Formatoi data päivämäärän mukaan. Kellon ajat pois näkyvistä taulukossa.
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime DateT { get; set; }
    }



}