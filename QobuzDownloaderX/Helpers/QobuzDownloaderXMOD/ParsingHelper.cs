using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{
    // Inspired by QobuzDownloaderX-MOD source-code.
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD
    internal class ParsingHelper
    {
        static readonly string primaryListSeparator = ", ";
        static readonly string listEndSeparator = " & ";

        private static readonly Regex unicodeRegex = new Regex(@"\\u(?<Value>[0-9A-Fa-f]{4})", RegexOptions.Compiled );

        /// <summary>
        /// Get the Artist names with given role as an array
        /// </summary>
        /// <param name="artists"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string[] GetArtistNames(List<QopenAPI.ArtistsList> artists, InvolvedPersonRoleType role)
        {
            return artists.Where(artist => artist.Roles.Exists(roleString => InvolvedPersonRoleMapping.GetRoleByString(roleString) == role))
                          .Select(artist => artist.Name)
                          .ToArray();
        }

        public static string MergeFeaturedArtistsWithMainArtists(string[] mainArtists, string[] featuresArtists)
        {
            string mergedMainArtists = MergeDoubleDelimitedList(mainArtists, primaryListSeparator, listEndSeparator);
            string mergedFeaturedArtists = MergeDoubleDelimitedList(featuresArtists, primaryListSeparator, listEndSeparator);

            if (string.IsNullOrEmpty(mergedFeaturedArtists))
            {
                return mergedMainArtists;
            }
            else
            {
                return $"{mergedMainArtists} Feat. {mergedFeaturedArtists}";
            }
        }

        // https://github.com/DJDoubleD/QobuzDownloaderX-MOD/blob/993c708f594faaab36ca4b3a97e4a7b84676ecf2/QobuzDownloaderX/Shared/Tools/StringTools.cs#L81
        public static string MergeDoubleDelimitedList(string[] stringList, string initialDelimiter, string finalDelimiter)
        {
            if (stringList != null)
            {
                string result;
                if (stringList.Length > 1)
                {
                    result = string.Join(initialDelimiter, stringList.Take(stringList.Length - 1)) + finalDelimiter + stringList.LastOrDefault();
                }
                else
                {
                    result = stringList.FirstOrDefault();
                }

                return DecodeEncodedNonAsciiCharacters(result);
            }
            else
            {
                return "";
            }
        }

        // https://github.com/DJDoubleD/QobuzDownloaderX-MOD/blob/993c708f594faaab36ca4b3a97e4a7b84676ecf2/QobuzDownloaderX/Shared/Tools/StringTools.cs#L16C16-L16C22
        /// <summary>
        /// Decodes the encoded non ascii characters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The decoded string.</returns>
        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            if (value == null)
                return null;

            return unicodeRegex.Replace(
                value,
                m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString()
            );
        }

        // Adapted by ElektroStudios from QobuzDownloaderX-MOD's source-code to use a QopenAPI.Item object.
        // Also, now it handles cases where the track title already contains " Feat. "-like words (case-insensitive)
        // and where the performer name is a composed name that already contains " Feat " word.
        // (i.e., does not add featured artists names to the resulting string.)
        public static string GetTrackPerformersName(QopenAPI.Item QoItem)
        {
            PerformersParser performersParser = new PerformersParser(QoItem);

            // Get main and featured performers
            string[] mainPerformers = performersParser.GetPerformersWithRole(InvolvedPersonRoleType.MainArtist);
            string[] featuredPerformers = performersParser.GetPerformersWithRole(InvolvedPersonRoleType.FeaturedArtist);

            string title = QoItem.Title;
            
            string[] featPatterns = { 
                "featuring ", " ft.",
                "(feat ", "(feat.",
                "[feat ", "[feat.", 
                " feat ", " feat. ", 
                "[ft ", "[ft.", 
                "(ft ", "(ft." 
            };
            // Note: using multiple IndexOf calls instead of Regex is preferable here for performance.
            bool hasFeat = featPatterns.Any(p => title.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);

            if (hasFeat)
            {
                // If the title already contains "feat."-like word, set the featuredPerformers to null.
                featuredPerformers = null;

                // Also, remove any main artists that appear in the track title, except the first main artist.
                // Case: Qobuz API returns the featured artists as "Main Artist".
                if (mainPerformers != null && mainPerformers.Length > 1)
                {
                    string titleNorm = PerformersParser.Normalize(QoItem.Title);

                    // Keep the first main artist.
                    string firstArtist = mainPerformers[0];

                    // Filter the rest
                    string[] filteredArtists = mainPerformers
                        .Skip(1)
                        .Where(mp => titleNorm.IndexOf(PerformersParser.Normalize(mp), StringComparison.OrdinalIgnoreCase) < 0)
                        .ToArray();

                    // Combine first artist with the filtered rest
                    mainPerformers = new[] { firstArtist }.Concat(filteredArtists).ToArray();
                }
            }

            // Merge main artists + featured artists
            string trackArtists = ParsingHelper.MergeFeaturedArtistsWithMainArtists(mainPerformers, featuredPerformers);

            string performerName;

            // Use merged main artists + featured artists if available
            if (!string.IsNullOrEmpty(trackArtists))
            {
                performerName = trackArtists;
            }
            else
            {
                // Fallback: single performer name from QoItem.Performer
                performerName = ParsingHelper.DecodeEncodedNonAsciiCharacters(QoItem.Performer?.Name);
            }

            // Final fallback: album artist name
            if (string.IsNullOrEmpty(performerName))
            {
                performerName = ParsingHelper.DecodeEncodedNonAsciiCharacters(QoItem.Album?.Artist?.Name);
            }

            // Case: the main artist name (QoItem.Performer.Name) or the name extracted from the artist role
            // is a composed name that includes a "Feat" word without a dot, for example: "David Feat Dj Mago, MainArtist".
            performerName = performerName.Replace(" Feat ", " Feat. ").
                                          Replace(" feat ", " Feat. ").
                                          Replace(" Featuring ", " Feat. ").
                                          Replace(" featuring ", " Feat. ");

            return performerName;
        }

        // Adapted by ElektroStudios from QobuzDownloaderX-MOD's source-code to use a QopenAPI.Album object.
        public static string[] GetAlbumArtistsNames(QopenAPI.Album QoAlbum)
        {
            string AlbumArtist;
            string[] AlbumArtists;
            AlbumArtists = ParsingHelper.GetArtistNames(QoAlbum.Artists, InvolvedPersonRoleType.MainArtist);
            string[] featuredArtists = ParsingHelper.GetArtistNames(QoAlbum.Artists, InvolvedPersonRoleType.FeaturedArtist);
            string albumArtists = ParsingHelper.MergeFeaturedArtistsWithMainArtists(AlbumArtists, featuredArtists);
            // Add Features Artists to Album Artists.
            AlbumArtists = AlbumArtists.Concat(featuredArtists).ToArray();
            if (!string.IsNullOrEmpty(albumArtists))
            {
                // User Main-Artists by default
                AlbumArtist = albumArtists;
            }
            else
            {
                AlbumArtist = ParsingHelper.DecodeEncodedNonAsciiCharacters(QoAlbum.Artist.Name);
            }
            // Qobuz doesn't return an array of Albumartists for compilations, so use singular AlbumArtist
            if (AlbumArtists.Length < 1)
            {
                AlbumArtists = new string[] { AlbumArtist };
            }

            return AlbumArtists;
        }
    }
}
