using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IClubRepository ClubRepository { get; }
        IPlayerRepository PlayerRepository { get; }
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
