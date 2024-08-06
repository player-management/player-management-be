using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class FootballClub
    {
        public FootballClub()
        {
            FootballPlayers = new HashSet<FootballPlayer>();
        }

        public string FootballClubId { get; set; } = null!;
        public string ClubName { get; set; } = null!;
        public string ClubShortDescription { get; set; } = null!;
        public string SoccerPracticeField { get; set; } = null!;
        public string Mascos { get; set; } = null!;

        public virtual ICollection<FootballPlayer> FootballPlayers { get; set; }
    }
}
