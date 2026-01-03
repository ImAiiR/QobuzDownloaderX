using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{
    // Inspired by QobuzDownloaderX-MOD source-code:
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD/blob/master/QobuzDownloaderX/Shared/Tools/PerformersParser.cs
    internal sealed class PerformersParser
    {
        private readonly List<string> mainArtists = new List<string>();
        private readonly List<string> featuredArtists = new List<string>();

        // Regex to match time-like patterns: 00:00, 1:23, etc.
        private static readonly Regex timePattern = new Regex(@"^\d{1,2}:\d{2}$", RegexOptions.Compiled);

        public PerformersParser(QopenAPI.Item QoItem)
        {
            // Variable to store the total different MainArtist role types detected
            // (e.g., "MainArtist", "Artist" = 2 role types)
            HashSet<string> detectedMainArtistRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Variable to store the total different FeaturedArtist role types detected
            // (e.g., "Featured Artist", "Featuring" = 2 role types)
            HashSet<string> detectedFeaturedArtistRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var performersFullString = QoItem?.Performers ?? "";

            if (!string.IsNullOrWhiteSpace(performersFullString))
            {
                // The performersFullString can be a multi-line string that contains line breaks.
                // We join them into a single-line string.
                performersFullString = NormalizePerformersLineBreaks(performersFullString);
            }

            if (string.IsNullOrWhiteSpace(performersFullString))
            {
                if (!string.IsNullOrWhiteSpace(QoItem?.Performer?.Name))
                    AddIfNotExists(mainArtists, QoItem.Performer.Name);
                return;
            }

#if DEBUG
            bool writeperformersFullString = false;
            if (writeperformersFullString)
            {
                File.AppendAllText(@".\_DEBUG_performersFullString.txt", Environment.NewLine + performersFullString);
            }
#endif

            var segments = performersFullString
                .Split(new[] { " - " }, StringSplitOptions.None)
                .Select(s => s.Trim())
                .ToArray();

            // merge comma-less tokens with the next token if next token is MainArtist
            var rebuiltSegments = new List<string>();
            for (int i = 0; i < segments.Length; i++)
            {
                string current = segments[i];
                // If current segment contains no comma
                if (!current.Contains(","))
                {
                    // If next segment exists AND is a MainArtist or FeaturedArtist
                    if (i + 1 < segments.Length && (IsMainArtistSegment(segments[i + 1]) || IsFeaturedArtistSegment(segments[i + 1])))
                    {
                        string merged = current + " - " + segments[i + 1];
                        rebuiltSegments.Add(merged);
                        // Skip the next one because it has been merged
                        i++;
                        continue;
                    }
                    else
                    {
                        // Ignore this token (invalid orphan token)
                        continue;
                    }
                }
                // Normal token, keep as-is.
                rebuiltSegments.Add(current);
            }
            // Replace original segments with rebuilt list.
            segments = rebuiltSegments.ToArray();

            // Iterate through each segment to extract the name and classify its roles.
            foreach (var seg in segments)
            {
                int commaIndex = seg.IndexOf(',');
                if (commaIndex == -1) continue;

                string name = seg.Substring(0, commaIndex).Trim();

                string rolesPart = seg.Substring(commaIndex + 1).Trim();

                var roles = rolesPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(r => r.Trim())
                                     .ToList();

                // Store roles that match MainArtist or FeaturedArtist mappings
                foreach (var role in roles)
                {
                    if (InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.MainArtist].Contains(role))
                    {
                        detectedMainArtistRoles.Add(role);
                    } else if (InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.MainArtist].Contains(role))
                    {
                        detectedMainArtistRoles.Add(role);
                    }
                }
                
                if (roles.Any(r => InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.MainArtist].Contains(r)))
                    AddIfNotExists(mainArtists, name);
                else if (roles.Any(r => InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.FeaturedArtist].Contains(r)))
                    AddIfNotExists(featuredArtists, name);
            }

            // Remove possible duplicates between featured and main artists (1st time).
            featuredArtists.RemoveAll(f => mainArtists.Any(m => AreEquivalentNames(f, m, onlyExactMatch: true)));

            // Ensure that QoItem.Performer.Name stays first in the ordering.
            string mainPerf = QoItem?.Performer?.Name?.Trim();

            // Safety checks for time-like patterns:
            bool mainPerfIsTimeLikePattern = !string.IsNullOrWhiteSpace(mainPerf) && timePattern.IsMatch(mainPerf);
            if (mainPerfIsTimeLikePattern) mainPerf = null;
            // Remove any entries in featuredArtists that match time-like patterns like "nn:nn" (e.g., "06:38")
            featuredArtists.RemoveAll(f => timePattern.IsMatch(f.Trim()));

            if (!string.IsNullOrWhiteSpace(mainPerf))
            {
                string mainPerfNorm = Normalize(mainPerf);

                if (mainArtists.Count == 1)
                {
                    string singlePerf = mainArtists[0];
                    string singlePerfNorm = Normalize(singlePerf);

                    // Case: after normalizing the strings, the single entry name equally matches the main performer name.
                    //       This could happen if a dash is missing in either the main performer name or the single entry name,
                    //       or if the word "DJ" appears at the start or end of one of the names, etc.
                    //       It's difficult to determine which name is more accurate,
                    //       so we simply use the single entry name, as the difference is not significant.
                    if (AreEquivalentNames(singlePerf, mainPerf, onlyExactMatch: true))
                    {
                        return;
                    }
                    // Case: the main performer name contains part of the single entry name
                    //       but is not exactly equal, as the main performer name also includes
                    //       additional artist names separated by commas.
                    else if (mainPerfNorm.Contains(singlePerfNorm) &&
                             !string.Equals(mainPerfNorm, singlePerfNorm, StringComparison.OrdinalIgnoreCase) &&
                             mainPerf.Contains(", "))
                    {
                        // Split names by comma and trim each one.
                        var splitted = mainPerf.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(name => name.Trim())
                                               .Where(name => !string.IsNullOrWhiteSpace(name))
                                               .ToList();

                        // Replace the single entry array with the new splitted names list.
                        mainArtists.Clear();
                        mainArtists.AddRange(splitted);
                        return;
                    }
                    // Case: QoItem.Performer.Name is actually a featured artist, not the main artist.
                    //       i.e., Qobuz has assigned the wrong main artist name.
                    else if (featuredArtists.Any(f => AreEquivalentNames(f, mainPerf, onlyExactMatch: true)))
                    {
                        return;
                    }
                    // Case: QoItem.Performer.Name is totally different than the main artist name in the full performers string.
                    //       This could be due one is the real person name and the other is the artistical pseudonym,
                    //       for example: Danny Masseling -> Angerfist
                    //       In this case we preffer the name from the full performers string,
                    //       which we assume it is the artistical pseudonym.
                    else if (!mainPerfNorm.Contains(singlePerfNorm))
                    {
                        if (mainArtists[0].Length < 3) // safety measure.
                        {
                            mainArtists[0] = mainPerf;
                        }

                    }
                    else // Unsure what to expect here, so we fallback to main performer name for safety.
                    {
                        mainArtists[0] = mainPerf;
                    }
                }
                else
                {
                    int idx = mainArtists.FindIndex(m => AreEquivalentNames(m, mainPerf, onlyExactMatch: true));

                    if (idx > 0)
                    {
                        mainArtists.RemoveAt(idx);
                        mainArtists.Insert(0, mainPerf);
                    }
                    else if (idx == -1)
                    {
                        if (detectedMainArtistRoles.Count > 1)
                        {
                            // Remove possible duplicates in main artists array, and preffer the longer version.
                            // For example, MainArtist:"Deadmau5"(shorter) -> Artist:"Chris Lake & Deadmau5"(longer)
                            // Case: https://play.qobuz.com/album/0811869163979
                            //       "Chris Lake, MainArtist - Deadmau5, MainArtist - Chris Lake & Deadmau5, Artist"
                            var mainArtistsCopy = new List<string>(mainArtists);
                            for (int i = 0; i < mainArtistsCopy.Count; i++)
                            {
                                string current = mainArtistsCopy[i];

                                // Find the index of an equivalent name that is longer than the current one
                                int idxMainArtists = mainArtistsCopy.FindIndex(m => AreEquivalentNames(m, current, onlyExactMatch: false) && m.Length > current.Length);

                                if (idxMainArtists >= 0)
                                {
                                    // Remove the shorter version
                                    // MessageBox.Show($"Removing shorter version: {current} (duplicate with {mainArtistsCopy[idxMainArtists]})");
                                    mainArtistsCopy.RemoveAt(i);
                                    i--; // Adjust index due to removal
                                }
                            }
                            mainArtists = mainArtistsCopy;
                        }
                    }

                    // Remove possible duplicates between featured and main artists (2nd time).
                    featuredArtists.RemoveAll(f => mainArtists.Any(m => AreEquivalentNames(f, m, onlyExactMatch: true)));
                }
            }

            // Ensure at least one main artists remains after all previous modifications.
            if (mainArtists.Count == 0) { 
                mainArtists.Add(QoItem.Performer.Name); 
            };

#if DEBUG
            bool showMessageBoxStrings = false;
            if (showMessageBoxStrings)
            {
                 MessageBox.Show(performersFullString, "performersFullString");
                 MessageBox.Show(QoItem.Performer.Name, "QoItem.Performer.Name");
                 MessageBox.Show(string.Join(Environment.NewLine, mainArtists), "main artists");
                 MessageBox.Show(string.Join(Environment.NewLine, featuredArtists), "featuredArtists");
            }
#endif

        }

        // Required to handle comparisons where 'QoItem.Performer.Name' slightly differs from the name in 'QoItem.Performers'.
        public static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "";

            // Trim and normalize accents
            string normalized = s.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (char c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);

                // Keep letters and digits
                if (uc == UnicodeCategory.UppercaseLetter ||
                    uc == UnicodeCategory.LowercaseLetter ||
                    uc == UnicodeCategory.DecimalDigitNumber)
                {
                    sb.Append(c);
                }
                // Ignore accents
                else if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(' '); // replace any other character with a space
                }
            }

            // Convert to lowercase
            string result = sb.ToString().ToLowerInvariant();

            // Replace multiple spaces with a single space
            result = RenameTemplates.spacesRegex.Replace(result, " ");

            return result.Trim();
        }

        /// <summary>
        /// Normalizes a multi-line string by removing line breaks, invisible characters,
        /// collapsing multiple spaces, and trimming the result.
        /// </summary>
        /// <param name="input">The multi-line string to normalize.</param>
        /// <returns>A single-line cleaned string.</returns>
        public static string NormalizePerformersLineBreaks(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // 1) Replace all known line breaks/separators with a space
            input = input
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\u0085", " ")   // NEL
                .Replace("\u2028", " ")   // line separator
                .Replace("\u2029", " ");  // paragraph separator

            // 2) Remove problematic invisible characters
            input = input
                .Replace("\u00A0", " ")   // NBSP -> space
                .Replace("\u200B", "")    // zero-width space -> nothing
                .Replace("\uFEFF", "");   // BOM -> nothing

            // 3) Collapse multiple spaces into a single space (without regex)
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }

            // 4) Trim leading and trailing spaces
            return input.Trim();
        }

        // returns true if the segment contains a MainArtist role
        private bool IsMainArtistSegment(string segment)
        {
            int commaIndex = segment.IndexOf(',');
            if (commaIndex == -1)
                return false;

            string rolesPart = segment.Substring(commaIndex + 1).Trim();
            var roles = rolesPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(r => r.Trim());

            return roles.Any(r =>
                InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.MainArtist].Contains(r));
        }

        // returns true if the segment contains a FeaturedArtist role
        private bool IsFeaturedArtistSegment(string segment)
        {
            int commaIndex = segment.IndexOf(',');
            if (commaIndex == -1)
                return false;

            string rolesPart = segment.Substring(commaIndex + 1).Trim();
            var roles = rolesPart.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(r => r.Trim());

            return roles.Any(r =>
                InvolvedPersonRoleMapping.RoleMappings[InvolvedPersonRoleType.FeaturedArtist].Contains(r));
        }

        // returns true if two names are considered the same person
        private static bool AreEquivalentNames(string a, string b, bool onlyExactMatch)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                return false;

            string na = Normalize(a);
            string nb = Normalize(b);
          
            // Compare words ignoring order.
            // Casse: one name can be specified as "Mikel Molina" and the other as "Molina Mikel", changing the surname position.
            //        the word "Dj" is positioned at the opposite end of the string, for example: "Dj Alexis" -> "Alexis Dj".
            var wordsA = new HashSet<string>(na.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);
            var wordsB = new HashSet<string>(nb.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);
            if (wordsA.SetEquals(wordsB))
                return true;

            if (!onlyExactMatch)
            {
                // If one normalized string contains the other, consider them equivalent
                // Case: one of the names can be longer if for example it contains the surname.
                if (na.IndexOf(nb, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    nb.IndexOf(na, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private static void AddIfNotExists(List<string> list, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                string existing = list[i];
                if (AreEquivalentNames(existing, name, onlyExactMatch: false))
                {
                    // Prefer the longer (more complete) original string (not normalized)
                    // If the incoming name is longer than the existing, replace it
                    if (name.Length > existing.Trim().Length)
                    {
                        list[i] = name;
                    }
                    return; // Equivalent already present (kept or replaced)
                }
            }

            // No equivalent found
            list.Add(name);
        }

        public string[] GetPerformersWithRole(InvolvedPersonRoleType role)
        {
            return role == InvolvedPersonRoleType.MainArtist ? mainArtists.ToArray() : featuredArtists.ToArray();
        }
    }
}