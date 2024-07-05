using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FamilieLaissSharedUI.Helper
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        private bool _preventNotification = false;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_preventNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T>? list)
        {
            if (list is not null)
            {
                _preventNotification = true;
                foreach (T item in list)
                    Add(item);
                _preventNotification = false;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Replace(T newItem, Func<T, bool> predicate)
        {
            _preventNotification = true;
            Remove(Items.First(predicate));
            Add(newItem);
            _preventNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
