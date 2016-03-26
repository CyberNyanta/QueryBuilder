﻿using QueryBuilder.DAL.Contracts;
using QueryBuilder.Services.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class QueryService: IQueryService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public QueryService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}