using System;

namespace AlbumV
{
    public class Albums
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Rating { get; set; }
        public string FilePath { get; set; }
        public string AudioPath { get; set; }
        public bool HasAudio => !string.IsNullOrEmpty(AudioPath);
    }
}