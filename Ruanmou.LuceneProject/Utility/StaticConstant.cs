using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Ruanmou.LuceneProject.Utility
{
    public class StaticConstant
    {
        public static string IndexPath = ConfigurationManager.AppSettings["IndexPath"];

        public static string TestIndexPath = ConfigurationManager.AppSettings["TestIndexPath"];
    }
}
