namespace LiveScore.Domain.CommonValues
{
    public static class EventTypes
    {
        //break_start
        //cancelled_video_assistant_referee
        //corner_kick
        //free_kick
        //goal_kick
        //injury
        //injury_return
        //injury_time_shown
        //match_ended
        //match_started
        //offside
        //penalty_awarded
        //penalty_missed
        //penalty_shootout
        //period_score
        //period_start
        //possible_video_assistant_referee
        //red_card
        public const string RedCard = "red_card";
        //score_change
        public const string ScoreChange = "score_change";
        //shot_off_target
        //shot_on_target
        //shot_saved
        //substitution
        public const string Substitution = "substitution";
        //throw_in
        //video_assistant_referee
        //video_assistant_referee_over
        //yellow_card
        public const string YellowCard = "red_card";
        //yellow_red_card
        public const string YellowRedCard = "red_card";
    }
}
