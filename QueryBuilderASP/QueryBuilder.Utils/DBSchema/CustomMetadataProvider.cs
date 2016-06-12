using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System;
using System.IO;

namespace QueryBuilder.Utils.DBSchema
{
	 public abstract class CustomMetadataProvider
	{
		const string TABLE = "BASE TABLE";
		const string VIEW = "VIEW";
		const string LINK = "LINK";
		const string TABLE_NAME = "TABLE_NAME";
		const string TABLE_TYPE = "TABLE_TYPE";
		const string TABLE_SCHEMA = "TABLE_SCHEMA";
		const string PROCEDURE_PARAMETERS = "PROCEDURE_PARAMETERS";

		const string querry = "Select kcup.Table_name, kcuc.TABLE_NAME " +
								  "From INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc " +
								  "Left Join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcup On " +
								  "rc.UNIQUE_CONSTRAINT_NAME = kcup.CONSTRAINT_NAME " +
								  "LEFT Join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcuc on " +
								  "rc.CONSTRAINT_NAME = kcuc.CONSTRAINT_NAME and kcup.ORDINAL_POSITION = kcuc.ORDINAL_POSITION";

		static Dictionary<string, string[]> datatypesConditions = new Dictionary<string, string[]>
		{
			//type_name = type size precision scale /// nullable 
			["bit"] = new string[]{ "Boolean", "1", "255", "255"},
			["bigint"] = new string[]{ "Int64", "8", "255", "255"},
			["binary"] = new string[]{ "Binary", "255", "255", "255"},
			["char"] = new string[]{ "String", "255", "255", "255" },
			["date"] = new string[]{ "DateTime", "255", "255", "255"},
			["datetime"] = new string[]{ "DateTime", "8", "23", "3" },
			["decimal"] = new string[]{ "Decimal", "8", "255", "255"},
			["float"] = new string[]{ "Double", "8", "255", "255"},
			["image"] = new string[]{ "Binary", "2147483647", "255", "255"},
			["int"] = new string[]{ "Int32", "4", "10", "255"},
			["money"] = new string[]{ "Decimal", "8", "19", "255"},
			["nchar"] = new string[]{ "String", "255", "255", "255" },
			["ntext"] = new string[]{ "String", "255", "255", "255" },
			["numeric"] = new string[]{ "Decimal", "8", "255", "255"},
			["nvarchar"] = new string[]{ "String", "255", "255", "255" },
			["real"] = new string[]{ "Single", "4", "24", "255" },
			["rowversion"] = new string[]{ "Binary", "255", "255", "255"},
			["smalldatetime"] = new string[]{ "DateTime", "8", "255", "255"},
			["smallint"] = new string[]{ "Int16", "2", "5", "255"},
			["smallmoney"] = new string[]{ "Decimal", "8", "255", "255"},
			["sql_variant"] = new string[]{ "Object", "255", "255", "255"},
			["text"] = new string[]{ "String", "255", "255", "255" },
			["time"] = new string[]{ "TimeSpan", "255", "255", "255"},
			["timestamp"] = new string[]{ "Binary", "255", "255", "255"},
			["tinyint"] = new string[]{ "Byte", "255", "255", "255"},
			["uniqueidentifier"] = new string[]{ "Guid", "255", "255", "255"},
			["varbinary"] = new string[]{ "Binary", "255", "255", "255"},
			["varchar"] = new string[]{ "String", "255", "255", "255" },
			["xml"] = new string[]{ "Xml", "255", "255", "255"}
		};


		public static Stream GetStream(SqlConnection[] connections)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(GetMetadata(connections));
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
		public static string GetMetadata(SqlConnection[] connections)
		{
			XNamespace xsiNs = "http://www.activequerybuilder.com/schemas/metadata2.xsd";
			XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
			var metadata = new XElement("metadata", new XAttribute(XNamespace.Xmlns +"xsi", xsi),new XAttribute(xsi+ "schemaLocation", xsiNs));
			
			foreach(var connection in connections)
			{
				var databaseElement = new XElement("database", new XAttribute("name", connection.Database), new XAttribute("default", "True"));
				metadata.Add(databaseElement);

				connection.Open();
				var dataTables = connection.GetSchema("Tables", new string[4] { null, null, null, TABLE});
				var tablesColumns = connection.GetSchema("Columns");
				//var keys = connection.GetSchema("KEY_COLUMN_USAGE");
				connection.Close();

				foreach(DataRow dr in dataTables.Rows)
				{
					var tableName = (string)dr[TABLE_NAME];
					var tableElement = new XElement("table", new XAttribute("name", tableName));
					databaseElement.Add(tableElement);
					

					foreach (DataRow drColumns in tablesColumns.AsEnumerable().Where(row => row.Field<string>(TABLE_NAME).Equals(tableName)))
					{
						var datatype = drColumns["DATA_TYPE"];
						var name = new XAttribute("name", drColumns["COLUMN_NAME"]);
						var type_name = new XAttribute("type_name", datatype);
						var type= new XAttribute("type", datatypesConditions[(string)datatype][0]);
						var sizetemp = drColumns["CHARACTER_MAXIMUM_LENGTH"].ToString();
						var size = new XAttribute("size",String.IsNullOrWhiteSpace(sizetemp)? datatypesConditions[(string)datatype][1]: sizetemp);
						var precision = new XAttribute("precision", datatypesConditions[(string)datatype][2]);
						var scale = new XAttribute("scale", datatypesConditions[(string)datatype][3]);
						var nullable = new XAttribute("nullable", ((string)drColumns["IS_NULLABLE"]).Equals("YES")? "True" : "False");
						tableElement.Add(new XElement("field", name, type_name, type, size, precision, scale, nullable));

					}

				}

			}
			
			var Result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + metadata.ToString(SaveOptions.DisableFormatting);
			return Result;
			//return "<?xml version=\"1.0\" encoding=\"utf-8\"?><metadata xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.activequerybuilder.com/schemas/metadata2.xsd\">	<database name=\"Northwind\" default=\"True\">		<table name=\"Categories\">	<field name=\"CategoryID\" type_name=\"int\" type=\"Int32\" size=\"8\" precision=\"10\" scale=\"255\" nullable=\"False\" readonly=\"True\" />	<field name=\"CategoryName\" type_name=\"nvarchar\" type=\"String\" size=\"15\" precision=\"255\" scale=\"255\" nullable=\"False\" />	<field name=\"Description\" type_name=\"ntext\" type=\"String\" size=\"1073741823\" precision=\"255\" scale=\"255\" />	<field name=\"Picture\" type_name=\"image\" type=\"Binary\" size=\"2147483647\" precision=\"255\" scale=\"255\" /></table></database></metadata>";
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
