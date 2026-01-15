using QobuzDownloaderX.Helpers.QobuzDownloaderXMOD;
using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class RenameTemplates
    {
        internal static readonly Regex percentRegex = new Regex(@"%(.*?)%", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        internal static readonly Regex spacesRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        internal static readonly Regex repeatedParenthesesRegex = new Regex(@"\(([^()]+)\)\s*(\(\1\))+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex spacesBeforeBackslashRegex = new Regex(@"\s+\\", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // "Various Artists" known variations
        readonly string[] variousArtistsNames = new[]
        {
            "Various Artists, Array",
            "Various Artists",
            "Various Aritsts",
            "Various Artist",
            "Various Interpreters",
            "Various Interpreter",
            "Various Interprets"
        };

        public string GetSafeFilename(string filename)
        {
            string safe = RenameTemplates.MakeValidWindowsFileName(filename);
            string safeTruncated = RenameTemplates.TruncateLongName(safe, (Byte)".flac".Length); // ".flac" = largest possible known file extension length on this application.
            return safeTruncated;
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

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "I don’t feel like changing this and it doesn’t matter")]
        public string renameTemplates(string template, int paddedTrackLength, int paddedDiscLength, string fileFormat, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Renaming user template - " + template);

            // Convert all text between % symbols to lowercase
            template = percentRegex.Replace(template, match => match.Value.ToLower());

            // Keep backslashes to be used to make new folders
            if (template.Contains(ZlpPathHelper.DirectorySeparatorChar))
            {
                template = template.Replace(@"\", "{backslash}").Replace(@"/", "{forwardslash}");
            }

            // Artist Templates
            /* bro there ain't shit here */

            // Track Templates
            if (QoItem != null)
            {
                if (QoAlbum != null)
                {
                    string artistsNames = GetReleaseArtists(QoAlbum) ?? "";
                    if (variousArtistsNames.Any(name => artistsNames.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Convert all text between % symbols to lowercase
                        template = percentRegex.Replace(Settings.Default.savedVaTrackTemplate, match => match.Value.ToLower());

                        template = template.Replace("%artistname%", "%trackartist%");
                    }
                }

                template = ReplaceParentalWarningTags(template, QoItem.ParentalWarning);
                template = template
                    .Replace("%trackid%", QoItem.Id.ToString())
                    .Replace("%trackcomposer%", QoItem?.Composer?.Name?.ToString())
                    .Replace("%tracknumber%", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))
                    .Replace("%isrc%", QoItem.ISRC.ToString())
                    .Replace("%trackbitdepth%", QoItem.MaximumBitDepth.ToString())
                    .Replace("%tracksamplerate%", QoItem.MaximumSamplingRate.ToString());

                string titleFormatted = QoItem.Version == null
                                        ? QoItem.Title
                                        : $"{QoItem.Title.TrimEnd()} ({QoItem.Version})";
                titleFormatted = repeatedParenthesesRegex.Replace(titleFormatted, "($1)");
                template = template.Replace("%tracktitle%", titleFormatted);

                if (Settings.Default.mergeArtistNames)
                {
                    string performerNames = ParsingHelper.GetTrackPerformersName(QoItem);
                    template = template.Replace("%artistname%", performerNames);
                    template = template.Replace("%trackartist%", performerNames);
                }
                else
                {
                    template = template.Replace("%trackartist%", QoItem?.Performer?.Name?.ToString());
                }

                // Track Format Templates
                template = template.Replace("%trackformat%", fileFormat.ToUpper().TrimStart('.'));
                template = RenameFormatTemplate(template, qbdlxForm._qbdlxForm.format_id, fileFormat, QoItem.MaximumBitDepth, QoItem.MaximumSamplingRate, "%trackformatwithhiresquality%", "%trackformatwithquality%");
            }

            // Album Templates
            if (QoAlbum != null)
            {
                template = ReplaceParentalWarningTags(template, QoAlbum.ParentalWarning);
                template = template
                    .Replace("%albumid%", QoAlbum.Id?.ToString() ?? "")
                    .Replace("%albumurl%", QoAlbum.Url?.ToString() ?? "")
                    .Replace("%artistname%", GetReleaseArtists(QoAlbum) ?? "")
                    .Replace("%albumgenre%", QoAlbum?.Genre?.Name ?? "")
                    .Replace("%albumcomposer%", QoAlbum?.Composer?.Name?.ToString() ?? "")
                    .Replace("%label%", spacesRegex.Replace(QoAlbum.Label?.Name ?? "", " "))
                    .Replace("%copyright%", QoAlbum.Copyright ?? "")
                    .Replace("%upc%", QoAlbum.UPC ?? "")
                    .Replace("%releasedate%", QoAlbum.ReleaseDateOriginal ?? "")
                    .Replace("%year%", UInt32.Parse(QoAlbum.ReleaseDateOriginal?.Substring(0, 4)).ToString() ?? "")
                    .Replace("%releasetype%", char.ToUpper(QoAlbum.ProductType.FirstOrDefault()) + QoAlbum.ProductType?.Substring(1)?.ToLower())
                    .Replace("%bitdepth%", QoAlbum.MaximumBitDepth.ToString() ?? "")
                    .Replace("%samplerate%", QoAlbum.MaximumSamplingRate.ToString() ?? "")
                    .Replace("%albumtitle%", QoAlbum.Version == null ? QoAlbum.Title : $"{QoAlbum.Title?.TrimEnd()} ({QoAlbum.Version})")
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

                if (QoItem != null)
                {


                    // Album Template for playlist path
                    template = template
                        .Replace("%albumid%", QoAlbum.Id?.ToString() ?? "")
                        .Replace("%albumurl%", QoAlbum.Url?.ToString() ?? "")
                        .Replace("%artistname%", GetReleaseArtists(QoAlbum) ?? "")
                        .Replace("%albumgenre%", QoAlbum?.Genre?.Name ?? "")
                        .Replace("%albumcomposer%", QoAlbum?.Composer?.Name?.ToString() ?? "")
                        .Replace("%label%", spacesRegex.Replace(QoAlbum.Label?.Name ?? "", " ")) // Qobuz sometimes has multiple spaces where a single one should be
                        .Replace("%copyright%", QoAlbum.Copyright ?? "")
                        .Replace("%upc%", QoAlbum.UPC ?? "")
                        .Replace("%releasedate%", QoAlbum.ReleaseDateOriginal ?? "")
                        .Replace("%year%", UInt32.Parse(QoAlbum.ReleaseDateOriginal?.Substring(0, 4)).ToString() ?? "")
                        .Replace("%releasetype%", char.ToUpper(QoAlbum.ProductType.FirstOrDefault()) + QoAlbum.ProductType?.Substring(1)?.ToLower())
                        .Replace("%bitdepth%", QoAlbum.MaximumBitDepth.ToString() ?? "")
                        .Replace("%samplerate%", QoAlbum.MaximumSamplingRate.ToString() ?? "")
                        .Replace("%albumtitle%", QoAlbum.Version == null ? QoAlbum.Title : $"{QoAlbum.Title?.TrimEnd()} ({QoAlbum.Version})")
                        .Replace("%format%", fileFormat.ToUpper().TrimStart('.'));
                }
            }

            // GetSafeFilename call to make sure path will be valid
            template = GetSafeFilename(template);

            // Remove any double spaces
            template = spacesBeforeBackslashRegex.Replace(
                           spacesRegex.Replace(
                               template.Replace("{backslash}", @"\").Replace("{forwardslash}", @"/").Replace(@" \", @"\"),
                               " "),
                           " "); // Replace slash placeholders & remove double spaces

            // Replace long ellipsis
            template = template.Replace("...", "…");

            qbdlxForm._qbdlxForm.logger.Debug("Template output - " + template);
            return template;
        }

        public static string MakeValidWindowsFileName(
            string fileName,
            char asteriskChar = '∗',
            char colonChar = '∶',
            char questionMarkChar = 'ʔ',
            char verticalBarChar = 'ǀ',
            char quoteChar = '″',
            char backSlashChar = '⧹',
            char forwardSlashChar = '⧸',
            char lessThanChar = '˂',
            char greaterThanChar = '˃')
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return fileName;

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            if (invalidFileNameChars.Contains(asteriskChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(asteriskChar)}.", nameof(asteriskChar));
            if (invalidFileNameChars.Contains(colonChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(colonChar)}.", nameof(colonChar));
            if (invalidFileNameChars.Contains(questionMarkChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(questionMarkChar)}.", nameof(questionMarkChar));
            if (invalidFileNameChars.Contains(verticalBarChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(verticalBarChar)}.", nameof(verticalBarChar));
            if (invalidFileNameChars.Contains(quoteChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(quoteChar)}.", nameof(quoteChar));
            if (invalidFileNameChars.Contains(backSlashChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(backSlashChar)}.", nameof(backSlashChar));
            if (invalidFileNameChars.Contains(forwardSlashChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(forwardSlashChar)}.", nameof(forwardSlashChar));
            if (invalidFileNameChars.Contains(lessThanChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(lessThanChar)}.", nameof(lessThanChar));
            if (invalidFileNameChars.Contains(greaterThanChar))
                throw new ArgumentException($"Invalid replacement character for {nameof(greaterThanChar)}.", nameof(greaterThanChar));

            var replacements = new Dictionary<char, char>
            {
                { '*', asteriskChar },
                { ':', colonChar },
                { '?', questionMarkChar },
                { '|', verticalBarChar },
                { '"', quoteChar },
                { '<', lessThanChar },
                { '>', greaterThanChar },
                { '\\', backSlashChar },
                { '/', forwardSlashChar }
            };

            fileName = fileName.Trim(new char[] {' ', '.'});

            var sb = new StringBuilder(fileName.Length);
            foreach (char c in fileName)
            {
                if (replacements.ContainsKey(c))
                    sb.Append(replacements[c]);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

        // UNUSED
        // ======
        //
        //public static string MakeValidWindowsDirectoryName(
        //    string dirPath,
        //    char asteriskChar = '✲',
        //    char colonChar = '∶',
        //    char questionMarkChar = 'ʔ',
        //    char verticalBarChar = 'ǀ',
        //    char quoteChar = '″'',
        //    char lessThanChar = '˂',
        //    char greaterThanChar = '˃')
        //{
        //    if (string.IsNullOrWhiteSpace(dirPath))
        //        throw new ArgumentNullException(nameof(dirPath));
        //
        //    char[] invalidPathChars = Path.GetInvalidPathChars();
        //
        //    if (invalidPathChars.Contains(asteriskChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(asteriskChar)}.", nameof(asteriskChar));
        //    if (invalidPathChars.Contains(colonChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(colonChar)}.", nameof(colonChar));
        //    if (invalidPathChars.Contains(questionMarkChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(questionMarkChar)}.", nameof(questionMarkChar));
        //    if (invalidPathChars.Contains(verticalBarChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(verticalBarChar)}.", nameof(verticalBarChar));
        //    if (invalidPathChars.Contains(quoteChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(quoteChar)}.", nameof(quoteChar));
        //    if (invalidPathChars.Contains(lessThanChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(lessThanChar)}.", nameof(lessThanChar));
        //    if (invalidPathChars.Contains(greaterThanChar))
        //        throw new ArgumentException($"Invalid replacement character for {nameof(greaterThanChar)}.", nameof(greaterThanChar));
        //
        //    var replacements = new Dictionary<char, char>
        //    {
        //        { '*', asteriskChar },
        //        { ':', colonChar },
        //        { '?', questionMarkChar },
        //        { '|', verticalBarChar },
        //        { '"', quoteChar },
        //        { '<', lessThanChar },
        //        { '>', greaterThanChar }
        //    };
        //
        //    dirPath = dirPath.Trim();
        //
        //    var sb = new StringBuilder(dirPath.Length);
        //    foreach (char c in dirPath)
        //    {
        //        if (replacements.ContainsKey(c))
        //            sb.Append(replacements[c]);
        //        else
        //            sb.Append(c);
        //    }
        //
        //    return sb.ToString();
        //}

        public static string TruncateLongName(string name, byte extLen, byte maxFileNameLength = 255)
        {

            if (string.IsNullOrEmpty(name))
                return name;

            if (maxFileNameLength == 0)
                throw new ArgumentException("Value must be greater than zero.", nameof(maxFileNameLength));

            if (maxFileNameLength == 0)
                throw new ArgumentException("Value must be greater than zero.", nameof(maxFileNameLength));

            if ((name.Length + extLen) >= maxFileNameLength)
            {
                name = name.Substring(0, maxFileNameLength - 1 - extLen) + '…';
            }

            return name;
        }

    }
}
