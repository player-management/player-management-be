using Repository.Models;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ClubRepository : BaseRepository<FootballClub>, IClubRepository
    {
        public ClubRepository(EnglishPremierLeague2024DBContext context) : base(context)
        {

        }
    }
}
