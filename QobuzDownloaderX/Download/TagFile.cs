using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using File = TagLib.File;
using QobuzDownloaderX;
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
                var customTagsFLAC = (TagLib.Ogg.XiphComment)file.GetTag(TagLib.TagTypes.Xiph);

                if (tempPath.Contains(".flac"))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("FLAC detected, setting FLAC specific tags");
                    if (Settings.Default.yearTag == true) { customTagsFLAC.SetField("DATE", QoAlbum.ReleaseDateOriginal); } // Release Date (FLAC)
                    if (Settings.Default.isrcTag == true) { customTagsFLAC.SetField("ISRC", QoItem.ISRC); } // ISRC (FLAC)
                    if (Settings.Default.typeTag == true) { customTagsFLAC.SetField("MEDIATYPE", QoAlbum.ProductType.ToUpper()); } // Type of release (FLAC)
                    if (Settings.Default.upcTag == true) { customTagsFLAC.SetField("BARCODE", QoAlbum.UPC); } // UPC / Barcode (FLAC)
                    if (Settings.Default.labelTag == true) { customTagsFLAC.SetField("LABEL", Regex.Replace(QoAlbum.Label.Name, @"\s+", " ")); } // Record Label (FLAC) [Removing any chance of double spaces]
                    if (Settings.Default.explicitTag == true) { if (QoItem.ParentalWarning == true) { customTagsFLAC.SetField("ITUNESADVISORY", "1"); } else { customTagsFLAC.SetField("ITUNESADVISORY", "0"); } } // Parental Advisory (FLAC)
                    if (Settings.Default.commentTag == true && Settings.Default.commentText != string.Empty) { customTagsFLAC.SetField("COMMENT", new string[] { Settings.Default.commentText.Replace("%description%", "%Description%").Replace("%Description%", QoAlbum.Description).Replace("<br/>", System.Environment.NewLine).Replace("<br />", System.Environment.NewLine) }); } // Comment (FLAC)
                }
                else
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Non-FLAC detected, setting MP3 specific tags");
                    TagLib.Id3v2.Tag mp3Tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);

                    UserTextInformationFrame barcodeFrame = UserTextInformationFrame.Get(mp3Tag, "BARCODE", true);
                    UserTextInformationFrame explicitFrame = UserTextInformationFrame.Get(mp3Tag, "ITUNESADVISORY", true);

                    if (Settings.Default.upcTag == true) { barcodeFrame.Text = new string[] { QoAlbum.UPC }; } // UPC / Barcode (MP3)
                    if (Settings.Default.explicitTag == true) { if (QoItem.ParentalWarning == true) { explicitFrame.Text = new string[] { "1" }; } else { explicitFrame.Text = new string[] { "0" }; } } // Parental Advisory (MP3)
                    
                    if (Settings.Default.yearTag == true) { file.Tag.Year = UInt32.Parse(QoAlbum.ReleaseDateOriginal.Substring(0, 4)); } // Release Date (MP3)
                    if (Settings.Default.isrcTag == true) { mp3Tag.SetTextFrame("TSRC", QoItem.ISRC); } // ISRC (MP3)
                    if (Settings.Default.labelTag == true) { mp3Tag.SetTextFrame("TPUB", Regex.Replace(QoAlbum.Label.Name, @"\s+", " ")); } // Record Label (MP3) [Removing any chance of double spaces]
                    if (Settings.Default.typeTag == true) { mp3Tag.SetTextFrame("TMED", QoAlbum.ProductType.ToUpper()); } // Type of release (MP3)
                    if (Settings.Default.commentTag == true && Settings.Default.commentText != string.Empty) { file.Tag.Comment = Settings.Default.commentText.Replace("%description%", "%Description%").Replace("%Description%", QoAlbum.Description).Replace("<br/>", System.Environment.NewLine).Replace("<br />", System.Environment.NewLine); } // Comment (MP3)
                }
                qbdlxForm._qbdlxForm.logger.Debug("Writing all other tags");
                if (Settings.Default.trackTitleTag == true) { file.Tag.Title = QoItem.Title; } // Track Title
                if (Settings.Default.artistTag == true) { file.Tag.Performers = new[] { QoItem?.Performer?.Name }; } // Track Artist
                if (Settings.Default.genreTag == true) { file.Tag.Genres = new[] { QoAlbum.Genre.Name }; } // Genre
                if (Settings.Default.albumTag == true) { if (QoAlbum.Version == null) { file.Tag.Album = QoAlbum.Title; } else { file.Tag.Album = QoAlbum.Title.TrimEnd() + " (" + QoAlbum.Version + ")"; } } // Album Title
                if (Settings.Default.trackTitleTag == true) { if (QoItem.Version == null) { file.Tag.Title = QoItem.Title; } else { file.Tag.Title = QoItem.Title.TrimEnd() + " (" + QoItem.Version + ")"; } } // Track Title
                if (Settings.Default.composerTag == true) { try { file.Tag.Composers = new[] { QoItem.Composer.Name }; } catch { qbdlxForm._qbdlxForm.logger.Warning("Failed to write track composer, usually means it wasn't available"); } } // Track Composer
                if (Settings.Default.trackTag == true) { file.Tag.Track = (uint)QoItem.TrackNumber; } // Track Number
                if (Settings.Default.totalTracksTag == true) { file.Tag.TrackCount = (uint)(QoAlbum.TracksCount); } // Total Tracks
                if (Settings.Default.discTag == true) { file.Tag.Disc = (uint)QoItem.MediaNumber; } // Disc Number
                if (Settings.Default.totalDiscsTag == true) { file.Tag.DiscCount = (uint)(QoAlbum.MediaCount); } // Total Discs
                if (Settings.Default.copyrightTag == true) { file.Tag.Copyright = QoAlbum.Copyright; } // Copyright

                // Album Artist (check for more than 1 main artist)
                if (QoAlbum.Artists.Count > 1)
                {
                    var mainArtists = QoAlbum.Artists.Where(a => a.Roles.Contains("main-artist")).ToList();
                    string allButLastArtist = string.Join(", ", mainArtists.Take(mainArtists.Count - 1).Select(a => a.Name));
                    string lastArtist = mainArtists.Last().Name;

                    if (mainArtists.Count > 1)
                    {
                        if (Settings.Default.albumArtistTag == true) { if (QoAlbum.Artist.Name != null) { file.Tag.AlbumArtists = new[] { allButLastArtist + " & " + lastArtist }; } }
                    }
                    else
                    {
                        if (Settings.Default.albumArtistTag == true) { if (QoAlbum.Artist.Name != null) { file.Tag.AlbumArtists = new[] { lastArtist }; } }
                    }
                }
                else
                {
                    if (Settings.Default.albumArtistTag == true) { if (QoAlbum.Artist.Name != null) { file.Tag.AlbumArtists = new[] { QoAlbum.Artist.Name }; } }
                }

                if (Settings.Default.imageTag == true)
                {
                    try
                    {
                        qbdlxForm._qbdlxForm.logger.Debug("Attempting to embed artwork");
                        // Define cover art to use for file(s)
                        TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                        pic.TextEncoding = TagLib.StringType.Latin1;
                        pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                        pic.Type = TagLib.PictureType.FrontCover;
                        pic.Data = TagLib.ByteVector.FromPath(artworkPath);

                        // Save cover art to file.
                        file.Tag.Pictures = new TagLib.IPicture[1] { pic };
                        qbdlxForm._qbdlxForm.logger.Debug("Artwork embed complete");
                    }
                    catch { 
                        qbdlxForm._qbdlxForm.logger.Error("Unable to write embedded artwork"); 
                    }
                }

                // Save All Tags
                file.Save();
                qbdlxForm._qbdlxForm.logger.Debug("File tagging completed!");
            }
        }
    }
}
