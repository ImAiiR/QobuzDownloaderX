using System;
using System.Collections.Generic;
using System.Linq;

namespace QobuzDownloaderX.Helpers.QobuzDownloaderXMOD
{

    // Credits to QobuzDownloaderX-MOD author(s):
    // https://github.com/DJDoubleD/QobuzDownloaderX-MOD
    public static class InvolvedPersonRoleMapping
    {
        public static readonly Dictionary<InvolvedPersonRoleType, List<string>> RoleMappings = new Dictionary<InvolvedPersonRoleType, List<string>>
       {
           { InvolvedPersonRoleType.Miscellaneous, new List<string> { 
               "A&R Director", "A&R", "AAndRAdministrator", "Additional Production",
               "AHH", "Assistant Mixer", "AssistantEngineer", "Assistant Producer", 
               "Asst. Recording Engineer", "AssociatedPerformer",
               "Author", "Choir", "Chorus Master", "Contractor", "Co-Producer", 
               "Engineer", "Masterer", "Mastering Engineer", "MasteringEngineer",
               "Misc.Prod.", "Mixer", "Mixing Engineer", "Music Production", 
               "Orchestra", "Performance Arranger", "Programming", "Programmer",
               "RecordingEngineer", "Soloist", "StudioPersonnel", "Vocals", "Writer"
           } }, // Default if not mapped!
           { InvolvedPersonRoleType.Composer, new List<string> { 
               "Composer", "ComposerLyricist", "Composer-Lyricist" 
           } },
           { InvolvedPersonRoleType.Conductor, new List<string> { 
               "Conductor" 
           } },
           { InvolvedPersonRoleType.FeaturedArtist, new List<string> {
               "FeaturedArtist", "Featuring", "featured-artist" 
           } },
           { InvolvedPersonRoleType.Instruments, new List<string> { 
               "Bass Guitar", "Cello", "Drums", "Guitar", "Horn", 
               "Keyboards", "Percussion", "Piano", "Trombone", "Tuba", 
               "Trumpet", "Viola", "Violin" 
           } },
           { InvolvedPersonRoleType.Lyricist, new List<string> { 
               "Lyricist", "ComposerLyricist", "Composer-Lyricist" 
           } },
           { InvolvedPersonRoleType.MainArtist, new List<string> { 
               "MainArtist", "main-artist", "Performer" 
           } },
           { InvolvedPersonRoleType.MixArtist, new List<string> { 
               "Remixer", "Re-Mixer"
           } },
           { InvolvedPersonRoleType.Producer, new List<string> { 
               "Producer"
           } },
           { InvolvedPersonRoleType.Publisher, new List<string> { 
               "Publisher", "MusicPublisher" 
           } }
       };

        // UNUSED
        // ======
        //
        //public static List<string> GetStringsByRole(InvolvedPersonRoleType role)
        //{
        //    if (RoleMappings.TryGetValue(role, out var strings))
        //    {
        //        return strings;
        //    }
        //    return new List<string>();
        //}

        public static InvolvedPersonRoleType GetRoleByString(string involvedPersonString)
        {
            // We just return PerformerRoleType.Miscellaneous if no match as we don't know all possible values
            // Search is done by ignoring case for maximum success.
            return RoleMappings.FirstOrDefault(kvp => kvp.Value.Contains(involvedPersonString, StringComparer.OrdinalIgnoreCase)).Key;
        }
    }
}
