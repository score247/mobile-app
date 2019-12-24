using System.Collections.Generic;
using System.Linq;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class FavoriteMatchService : FavoriteService<IMatch>
    {
        public FavoriteMatchService(IUserSettingService userSettingService, IEventAggregator eventAggrerator)
                : base(userSettingService, eventAggrerator)
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
