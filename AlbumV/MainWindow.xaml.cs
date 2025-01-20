using System.IO;
using System.Windows;
using System.Text.Json;
using System.Collections.ObjectModel;
using System;

namespace AlbumV
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Albums> Album { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            LoadJsonData();
            DataContext = this;
        }

        private void LoadJsonData()
        {
            string jsonFilePath = "A:\\Projects\\AlbumV\\AlbumV\\JSON\\albumData.json";
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                Album = JsonSerializer.Deserialize<ObservableCollection<Albums>>(json);

                // Debugging output
                Console.WriteLine($"Loaded {Album?.Count} albums.");
            }
            else
            {
                Console.WriteLine($"JSON file not found at: {jsonFilePath}");
            }
        }
    }
}
