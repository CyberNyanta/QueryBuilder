using QueryBuilder.DAL.Contracts;
using QueryBuilder.Services.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class ConnectionDbService: IConnectionDbService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ConnectionDbService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}