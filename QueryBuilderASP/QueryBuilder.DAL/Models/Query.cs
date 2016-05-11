using System;
using System.Collections.Generic;

namespace QueryBuilder.DAL.Models
{
#pragma warning disable 0436

    public class Query
    {
        public int QueryID { get; set; }

        public string QueryName { get; set; }

        public int ProjectID { get; set; }

        public string QueryBody { get; set; }

		public virtual ICollection<QueryHistory> QueriesHistory { get; set; }

		public int Delflag { get; set; }

        public virtual Project Project { get; set; }

		//public DateTime QueryDate { get; set; }

		//public byte[] QueryResult { get; set; }

		// public int QueryOwner { get; set; }
	}
}
