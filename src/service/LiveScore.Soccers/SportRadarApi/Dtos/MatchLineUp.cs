﻿////// <auto-generated />
////using System;

////namespace LiveScore.Shared.Models.Dtos
////{


////    public class MatchLineUp
////    {
////        public DateTime generated_at { get; set; }
////        public string schema { get; set; }
////        public Sport_Event sport_event { get; set; }
////        public Lineup[] lineups { get; set; }
////    }

////    public class Sport_Event
////    {
////        public string id { get; set; }
////        public DateTime scheduled { get; set; }
////        public bool start_time_tbd { get; set; }
////        public Tournament_Round tournament_round { get; set; }
////        public Season season { get; set; }
////        public Tournaments tournament { get; set; }
////        public Competitor[] competitors { get; set; }
////        public Venue venue { get; set; }
////    }

////    public class Tournament_Round
////    {
////        public string type { get; set; }
////        public string name { get; set; }
////        public int cup_round_match_number { get; set; }
////        public int cup_round_matches { get; set; }
////        public string other_match_id { get; set; }
////    }

////    public class Season
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public string start_date { get; set; }
////        public string end_date { get; set; }
////        public string year { get; set; }
////        public string tournament_id { get; set; }
////    }

////    public class Tournaments
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public Sport sport { get; set; }
////        public Category category { get; set; }
////    }

////    public class Sport
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////    }

////    public class Category
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////    }

////    public class Venue
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public int capacity { get; set; }
////        public string city_name { get; set; }
////        public string country_name { get; set; }
////        public string map_coordinates { get; set; }
////        public string country_code { get; set; }
////    }

////    public class Competitor
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public string country { get; set; }
////        public string country_code { get; set; }
////        public string abbreviation { get; set; }
////        public string qualifier { get; set; }
////    }

////    public class Lineup
////    {
////        public string team { get; set; }
////        public string formation { get; set; }
////        public Manager manager { get; set; }
////        public Jersey jersey { get; set; }
////        public Starting_Lineup[] starting_lineup { get; set; }
////        public Substitute[] substitutes { get; set; }
////    }

////    public class Manager
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public string nationality { get; set; }
////        public string country_code { get; set; }
////    }

////    public class Jersey
////    {
////        public string type { get; set; }
////        public string _base { get; set; }
////        public string sleeve { get; set; }
////        public string number { get; set; }
////        public bool squares { get; set; }
////        public bool stripes { get; set; }
////        public bool horizontal_stripes { get; set; }
////        public bool split { get; set; }
////        public string shirt_type { get; set; }
////        public string sleeve_detail { get; set; }
////    }

////    public class Starting_Lineup
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public string type { get; set; }
////        public int jersey_number { get; set; }
////        public string position { get; set; }
////        public int order { get; set; }
////    }

////    public class Substitute
////    {
////        public string id { get; set; }
////        public string name { get; set; }
////        public string type { get; set; }
////        public int jersey_number { get; set; }
////    }


////}