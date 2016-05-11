using System;

namespace QueryBuilder.DAL.Models
{
	public class QueryHistory
	{
#pragma warning disable 0436
		public int QueryID { get; set; }

		public string QueryHistoryID { get; set; }

		public int UserID { get; set; }
		public DateTime QueryDate { get; set; }

		public string QueryBody { get; set; }

		public int Delflag { get; set; }

		public virtual Query Query { get; set; }

	}
}
