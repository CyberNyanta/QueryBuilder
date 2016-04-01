using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;
using System.Diagnostics;

namespace QueryBuilderMVC.Models
{
	public class ERModelViewModel
	{
		private readonly SqlConnection conectionString;
		public ERModelViewModel(SqlConnection conectionString)
		{
			this.conectionString = conectionString;
			ERModelHelper.GetERModel(conectionString);
		}
	}


	public class ERModelHelper
	{
		const string TABLE = "BASE TABLE";
		const string VIEW = "VIEW";
		const string LINK = "LINK";
		const string TABLE_NAME = "TABLE_NAME";
		const string TABLE_TYPE = "TABLE_TYPE";
		const string TABLE_SCHEMA = "TABLE_SCHEMA";
		const string PROCEDURE_PARAMETERS = "PROCEDURE_PARAMETERS";

		public static string GetERModel(SqlConnection conectionString)
		{
			var shema = new List<dynamic>[2];
			shema[0] = new List<object>();
			shema[1] = new List<object>();
			conectionString.Open();
			var dataTables = conectionString.GetSchema("Tables");
			foreach (DataRow dr in dataTables.Rows)
			{

				var type = (string)dr[TABLE_TYPE];
				if (type != TABLE && type != VIEW && type != LINK)
				{
					continue;
				}

				var name = (string)dr[TABLE_NAME];

				dynamic table = new ExpandoObject();
				table.key = name;
				table.items = new List<object>();

				var datatable = new DataTable(name);
				datatable.ExtendedProperties[TABLE_TYPE] = type;

				try
				{
					var select = GetSelectStatement(datatable);
					var da = new SqlDataAdapter(select, conectionString);
					da.FillSchema(datatable, SchemaType.Mapped);
				}
				catch { }

				foreach (DataColumn column in datatable.Columns)
				{
					table.items.Add(new { name = column.ColumnName });
				}

				shema[0].Add(table);
			}
			string df = JsonConvert.SerializeObject(shema);
			Debug.WriteLine(df);
			return df;
		}

		private static string GetSelectStatement(DataTable table)
		{
			return string.Format("SELECT * from {0}", GetFullTableName(table));
		}
		private static string GetFullTableName(DataTable table)
		{
			var sb = new StringBuilder();

			var schema = table.ExtendedProperties[TABLE_SCHEMA] as string;
			if (schema != null)
			{
				sb.AppendFormat("{0}.", schema);
			}

			sb.Append(BracketName(table.TableName));

			return sb.ToString();
		}

		private static string BracketName(string name)
		{
			if (name.Length > 1 && name[0] == '[' && name[name.Length - 1] == ']')
			{
				return name;
			}
			bool needsBrackets = false;
			if (!IsExpression(name))
			{
				for (int i = 0; i < name.Length && !needsBrackets; i++)
				{
					char c = name[i];
					needsBrackets = i == 0
						? !char.IsLetter(c)
						: !char.IsLetterOrDigit(c) && c != '_';
				}
			}
			return needsBrackets
				? string.Format("[{0}]", name)
				: name;
		}
		private static char[] _expressionChars = "(),*".ToCharArray();
		private static bool IsExpression(string name)
		{
			return name.IndexOfAny(_expressionChars) > -1;
		}
	}
}
