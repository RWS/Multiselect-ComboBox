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
        IList<object> GetMissingItems(string criteria, CancellationToken cancellationToken);
        bool MoreDataAvailable { get; }
        IList<object> GetMissingItems(CancellationToken cancellationToken);
    }
}
