using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{
    // Credits to QobuzDownloaderX-MOD author(s):
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD
    internal class ParsingHelper
    {
        static readonly string primaryListSeparator = ", ";
        static readonly string listEndSeparator = " & ";

        // Adapted by ElektroStudios to use a QopenAPI.ArtistsList object
        //
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
            string mergedFeaturedArtists = MergeDoubleDelimitedList(featuresArtists, primaryListSeparator, primaryListSeparator);

            if (string.IsNullOrEmpty(mergedFeaturedArtists))
            {
                return mergedMainArtists;
            }
            else
            {
                return $"{mergedMainArtists} Feat. {mergedFeaturedArtists}";
            }

        }

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

        /// <summary>
        /// Decodes the encoded non ascii characters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The decoded string.</returns>
        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            if (value != null)
            {
                return Regex.Replace(
                    value,
                    @"\\u(?<Value>[a-zA-Z0-9]{4})",
                    m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
            }
            else
            {
                return null;
            }
        }

        // Adapted by ElektroStudios from QobuzDownloaderX-MOD's source-code to use a QopenAPI.Item object.
        // Also, handle cases where the track title already contains " Feat. "-like strings (case-insensitive).
        // (i.e., does not add featured artists names to the final string.)
        public static string GetTrackPerformersName(QopenAPI.Item QoItem)
        {
            PerformersParser performersParser = new PerformersParser(QoItem);

            // Get main and featured performers
            string[] mainPerformers = performersParser.GetPerformersWithRole(InvolvedPersonRoleType.MainArtist);
            string[] featuredPerformers = performersParser.GetPerformersWithRole(InvolvedPersonRoleType.FeaturedArtist);

            string title = QoItem.Title;
            if (title.IndexOf("[feat.", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("(feat.", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf(" feat. ", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf(" feat ", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                featuredPerformers = null;
            }
            // MessageBox.Show(string.Join(", ", mainPerformers), "Main Performers");
            // MessageBox.Show(string.Join(", ", featuredPerformers), "Featured Artist");

            // Merge main + featured
            string trackArtists = ParsingHelper.MergeFeaturedArtistsWithMainArtists(mainPerformers, featuredPerformers);

            string performerName;

            // Use merged main + featured artists if available
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
