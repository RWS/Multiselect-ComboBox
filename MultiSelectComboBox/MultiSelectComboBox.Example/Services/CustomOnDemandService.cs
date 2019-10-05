using Sdl.MultiSelectComboBox.API;
using Sdl.MultiSelectComboBox.Example.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sdl.MultiSelectComboBox.Example.Services
{
    public class CustomOnDemandService : IOnDemandService
    {
        private const int batchSize = 30;
        private string _criteria = string.Empty;
        private int _skipCount;

        private readonly ObservableCollection<LanguageItem> _observableCollection;
        private readonly List<LanguageItem> _source;

        public CustomOnDemandService(ObservableCollection<LanguageItem> observableCollection, List<LanguageItem> source)
        {
            _observableCollection = observableCollection;
            _source = source;
        }

        public bool MoreDataAvailable { get; private set; } = true;

        public Task<IList<object>> GetMissingItemsAsync(string criteria, CancellationToken cancellationToken)
        {
            _criteria = criteria;
			var newItems = _source.Where(x => x.Name.IndexOf(_criteria, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
			if (cancellationToken.IsCancellationRequested)
                return null;
            MoreDataAvailable = newItems.Count > batchSize;
            _skipCount = batchSize;
            return Task.Run(() => (IList<object>)newItems.Take(batchSize).Cast<object>().ToList());
        }

        public Task<IList<object>> GetMissingItemsAsync(CancellationToken cancellationToken)
        {
            var newItems = _source.Where(x => x.Name.StartsWith(_criteria)).Skip(_skipCount).ToList();
            if (cancellationToken.IsCancellationRequested)
                return null;
            MoreDataAvailable = newItems.Count > batchSize;
            _skipCount += batchSize;
            return Task.Run(() => (IList<object>)newItems.Take(batchSize).Where(x => !_observableCollection.Any(y => y.Id == x.Id)).Cast<object>().ToList());
        }
    }
}
