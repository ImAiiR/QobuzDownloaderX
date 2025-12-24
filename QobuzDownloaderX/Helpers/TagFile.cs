using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Helpers.QobuzDownloaderXMOD;
using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.Globalization;
using System.Linq;
using TagLib;
using TagLib.Id3v2;
using File = TagLib.File;

namespace QobuzDownloaderX
{
    class TagFile
    {
        public static void WriteToFile(string tempPath, string artworkPath, Album QoAlbum, Item QoItem)
        {
            // Use ID3v2.4 as default mp3 tag version
            TagLib.Id3v2.Tag.DefaultVersion = 4;
            TagLib.Id3v2.Tag.ForceDefaultVersion = true;

            using (var file = File.Create(tempPath))
            {
                file.RemoveTags(TagTypes.Id3v1);

                bool isFlac = tempPath.Contains(".flac");
                qbdlxForm._qbdlxForm.logger.Debug(isFlac ? "FLAC detected, setting FLAC specific tags" : "Non-FLAC detected, setting MP3 specific tags");

                if (isFlac)
                {
                    var customTagsFLAC = (TagLib.Ogg.XiphComment)file.GetTag(TagTypes.Xiph, create: true);
                    SetFlacTags(file, customTagsFLAC, QoAlbum, QoItem);
                }
                else
                {
                    var mp3Tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, create: true);
                    SetMp3Tags(file, mp3Tag, QoAlbum, QoItem);
                }

                SetCommonTags(file, QoAlbum, QoItem);
                SetAlbumArtists(file, QoAlbum);
                EmbedArtwork(file, artworkPath);

                // Save All Tags
                file.Save();
                qbdlxForm._qbdlxForm.logger.Debug("File tagging completed!");
            }
        }

        private static void SetFlacTags(TagLib.File file, TagLib.Ogg.XiphComment customTags, Album QoAlbum, Item QoItem)
        {

            if (Settings.Default.trackTag) file.Tag.Track = (uint)QoItem.TrackNumber;
            if (Settings.Default.isrcTag) customTags.SetField("ISRC", QoItem.ISRC);
            if (Settings.Default.typeTag) customTags.SetField("MEDIATYPE", QoAlbum.ProductType.ToUpper());
            if (Settings.Default.upcTag) customTags.SetField("BARCODE", QoAlbum.UPC);
            if (Settings.Default.labelTag) customTags.SetField("LABEL", RenameTemplates.spacesRegex.Replace(QoAlbum.Label.Name, " "));
            if (Settings.Default.explicitTag) customTags.SetField("ITUNESADVISORY", QoItem.ParentalWarning ? "1" : "0");

            if (Settings.Default.commentTag && !string.IsNullOrEmpty(Settings.Default.commentText))
            {
                customTags.SetField("COMMENT", new string[]
                {
                    RenameTemplates.percentRegex.Replace(Settings.Default.commentText, match => match.Value.ToLower())
                        .Replace("%description%", QoAlbum.Description)
                        .Replace("<br/>", Environment.NewLine)
                        .Replace("<br />", Environment.NewLine)
                });
            }

            if (Settings.Default.urlTag)
            {
                string url = !string.IsNullOrWhiteSpace(QoItem?.Url) ? QoItem.Url : QoAlbum?.Url;
                url = url?.Replace("/fr-fr/", "/");
                if (!string.IsNullOrWhiteSpace(url))
                {
                    customTags.SetField("Qobuz URL", new string[] { url });
                }
            }

            if (Settings.Default.yearTag)
            {
                string releaseDate = !string.IsNullOrWhiteSpace(QoItem?.ReleaseDateOriginal) ? QoItem.ReleaseDateOriginal : QoAlbum?.ReleaseDateOriginal;
                customTags.SetField("DATE", releaseDate);

                if (!string.IsNullOrEmpty(releaseDate) && releaseDate.Length >= 4)
                {
                    string yearOnly = releaseDate.Substring(0, 4);
                    customTags.SetField("YEAR", yearOnly);
                }
            }
        }

        private static void SetMp3Tags(TagLib.File file, TagLib.Id3v2.Tag mp3Tag, Album QoAlbum, Item QoItem)
        {
            if (Settings.Default.upcTag) UserTextInformationFrame.Get(mp3Tag, "BARCODE", true).Text = new[] { QoAlbum.UPC };
            if (Settings.Default.explicitTag) UserTextInformationFrame.Get(mp3Tag, "ITUNESADVISORY", true).Text = new[] { QoItem.ParentalWarning ? "1" : "0" };
            if (Settings.Default.isrcTag) mp3Tag.SetTextFrame("TSRC", QoItem.ISRC);
            if (Settings.Default.labelTag) mp3Tag.SetTextFrame("TPUB", RenameTemplates.spacesRegex.Replace(QoAlbum.Label.Name, " "));
            if (Settings.Default.typeTag) mp3Tag.SetTextFrame("TMED", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(QoAlbum.ProductType.ToLower()));

            if (Settings.Default.commentTag && !string.IsNullOrEmpty(Settings.Default.commentText))
            {
                mp3Tag.Comment = RenameTemplates.percentRegex.Replace(Settings.Default.commentText, match => match.Value.ToLower())
                    .Replace("%description%", QoAlbum.Description)
                    .Replace("<br/>", Environment.NewLine)
                    .Replace("<br />", Environment.NewLine);
            }

            if (Settings.Default.trackTag)
            {
                // Credits to QobuzDownloaderX-MOD author(s):
                // https://github.com/DJDoubleD/QobuzDownloaderX-MOD
                //
                // !! Set Track Number after Total Tracks to prevent taglib-sharp from re-formatting the field to a "two-digit zero-filled value" !!
                // Set TRCK tag manually to prevent using "two-digit zero-filled value"
                // See https://github.com/mono/taglib-sharp/pull/240 where this change was introduced in taglib-sharp v2.3
                // Original command: tfile.Tag.Track = Convert.ToUInt32(TrackNumber);
                mp3Tag.SetNumberFrame("TRCK", Convert.ToUInt32(QoItem.TrackNumber), file.Tag.TrackCount);
            }

            if (Settings.Default.urlTag)
            {
                string url = !string.IsNullOrWhiteSpace(QoItem?.Url) ? QoItem.Url : QoAlbum?.Url;
                url = url?.Replace("/fr-fr/", "/");
                if (!string.IsNullOrWhiteSpace(url))
                {
                    // Custom User URL
                    var data = new ByteVector {
                        0, ByteVector.FromString("Qobuz URL", StringType.UTF8),
                        0, ByteVector.FromString(url, StringType.UTF8)
                    };
                    var wxxx = new UnknownFrame("WXXX", data);
                    mp3Tag.AddFrame(wxxx);
                }
            }

            if (Settings.Default.yearTag)
            {
                string releaseDate = !string.IsNullOrWhiteSpace(QoItem?.ReleaseDateOriginal) ? QoItem.ReleaseDateOriginal : QoAlbum?.ReleaseDateOriginal;
                if (!string.IsNullOrEmpty(releaseDate) && releaseDate.Length >= 4)
                {
                    // Release Year tag (writes to "TDRC" (recording date) Frame)
                    mp3Tag.Year = uint.Parse(QoItem.ReleaseDateOriginal.Substring(0, 4));

                    // Release Date tag (use "TDRL" (release date) Frame for full date)
                    mp3Tag.SetTextFrame("TDRL", QoItem.ReleaseDateOriginal);
                }
            }
        }

        private static void SetCommonTags(TagLib.File file, Album QoAlbum, Item QoItem)
        {
            if (Settings.Default.genreTag) file.Tag.Genres = new[] { QoAlbum.Genre.Name };
            if (Settings.Default.albumTag) file.Tag.Album = QoAlbum.Version == null ? QoAlbum.Title : $"{QoAlbum.Title.TrimEnd()} ({QoAlbum.Version})";
            if (Settings.Default.composerTag) file.Tag.Composers = new[] { QoItem.Composer?.Name };
            if (Settings.Default.totalTracksTag) file.Tag.TrackCount = (uint)QoAlbum.TracksCount;
            if (Settings.Default.discTag) file.Tag.Disc = (uint)QoItem.MediaNumber;
            if (Settings.Default.totalDiscsTag) file.Tag.DiscCount = (uint)QoAlbum.MediaCount;
            if (Settings.Default.copyrightTag) file.Tag.Copyright = QoAlbum.Copyright;

            if (Settings.Default.trackTitleTag)
            {
                string titleFormatted = QoItem.Version == null
                                        ? QoItem.Title
                                        : $"{QoItem.Title.TrimEnd()} ({QoItem.Version})";
                titleFormatted = RenameTemplates.repeatedParenthesesRegex.Replace(titleFormatted, "($1)");
                file.Tag.Title = titleFormatted;
            }

            if (Settings.Default.artistTag)
            {
                if (Settings.Default.mergeArtistNames)
                {
                    string performerNames = ParsingHelper.GetTrackPerformersName(QoItem);
                    file.Tag.Performers = new string[] { performerNames };
                } else {
                    file.Tag.Performers = new[] { QoItem.Performer?.Name }; 
                }
            }
        }

        private static void SetAlbumArtists(TagLib.File file, Album QoAlbum)
        {
            if (!Settings.Default.albumArtistTag) return;

            if (Settings.Default.mergeArtistNames)
            {
                string[] AlbumArtists = ParsingHelper.GetAlbumArtistsNames(QoAlbum);
                file.Tag.AlbumArtists = AlbumArtists;
            } else
            {
                var mainArtists = QoAlbum.Artists.Where(a => a.Roles.Contains("main-artist")).ToList();
                if (mainArtists.Count > 1)
                {
                    var allButLastArtist = string.Join(", ", mainArtists.Take(mainArtists.Count - 1).Select(a => a.Name));
                    var lastArtist = mainArtists.Last().Name;
                    file.Tag.AlbumArtists = new[] { $"{allButLastArtist} & {lastArtist}" };
                    return;
                }

                file.Tag.AlbumArtists = new[] { QoAlbum.Artist.Name };
            }
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
