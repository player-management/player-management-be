using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories;
using Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly EnglishPremierLeague2024DBContext _dbContext;
        private IAccountRepository _accountRepository;
        private IClubRepository _clubRepository;
        private IPlayerRepository _playerRepository;

        public IAccountRepository AccountRepository
        {
            get
            {
                return _accountRepository = _accountRepository ?? new AccountRepository(_dbContext);
            }
        }

        public IClubRepository ClubRepository
        {
            get
            {
                return _clubRepository = _clubRepository ?? new ClubRepository(_dbContext);
            }
        }

        public IPlayerRepository PlayerRepository
        {
            get
            {
                return _playerRepository = _playerRepository ?? new PlayerRepository(_dbContext);
            }
        }
        public UnitOfWorks(EnglishPremierLeague2024DBContext dBContext)
        {
            this._dbContext = dBContext;
        }
        public void Commit()
         => _dbContext.SaveChanges();
        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();
        public void Rollback()
            => _dbContext.Dispose();


        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();
    }
}
