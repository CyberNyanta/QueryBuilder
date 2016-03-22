using QueryBuilder.DAL.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class QueryService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public QueryService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}