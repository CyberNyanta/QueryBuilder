using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Timers;
using QueryBuilder.Utils.Extentions;
using QueryBuilder.Constants.DbConstants;

namespace QueryBuilderMVC.Models
{
	public class QueryViewModel
	{
        public IEnumerable<QueryListViewModel> Queries { get; set; }

		[Required(ErrorMessage = @"Please enter query name")]
		public string QueryName { get; set; }

		public int QueryID { get; set; }

		public int UserID { get; set; }

		public int ProjectID { get; set; }
		[Required(ErrorMessage = @"Your query is empty")]
		public string QueryBody { get; set; }
		[Required]
		public DateTime QueryDate { get; set; }



	}


}