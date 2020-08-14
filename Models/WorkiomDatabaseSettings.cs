using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkiomBackendTest.Models
{
    public class WorkiomDatabaseSettings : IWorkiomDatabaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IWorkiomDatabaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
