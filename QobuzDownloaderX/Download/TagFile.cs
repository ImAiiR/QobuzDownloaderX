using System;
using System.Linq;
using TagLib;
using File = TagLib.File;
using QobuzDownloaderX.Properties;
using QopenAPI;
using TagLib.Id3v2;
using System.Text.RegularExpressions;

namespace QobuzDownloaderX
{
    class TagFile
    {
        public static void WriteToFile(string tempPath, string artworkPath, Album QoAlbum, Item QoItem)
        {
            using (var file = File.Create(tempPath))
            {
                bool isFlac = tempPath.Contains(".flac");
                qbdlxForm._qbdlxForm.logger.Debug(isFlac ? "FLAC detected, setting FLAC specific tags" : "Non-FLAC detected, setting MP3 specific tags");

                if (isFlac)
                {
                    var customTagsFLAC = (TagLib.Ogg.XiphComment)file.GetTag(TagLib.TagTypes.Xiph);
                    SetFlacTags(customTagsFLAC, QoAlbum, QoItem);
                }
                else
                {
                    var mp3Tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                    SetMp3Tags(mp3Tag, QoAlbum, QoItem);
                }

                SetCommonTags(file, QoAlbum, QoItem);
                SetAlbumArtists(file, QoAlbum);
                EmbedArtwork(file, artworkPath);

                // Save All Tags
                file.Save();
                qbdlxForm._qbdlxForm.logger.Debug("File tagging completed!");
            }
        }

        private static void SetFlacTags(TagLib.Ogg.XiphComment customTags, Album QoAlbum, Item QoItem)
        {
            if (Settings.Default.yearTag) customTags.SetField("DATE", QoAlbum.ReleaseDateOriginal);
            if (Settings.Default.isrcTag) customTags.SetField("ISRC", QoItem.ISRC);
            if (Settings.Default.typeTag) customTags.SetField("MEDIATYPE", QoAlbum.ProductType.ToUpper());
            if (Settings.Default.upcTag) customTags.SetField("BARCODE", QoAlbum.UPC);
            if (Settings.Default.labelTag) customTags.SetField("LABEL", Regex.Replace(QoAlbum.Label.Name, @"\s+", " "));
            if (Settings.Default.explicitTag) customTags.SetField("ITUNESADVISORY", QoItem.ParentalWarning ? "1" : "0");
            if (Settings.Default.commentTag && !string.IsNullOrEmpty(Settings.Default.commentText))
            {
                customTags.SetField("COMMENT", new string[]
                {
                    Regex.Replace(Settings.Default.commentText, @"%(.*?)%", match => match.Value.ToLower())
                        .Replace("%description%", QoAlbum.Description)
                        .Replace("<br/>", Environment.NewLine)
                        .Replace("<br />", Environment.NewLine)
                });
            }
        }

        private static void SetMp3Tags(TagLib.Id3v2.Tag mp3Tag, Album QoAlbum, Item QoItem)
        {
            if (Settings.Default.upcTag) UserTextInformationFrame.Get(mp3Tag, "BARCODE", true).Text = new[] { QoAlbum.UPC };
            if (Settings.Default.explicitTag) UserTextInformationFrame.Get(mp3Tag, "ITUNESADVISORY", true).Text = new[] { QoItem.ParentalWarning ? "1" : "0" };
            if (Settings.Default.yearTag) mp3Tag.Year = uint.Parse(QoAlbum.ReleaseDateOriginal.Substring(0, 4));
            if (Settings.Default.isrcTag) mp3Tag.SetTextFrame("TSRC", QoItem.ISRC);
            if (Settings.Default.labelTag) mp3Tag.SetTextFrame("TPUB", Regex.Replace(QoAlbum.Label.Name, @"\s+", " "));
            if (Settings.Default.typeTag) mp3Tag.SetTextFrame("TMED", QoAlbum.ProductType.ToUpper());
            if (Settings.Default.commentTag && !string.IsNullOrEmpty(Settings.Default.commentText))
            {
                mp3Tag.Comment = Regex.Replace(Settings.Default.commentText, @"%(.*?)%", match => match.Value.ToLower())
                    .Replace("%description%", QoAlbum.Description)
                    .Replace("<br/>", Environment.NewLine)
                    .Replace("<br />", Environment.NewLine);
            }
        }

        private static void SetCommonTags(TagLib.File file, Album QoAlbum, Item QoItem)
        {
            if (Settings.Default.trackTitleTag) file.Tag.Title = QoItem.Version == null ? QoItem.Title : $"{QoItem.Title.TrimEnd()} ({QoItem.Version})";
            if (Settings.Default.artistTag) file.Tag.Performers = new[] { QoItem.Performer?.Name };
            if (Settings.Default.genreTag) file.Tag.Genres = new[] { QoAlbum.Genre.Name };
            if (Settings.Default.albumTag) file.Tag.Album = QoAlbum.Version == null ? QoAlbum.Title : $"{QoAlbum.Title.TrimEnd()} ({QoAlbum.Version})";
            if (Settings.Default.composerTag) file.Tag.Composers = new[] { QoItem.Composer?.Name };
            if (Settings.Default.trackTag) file.Tag.Track = (uint)QoItem.TrackNumber;
            if (Settings.Default.totalTracksTag) file.Tag.TrackCount = (uint)QoAlbum.TracksCount;
            if (Settings.Default.discTag) file.Tag.Disc = (uint)QoItem.MediaNumber;
            if (Settings.Default.totalDiscsTag) file.Tag.DiscCount = (uint)QoAlbum.MediaCount;
            if (Settings.Default.copyrightTag) file.Tag.Copyright = QoAlbum.Copyright;
        }

        private static void SetAlbumArtists(TagLib.File file, Album QoAlbum)
        {
            if (!Settings.Default.albumArtistTag) return;

            var mainArtists = QoAlbum.Artists.Where(a => a.Roles.Contains("main-artist")).ToList();
            string albumArtist = mainArtists.Count > 1
                ? string.Join(", ", mainArtists.Select(a => a.Name))
                : QoAlbum.Artist.Name;

            file.Tag.AlbumArtists = new[] { albumArtist };
        }

        private static void EmbedArtwork(TagLib.File file, string artworkPath)
        {
            if (!Settings.Default.imageTag) return;
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Attempting to embed artwork");
                var pic = new AttachedPictureFrame
                {
                    TextEncoding = TagLib.StringType.Latin1,
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    Type = PictureType.FrontCover,
                    Data = ByteVector.FromPath(artworkPath)
                };
                file.Tag.Pictures = new IPicture[] { pic };
                qbdlxForm._qbdlxForm.logger.Debug("Artwork embed complete");
            }
            catch
            {
                qbdlxForm._qbdlxForm.logger.Error("Unable to write embedded artwork");
            }
        }
    }
}
