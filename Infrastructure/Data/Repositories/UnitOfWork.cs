using Core.Entities;
using Core.Entities.Identity;
using Infrastructure.Data.Library;
using Infrastructure.Data.ViewModel;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork
    {
        private readonly EntityDataContext _context;
        private readonly JwtSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnitOfWork(EntityDataContext entities, JwtSettings appSettings)
        {
            _context = entities;
            _appSettings = appSettings;
        }

        // #region Permission
        // private AuthRepository _repoAuth;
        // public AuthRepository AuthRepository
        // {
        //     get
        //     {
        //         if (_repoAuth == null)
        //         {
        //             _repoAuth = new AuthRepository(_context);
        //         }
        //         return _repoAuth;
        //     }
        // }
        // #endregion

        // #region Account
        // private AccountRepository _accountRepository;
        // public AccountRepository AccountRepository
        // {
        //     get
        //     {
        //         if (_accountRepository == null)
        //         {
        //             _accountRepository = new AccountRepository(_context);
        //         }
        //         return _accountRepository;
        //     }
        // }
        // #endregion
        private UserService _userService;
        public UserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userService = new UserService(_userManager, _appSettings );
                }
                return _userService;
            }
        }
        #region Work

        #endregion
        #region Sync Data

        #endregion

        #region RepositoryLibrary
        private RepositoryLibrary _repositoryLibrary;
        public RepositoryLibrary RepositoryLibrary
        {
            get
            {
                if (_repositoryLibrary == null)
                {
                    _repositoryLibrary = new RepositoryLibrary();
                }

                return _repositoryLibrary;
            }
        }
        #endregion



        #region Warehouse

        #endregion

        #region Libary
        //private UtilitiesRepository _utilitiesRepository;
        //public UtilitiesRepository UtilitiesRepository
        //{
        //    get
        //    {
        //        if (_utilitiesRepository == null)
        //        {
        //            _utilitiesRepository = new UtilitiesRepository();
        //        }
        //        return _utilitiesRepository;
        //    }
        //}
        #endregion



        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
