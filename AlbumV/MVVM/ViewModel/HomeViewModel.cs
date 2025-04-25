using AlbumV.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlbumV.MVVM.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<AlbumModel> _albums;
        private MediaElement _currentlyPlaying;

        public ObservableCollection<AlbumModel> Albums 
        {
            get { return _albums; }
            set
            {
                _albums = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayAudioCommand { get; }

        public HomeViewModel()
        {
            Albums = new ObservableCollection<AlbumModel>();
            // Initialize albums here
            
            PlayAudioCommand = new RelayCommand(PlayAudio);
        }

        private void PlayAudio(object parameter)
        {
            if (parameter is AlbumModel album && !string.IsNullOrEmpty(album.AudioClipPath))
            {
                // Find the MediaElement associated with the album
                ItemsControl itemsControl = FindVisualParent<ItemsControl>(Application.Current.MainWindow);
                if (itemsControl != null)
                {
                    // Stop any currently playing audio
                    if (_currentlyPlaying != null)
                    {
                        _currentlyPlaying.Stop();
                    }

                    // Get the container for this item
                    var container = itemsControl.ItemContainerGenerator.ContainerFromItem(album) as FrameworkElement;
                    if (container != null)
                    {
                        // Find the MediaElement within this container
                        MediaElement audioPlayer = FindChild<MediaElement>(container, "AudioPlayer");
                        if (audioPlayer != null)
                        {
                            audioPlayer.Play();
                            _currentlyPlaying = audioPlayer;
                        }
                    }
                }
            }
        }

        // Helper methods to find elements in the visual tree
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            return parent ?? FindVisualParent<T>(parentObject);
        }

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        foundChild = childType;
                        break;
                    }
                }
                else
                {
                    foundChild = childType;
                    break;
                }
            }
            return foundChild;
        }
    }

    // Add this if you don't already have a RelayCommand class
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    // Add this if you don't already have a ViewModelBase class
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}