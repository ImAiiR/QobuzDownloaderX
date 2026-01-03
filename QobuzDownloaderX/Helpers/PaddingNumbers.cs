using System;

using QopenAPI;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class PaddingNumbers
    {
        public Album QoAlbum = new Album();

        public int padTracks(Album QoAlbum)
        {

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoAlbum.TracksCount) + 1).ToString();
            switch (paddingLog)
            {
                case "1":
                    return 2;
                default:
                    return (int)Math.Floor(Math.Log10(QoAlbum.TracksCount) + 1);
            }
        }

        public int padPlaylistTracks(Playlist QoPlaylist)
        {

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoPlaylist.TracksCount) + 1).ToString();
            switch (paddingLog)
            {
                case "1":
                    return 2;
                default:
                    return (int)Math.Floor(Math.Log10(QoPlaylist.TracksCount) + 1);
            }
        }

        public int padDiscs(Album QoAlbum)
        {

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(QoAlbum.MediaCount) + 1).ToString();
            switch (paddingLog)
            {
                case "1":
                    return 2;
                default:
                    return (int)Math.Floor(Math.Log10(QoAlbum.MediaCount) + 1);
            }
        }
    }
}
