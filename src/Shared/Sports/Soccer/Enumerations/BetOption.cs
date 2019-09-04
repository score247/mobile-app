﻿namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class BetOption : Enumeration
    {
        private const string HomeType = "home";

        public static readonly BetOption Home = new BetOption(1, HomeType);

        private const string DrawType = "draw";

        public static readonly BetOption Draw = new BetOption(2, DrawType);

        private const string AwayType = "away";

        public static readonly BetOption Away= new BetOption(3, AwayType);

        private const string  OverType = "over";

        public static readonly BetOption Over = new BetOption(4, OverType);

        private const string  UnderType = "under";

        public static readonly BetOption Under = new BetOption(5, UnderType);

        private BetOption(byte value, string displayName)
           : base(value, displayName)
        {
        }
    }
}