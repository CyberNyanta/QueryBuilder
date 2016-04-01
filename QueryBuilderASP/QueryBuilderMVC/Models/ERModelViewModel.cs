using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using QueryBuilder.Utils.DBSchema;

namespace QueryBuilderMVC.Models
{
	public class ERModelViewModel
	{

		public ERModelViewModel(SqlConnection conectionString)
		{
			ERModel = JsonERModel.GetERModel(conectionString);
		}

		public string ERModel
		{
			get; set;
		}
	}

}
