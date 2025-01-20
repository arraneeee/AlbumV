using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AlbumV.Core;
using System.IO;
using System.Text.Json;

namespace AlbumV.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private ObservableCollection<Albums> _album;
        public ObservableCollection<Albums> Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            LoadJsonData();
        }

        private void LoadJsonData()
        {
            string jsonFilePath = "A:\\Projects\\AlbumV\\AlbumV\\JSON\\albumData.json";
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                Album = JsonSerializer.Deserialize<ObservableCollection<Albums>>(json);
            }
        }
    }
}
