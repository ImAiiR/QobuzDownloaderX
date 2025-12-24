using System;
using System.Collections.Generic;
using System.Linq;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{
    // Credits to QobuzDownloaderX-MOD author(s).
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD/blob/master/QobuzDownloaderX/Shared/Tools/InvolvedPersonRoleMapping.cs
    public static class InvolvedPersonRoleMapping
    {
        // Only the role dictionaries we care about for this parser.
        public static readonly Dictionary<InvolvedPersonRoleType, List<string>> RoleMappings = new Dictionary<InvolvedPersonRoleType, List<string>>
        {
            {
                InvolvedPersonRoleType.MainArtist, new List<string> {
                    "Artist", // This unique variant was found on many "Various Artists" albums.
                    "Main Artist",
                    "MainArtist",
                    "main-artist",
                    "Performer",
                    "Primary"
            }},
            {
                InvolvedPersonRoleType.FeaturedArtist, new List<string> {
                    "Featured Artist",
                    "FeaturedArtist",
                    "featured-artist", // This variant was not found during my tests, but who knows? It's better to have it.
                    "Featuring",
                    "Featuring Vocals",
                    "Featuring Vocalist"
            }}
        };

        public static InvolvedPersonRoleType GetRoleByString(string involvedPersonString)
        {
            // We just return InvolvedPersonRoleType.Unknown if no match.
            // Search is done by ignoring case for maximum success.
            return InvolvedPersonRoleMapping.RoleMappings.
                FirstOrDefault(kvp => kvp.Value.Contains(involvedPersonString, StringComparer.OrdinalIgnoreCase)).Key;
        }
    }
}
