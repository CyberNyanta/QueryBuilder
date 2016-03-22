using QueryBuilder.DAL.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class ConnectionDbService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ConnectionDbService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}