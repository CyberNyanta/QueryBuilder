using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel
{
    static class MainWindowData
    {
        public static string SqlQuerry { get; set; }
        public static string StringConnect { get; set; }
        public static string ProjectName { get; set; }
        public static string DescriptionProject { get; set; }
        public static string ProjectOwner { get; set; }

        public static Users CurrentUser { get; set; }
    }
}
