using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlbumV.Core;

namespace AlbumV.MVVM.ViewModel
{
    class MainViewModel : ObservableObject 
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand UploadViewCommand { get; set; }


        public HomeViewModel HomeVM { get; set; }
        public UploadViewModel UploadVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() 
        { 
            HomeVM = new HomeViewModel();
            UploadVM = new UploadViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            { 
                CurrentView = HomeVM;
            });

            UploadViewCommand = new RelayCommand(o =>
            {
                CurrentView = UploadVM;
            });
        }
    }
}
