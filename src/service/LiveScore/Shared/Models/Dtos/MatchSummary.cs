﻿// <auto-generated />
using System;

namespace LiveScore.Shared.Models.Dtos
{


    public class MatchSummary
    {
        public DateTime generated_at { get; set; }
        public string schema { get; set; }
        public Sport_Event sport_event { get; set; }
        public Sport_Event_Conditions sport_event_conditions { get; set; }
        public Sport_Event_Status sport_event_status { get; set; }
        public Statistics statistics { get; set; }
    }

    public class Sport_Event
    {
        public string id { get; set; }
        public DateTime scheduled { get; set; }
        public bool start_time_tbd { get; set; }
        public Tournament_Round tournament_round { get; set; }
        public Season season { get; set; }
        public DailyTournament tournament { get; set; }
        public Competitor[] competitors { get; set; }
        public Venue venue { get; set; }
    }

    public class Sport_Event_Conditions
    {
        public Referee referee { get; set; }
        public Referee_Assistants[] referee_assistants { get; set; }
        public Venue1 venue { get; set; }
        public int attendance { get; set; }
        public Weather_Info weather_info { get; set; }
    }

    public class Referee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string country_code { get; set; }
    }

    public class Venue1
    {
        public string id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string city_name { get; set; }
        public string country_name { get; set; }
        public string map_coordinates { get; set; }
        public string country_code { get; set; }
    }

    public class Weather_Info
    {
        public string pitch { get; set; }
        public string weather_conditions { get; set; }
    }

    public class Referee_Assistants
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string country_code { get; set; }
    }

    public class Sport_Event_Status
    {
        public string status { get; set; }
        public string match_status { get; set; }
        public int home_score { get; set; }
        public int away_score { get; set; }
        public string winner_id { get; set; }
        public int aggregate_home_score { get; set; }
        public int aggregate_away_score { get; set; }
        public string aggregate_winner_id { get; set; }
        public Period_Scores[] period_scores { get; set; }
    }

    public class Period_Scores
    {
        public int home_score { get; set; }
        public int away_score { get; set; }
        public string type { get; set; }
        public int number { get; set; }
    }

    public class Statistics
    {
        public Team[] teams { get; set; }
    }

    public class Team
    {
        public string id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string qualifier { get; set; }
        public Statistics1 statistics { get; set; }
        public Player[] players { get; set; }
    }

    public class Statistics1
    {
        public int ball_possession { get; set; }
        public int free_kicks { get; set; }
        public int throw_ins { get; set; }
        public int goal_kicks { get; set; }
        public int shots_blocked { get; set; }
        public int shots_on_target { get; set; }
        public int shots_off_target { get; set; }
        public int corner_kicks { get; set; }
        public int fouls { get; set; }
        public int shots_saved { get; set; }
        public int offsides { get; set; }
        public int yellow_cards { get; set; }
        public int injuries { get; set; }
    }

    public class Player
    {
        public string id { get; set; }
        public string name { get; set; }
        public int substituted_in { get; set; }
        public int substituted_out { get; set; }
        public int goals_scored { get; set; }
        public int assists { get; set; }
        public int own_goals { get; set; }
        public int yellow_cards { get; set; }
        public int yellow_red_cards { get; set; }
        public int red_cards { get; set; }
        public int goal_line_clearances { get; set; }
        public int interceptions { get; set; }
        public int chances_created { get; set; }
        public int crosses_successful { get; set; }
        public int crosses_total { get; set; }
        public int passes_short_successful { get; set; }
        public int passes_medium_successful { get; set; }
        public int passes_long_successful { get; set; }
        public int passes_short_total { get; set; }
        public int passes_medium_total { get; set; }
        public int passes_long_total { get; set; }
        public int duels_header_successful { get; set; }
        public int duels_sprint_successful { get; set; }
        public int duels_tackle_successful { get; set; }
        public int duels_header_total { get; set; }
        public int duels_sprint_total { get; set; }
        public int duels_tackle_total { get; set; }
        public int goals_conceded { get; set; }
        public int shots_faced_saved { get; set; }
        public int shots_faced_total { get; set; }
        public int penalties_faced { get; set; }
        public int penalties_saved { get; set; }
        public int fouls_committed { get; set; }
        public int was_fouled { get; set; }
        public int offsides { get; set; }
        public int shots_on_goal { get; set; }
        public int shots_off_goal { get; set; }
        public int shots_blocked { get; set; }
        public int minutes_played { get; set; }
        public float performance_score { get; set; }
        public int goals_by_head { get; set; }
        public int goals_by_penalty { get; set; }
        public int penalty_goals_scored { get; set; }
    }


}
