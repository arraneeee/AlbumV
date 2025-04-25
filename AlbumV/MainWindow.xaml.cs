using System.IO;
using System.Windows;
using System.Text.Json;
using System.Collections.ObjectModel;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlbumV
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Albums> Albums { get; set; }
        private MediaPlayer mediaPlayer;
        private Albums selectedAlbum;
        
        public MainWindow()
        {
            Albums = new ObservableCollection<Albums>();
            LoadJsonData();
            DataContext = this;
            InitializeComponent();
            
            mediaPlayer = new MediaPlayer();
        }
        public void SetSelectedAlbum(Albums album)
        {
            selectedAlbum = album;
            
            if (selectedAlbum != null && 
                !string.IsNullOrEmpty(selectedAlbum.AudioFilePath) && 
                File.Exists(selectedAlbum.AudioFilePath))
            {
                playButton.IsEnabled = true;
                stopButton.IsEnabled = true;
            }
            else
            {
                playButton.IsEnabled = false;
                stopButton.IsEnabled = false;
            }
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAlbum != null && 
                !string.IsNullOrEmpty(selectedAlbum.AudioFilePath) && 
                File.Exists(selectedAlbum.AudioFilePath))
            {
                try
                {
                    mediaPlayer.Open(new Uri(selectedAlbum.AudioFilePath));
                    mediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing audio: {ex.Message}", "Playback Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void LoadJsonData()
        {
            string jsonFilePath = @"..\..\JSON\albumData.json";
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        Albums = JsonSerializer.Deserialize<ObservableCollection<Albums>>(json);
                    }
                    catch
                    {
                        Albums = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Albums>>(json);
                    }
                }

                Console.WriteLine($"Loaded {Albums?.Count} albums.");
            }
            else
            {
                Console.WriteLine($"JSON file not found at: {jsonFilePath}");
            }
        }
    }
}