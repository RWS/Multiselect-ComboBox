using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sdl.MultiSelectComboBox.API
{
    public interface ISuggestionProvider
    {
        Task<IList<object>> GetSuggestionsAsync(string criteria, CancellationToken cancellationToken);
        bool HasMoreSuggestions { get; }
        Task<IList<object>> GetSuggestionsAsync(CancellationToken cancellationToken);
    }
}
