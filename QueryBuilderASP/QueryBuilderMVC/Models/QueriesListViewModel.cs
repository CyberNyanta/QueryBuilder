using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderMVC.Models
{
	public class QueriesListViewModel
	{
		public int QueryID { get; set; }

		public string QueryName { get; set; }

		public int QueryOwner { get; set; }

		public int ProjectID { get; set; }

		public string QueryBody { get; set; }

		public DateTime QueryDate { get; set; }


	}
}
