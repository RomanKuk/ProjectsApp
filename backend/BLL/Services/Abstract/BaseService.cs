using AutoMapper;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Abstract
{
    public abstract class BaseService
    {
        private protected readonly IUnitOfWork unitOfWork;
        private protected readonly IMapper mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
    }
}
