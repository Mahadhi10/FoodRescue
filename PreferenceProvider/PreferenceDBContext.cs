using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace helloworld_dotnetcore5.PreferenceProvider
{
    public class PreferenceDBContext 
    {
        public static string _connectionString { get; set; }
        public PreferenceDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
