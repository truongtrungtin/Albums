﻿
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.ViewModel;
using AutoMapper;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data.Models
{
    public abstract class BaseController : ControllerBase
    {
        public UserManager<ApplicationUser> _userManager;
        public EntityDataContext _context;
        public UnitOfWork _unitOfWork;
        public IMapper _mapper;
        public IHostingEnvironment _hostingEnvironment;
        //public List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();
        protected BaseController()
        {
            this._context = new EntityDataContext();
            this._unitOfWork = new UnitOfWork(_context, new JwtSettings());

        }

    }
}