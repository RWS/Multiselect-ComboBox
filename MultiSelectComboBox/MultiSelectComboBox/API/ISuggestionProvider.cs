using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sdl.MultiSelectComboBox.API
{
    public interface ISuggestionProvider
    {
        Task<IList<object>> GetSuggestions(string criteria, CancellationToken cancellationToken);
        bool HasMoreSuggestions { get; }
        Task<IList<object>> GetSuggestions(CancellationToken cancellationToken);
    }
}
