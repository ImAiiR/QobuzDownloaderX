using System;
using QopenAPI;

namespace QobuzDownloaderX
{
    class PaddingNumbers
    {
        public Album QoAlbum = new Album();

        public int padTracks(Album QoAlbum)
        {
            var paddingLength = 2;

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoAlbum.TracksCount) + 1).ToString();

            switch (paddingLog)
            {
                case "1":
                    return paddingLength = 2;
                default:
                    return paddingLength = (int)Math.Floor(Math.Log10(QoAlbum.TracksCount) + 1);
            }
        }

        public int padPlaylistTracks(Playlist QoPlaylist)
        {
            var paddingLength = 2;

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoPlaylist.TracksCount) + 1).ToString();

            switch (paddingLog)
            {
                case "1":
                    return paddingLength = 2;
                default:
                    return paddingLength = (int)Math.Floor(Math.Log10(QoPlaylist.TracksCount) + 1);
            }
        }

        public int padDiscs(Album QoAlbum)
        {
            var paddingLength = 2;

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoAlbum.MediaCount) + 1).ToString();

            switch (paddingLog)
            {
                case "1":
                    return paddingLength = 2;
                default:
                    return paddingLength = (int)Math.Floor(Math.Log10(QoAlbum.MediaCount) + 1);
            }
        }
    }
}
