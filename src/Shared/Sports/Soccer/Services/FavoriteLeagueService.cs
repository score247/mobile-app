using System.Collections.Generic;
using System.Linq;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Models.Leagues;

namespace LiveScore.Soccer.Services
{
    public class FavoriteLeagueService : FavoriteService<ILeague>
    {
        public FavoriteLeagueService(IUserSettingService userSettingService)
                : base(userSettingService)
        {
            Init(nameof(FavoriteLeagueService), 30);
        }

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(Key, Objects.Select(obj => obj as League).ToList());
        }

        public override IList<ILeague> LoadCache()
        => userSettingService.GetValueOrDefault(Key, Enumerable.Empty<League>()).Select(l => l as ILeague).ToList();
    }
}
