using System;
using System.Linq;
using System.IO;
using QopenAPI;
using System.Text.RegularExpressions;

namespace QobuzDownloaderX
{
    class RenameTemplates
    {
        public string GetSafeFilename(string filename)
        {
            // For converting illegal filename characters to an underscore.
            Console.WriteLine(string.Join("_", filename.TrimEnd().TrimEnd('.').TrimEnd('.').Split(Path.GetInvalidFileNameChars())));
            return string.Join("_", filename.TrimEnd().TrimEnd('.').TrimEnd('.').Split(Path.GetInvalidFileNameChars()));
        }

        public string GetReleaseArtists(Album QoAlbum)
        {
            var mainArtists = QoAlbum.Artists.Where(a => a.Roles.Contains("main-artist")).ToList();
            if (mainArtists.Count > 1)
            {
                var allButLastArtist = string.Join(", ", mainArtists.Take(mainArtists.Count - 1).Select(a => a.Name));
                var lastArtist = mainArtists.Last().Name;
                return $"{allButLastArtist} & {lastArtist}";
            }
            return QoAlbum.Artist.Name;
        }

        private string ReplaceParentalWarningTags(string template, bool isExplicit)
        {
            template = template.Replace("%trackpa%", isExplicit ? "Explicit" : "Clean");
            template = template.Replace("%trackpashort%", isExplicit ? "E" : "C");
            template = template.Replace("%trackpaifex%", isExplicit ? "Explicit" : "");
            template = template.Replace("%trackpaifexshort%", isExplicit ? "E" : "");
            template = template.Replace("%trackpaifcl%", isExplicit ? "" : "Clean");
            template = template.Replace("%trackpaifclshort%", isExplicit ? "" : "C");
            template = template.Replace("%trackpaenclosed%", isExplicit ? $"(Explicit)" : $"(Clean)");
            template = template.Replace("%trackpaenclosed[]%", isExplicit ? $"[Explicit]" : $"[Clean]");
            template = template.Replace("%trackpaenclosedshort%", isExplicit ? $"(E)" : $"(C)");
            template = template.Replace("%trackpaenclosedshort[]%", isExplicit ? $"[E]" : $"[C]");
            template = template.Replace("%trackpaifexenclosed%", isExplicit ? $"(Explicit)" : $"");
            template = template.Replace("%trackpaifexenclosed[]%", isExplicit ? $"[Explicit]" : $"");
            template = template.Replace("%trackpaifexenclosedshort%", isExplicit ? $"(E)" : $"");
            template = template.Replace("%trackpaifexenclosedshort[]%", isExplicit ? $"[E]" : $"");
            template = template.Replace("%trackpaifclenclosed%", isExplicit ? $"" : $"(Clean)");
            template = template.Replace("%trackpaifclenclosed[]%", isExplicit ? $"" : $"[Clean]");
            template = template.Replace("%trackpaifclenclosedshort%", isExplicit ? $"" : $"(C)");
            template = template.Replace("%trackpaifclenclosedshort[]%", isExplicit ? $"" : $"[C]");
            template = template.Replace("%albumpa%", isExplicit ? "Explicit" : "Clean");
            template = template.Replace("%albumpashort%", isExplicit ? "E" : "C");
            template = template.Replace("%albumpaifex%", isExplicit ? "Explicit" : "");
            template = template.Replace("%albumpaifexshort%", isExplicit ? "E" : "");
            template = template.Replace("%albumpaifcl%", isExplicit ? "" : "Clean");
            template = template.Replace("%albumpaifclshort%", isExplicit ? "" : "C");
            template = template.Replace("%albumpaenclosed%", isExplicit ? $"(Explicit)" : $"(Clean)");
            template = template.Replace("%albumpaenclosed[]%", isExplicit ? $"[Explicit]" : $"[Clean]");
            template = template.Replace("%albumpaenclosedshort%", isExplicit ? $"(E)" : $"(C)");
            template = template.Replace("%albumpaenclosedshort[]%", isExplicit ? $"[E]" : $"[C]");
            template = template.Replace("%albumpaifexenclosed%", isExplicit ? $"(Explicit)" : $"");
            template = template.Replace("%albumpaifexenclosed[]%", isExplicit ? $"[Explicit]" : $"");
            template = template.Replace("%albumpaifexenclosedshort%", isExplicit ? $"(E)" : $"");
            template = template.Replace("%albumpaifexenclosedshort[]%", isExplicit ? $"[E]" : $"");
            template = template.Replace("%albumpaifclenclosed%", isExplicit ? $"" : $"(Clean)");
            template = template.Replace("%albumpaifclenclosed[]%", isExplicit ? $"" : $"[Clean]");
            template = template.Replace("%albumpaifclenclosedshort%", isExplicit ? $"" : $"(C)");
            template = template.Replace("%albumpaifclenclosedshort[]%", isExplicit ? $"" : $"[C]");
            return template;
        }

        private string RenameFormatTemplate(string template, string formatId, string fileFormat, int maximumBitDepth, double maximumSamplingRate, string formatWithHiresQualityPlaceholder, string formatWithQualityPlaceholder)
        {
            fileFormat = fileFormat.ToUpper().TrimStart('.');

            switch (formatId)
            {
                case "5":
                    template = template
                        .Replace(formatWithHiresQualityPlaceholder, fileFormat)
                        .Replace(formatWithQualityPlaceholder, fileFormat);
                    break;

                case "6":
                    template = template
                        .Replace(formatWithHiresQualityPlaceholder, fileFormat)
                        .Replace(formatWithQualityPlaceholder, $"{fileFormat} ({maximumBitDepth}bit-{maximumSamplingRate}kHz)");
                    break;

                case "7":
                case "27":
                    if (maximumBitDepth == 16)
                    {
                        template = template
                            .Replace(formatWithHiresQualityPlaceholder, fileFormat)
                            .Replace(formatWithQualityPlaceholder, $"{fileFormat} ({maximumBitDepth}bit-{maximumSamplingRate}kHz)");
                    }
                    else if (maximumSamplingRate < 192)
                    {
                        template = template.Replace(formatWithQualityPlaceholder, formatWithHiresQualityPlaceholder);

                        if (maximumSamplingRate < 96)
                        {
                            template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} ({maximumBitDepth}bit-{maximumSamplingRate}kHz)");
                        }
                        else if (maximumSamplingRate > 96 && maximumSamplingRate < 192)
                        {
                            if (formatId == "7" && maximumSamplingRate == 176.4)
                            {
                                template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} (24bit-88.2kHz)");
                            }
                            else if (formatId == "7")
                            {
                                template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} (24bit-96kHz)");
                            }
                            else
                            {
                                template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} ({maximumBitDepth}bit-{maximumSamplingRate}kHz)");
                            }
                        }
                        else
                        {
                            template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} (24bit-96kHz)");
                        }
                    }
                    else
                    {
                        template = template.Replace(formatWithQualityPlaceholder, formatWithHiresQualityPlaceholder);
                        template = template.Replace(formatWithHiresQualityPlaceholder, $"{fileFormat} (24bit-192kHz)");
                    }
                    break;
            }

            return template;
        }

        public string renameTemplates(string template, int paddedTrackLength, int paddedDiscLength, string fileFormat, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Renaming user template - " + template);

            // Convert all text between % symbols to lowercase
            template = Regex.Replace(template, @"%(.*?)%", match => match.Value.ToLower());

            // Keep backslashes to be used to make new folders
            if (template.Contains(Path.DirectorySeparatorChar))
            {
                template = template.Replace(@"\", "{backslash}").Replace(@"/", "{forwardslash}");
            }

            // Artist Templates
            /* bro there ain't shit here */

            // Track Templates
            if (QoItem != null)
            {
                template = ReplaceParentalWarningTags(template, QoItem.ParentalWarning);
                template = template
                    .Replace("%trackid%", QoItem.Id.ToString())
                    .Replace("%trackartist%", QoItem?.Performer?.Name?.ToString())
                    .Replace("%trackcomposer%", QoItem?.Composer?.Name?.ToString())
                    .Replace("%tracknumber%", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))
                    .Replace("%isrc%", QoItem.ISRC.ToString())
                    .Replace("%trackbitdepth%", QoItem.MaximumBitDepth.ToString())
                    .Replace("%tracksamplerate%", QoItem.MaximumSamplingRate.ToString())
                    .Replace("%tracktitle%", QoItem.Version == null ? QoItem.Title : $"{QoItem.Title.TrimEnd()} ({QoItem.Version})");
                
                // Track Format Templates
                template = template.Replace("%trackformat%", fileFormat.ToUpper().TrimStart('.'));
                template = RenameFormatTemplate(template, qbdlxForm._qbdlxForm.format_id, fileFormat, QoItem.MaximumBitDepth, QoItem.MaximumSamplingRate, "%trackformatwithhiresquality%", "%trackformatwithquality%");
            }

            // Album Templates
            if (QoAlbum != null)
            {
                template = ReplaceParentalWarningTags(template, QoAlbum.ParentalWarning);
                template = template
                    .Replace("%albumid%", QoAlbum.Id.ToString())
                    .Replace("%albumurl%", QoAlbum.Url.ToString())
                    .Replace("%artistname%", GetReleaseArtists(QoAlbum))
                    .Replace("%albumgenre%", QoAlbum?.Genre?.Name)
                    .Replace("%albumcomposer%", QoAlbum?.Composer?.Name?.ToString())
                    .Replace("%label%", Regex.Replace(QoAlbum.Label.Name, @"\s+", " ")) // Qobuz sometimes has multiple spaces in place of where a single space should be when it comes to Labels
                    .Replace("%copyright%", QoAlbum.Copyright)
                    .Replace("%upc%", QoAlbum.UPC)
                    .Replace("%releasedate%", QoAlbum.ReleaseDateOriginal)
                    .Replace("%year%", UInt32.Parse(QoAlbum.ReleaseDateOriginal.Substring(0, 4)).ToString())
                    .Replace("%releasetype%", char.ToUpper(QoAlbum.ProductType.First()) + QoAlbum.ProductType.Substring(1).ToLower())
                    .Replace("%bitdepth%", QoAlbum.MaximumBitDepth.ToString())
                    .Replace("%samplerate%", QoAlbum.MaximumSamplingRate.ToString())
                    .Replace("%albumtitle%", QoAlbum.Version == null ? QoAlbum.Title : $"{QoAlbum.Title.TrimEnd()} ({QoAlbum.Version})")
                    .Replace("%format%", fileFormat.ToUpper().TrimStart('.'));
            }

            if (QoPlaylist == null)
            {
                // Release Format Templates
                template = RenameFormatTemplate(template, qbdlxForm._qbdlxForm.format_id, fileFormat, QoAlbum.MaximumBitDepth, QoAlbum.MaximumSamplingRate, "%formatwithhiresquality%", "%formatwithquality%");
            }
            else
            {
                // Playlist Templates
                template = template
                    .Replace("%playlistid%", QoPlaylist.Id.ToString())
                    .Replace("%playlisttitle%", QoPlaylist.Name)
                    .Replace("%format%", fileFormat.ToUpper().TrimStart('.'))
                    .Replace("%formatwithhiresquality%", fileFormat.ToUpper().TrimStart('.'))
                    .Replace("%formatwithquality%", fileFormat.ToUpper().TrimStart('.'));
            }

            // GetSafeFilename call to make sure path will be valid
            template = GetSafeFilename(template);

            // Remove any double spaces
            template = Regex.Replace(Regex.Replace(template.Replace("{backslash}", @"\").Replace("{forwardslash}", @"/"), @"\s+", " ").Replace(@" \", @"\"), @"\s+\\", " "); // Replace slash placeholders & remove double spaces

            qbdlxForm._qbdlxForm.logger.Debug("Template output - " + template);
            return template;
        }
    }
}
