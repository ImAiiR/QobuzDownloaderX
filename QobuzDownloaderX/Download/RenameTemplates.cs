﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using QobuzDownloaderX;
using QopenAPI;
using System.Text.RegularExpressions;

namespace QobuzDownloaderX
{
    class RenameTemplates
    {
        // For converting illegal filename characters to an underscore.
        public string GetSafeFilename(string filename)
        {
            Console.WriteLine(string.Join("_", filename.TrimEnd().TrimEnd('.').TrimEnd('.').Split(Path.GetInvalidFileNameChars())));
            return string.Join("_", filename.TrimEnd().TrimEnd('.').TrimEnd('.').Split(Path.GetInvalidFileNameChars()));
        }

        public string renameTemplates(string template, int paddedTrackLength, int paddedDiscLength, string fileFormat, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Renaming user template - " + template);

            // Keep backslashes to be used to make new folders
            if (template.Contains(Path.DirectorySeparatorChar))
            {
                template = template.Replace(@"\", "{backslash}").Replace(@"/", "{forwardslash}");
            }

            // Artist Templates

            // Track Templates
            if (QoItem != null)
            {
                template = GetSafeFilename(template.Replace("%TrackID%", QoItem.Id.ToString()));
                template = GetSafeFilename(template.Replace("%TrackArtist%", QoItem.Performer.Name.ToString()));
                if (QoItem.Composer != null) { template = GetSafeFilename(template.Replace("%TrackComposer%", QoItem.Composer.Name.ToString())); }
                if (QoItem.Version == null) { template = GetSafeFilename(template.Replace("%TrackTitle%", QoItem.Title)); } else { template = GetSafeFilename(template.Replace("%TrackTitle%", QoItem.Title.TrimEnd() + " (" + QoItem.Version + ")")); }
                template = GetSafeFilename(template.Replace("%TrackNumber%", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0')));
                template = GetSafeFilename(template.Replace("%ISRC%", QoItem.ISRC.ToString()));
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPA%", "Explicit")); } else { template = GetSafeFilename(template.Replace("%TrackPA%", "Clean")); }
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPAifEx%", "Explicit")); } else { template = GetSafeFilename(template.Replace("%TrackPAifEx%", "")); }
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPAifCl%", "")); } else { template = GetSafeFilename(template.Replace("%TrackPAifCl%", "Clean")); }
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPAEnclosed%", "(Explicit)")); } else { template = GetSafeFilename(template.Replace("%TrackPAEnclosed%", "(Clean)")); }
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPAifExEnclosed%", "(Explicit)")); } else { template = GetSafeFilename(template.Replace("%TrackPAifExEnclosed%", "")); }
                if (QoItem.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%TrackPAifClEnclosed%", "")); } else { template = GetSafeFilename(template.Replace("%TrackPAifClEnclosed%", "(Clean)")); }

                // Track Format Templates
                template = GetSafeFilename(template.Replace("%TrackFormat%", fileFormat.ToUpper().TrimStart('.')));

                if (qbdlxForm._qbdlxForm.format_id == "5" || qbdlxForm._qbdlxForm.format_id == "6")
                {
                    template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
                }
                else if (qbdlxForm._qbdlxForm.format_id == "7" || qbdlxForm._qbdlxForm.format_id == "27")
                {
                    if (QoItem.MaximumBitDepth == 16)
                    {
                        template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
                    }
                    else if (QoItem.MaximumSamplingRate < 192)
                    {
                        if (QoItem.MaximumSamplingRate < 96)
                        {
                            template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (" + QoItem.MaximumBitDepth.ToString() + "bit-" + QoItem.MaximumSamplingRate.ToString() + "kHz)"));
                        }
                        else if (QoItem.MaximumSamplingRate > 96 && QoItem.MaximumSamplingRate < 192)
                        {
                            if (qbdlxForm._qbdlxForm.format_id == "7" && QoItem.MaximumSamplingRate == 176.4) { template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-88.2kHz)")); }
                            else if (qbdlxForm._qbdlxForm.format_id == "7") { template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-96kHz)")); }
                            else { template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (" + QoItem.MaximumBitDepth.ToString() + "bit-" + QoItem.MaximumSamplingRate.ToString() + "kHz)")); }
                        }
                        else
                        {
                            template = GetSafeFilename(template.Replace("%TrackFormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-96kHz)"));
                        }
                    }
                    else
                    {
                        template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-192kHz)"));
                    }
                }
            }
            
            // Album Templates
            if (QoAlbum != null)
            {
                template = GetSafeFilename(template.Replace("%AlbumID%", QoAlbum.Id.ToString()));
                template = GetSafeFilename(template.Replace("%AlbumURL%", QoAlbum.Url.ToString()));
                template = GetSafeFilename(template.Replace("%AlbumGenre%", QoAlbum.Genre.Name));
                try { template = GetSafeFilename(template.Replace("%AlbumComposer%", QoAlbum.Composer.Name)); } catch { }
                if (QoAlbum.Version == null) { template = GetSafeFilename(template.Replace("%AlbumTitle%", GetSafeFilename(QoAlbum.Title))); } else { template = GetSafeFilename(template.Replace("%AlbumTitle%", GetSafeFilename(QoAlbum.Title).TrimEnd() + " (" + QoAlbum.Version + ")")); }
                template = GetSafeFilename(template.Replace("%Label%", Regex.Replace(QoAlbum.Label.Name, @"\s+", " "))); // Qobuz sometimes has multiple spaces in place of where a single space should be when it comes to Labels
                template = GetSafeFilename(template.Replace("%Copyright%", QoAlbum.Copyright));
                template = GetSafeFilename(template.Replace("%UPC%", QoAlbum.UPC));
                template = GetSafeFilename(template.Replace("%ReleaseDate%", QoAlbum.ReleaseDateOriginal));
                template = GetSafeFilename(template.Replace("%Year%", UInt32.Parse(QoAlbum.ReleaseDateOriginal.Substring(0, 4)).ToString()));
                template = GetSafeFilename(template.Replace("%ReleaseType%", char.ToUpper(QoAlbum.ProductType.First()) + QoAlbum.ProductType.Substring(1).ToLower()));
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPA%", "Explicit")); } else { template = GetSafeFilename(template.Replace("%AlbumPA%", "Clean")); }
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPAifEx%", "Explicit")); } else { template = GetSafeFilename(template.Replace("%AlbumPAifEx%", "")); }
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPAifCl%", "")); } else { template = GetSafeFilename(template.Replace("%AlbumPAifCl%", "Clean")); }
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPAEnclosed%", "(Explicit)")); } else { template = GetSafeFilename(template.Replace("%AlbumPAEnclosed%", "(Clean)")); }
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPAifExEnclosed%", "(Explicit)")); } else { template = GetSafeFilename(template.Replace("%AlbumPAifExEnclosed%", "")); }
                if (QoAlbum.ParentalWarning == true) { template = GetSafeFilename(template.Replace("%AlbumPAifClEnclosed%", "")); } else { template = GetSafeFilename(template.Replace("%AlbumPAifClEnclosed%", "(Clean)")); }
                template = GetSafeFilename(template.Replace("%Format%", fileFormat.ToUpper().TrimStart('.')));

                // For albums with multiple main artists listed
                if (QoAlbum.Artists.Count > 1)
                {
                    var mainArtists = QoAlbum.Artists.Where(a => a.Roles.Contains("main-artist")).ToList();
                    string allButLastArtist = string.Join(", ", mainArtists.Take(QoAlbum.Artists.Count - 1).Select(a => a.Name));
                    string lastArtist = mainArtists.Last().Name;
                    template = GetSafeFilename(template.Replace("%ArtistName%", allButLastArtist + " & " + lastArtist));
                }
                else
                {
                    template = GetSafeFilename(template.Replace("%ArtistName%", QoAlbum.Artist.Name));
                }
            }

            // Playlist Templates
            if (QoPlaylist != null)
            {
                template = GetSafeFilename(template.Replace("%PlaylistID%", QoPlaylist.Id.ToString()));
                template = GetSafeFilename(template.Replace("%PlaylistTitle%", QoPlaylist.Name));
                template = GetSafeFilename(template.Replace("%Format%", fileFormat.ToUpper().TrimStart('.')));
                template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
            }

            // Release Format Templates
            if (QoPlaylist == null)
            {
                template = GetSafeFilename(template.Replace("%Format%", fileFormat.ToUpper().TrimStart('.')));

                if (qbdlxForm._qbdlxForm.format_id == "5" || qbdlxForm._qbdlxForm.format_id == "6")
                {
                    template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
                }
                else if (qbdlxForm._qbdlxForm.format_id == "7" || qbdlxForm._qbdlxForm.format_id == "27")
                {
                    if (QoAlbum.MaximumBitDepth == 16)
                    {
                        template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
                    }
                    else if (QoAlbum.MaximumSamplingRate < 192)
                    {
                        if (QoAlbum.MaximumSamplingRate < 96)
                        {
                            template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (" + QoAlbum.MaximumBitDepth.ToString() + "bit-" + QoAlbum.MaximumSamplingRate.ToString() + "kHz)"));
                        }
                        else if (QoAlbum.MaximumSamplingRate > 96 && QoAlbum.MaximumSamplingRate < 192)
                        {
                            if (qbdlxForm._qbdlxForm.format_id == "7" && QoAlbum.MaximumSamplingRate == 176.4) { template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-88.2kHz)")); }
                            else if (qbdlxForm._qbdlxForm.format_id == "7") { template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-96kHz)")); }
                            else { template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (" + QoAlbum.MaximumBitDepth.ToString() + "bit-" + QoAlbum.MaximumSamplingRate.ToString() + "kHz)")); }
                        }
                        else
                        {
                            template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-96kHz)"));
                        }
                    }
                    else
                    {
                        template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.') + " (24bit-192kHz)"));
                    }
                }
            }

            qbdlxForm._qbdlxForm.logger.Debug("Template output - " + template.Replace("{backslash}", @"\").Replace("{forwardslash}", @"/"));
            return template.Replace("{backslash}", @"\").Replace("{forwardslash}", @"/");
        }
    }
}
