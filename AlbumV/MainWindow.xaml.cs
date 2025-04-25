using System.IO;
using System.Windows;
using System.Text.Json;
using System.Collections.ObjectModel;
using System;

namespace AlbumV
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Albums> Albums { get; set; }
        public MainWindow()
        {
            Albums = new ObservableCollection<Albums>();
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
                Albums = JsonSerializer.Deserialize<ObservableCollection<Albums>>(json);

                // Debugging output
                Console.WriteLine($"Loaded {Albums?.Count} albums.");
                foreach (var album in Albums)
                {
                    Console.WriteLine($"Name: {album.Name}, Artist: {album.Artist}, Rating: {album.Rating}, FilePath: {album.FilePath}");
                }
            }
            else
            {
                Console.WriteLine($"JSON file not found at: {jsonFilePath}");
            }
        }
    }
}