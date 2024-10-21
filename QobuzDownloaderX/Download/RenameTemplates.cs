using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using QobuzDownloaderX;
using QopenAPI;

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

            }

            // Album Templates
            if (QoAlbum != null)
            {
                template = GetSafeFilename(template.Replace("%AlbumID%", QoAlbum.Id.ToString()));
                template = GetSafeFilename(template.Replace("%AlbumURL%", QoAlbum.Url.ToString()));
                template = GetSafeFilename(template.Replace("%AlbumGenre%", QoAlbum.Genre.Name));
                try { template = GetSafeFilename(template.Replace("%AlbumComposer%", QoAlbum.Composer.Name)); } catch { }
                if (QoAlbum.Version == null) { template = GetSafeFilename(template.Replace("%AlbumTitle%", GetSafeFilename(QoAlbum.Title))); } else { template = GetSafeFilename(template.Replace("%AlbumTitle%", GetSafeFilename(QoAlbum.Title).TrimEnd() + " (" + QoAlbum.Version + ")")); }
                template = GetSafeFilename(template.Replace("%Label%", QoAlbum.Label.Name));
                template = GetSafeFilename(template.Replace("%Copyright%", QoAlbum.Copyright));
                template = GetSafeFilename(template.Replace("%UPC%", QoAlbum.UPC));
                template = GetSafeFilename(template.Replace("%ReleaseDate%", QoAlbum.ReleaseDateOriginal));
                template = GetSafeFilename(template.Replace("%Year%", UInt32.Parse(QoAlbum.ReleaseDateOriginal.Substring(0, 4)).ToString()));
                template = GetSafeFilename(template.Replace("%ReleaseType%", char.ToUpper(QoAlbum.ProductType.First()) + QoAlbum.ProductType.Substring(1).ToLower()));
                template = GetSafeFilename(template.Replace("%ArtistName%", QoAlbum.Artist.Name));
                template = GetSafeFilename(template.Replace("%Format%", fileFormat.ToUpper().TrimStart('.')));
            }

            // Playlist Templates
            if (QoPlaylist != null)
            {
                template = GetSafeFilename(template.Replace("%PlaylistID%", QoPlaylist.Id.ToString()));
                template = GetSafeFilename(template.Replace("%PlaylistTitle%", QoPlaylist.Name));
                template = GetSafeFilename(template.Replace("%Format%", fileFormat.ToUpper().TrimStart('.')));
                template = GetSafeFilename(template.Replace("%FormatWithHiResQuality%", fileFormat.ToUpper().TrimStart('.')));
            }

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
