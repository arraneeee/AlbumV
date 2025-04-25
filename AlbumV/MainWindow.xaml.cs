using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using System;
using System.Media;
using Newtonsoft.Json;

namespace AlbumV
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Albums> Albums { get; set; }
        private SoundPlayer soundPlayer;
        
        public MainWindow()
        {
            Albums = new ObservableCollection<Albums>();
            soundPlayer = new SoundPlayer();
            LoadJsonData();
            DataContext = this;
            InitializeComponent();
        }

        private void LoadJsonData()
        {
            string jsonFilePath = @"..\..\JSON\albumData.json";
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                
                if (!string.IsNullOrWhiteSpace(json))
                {
                    Albums = JsonConvert.DeserializeObject<ObservableCollection<Albums>>(json);

                    Console.WriteLine($"Loaded {Albums?.Count} albums.");
                    foreach (var album in Albums)
                    {
                        Console.WriteLine($"Name: {album.Name}, Artist: {album.Artist}, Rating: {album.Rating}, FilePath: {album.FilePath}, AudioPath: {album.AudioPath}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"JSON file not found at: {jsonFilePath}");
                // Initialize empty collection if file doesn't exist
                Albums = new ObservableCollection<Albums>();
            }
        }
        
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button?.DataContext is Albums album && !string.IsNullOrEmpty(album.AudioPath) && File.Exists(album.AudioPath))
            {
                try
                {
                    if (album.AudioPath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {
                        soundPlayer.SoundLocation = album.AudioPath;
                        soundPlayer.Play();
                    }
                    else
                    {
                        MessageBox.Show("Only WAV files are supported for direct playback.",
                                      "Format Not Supported",
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
            else
            {
                MessageBox.Show("No audio file available for this album.",
                               "No Audio",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
            }
        }
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            soundPlayer.Stop();
        }
    }
}