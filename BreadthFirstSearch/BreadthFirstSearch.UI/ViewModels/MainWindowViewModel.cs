using BreadthFirstSearch.Model;
using BreadthFirstSearch.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace BreadthFirstSearch.UI.ViewModels
{
    class MainWindowViewModel :  DependencyObject, INotifyPropertyChanged
    {
        private IList<SearchResult> resultList;
        public MainWindowViewModel()
        {
            resultList = new List<SearchResult>();
            SearchResult = CollectionViewSource.GetDefaultView(resultList);
        }

        private string _url;
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyRaised(nameof(URL));
            }
        }

        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyRaised(nameof(SearchString));
            }
        }

        private ushort _threadsNumber;
        public ushort ThreadsNumber
        {
            get { return _threadsNumber; }
            set
            {
                _threadsNumber = value;
                OnPropertyRaised(nameof(ThreadsNumber));
            }
        }

        private ushort _depth;
        public ushort Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                OnPropertyRaised(nameof(Depth));
            }
        }

        private ICollectionView _searchResult;
        public ICollectionView SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                OnPropertyRaised(nameof(SearchResult));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
#if DEBUG
                Debug.WriteLine(this.GetType().GetProperty(propertyname).GetValue(this));
#endif
            }
        }

        public ICommand StartCommand
        {
            get { return new CommandExecutor(SendData); }
        }

        public ICommand PauseCommand
        {
            get { return new CommandExecutor(PauseProcess); }
        }

        private void SendData()
        {
            SearchResult.Refresh();
            if (!CheckURL())
                return;

#if DEBUG
            Debug.WriteLine("Start!");
#endif
            OnResultFound(new SearchResult { URL = _url, Amount = 64});
        }

        private bool CheckURL()
        {
            Uri uriResult;
            bool result = Uri.TryCreate(_url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void PauseProcess()
        {
#if DEBUG
            Debug.WriteLine("Pause!");
#endif

        }

        private void OnResultFound(SearchResult result)
        {
            Dispatcher.BeginInvoke(new Action(() => 
            {
                resultList.Add(result);
                SearchResult.Refresh();
            }));
          
        }
    }
}
