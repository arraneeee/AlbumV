using System.Collections.ObjectModel;
using AlbumV.Models;

namespace AlbumV.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public ObservableCollection<Album> Albums { get; set; }

        public HomeViewModel()
        {
            Albums = new ObservableCollection<Album>
            {
                new Album { Name = "Graduation", Artist = "Kanye West", FilePath = "/Images/graduation.jpg", Rating = 87 },
                new Album { Name = "Some Rap Songs", Artist = "Earl Sweatshirt", FilePath = "/Images/somerapsongs.jpg", Rating = 85 },
                new Album { Name = "Astroworld", Artist = "Travis Scott", FilePath = "/Images/astroworld.jpg", Rating = 83 }
            };
        }
    }
}