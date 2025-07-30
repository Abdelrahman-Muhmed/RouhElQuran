using AutoMapper;
using Core.IUnitOfWork;
using Microsoft.Extensions.Logging;

namespace Service.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _UnitOfWork;

        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
    }

}

