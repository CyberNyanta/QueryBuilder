﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilderMVC.Models
{
	public class QueryHistoryListViewModel
	{
		public int QueryHistoryID { get; set; }

		public int UserID { get; set; }

		public int ProjectID { get; set; }

		public string QueryBody { get; set; }

		public DateTime QueryDate { get; set; }


	}
}
