using System.Collections.Generic;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.Views.Templates.MatchDetails.LineUps;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsItemViewModel
    {
        public LineupsItemViewModel(
            IDependencyResolver dependencyResolver,
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null,
            bool isSubstitution = false)
        {
            DependencyResolver = dependencyResolver;
            HomeName = homeName;
            AwayName = awayName;
            HomeJerseyNumber = homeJerseyNumber;
            AwayJerseyNumber = awayJerseyNumber;
            IsSubstitution = isSubstitution;
        }

        public string HomeName { get; }

        public string AwayName { get; }

        public int? HomeJerseyNumber { get; }

        public int? AwayJerseyNumber { get; }
        public bool IsSubstitution { get; }

        public IDependencyResolver DependencyResolver { get; protected set; }

        public DataTemplate CreateTemplate()
        {
            if (IsSubstitution)
            {
                return new SubstitutionTemplate();
            }
            else
            {
                return new LineupsPlayerTemplate();
            }
        }
    }
}