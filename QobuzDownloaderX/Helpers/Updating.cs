using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace QobuzDownloaderX.Helpers
{
    public class TranslationUpdater
    {
        // List of language files
        private static readonly Dictionary<string, string> LanguageFiles = new Dictionary<string, string>
        {
            { "en.json", "Languages/en.json" },
            { "ru.json", "Languages/ru.json" },
            { "zh-cn.json", "Languages/zh-cn.json" }
        };

        public static async Task CheckAndUpdateLanguageFiles()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "TranslationUpdater");

                foreach (var languageFile in LanguageFiles)
                {
                    string fileName = languageFile.Key;
                    string localFilePath = languageFile.Value;

                    string apiUrl = $"https://api.github.com/repos/ImAiiR/QobuzDownloaderX/contents/QobuzDownloaderX/Resources/{localFilePath}";

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            JObject fileMetadata = JObject.Parse(jsonResponse);

                            // Retrieve the download URL for the remote file
                            string downloadUrl = fileMetadata["download_url"]?.ToString();
                            if (!string.IsNullOrEmpty(downloadUrl))
                            {
                                // Fetch the remote file content
                                string remoteContent = await client.GetStringAsync(downloadUrl);

                                // Parse the "TranslationUpdatedOn" field from the remote file
                                JObject remoteJson = JObject.Parse(remoteContent);
                                string remoteUpdatedOnString = remoteJson["TranslationUpdatedOn"]?.ToString();

                                // Parse the local file's "TranslationUpdatedOn" field
                                if (File.Exists(localFilePath.ToLower()))
                                {
                                    string localContent = File.ReadAllText(localFilePath.ToLower());
                                    JObject localJson = JObject.Parse(localContent);
                                    string localUpdatedOnString = localJson["TranslationUpdatedOn"]?.ToString();

                                    // Compare updated date
                                    if (remoteUpdatedOnString != localUpdatedOnString)
                                    {
                                        File.WriteAllText(localFilePath.ToLower(), remoteContent);
                                        qbdlxForm._qbdlxForm.logger.Debug($"File {fileName} updated successfully.");
                                    }
                                    else
                                    {
                                        qbdlxForm._qbdlxForm.logger.Debug($"File {fileName} is already up-to-date.");
                                    }
                                }
                                else
                                {
                                    // Local file does not exist, download it
                                    File.WriteAllText(localFilePath.ToLower(), remoteContent);
                                    qbdlxForm._qbdlxForm.logger.Debug($"File {fileName} downloaded successfully.");
                                }
                            }
                            else
                            {
                                qbdlxForm._qbdlxForm.logger.Error($"Failed to retrieve the download URL for {fileName}.");
                            }
                        }
                        else
                        {
                            qbdlxForm._qbdlxForm.logger.Error($"Failed to fetch metadata for {fileName}: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        qbdlxForm._qbdlxForm.logger.Error($"Error updating {fileName}: {ex.Message}");
                    }
                }
            }
        }
    }

    public static class VersionChecker
    {
        public static async Task<(bool isUpdateAvailable, string newVersion, string currentVersion, string changes)> CheckForUpdate()
        {
            string changes = string.Empty;
            string currentVersion = string.Empty;
            string newVersion = string.Empty;
            bool isUpdateAvailable = false;

            try
            {
                // Initialize HttpClient to grab version number from GitHub
                using (var versionURLClient = new HttpClient())
                {
                    qbdlxForm._qbdlxForm.logger.Debug("versionURLClient initialized");

                    // Configure TLS for secure connection
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    // Set user-agent to Firefox
                    versionURLClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0");

                    // Request the latest release from GitHub
                    qbdlxForm._qbdlxForm.logger.Debug("Starting request for latest GitHub version");
                    var versionURL = "https://api.github.com/repos/ImAiiR/QobuzDownloaderX/releases/latest";
                    var versionURLResponse = await versionURLClient.GetAsync(versionURL);
                    string versionURLResponseString = await versionURLResponse.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    JObject joVersionResponse = JObject.Parse(versionURLResponseString);

                    // Extract version number and changelog
                    newVersion = (string)joVersionResponse["tag_name"];
                    qbdlxForm._qbdlxForm.logger.Debug("Received version from GitHub: " + newVersion);
                    changes = (string)joVersionResponse["body"];

                    // Get the current version from the assembly
                    currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    // Compare versions
                    if (!currentVersion.Contains(newVersion))
                    {
                        isUpdateAvailable = true;
                        qbdlxForm._qbdlxForm.logger.Debug("New version available: " + newVersion);
                    }
                    else
                    {
                        qbdlxForm._qbdlxForm.logger.Debug("Current version matches the latest version.");
                    }
                }
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("Connection to GitHub failed: " + ex.Message);
            }

            return (isUpdateAvailable, newVersion, currentVersion, changes);
        }
    }
}
