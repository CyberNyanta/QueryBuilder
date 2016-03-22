using System.Collections.Generic;
using QueryBuilder.DAL.Contracts;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.Services.DbServices
{
    public class UserService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IEnumerable<User> GetUsers()
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Users.GetMany(p => p.Delflag == 0);
            }
        }
    }
}