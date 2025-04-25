using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlbumV.MVVM.Model
{
    public class AlbumModel : INotifyPropertyChanged
    {
        // Existing properties
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Rating { get; set; }
        public string FilePath { get; set; }

        // New audio clip properties
        private string _audioClipPath;
        public string AudioClipPath
        {
            get { return _audioClipPath; }
            set
            {
                _audioClipPath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAudioClip));
            }
        }

        public bool HasAudioClip => !string.IsNullOrEmpty(AudioClipPath);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
