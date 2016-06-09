using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Elusive.Solodex.Data
{
    [Export(typeof(IDatabaseManagementService))]
    [Export(typeof(IStartupAction))]
    public class DatabaseManagementService : IDatabaseManagementService, IStartupAction
    {
        /// <summary>
        /// Default database name
        /// </summary>
        private const string DatabaseName = "Solodex";

        /// <summary>
        /// prevent multiple initializations of static object
        /// </summary>
        private static readonly object DatabaseInitializationLock = new object();
        
        /// <summary>
        /// Indicates whether database is initialized
        /// </summary>
        private static bool _databaseInitialized;
        
        /// <summary>
        /// Startup priority
        /// </summary>
        public StartupPriorityEnum Priority
        {
            get
            {
                return StartupPriorityEnum.DatabaseManagement;
            }
        }
        
        /// <summary>
        /// Startup action(s) to perform
        /// </summary>
        public void ProcessStartupAction()
        {
            InitializeDatabase();
        }

        /// <summary>
        /// Confirms the database is in a valid state.
        /// Constructs the database (if it doesn't exist) and upgrades the schema to the current version
        /// </summary>
        /// <returns>True if the database was initialized correctly, otherwise throws exception</returns>
        internal bool InitializeDatabase()
        {
            if (!_databaseInitialized)
            {

                lock (DatabaseInitializationLock)
                {
                    if (!_databaseInitialized)
                    {
                        Initialize();
                        _databaseInitialized = true;
                    }
                }
            }
            return _databaseInitialized;
        }

        /// <summary>
        /// Constructs a SqlConnection
        /// </summary>
        /// <returns>The connection</returns>
        private ServerConnection CreateNoDefaultCatalogDbConnection()
        {
            return new ServerConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["NoDefaultCatalog"].ConnectionString
            };
        }

        /// <summary>
        /// Extracts an embedded file out of a given assembly.
        /// </summary>
        /// <returns>A stream containing the script data.</returns>
        private Stream GetSqlScriptEmbeddedFile()
        {
            var assem = Assembly.GetExecutingAssembly();
            var scriptName = assem.GetManifestResourceNames().Single(n => n.Contains("Schema.sql"));
            var resourceStream = assem.GetManifestResourceStream(scriptName);

            if (resourceStream == null)
            {
                throw new Exception("Could not locate the embedded SQL script resource.");
            }

            return resourceStream;
        }

        /// <summary>Initializes / updates the database</summary>
        private void Initialize()
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                // Deserialize the embedded sql script into a Stream
                using (var sqlScript = GetSqlScriptEmbeddedFile())
                {
                    if (sqlScript == null) return;

                    using (var reader = new StreamReader(sqlScript))
                    {
                        var tsql = reader.ReadToEnd();
                        var cn = CreateNoDefaultCatalogDbConnection();
                        cn.Connect();
                        var server = new Server(cn); 
                        var masterDb = server.Databases["master"];
                        if (masterDb != null)
                        {
                            masterDb.ExecuteNonQuery(tsql);
                        }
                        cn.Disconnect();
                        reader.Close();
                    }

                    sqlScript.Close();
                }
            }
        }
    }
}
