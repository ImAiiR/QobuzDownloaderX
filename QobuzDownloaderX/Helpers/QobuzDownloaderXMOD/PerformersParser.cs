using System;
using System.Collections.Generic;
using System.Linq;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{
    // Credits to QobuzDownloaderX-MOD author(s):
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD
    internal class PerformersParser
    {
        private readonly List<KeyValuePair<string, List<string>>> performers;

        // Improved revision by ElektroStudios:
        //
        //  - Correctly preserves the order of performer names, fixing the original mis-ordering issue.
        //    (i.e., ensures the main performer (QoItem.Performer.Name) always appears first in the resulting string.)
        //
        //  - Properly handles performer names that contain " - " inside the name segment.
        //    (i.e., add the names to the resulting string.)
        //
        //  - Properly handles performer names that contain "," inside the name segment.
        //    (i.e., add the names to the resulting string.)
        public PerformersParser(QopenAPI.Item QoItem)
        {
            string performersFullString = QoItem?.Performers;
            performers = new List<KeyValuePair<string, List<string>>>();

            if (string.IsNullOrWhiteSpace(performersFullString))
                return;

            // Split segments by " - " because some roles include "-" without spaces.
            var segments = performersFullString
                .Split(new[] { " - " }, StringSplitOptions.None)
                .Select(s => s.Trim())
                .ToArray();

            // Flatten all role words for fast checking (case-sensitive)
            var allRoleWords = InvolvedPersonRoleMapping.RoleMappings
                .Values
                .SelectMany(list => list)
                .Distinct(StringComparer.Ordinal)
                .ToList();

            // Helper to add or merge a performer entry while preserving the original order.
            //
            // - If the performer name already exists, merge the new roles with the existing ones.
            // - If the performer name does not exist, append it at the end of the list.
            void AddOrMerge(string name, IEnumerable<string> roles)
            {
                // Ignore empty or whitespace-only names.
                if (string.IsNullOrWhiteSpace(name))
                    return;

                // Look for an existing performer entry with the same name (case-insensitive).
                var idx = performers.FindIndex(kvp =>
                    string.Equals(kvp.Key, name, StringComparison.OrdinalIgnoreCase));

                // Normalize and deduplicate incoming roles (case-sensitive).
                var rolesList = roles?
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Select(r => r.Trim())
                    .Distinct(StringComparer.Ordinal)
                    .ToList() ?? new List<string>();

                if (idx >= 0)
                {
                    // Performer already exists: merge roles with existing ones (case-sensitive).
                    var existing = performers[idx];
                    var merged = existing.Value
                        .Concat(rolesList)
                        .Distinct(StringComparer.Ordinal)
                        .ToList();

                    // Replace original entry with updated role list.
                    performers[idx] = new KeyValuePair<string, List<string>>(existing.Key, merged);
                }
                else
                {
                    // New performer: append to list while preserving order.
                    performers.Add(new KeyValuePair<string, List<string>>(name, rolesList));
                }
            }

            // Current name segments (joined with " - " on commit)
            var currentNameParts = new List<string>();

            for (int i = 0; i < segments.Length; i++)
            {
                var seg = segments[i];
                // tokens inside a segment are comma-separated
                var tokens = seg.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(t => t.Trim())
                                .ToList();

                // Does this segment contain at least one exact role token? (case-sensitive)
                bool segHasRole = 
                    tokens.Any(tok => allRoleWords.Any(role => string.Equals(tok, role, StringComparison.Ordinal)));

                if (segHasRole)
                {
                    // Tokens that are roles vs. tokens that are name parts (case-sensitive)
                    var roleTokens = 
                        tokens.Where(tok => allRoleWords.Any(role => string.Equals(tok, role, StringComparison.Ordinal)))
                              .ToList();
                    var nameTokens = tokens.Except(roleTokens).ToList();

                    // Append name tokens to current name parts (if any).
                    if (nameTokens.Count > 0)
                    {
                        // Join multiple nameTokens back with "," because they came from the same segment.
                        // Do not add white-space when joining this.
                        currentNameParts.Add(string.Join(",", nameTokens));
                    }

                    // Form full name and add performer
                    var fullName = string.Join(" - ", currentNameParts).Trim();
                    if (string.IsNullOrWhiteSpace(fullName) && nameTokens.Count > 0)
                        fullName = string.Join(", ", nameTokens);

                    AddOrMerge(fullName, roleTokens);

                    // Reset name parts for the next performer
                    currentNameParts = new List<string>();
                }
                else
                {
                    // No role in this segment: it's a name fragment, keep it for later
                    // rejoin tokens with comma in case the fragment itself had commas
                    currentNameParts.Add(string.Join(", ", tokens));
                }
            }

            // NOTE: if there are leftover name parts without roles at the end, we ignore them
            // because there's no role to attach. If you want to keep them, you could add them as
            // performers with an empty role list here.

            // Ensure main performer name stays first in the list (case-sensitive, normalizing dashes)
            if (!string.IsNullOrWhiteSpace(QoItem.Performer.Name))
            {
                // The dash replacement handles cases where 'QoItem.Performer.Name' contains " - " but 'QoItem.Performers' don't.
                string mainPerformerNormalized = QoItem.Performer.Name.Replace(" - ", " ");
                var idx = performers.FindIndex(kvp =>
                    string.Equals(kvp.Key.Replace(" - ", " "), mainPerformerNormalized, StringComparison.Ordinal));

                if (idx > 0)
                {
                    var main = performers[idx];
                    performers.RemoveAt(idx);
                    performers.Insert(0, main);
                }
            }
        }
        // ORIGINAL IMPLEMENTATION
        // =======================
        //
        // Example album:
        //   https://play.qobuz.com/album/p3n6tx6svbgja
        //
        // Current (incorrect) order:
        //   Tamika & S3rl Feat. Amy - Here We Go (2021 Digital Re-Master) (Original Mix)
        //
        // Correct order should be like this:
        //   S3rl & Tamika Feat. Amy - Here We Go (2021 Digital Re-Master) (Original Mix)
        //     -> https://play.qobuz.com/album/x08a1xibhtzea
        //        (Artist name: S3rl, Tamika)
        //     -> https://www.qobuz.com/album/here-we-go-freakshow-its-time-2-roll-s3rl-feat-amy-tamika/x08a1xibhtzea
        //        (Artist name: S3RL Feat. Amy & Tamika)
        //     -> https://www.discogs.com/release/1723312-S3RL-Here-We-Go-Freak-Show-Its-Time-2-Roll
        //        (Artist name: S3RL Feat. Amy & Tamika)
        // ==========================================================================================================
        //
        //public PerformersParser(QopenAPI.Item QoItem)
        //{
        //    string mainPerformerName = QoItem.Performer.Name;
        //    string performersFullString = QoItem.Performers;
        //    performers = new Dictionary<string, List<string>>();
        //    if (!string.IsNullOrEmpty(performersFullString))
        //    {
        //        performers = performersFullString
        //            .Split(new string[] { " - " }, StringSplitOptions.None) // Split performers by " - " because some roles include '-'
        //            .Select(performer => performer.Split(',')) // Split name & roles in best effort by ',', first part is name, next parts roles
        //            .GroupBy(parts => parts[0].Trim()) // Group performers by name since they can occure multiple times
        //            .ToDictionary(group => group.Key,
        //                          group => group.SelectMany(parts => parts.Skip(1).Select(role => role.Trim())).Distinct().ToList()); // Flatten roles by performer and remove duplicates
        //    }
        //}

        // Adapted to rely on: private readonly List<KeyValuePair<string, List<string>>> performers;
        public string[] GetPerformersWithRole(InvolvedPersonRoleType role)
        {
            IEnumerable<string> roleStrings = InvolvedPersonRoleMapping.RoleMappings.TryGetValue(role, out var list)
                ? list
                : Enumerable.Empty<string>();

            return performers
                .Where(kvp => kvp.Value.Any(value => roleStrings.Contains(value, StringComparer.Ordinal))) // (case -sensitive)
                .Select(kvp => kvp.Key)
                .ToArray();
        }
        // ORIGINAL IMPLEMENTATION
        // =======================
        //
        //public string[] GetPerformersWithRole(InvolvedPersonRoleType role)
        //{
        //    var roleStrings = InvolvedPersonRoleMapping.GetStringsByRole(role);
        //    return performers.Keys
        //        .Where(key => performers[key].Exists(value => roleStrings.Contains(value, StringComparer.OrdinalIgnoreCase)))
        //        .ToArray();
        //}
    }
}
