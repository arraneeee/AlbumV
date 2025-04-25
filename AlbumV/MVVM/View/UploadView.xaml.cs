using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Media;
using System;

namespace AlbumV.MVVM.View
{
    public partial class UploadView : UserControl
    {
        public ObservableCollection<Albums> Albums { get; set; }
        private SoundPlayer soundPlayer;

        public UploadView()
        {
            Albums = new ObservableCollection<Albums>();
            InitializeComponent();
            soundPlayer = new SoundPlayer();
        }

        private void BrowseAudioClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.wav;*.mp3)|*.wav;*.mp3|All files (*.*)|*.*",
                Title = "Select an audio file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Check file size - limit to reasonable size for 30 second clip
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                const long maxSizeBytes = 5 * 1024 * 1024; // 5MB limit
                
                if (fileInfo.Length > maxSizeBytes)
                {
                    MessageBox.Show("Audio file is too large. Please select a smaller file (max 5MB).", 
                                    "File Size Error", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                    return;
                }
                
                audioInput.Text = openFileDialog.FileName;
            }
        }

        private void TestPlayClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(audioInput.Text) || !File.Exists(audioInput.Text))
            {
                MessageBox.Show("Please select a valid audio file first.", 
                               "No Audio File", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }

            try
            {
                // For .wav files we can use SoundPlayer
                if (audioInput.Text.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    soundPlayer.SoundLocation = audioInput.Text;
                    soundPlayer.Play();
                }
                else
                {
                    // For other formats, we'd need a media player
                    MessageBox.Show("Only .wav files can be previewed directly. For MP3 files, use a MediaElement control.",
                                   "Format Limitation",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing audio: {ex.Message}", 
                               "Playback Error", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
            }
        }

        private void uploadClick(object sender, RoutedEventArgs e)
        {
            var albumData = new Albums
            {
                Name = albumInput.Text,
                Artist = artistInput.Text,
                Rating = ratingInput.Text,
                FilePath = imageInput.Text,
                AudioPath = audioInput.Text
            };
            Albums.Add(albumData);
            SaveAlbumsToJson();

            // clear input fields
            albumInput.Clear();
            artistInput.Clear();
            ratingInput.Clear();
            imageInput.Clear();
            audioInput.Clear();
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
                FilePath = imageInput.Text,
                AudioPath = audioInput.Text
            };

            existingAlbums.Add(albumData);

            string updatedJson = JsonConvert.SerializeObject(existingAlbums, Formatting.Indented);

            File.WriteAllText(jsonFilePath, updatedJson);
        }
    }
}