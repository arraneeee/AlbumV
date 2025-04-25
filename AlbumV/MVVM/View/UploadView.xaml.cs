using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;


namespace AlbumV.MVVM.View
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : UserControl
    {
        public ObservableCollection<Albums> Albums { get; set; }
        public UploadView()
        {
            Albums = new ObservableCollection<Albums>();
            InitializeComponent();
        }

        private void uploadClick(object sender, RoutedEventArgs e)
        {
            var albumData = new Albums
            {
                Name = albumInput.Text,
                Artist = artistInput.Text,
                Rating = ratingInput.Text,
                FilePath = imageInput.Text
            };
            Albums.Add(albumData);
            SaveAlbumsToJson();

            // clear input fields
            albumInput.Clear();
            artistInput.Clear();
            ratingInput.Clear();
            imageInput.Clear();

        }
        private void SaveAlbumsToJson()
        {
            string jsonFilePath = @"..\..\JSON\albumData.json";
            ObservableCollection<Albums> existingAlbums = new ObservableCollection<Albums>();

            if (File.Exists(jsonFilePath))
            {
                string existingJson = File.ReadAllText(jsonFilePath);

                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    existingAlbums = JsonConvert.DeserializeObject<ObservableCollection<Albums>>(existingJson);
                }
            }

            var albumData = new Albums
            {
                Name = albumInput.Text,
                Artist = artistInput.Text,
                Rating = ratingInput.Text,
                FilePath = imageInput.Text
            };

            existingAlbums.Add(albumData);

            string updatedJson = JsonConvert.SerializeObject(existingAlbums, Formatting.Indented);

            File.WriteAllText(jsonFilePath, updatedJson);
        }
    }
}
