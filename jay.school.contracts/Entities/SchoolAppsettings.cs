using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class SchoolAppsettings
    {
        public MangoDbConnection MangoDbConnection { get; set; } 
    }

    public class MangoDbConnection
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
