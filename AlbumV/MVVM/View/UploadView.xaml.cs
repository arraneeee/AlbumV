using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Media;
using System;
using System.Diagnostics;

namespace AlbumV.MVVM.View
{
    /// <summary>
    /// Interaction logic for UploadView.xaml
    /// </summary>
    public partial class UploadView : UserControl
    {
        public ObservableCollection<Albums> Albums { get; set; }
        private MediaPlayer mediaPlayer;
        
        public UploadView()
        {
            Albums = new ObservableCollection<Albums>();
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
        }

        private void uploadClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(audioInput.Text) && !ValidateAudioFile(audioInput.Text))
            {
                MessageBox.Show("Please select a valid MP3 file (maximum 30 seconds).", "Invalid Audio File", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var albumData = new Albums
            {
                Name = albumInput.Text,
                Artist = artistInput.Text,
                Rating = ratingInput.Text,
                FilePath = imageInput.Text,
                AudioFilePath = audioInput.Text
            };
            
            Albums.Add(albumData);
            SaveAlbumsToJson();

            albumInput.Clear();
            artistInput.Clear();
            ratingInput.Clear();
            imageInput.Clear();
            audioInput.Clear();
        }
        
        private void BrowseAudioClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            openFileDialog.Title = "Select an MP3 File (max 30 seconds)";
            
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                
                if (ValidateAudioFile(filePath))
                {
                    audioInput.Text = filePath;
                }
                else
                {
                    MessageBox.Show("Please select an MP3 file that is 30 seconds or less.", "File Too Long", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        
        private void TestPlayClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(audioInput.Text) && File.Exists(audioInput.Text))
            {
                try
                {
                    mediaPlayer.Stop();
                    mediaPlayer.Open(new Uri(audioInput.Text));
                    mediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing audio: {ex.Message}", "Playback Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a valid audio file first.", "No Audio File", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool ValidateAudioFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;
                
            try
            {
                if (!filePath.ToLower().EndsWith(".mp3"))
                    return false;
                    
                MediaPlayer tempPlayer = new MediaPlayer();
                tempPlayer.Open(new Uri(filePath));
                
                System.Threading.Thread.Sleep(500);
                
                if (tempPlayer.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan duration = tempPlayer.NaturalDuration.TimeSpan;
                    tempPlayer.Close();
                    
                    return duration.TotalSeconds <= 30;
                }
                
                return true;
            }
            catch
            {
                return false;
            }
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
                AudioFilePath = audioInput.Text
            };

            existingAlbums.Add(albumData);

            string updatedJson = JsonConvert.SerializeObject(existingAlbums, Formatting.Indented);

            File.WriteAllText(jsonFilePath, updatedJson);
        }
    }
}