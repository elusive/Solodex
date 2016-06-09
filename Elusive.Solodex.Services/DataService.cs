using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Data;
using Prism.Events;

namespace Elusive.Solodex.Services
{
    /// <summary>
    /// The application data service
    /// </summary>
    [Export(typeof(IDataService))]
    public class DataService : IDataService
    {
        private readonly IEventAggregator _eventAggregator;

        /// <summary>Initializes a new instance of the <see cref="DataService"/> class.</summary>
        [ImportingConstructor]
        public DataService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>Gets a new repository instance</summary>
        public IRepository Repository
        {
            get
            {
                return new Repository(_eventAggregator);
            }
        }
    }
}
