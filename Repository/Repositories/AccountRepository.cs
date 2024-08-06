using Repository.Models;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AccountRepository :BaseRepository<PremierLeagueAccount>, IAccountRepository
    {
        public AccountRepository(EnglishPremierLeague2024DBContext context):base(context) 
        {
            
        }
    }
}
