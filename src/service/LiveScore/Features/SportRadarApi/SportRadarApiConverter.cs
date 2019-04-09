namespace LiveScore.Features.SportRadarApi
{
    public static class SportRadarApiConverter
    {
        private const int BaseketballSportId = 2;
        private const int ESportsSportId = 3;

        public static string SportIdToName(int sportId)
        {
            switch (sportId)
            {
                case BaseketballSportId:
                    return "baseketball";

                case ESportsSportId:
                    return "sports";

                default:
                    return "soccer";
            }
        }
    }
}