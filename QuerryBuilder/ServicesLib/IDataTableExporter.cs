using System.Data;

namespace ServicesLib
{
    public interface IDataTableExporter
    {
        bool IncludeTitle { get; set; }

        void DataTableExport(DataTable dataTable, string filePath, string title);
    }
}