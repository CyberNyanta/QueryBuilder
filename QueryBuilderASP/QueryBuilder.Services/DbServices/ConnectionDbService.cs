using System;
using System.Collections.Generic;
using QueryBuilder.Constants;
using QueryBuilder.DAL.Contracts;
using QueryBuilder.DAL.Models;
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

        public IEnumerable<ConnectionDB> GetConnectionDBs()
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.ConnectionDBs.GetMany(p => p.Delflag == DelflagConstants.ActiveSet);
            }
        }
        public IEnumerable<ConnectionDB> GetConnectionDBs(int owner)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.ConnectionDBs.GetMany(p => p.Delflag == DelflagConstants.ActiveSet && p.ConnectionOwner == owner);
            }
        }
        public void SaveConnection(ConnectionDB connectionDb)
        {
            if (connectionDb == null)
            {
                throw new ArgumentNullException(nameof(connectionDb));
            }

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                if (connectionDb.ConnectionID == 0)
                    unitOfWork.ConnectionDBs.Create(connectionDb);
                else
                    unitOfWork.ConnectionDBs.Update(connectionDb);

                unitOfWork.Save();
            }
        }

        public ConnectionDB GetConnectionDb(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.ConnectionDBs.GetById(id);
            }
        }
    }
}