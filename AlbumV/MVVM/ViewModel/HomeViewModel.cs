using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace AlbumV.MVVM.ViewModel
{
    public class HomeViewModel
    {
        public ObservableCollection<Albums> Albums { get; set; }

        public HomeViewModel()
        {
            LoadAlbums();
        }

        private void LoadAlbums()
        {
            var json = File.ReadAllText("A:\\Projects\\AlbumV\\AlbumV\\JSON\\albumData.json");
            Albums = JsonConvert.DeserializeObject<ObservableCollection<Albums>>(json);

            // Debugging: Print out the loaded albums
            foreach (var album in Albums)
            {
                System.Diagnostics.Debug.WriteLine($"Name: {album.Name}, Artist: {album.Artist}, Rating: {album.Rating}, FilePath: {album.FilePath}");
            }
        }
    }
}