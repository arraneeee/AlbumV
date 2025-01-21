using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using AlbumV.Models;

namespace AlbumV.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public ObservableCollection<Album> Albums { get; set; }

        public HomeViewModel()
        {
            LoadAlbums();
        }

        private void LoadAlbums()
        {
            var json = File.ReadAllText("d:/GitHub/[Remi]/AlbumV/AlbumV/JSON/albumData.json");
            var albums = JsonConvert.DeserializeObject<List<Album>>(json);
            Albums = new ObservableCollection<Album>(albums);
        }
    }
}