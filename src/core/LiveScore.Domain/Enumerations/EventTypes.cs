namespace LiveScore.Domain.Enumerations
{
    public class EventTypes : Enumeration
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
        public static readonly EventTypes RedCardType = new EventTypes(RedCard, nameof(RedCard));

        //score_change
        public const string ScoreChange = "score_change";
        public static readonly EventTypes ScoreChangeType = new EventTypes(ScoreChange, nameof(ScoreChange));

        //shot_off_target
        //shot_on_target
        //shot_saved
        //substitution
        public const string Substitution = "substitution";
        public static readonly EventTypes SubstitutionType = new EventTypes(Substitution, nameof(Substitution));

        //throw_in
        //video_assistant_referee
        //video_assistant_referee_over
        //yellow_card
        public const string YellowCard = "red_card";
        public static readonly EventTypes YellowCardType = new EventTypes(YellowCard, nameof(YellowCard));

        //yellow_red_card
        public const string YellowRedCard = "red_card";
        public static readonly EventTypes YellowRedCardType = new EventTypes(YellowRedCard, nameof(YellowRedCard));

        public EventTypes()
        {
        }

        public EventTypes(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}