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
		private readonly SqlConnection conectionString;
		public ERModelViewModel(SqlConnection conectionString)
		{
			this.conectionString = conectionString;
			JsonERModel.GetERModel(conectionString);
		}
	}

}
