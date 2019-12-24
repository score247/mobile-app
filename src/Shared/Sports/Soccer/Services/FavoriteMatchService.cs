using System.Collections.Generic;
using System.Linq;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Services
{
    public class FavoriteMatchService : FavoriteService<IMatch>
    {
        public FavoriteMatchService(IUserSettingService userSettingService)
                : base(userSettingService)
        {
            Init(nameof(FavoriteMatchService), 99);
        }

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(Key, Objects.Select(obj => obj as SoccerMatch).ToList());
        }

        public override IList<IMatch> LoadCache()
        => userSettingService.GetValueOrDefault(Key, Enumerable.Empty<SoccerMatch>()).Select(l => l as IMatch).ToList();
    }
}
