using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sdl.MultiSelectComboBox.API
{
    public interface IOnDemandService
    {
        Task<IList<object>> GetMissingItemsAsync(string criteria, CancellationToken cancellationToken);
        bool MoreDataAvailable { get; }
        Task<IList<object>> GetMissingItemsAsync(CancellationToken cancellationToken);
    }
}
