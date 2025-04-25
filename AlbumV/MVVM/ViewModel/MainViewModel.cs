using AlbumV.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace AlbumV.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private ObservableCollection<Albums> _allAlbums;
        private ObservableCollection<Albums> _albums;
        private string _searchText;

        public ObservableCollection<Albums> Albums
        {
            get { return _albums; }
            set 
            { 
                _albums = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterAlbums();
            }
        }

        public HomeViewModel()
        {
            _allAlbums = new ObservableCollection<Albums>();

            Albums = new ObservableCollection<Albums>(_allAlbums);
        }

        private void FilterAlbums()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                Albums = new ObservableCollection<Albums>(_allAlbums);
            }
            else
            {
                var filtered = _allAlbums.Where(a => 
                    a.Name.ToLower().Contains(SearchText.ToLower()) || 
                    a.Artist.ToLower().Contains(SearchText.ToLower())).ToList();
                Albums = new ObservableCollection<Albums>(filtered);
            }
        }
    }
}