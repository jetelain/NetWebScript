using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace NetWebScript.Debug.App.ViewModel
{
    /// 
    /// Provides a cross-thread freindly collection
    /// 
    public class DispatcherObservableCollection<T> : ObservableCollection<T>
    {
        private readonly Dispatcher _dispatcher;


        public DispatcherObservableCollection(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void AddAsync(T item)
        {
            BeginInvoke(() => Add(item));
        }

        public void RemoveAsync(T item)
        {
            BeginInvoke(() => Remove(item));
        }

        private void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.DataBind, action);
        }
    }

}
