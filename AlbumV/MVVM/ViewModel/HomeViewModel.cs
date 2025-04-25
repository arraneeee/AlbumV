using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using AlbumV.Core;

namespace AlbumV.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
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
            LoadAlbums();
        }

        private void LoadAlbums()
        {
            var json = File.ReadAllText(@"..\..\JSON\albumData.json");
            _allAlbums = JsonConvert.DeserializeObject<ObservableCollection<Albums>>(json);
            Albums = new ObservableCollection<Albums>(_allAlbums);

            if (_allAlbums == null || _allAlbums.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No albums found.");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Loaded {_allAlbums.Count} albums.");
            foreach (var album in _allAlbums)
            {
                System.Diagnostics.Debug.WriteLine($"Name: {album.Name}, Artist: {album.Artist}, Rating: {album.Rating}, FilePath: {album.FilePath}");
            }
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
                    (a.Name != null && a.Name.ToLower().Contains(SearchText.ToLower())) || 
                    (a.Artist != null && a.Artist.ToLower().Contains(SearchText.ToLower())))
                    .ToList();
                Albums = new ObservableCollection<Albums>(filtered);
            }
        }
    }
}