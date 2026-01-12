using QobuzDownloaderX.Helpers.QobuzDownloaderXMOD;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.UI.DevCase.UI.Components;
using QobuzDownloaderX.Win32;
using QopenAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class Miscellaneous
    {
        [DebuggerStepThrough]
        internal static void SetTLSSetting()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12;

            if (Settings.Default.useTLS13)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
            }
        }

        [DebuggerStepThrough]
        internal static void SafeSetClipboardText(string text)
        {
            var thread = new Thread(() =>
            {
                Clipboard.SetText(text);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        [DebuggerStepThrough]
        internal static void ClearOldLogs()
        {
            if (!Settings.Default.clearOldLogs) return;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "logs");

            if (!Directory.Exists(folderPath))
                return;

            try
            {
                foreach (string file in Directory.GetFiles(folderPath, "*.log", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore file deletion errors
                    }
                }
            }
            catch
            {
                // Ignore directory access errors
            }
        }

        [DebuggerStepThrough]
        internal static void DeleteFilesFromTempFolder()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "qbdlx-temp");

            if (!Directory.Exists(folderPath))
                return;

            try
            {
                foreach (string file in Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore file deletion errors
                    }
                }
            }
            catch
            {
                // Ignore directory access errors
            }
        }

        [DebuggerStepThrough]
        internal static void ToggleMainFormVisibility(qbdlxForm f)
        {
            void act()
            {
                if (f.Visible)
                {
                    f.Hide();
                    if (qbdlxForm.lastTaskBarProgressMaxValue > 0)
                    {
                        using (Icon baseIcon = (Icon)Resources.QBDLX_Icon1.Clone())
                        {
                            NotifyIconHelper.RenderNotifyIconProgressBar(
                                f.notifyIcon1,
                                baseIcon,
                                qbdlxForm.ntfyProgressBar,
                                qbdlxForm.lastTaskBarProgressCurrentValue,
                                qbdlxForm.lastTaskBarProgressMaxValue);
                        }
                    }
                }
                else
                {
                    if (!f.Visible)
                    {
                        f.Show();
                        f.notifyIcon1.Icon = Resources.QBDLX_Icon1;
                    }

                    f.WindowState = FormWindowState.Normal;
                    Process pr = Process.GetCurrentProcess();
                    IntPtr hwnd = pr.MainWindowHandle;
                    if (NativeMethods.IsIconic(hwnd))
                        NativeMethods.ShowWindow(hwnd, Constants.SW_RESTORE);

                    f.BringToFront();
                    f.Activate();

                    TaskbarHelper.SetProgressValue(qbdlxForm.lastTaskBarProgressCurrentValue, qbdlxForm.lastTaskBarProgressMaxValue);
                    TaskbarHelper.SetProgressState(qbdlxForm.lastTaskBarProgressState);
                }
            }

            if (f.InvokeRequired)
            {
                f.Invoke((Action)(() => act()));
            }
            else
            {
                act();
            }
        }

        internal static void update(qbdlxForm f, string text)
        {
            f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = text));
        }

        internal static void updateTemplates(qbdlxForm f)
        {
            f.logger.Debug("Updating templates");
            f.artistTemplate = f.artistTemplateTextbox.Text;
            f.albumTemplate = f.albumTemplateTextbox.Text;
            f.trackTemplate = f.trackTemplateTextbox.Text;
            f.playlistTemplate = f.playlistTemplateTextbox.Text;
            f.favoritesTemplate = f.favoritesTemplateTextbox.Text;
        }

        internal static void LoadSavedTemplates(qbdlxForm f)
        {
            f.artistTemplateTextbox.Text = Settings.Default.savedArtistTemplate;
            f.albumTemplateTextbox.Text = Settings.Default.savedAlbumTemplate;
            f.trackTemplateTextbox.Text = Settings.Default.savedTrackTemplate;
            f.vaTrackTemplateTextbox.Text = Settings.Default.savedVaTrackTemplate;
            f.playlistTemplateTextbox.Text = Settings.Default.savedPlaylistTemplate;
            f.favoritesTemplateTextbox.Text = Settings.Default.savedFavoritesTemplate;
            updateTemplates(f);
        }

        internal static void LoadQualitySettings(qbdlxForm f)
        {
            f.format_id = Settings.Default.qualityFormat;
            f.audio_format = Settings.Default.audioType;

            f.mp3Button2.Checked = Settings.Default.quality1;
            f.flacLowButton2.Checked = Settings.Default.quality2;
            f.flacMidButton2.Checked = Settings.Default.quality3;
            f.flacHighButton2.Checked = Settings.Default.quality4;

            UpdateQualitySelectButtonText(f);
        }

        internal static void UpdateQualitySelectButtonText(qbdlxForm f)
        {
            string baseText = f.languageManager?.GetTranslation("qualitySelectButton");

            switch (true)
            {
                case var _ when f.mp3Button2.Checked:
                    f.qualitySelectButton.Text = baseText + " (MP3 320)";
                    break;

                case var _ when f.flacLowButton2.Checked:
                    f.qualitySelectButton.Text = baseText + " (FLAC LOW)";
                    break;

                case var _ when f.flacMidButton2.Checked:
                    f.qualitySelectButton.Text = baseText + " (FLAC MID)";
                    break;

                case var _ when f.flacHighButton2.Checked:
                    f.qualitySelectButton.Text = baseText + " (FLAC HIGH)";
                    break;

                default:
                    break;
            }

        }

        internal static void LoadTaggingSettings(qbdlxForm f)
        {
            f.albumTitleCheckbox.Checked = Settings.Default.albumTag;
            f.albumArtistCheckbox.Checked = Settings.Default.albumArtistTag;
            f.trackArtistCheckbox.Checked = Settings.Default.artistTag;
            f.composerCheckbox.Checked = Settings.Default.composerTag;
            f.copyrightCheckbox.Checked = Settings.Default.copyrightTag;
            f.labelCheckbox.Checked = Settings.Default.labelTag;
            f.discNumberCheckbox.Checked = Settings.Default.discTag;
            f.discTotalCheckbox.Checked = Settings.Default.totalDiscsTag;
            f.genreCheckbox.Checked = Settings.Default.genreTag;
            f.isrcCheckbox.Checked = Settings.Default.isrcTag;
            f.urlCheckbox.Checked = Settings.Default.urlTag;
            f.releaseTypeCheckbox.Checked = Settings.Default.typeTag;
            f.explicitCheckbox.Checked = Settings.Default.explicitTag;
            f.trackTitleCheckbox.Checked = Settings.Default.trackTitleTag;
            f.trackNumberCheckbox.Checked = Settings.Default.trackTag;
            f.trackTotalCheckbox.Checked = Settings.Default.totalTracksTag;
            f.upcCheckbox.Checked = Settings.Default.upcTag;
            f.releaseDateCheckbox.Checked = Settings.Default.yearTag;
            f.coverArtCheckbox.Checked = Settings.Default.imageTag;
            f.commentCheckbox.Checked = Settings.Default.commentTag;
            f.commentTextbox.Text = Settings.Default.commentText;
            f.embeddedArtSizeSelect.SelectedIndex = Settings.Default.savedEmbeddedArtSize;
            f.savedArtSizeSelect.SelectedIndex = Settings.Default.savedSavedArtSize;
            f.dontSaveArtworkToDiskCheckBox.Checked = Settings.Default.dontSaveArtworkToDisk;
            f.downloadAllFromArtistCheckBox.Checked = Settings.Default.downloadAllFromArtist;
            f.primaryListSeparatorTextBox.Text = Settings.Default.primaryListSeparator;
            f.listEndSeparatorTextBox.Text = Settings.Default.listEndSeparator;
            ParsingHelper.primaryListSeparator = Settings.Default.primaryListSeparator;
            ParsingHelper.listEndSeparator = Settings.Default.listEndSeparator;
        }

        internal static void LoadOtherSettings(qbdlxForm f)
        {
            f.streamableCheckbox.Checked = Settings.Default.streamableCheck;
            f.useTLS13Checkbox.Checked = Settings.Default.useTLS13;
            f.fixMD5sCheckbox.Checked = Settings.Default.fixMD5s;
            f.downloadGoodiesCheckbox.Checked = Settings.Default.downloadGoodies;
            f.downloadSpeedCheckbox.Checked = Settings.Default.showDownloadSpeed;
            f.clearOldLogsCheckBox.Checked = Settings.Default.clearOldLogs;
            f.downloadAllFromArtistCheckBox.Checked = Settings.Default.downloadAllFromArtist;
            f.mergeArtistNamesCheckbox.Checked = Settings.Default.mergeArtistNames;
            f.artistNamesSeparatorsPanel.Enabled = f.mergeArtistNamesCheckbox.Checked;
            f.useItemPosInPlaylistCheckbox.Checked = Settings.Default.useItemPosInPlaylist;
            f.showTipsCheckBox.Checked = Settings.Default.showTips;
            RestoreDownloadFromArtistSelectedIndices();
        }

        internal static void SaveDownloadFromArtistSelectedIndices()
        {
            var lb = qbdlxForm._qbdlxForm.downloadFromArtistListBox;

            var selectedIndices = lb.CheckedIndices.Cast<int>();

            string indicesString = string.Join(",", selectedIndices);

            Settings.Default.downloadFromArtistSelectedIndices = indicesString;
            Settings.Default.Save();
        }

        private static void RestoreDownloadFromArtistSelectedIndices()
        {
            if (Settings.Default.downloadAllFromArtist) return;

            var lb = qbdlxForm._qbdlxForm.downloadFromArtistListBox;

            string indicesString = Settings.Default.downloadFromArtistSelectedIndices;

            if (!string.IsNullOrEmpty(indicesString))
            {
                var indices = indicesString.Split(',')
                                           .Select(s => int.TryParse(s, out int i) ? i : -1)
                                           .Where(i => i >= 0 && i < lb.Items.Count);

                foreach (int i in indices)
                {
                    lb.SetItemChecked(i, true);
                }
            }

            if (lb.CheckedItems.Count == 0 && lb.Items.Count > 0)
            {
                lb.SetItemChecked(0, true);
            }
        }

        internal static void SetDownloadPath(qbdlxForm f)
        {
            f.downloadLocation = Settings.Default.savedFolder;
            f.downloadFolderTextbox.Text = !string.IsNullOrEmpty(f.downloadLocation) ? f.downloadLocation : f.downloadFolderPlaceholder;
            f.folderBrowser.SelectedPath = f.downloadLocation;
            f.logger.Info("Saved download path: " + f.folderBrowser.SelectedPath);
        }

        internal static void InitializePanels(qbdlxForm f)
        {
            // Set all panels to specific point
            var panelPosition = new Point(179, 0);
            f.downloaderPanel.Location = panelPosition;
            f.aboutPanel.Location = panelPosition;
            f.settingsPanel.Location = panelPosition;
            f.extraSettingsPanel.Location = panelPosition;
            f.searchPanel.Location = panelPosition;

            // Startup with downloadPanel active and visable, all others not visable
            f.downloaderPanel.Visible = true;
            f.aboutPanel.Visible = false;
            f.settingsPanel.Visible = false;
            f.downloadPanelActive = true;
            f.downloaderButton.BackColor = ColorTranslator.FromHtml(f._themeManager._currentTheme.HighlightedButtonBackground);
        }

        internal static void InitializeTheme(qbdlxForm f)
        {
            // Populate theme options in settings
            f._themeManager.PopulateThemeOptions(f);

            // Set and load theme
            f.themeName = Settings.Default.currentTheme;
            if (!string.IsNullOrEmpty(f.themeName)) { f.themeComboBox.SelectedItem = f.themeName; }
            f.theme = f._themeManager._currentTheme;
        }

        internal static void InitializeLanguage(qbdlxForm f)
        {
            // Set saved language
            f.languageManager = new LanguageManager();
            f.languageManager.LoadLanguage($"languages/{Settings.Default.currentLanguage.ToLower()}.json");

            // Populate theme options in settings
            f.languageManager.PopulateLanguageComboBox(f);
            f.languageComboBox.SelectedItem = Settings.Default.currentLanguage.ToUpper();

            // Load theme
            UpdateUILanguage(f);
        }

        private static void UpdateUILanguage(qbdlxForm f)
        {
            // Load the font name from the translation file
            string fontName = f.languageManager.GetTranslation("TranslationFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                // Call method to update fonts
                f.languageManager.UpdateControlFont(f.Controls, fontName);
            }

            /* Update labels, buttons, textboxes, etc., based on the loaded language */

            // Buttons
            f.additionalSettingsButton.Text = f.languageManager.GetTranslation("additionalSettingsButton");
            f.aboutButton.Text = f.languageManager.GetTranslation("aboutButton");
            f.closeAdditionalButton.Text = f.languageManager.GetTranslation("closeAdditionalButton");
            f.downloadButton.Text = f.languageManager.GetTranslation("downloadButton");
            f.batchDownloadButton.Text = f.languageManager.GetTranslation("batchDownloadButton");
            f.abortButton.Text = f.languageManager.GetTranslation("abortButton");
            f.skipButton.Text = f.languageManager.GetTranslation("skipButton");
            f.downloaderButton.Text = f.languageManager.GetTranslation("downloaderButton");
            f.logoutButton.Text = f.languageManager.GetTranslation("logoutButton");
            f.openFolderButton.Text = f.languageManager.GetTranslation("openFolderButton");
            f.qualitySelectButton.Text = f.qualitySelectButton.Text.Insert(0, f.languageManager.GetTranslation("qualitySelectButton"));
            f.resetTemplatesButton.Text = f.languageManager.GetTranslation("resetTemplatesButton");
            f.saveTemplatesButton.Text = f.languageManager.GetTranslation("saveTemplatesButton");
            f.searchButton.Text = f.languageManager.GetTranslation("searchButton");
            f.searchAlbumsButton.Text = f.languageManager.GetTranslation("searchAlbumsButton");
            f.searchTracksButton.Text = f.languageManager.GetTranslation("searchTracksButton");
            f.selectFolderButton.Text = f.languageManager.GetTranslation("selectFolderButton");
            f.settingsButton.Text = f.languageManager.GetTranslation("settingsButton");
            f.closeBatchDownloadbutton.Text = f.languageManager.GetTranslation("closeBatchDownloadbutton");
            f.getAllBatchDownloadButton.Text = f.languageManager.GetTranslation("getAllBatchDownloadButton");
            f.selectAllRowsButton.Text = f.languageManager.GetTranslation("selectAllRowsButton");
            f.deselectAllRowsButton.Text = f.languageManager.GetTranslation("deselectAllRowsButton");
            f.batchDownloadSelectedRowsButton.Text = f.languageManager.GetTranslation("batchDownloadRowsButton");

            /* Center additional settings button to center of panel */
            f.additionalSettingsButton.Location = new Point((f.settingsPanel.Width - f.additionalSettingsButton.Width) / 2, f.additionalSettingsButton.Location.Y);

            /* Center quality panel to center of quality button */
            f.qualitySelectPanel.Location = new Point(f.qualitySelectButton.Left + (f.qualitySelectButton.Width / 2) - (f.qualitySelectPanel.Width / 2), f.qualitySelectPanel.Location.Y);

            // Labels
            f.aboutLabel.Text = f.languageManager.GetTranslation("aboutButton") + "                                                                                                 ";
            f.advancedOptionsLabel.Text = f.languageManager.GetTranslation("advancedOptionsLabel");
            f.albumTemplateLabel.Text = f.languageManager.GetTranslation("albumTemplateLabel");
            f.artistTemplateLabel.Text = f.languageManager.GetTranslation("artistTemplateLabel");
            f.commentLabel.Text = f.languageManager.GetTranslation("commentLabel");
            f.downloadLabel.Text = f.languageManager.GetTranslation("downloaderButton") + "                                                                                         ";
            f.downloadFolderLabel.Text = f.languageManager.GetTranslation("downloadFolderLabel");
            f.downloadOptionsLabel.Text = f.languageManager.GetTranslation("downloadOptionsLabel");
            f.embeddedArtLabel.Text = f.languageManager.GetTranslation("embeddedArtLabel");
            f.extraSettingsLabel.Text = f.languageManager.GetTranslation("extraSettingsLabel");
            f.languageLabel.Text = f.languageManager.GetTranslation("languageLabel");
            f.playlistTemplateLabel.Text = f.languageManager.GetTranslation("playlistTemplateLabel");
            f.favoritesTemplateLabel.Text = f.languageManager.GetTranslation("favoritesTemplateLabel");
            f.savedArtLabel.Text = f.languageManager.GetTranslation("savedArtLabel");
            f.searchLabel.Text = f.languageManager.GetTranslation("searchButton") + "                                                                                               ";
            f.searchingLabel.Text = f.languageManager.GetTranslation("searchingLabel");
            f.settingsLabel.Text = f.languageManager.GetTranslation("settingsButton") + "                                                                                           ";
            f.taggingOptionsLabel.Text = f.languageManager.GetTranslation("taggingOptionsLabel");
            f.templatesLabel.Text = f.languageManager.GetTranslation("templatesLabel");
            f.templatesListLabel.Text = f.languageManager.GetTranslation("templatesListLabel");
            f.themeLabel.Text = f.languageManager.GetTranslation("themeLabel");
            f.themeSectionLabel.Text = f.languageManager.GetTranslation("themeSectionLabel");
            f.downloadFromArtistLabel.Text = f.languageManager.GetTranslation("downloadFromArtistLabel");
            f.trackTemplateLabel.Text = f.languageManager.GetTranslation("trackTemplateLabel");
            f.vaTrackTemplateLabel.Text = f.languageManager.GetTranslation("vaTrackTemplateLabel");
            f.userInfoLabel.Text = f.languageManager.GetTranslation("userInfoLabel");
            f.disclaimerLabel.Text = f.languageManager.GetTranslation("disclaimer");
            f.welcomeLabel.Text = f.languageManager.GetTranslation("welcomeLabel");
            f.batchDownloadLabel.Text = f.languageManager.GetTranslation("batchDownloadLabel");
            f.limitSearchResultsLabel.Text = f.languageManager.GetTranslation("limitSearchResultsLabel");
            f.searchSortingLabel.Text = f.languageManager.GetTranslation("searchSortingLabel");
            f.sortReleaseDateLabel.Text = f.languageManager.GetTranslation("sortReleaseDateLabel");
            f.sortGenreLabel.Text = f.languageManager.GetTranslation("sortGenreLabel");
            f.sortArtistNameLabel.Text = f.languageManager.GetTranslation("sortArtistNameLabel");
            f.sortAlbumTrackNameLabel.Text = f.languageManager.GetTranslation("sortAlbumTrackNameLabel");
            f.sortingSearchResultsLabel.Text = f.languageManager.GetTranslation("sortingSearchResultsLabel");
            f.selectedRowsCountLabel.Text = string.Empty;
            f.primaryListSeparatorLabel.Text = f.languageManager.GetTranslation("primaryListSeparatorLabel");
            f.listEndSeparatorLabel.Text = f.languageManager.GetTranslation("listEndSeparatorLabel");
            f.playlistSectionLabel.Text = f.languageManager.GetTranslation("playlistSectionLabel");
            f.prevTipButton.Text = f.languageManager.GetTranslation("prevTip");
            f.nextTipButton.Text = f.languageManager.GetTranslation("nextTip");

            // Checkboxes
            f.albumArtistCheckbox.Text = f.languageManager.GetTranslation("albumArtistCheckbox");
            f.albumTitleCheckbox.Text = f.languageManager.GetTranslation("albumTitleCheckbox");
            f.trackArtistCheckbox.Text = f.languageManager.GetTranslation("trackArtistCheckbox");
            f.trackTitleCheckbox.Text = f.languageManager.GetTranslation("trackTitleCheckbox");
            f.releaseDateCheckbox.Text = f.languageManager.GetTranslation("releaseDateCheckbox");
            f.releaseTypeCheckbox.Text = f.languageManager.GetTranslation("releaseTypeCheckbox");
            f.genreCheckbox.Text = f.languageManager.GetTranslation("genreCheckbox");
            f.trackNumberCheckbox.Text = f.languageManager.GetTranslation("trackNumberCheckbox");
            f.trackTotalCheckbox.Text = f.languageManager.GetTranslation("trackTotalCheckbox");
            f.discNumberCheckbox.Text = f.languageManager.GetTranslation("discNumberCheckbox");
            f.discTotalCheckbox.Text = f.languageManager.GetTranslation("discTotalCheckbox");
            f.composerCheckbox.Text = f.languageManager.GetTranslation("composerCheckbox");
            f.explicitCheckbox.Text = f.languageManager.GetTranslation("explicitCheckbox");
            f.coverArtCheckbox.Text = f.languageManager.GetTranslation("coverArtCheckbox");
            f.copyrightCheckbox.Text = f.languageManager.GetTranslation("copyrightCheckbox");
            f.labelCheckbox.Text = f.languageManager.GetTranslation("labelCheckbox");
            f.upcCheckbox.Text = f.languageManager.GetTranslation("upcCheckbox");
            f.isrcCheckbox.Text = f.languageManager.GetTranslation("isrcCheckbox");
            f.urlCheckbox.Text = f.languageManager.GetTranslation("urlCheckbox");
            f.mergeArtistNamesCheckbox.Text = f.languageManager.GetTranslation("mergeArtistNamesCheckbox");
            f.streamableCheckbox.Text = f.languageManager.GetTranslation("streamableCheckbox");
            f.fixMD5sCheckbox.Text = f.languageManager.GetTranslation("fixMD5sCheckbox");
            f.downloadSpeedCheckbox.Text = f.languageManager.GetTranslation("downloadSpeedCheckbox");
            f.sortAscendantCheckBox.Text = f.languageManager.GetTranslation("sortAscendantCheckBox");
            f.downloadGoodiesCheckbox.Text = f.languageManager.GetTranslation("downloadGoodiesCheckbox");
            f.useTLS13Checkbox.Text = f.languageManager.GetTranslation("useTLS13Checkbox");
            f.dontSaveArtworkToDiskCheckBox.Text = f.languageManager.GetTranslation("dontSaveArtworkToDiskCheckBox");
            f.downloadAllFromArtistCheckBox.Text = f.languageManager.GetTranslation("downloadAllFromArtistCheckBox");
            f.clearOldLogsCheckBox.Text = f.languageManager.GetTranslation("clearOldLogsCheckBox");
            f.useItemPosInPlaylistCheckbox.Text = f.languageManager.GetTranslation("useItemPosInPlaylistCheckbox");
            f.showTipsCheckBox.Text = f.languageManager.GetTranslation("showTips");

            // downloadFromArtistListBox
            string translatedNames = f.languageManager.GetTranslation("downloadFromArtistListBox");
            string[] names = translatedNames.Split(',');
            for (int i = 0; i < names.Length && i < f.downloadFromArtistListBox.Items.Count; i++)
            {
                f.downloadFromArtistListBox.Items[i] = names[i].Trim();
            }

            /* Center certain checkboxes in panels */
            f.fixMD5sCheckbox.Location = new Point((f.extraSettingsPanel.Width - f.fixMD5sCheckbox.Width) / 2, f.fixMD5sCheckbox.Location.Y);
            f.downloadSpeedCheckbox.Location = new Point((f.extraSettingsPanel.Width - f.downloadSpeedCheckbox.Width) / 2, f.downloadSpeedCheckbox.Location.Y);

            f.streamableCheckbox.Location = new Point(f.fixMD5sCheckbox.Left - 100, f.streamableCheckbox.Location.Y);
            f.useTLS13Checkbox.Location = new Point(f.streamableCheckbox.Right + 16, f.streamableCheckbox.Location.Y);
            f.downloadGoodiesCheckbox.Location = new Point(f.useTLS13Checkbox.Right + 16, f.streamableCheckbox.Location.Y);

            // Context menu items
            f.showWindowToolStripMenuItem.Text = f.languageManager.GetTranslation("showWindowCmItem");
            f.hideWindowToolStripMenuItem.Text = f.languageManager.GetTranslation("hideWindowCmItem");
            f.closeProgramToolStripMenuItem.Text = f.languageManager.GetTranslation("closeProgramCmItem");

            // Placeholders
            f.albumLabelPlaceholder = f.languageManager.GetTranslation("albumLabelPlaceholder");
            f.artistLabelPlaceholder = f.languageManager.GetTranslation("artistLabelPlaceholder");
            f.infoLabelPlaceholder = f.languageManager.GetTranslation("infoLabelPlaceholder");
            f.inputTextboxPlaceholder = f.languageManager.GetTranslation("inputTextboxPlaceholder");
            f.searchTextboxPlaceholder = f.languageManager.GetTranslation("searchTextboxPlaceholder");
            f.downloadFolderPlaceholder = f.languageManager.GetTranslation("downloadFolderPlaceholder");
            f.userInfoTextboxPlaceholder = f.languageManager.GetTranslation("userInfoTextboxPlaceholder");
            f.downloadOutputWelcome = f.languageManager.GetTranslation("downloadOutputWelcome");
            f.downloadOutputExpired = f.languageManager.GetTranslation("downloadOutputExpired");
            f.downloadOutputPath = f.languageManager.GetTranslation("downloadOutputPath");
            f.downloadOutputNoPath = f.languageManager.GetTranslation("downloadOutputNoPath");
            f.downloadOutputNoUrl = f.languageManager.GetTranslation("downloadOutputNoUrl");
            f.downloadOutputAPIError = f.languageManager.GetTranslation("downloadOutputAPIError");
            f.downloadOutputNotImplemented = f.languageManager.GetTranslation("downloadOutputNotImplemented");
            f.downloadOutputCheckLink = f.languageManager.GetTranslation("downloadOutputCheckLink");
            f.downloadOutputTrNotStream = f.languageManager.GetTranslation("downloadOutputTrNotStream");
            f.downloadOutputAlNotStream = f.languageManager.GetTranslation("downloadOutputAlNotStream");
            f.downloadOutputGoodyFound = f.languageManager.GetTranslation("downloadOutputGoodyFound");
            f.downloadOutputGoodyExists = f.languageManager.GetTranslation("downloadOutputGoodyExists");
            f.downloadOutputGoodyNoURL = f.languageManager.GetTranslation("downloadOutputGoodyNoURL");
            f.downloadOutputFileExists = f.languageManager.GetTranslation("downloadOutputFileExists");
            f.downloadOutputDownloading = f.languageManager.GetTranslation("downloadOutputDownloading");
            f.downloadOutputDone = f.languageManager.GetTranslation("downloadOutputDone");
            f.downloadOutputCompleted = f.languageManager.GetTranslation("downloadOutputCompleted");
            f.progressLabelInactive = f.languageManager.GetTranslation("progressLabelInactive");
            f.progressLabelActive = f.languageManager.GetTranslation("progressLabelActive");
            f.formClosingWarning = f.languageManager.GetTranslation("formClosingWarning");
            f.downloadAborted = f.languageManager.GetTranslation("downloadAborted");
            f.albumSkipped = f.languageManager.GetTranslation("albumSkipped");

            // Set the placeholders as needed
            f.inputTextbox.Text = f.inputTextboxPlaceholder;
            f.searchTextbox.Text = f.searchTextboxPlaceholder;
        }

        internal static void updateAlbumInfoLabels(qbdlxForm f, Album QoAlbum)
        {
            string trackOrTracks = "tracks";
            if (QoAlbum.TracksCount == 1) { trackOrTracks = "track"; }
            f.artistLabel.Text = f.renameTemplates.GetReleaseArtists(QoAlbum).Replace("&", "&&");
            if (QoAlbum.Version == null) { f.albumLabel.Text = QoAlbum.Title.Replace(@"&", @"&&"); } else { f.albumLabel.Text = QoAlbum.Title.Replace(@"&", @"&&").TrimEnd() + " (" + QoAlbum.Version + ")"; }
            f.infoLabel.Text = $"{f.infoLabelPlaceholder} {QoAlbum.ReleaseDateOriginal} • {QoAlbum.TracksCount} {trackOrTracks} • {QoAlbum.UPC}";

            try
            {
                string newImageLocation = QoAlbum.Image?.Small;
                if (!((string)f.albumPictureBox.Tag == "playlist") && 
                    !string.Equals(f.albumPictureBox.ImageLocation, newImageLocation, StringComparison.Ordinal))
                {
                    f.albumPictureBox.ImageLocation = newImageLocation;
                    f.albumPictureBox.Cursor = Cursors.Hand;
                }
            }
            catch
            {
                f.albumPictureBox.Cursor = Cursors.Default;
            }
        }

        private static void updatePlaylistInfoLabels(qbdlxForm f, Playlist QoPlaylist)
        {
            f.artistLabel.Text = QoPlaylist.Owner.Name.Replace(@"&", @"&&") + "'s Playlist";
            f.albumLabel.Text = QoPlaylist.Name.Replace(@"&", @"&&");
            f.infoLabel.Text = "";

            try
            {
                string newImageLocation = QoPlaylist.ImageRectangle[0];
                if (!string.Equals(f.albumPictureBox.ImageLocation, newImageLocation, StringComparison.Ordinal))
                {
                    f.albumPictureBox.ImageLocation = newImageLocation;
                    f.albumPictureBox.Cursor = Cursors.Hand;
                }
            } catch
            {
                f.albumPictureBox.Cursor = Cursors.Default;
            }
        }

        internal static void SetPlaceholder(qbdlxForm f, TextBox textBox, string placeholderText, bool isFocused)
        {
            if (isFocused)
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = null;
                    textBox.ForeColor = ColorTranslator.FromHtml(f.theme.TextBoxText);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.ForeColor = ColorTranslator.FromHtml(f.theme.PlaceholderTextBoxText);
                    textBox.Text = placeholderText;
                }
            }
        }

        internal static void ToggleStatusStripVisibility(qbdlxForm f)
        {
            if (f.Tag == null) f.Tag = f.ClientSize.Height; // Save original ClientSize height
            int originalClientHeight = (int)f.Tag;
            bool statusWasVisible = f.statusStrip1.Visible;

            f.SuspendLayout();
            NativeMethods.SendMessage(f.Handle, Constants.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

            f.FormBorderStyle = FormBorderStyle.None;

            if (f.showTipsCheckBox.Checked)
            {
                if (!statusWasVisible)
                {
                    f.statusStrip1.Visible = true;

                    int borderHeight = f.Height - f.ClientSize.Height;
                    f.Height = originalClientHeight + f.statusStrip1.Height + borderHeight;

                    // Adjust position if bottom exceeds the taskbar
                    Rectangle workingArea = Screen.FromControl(f).WorkingArea;
                    int overflow = f.Bottom - workingArea.Bottom;
                    if (overflow > 0)
                    {
                        f.Top -= overflow; // move form up so the extended part is visible
                        if (f.Top < workingArea.Top) f.Top = workingArea.Top; // never move above screen
                    }
                }

                f.tipLabel.AutoSize = false;
                f.tipLabel.Spring = true;
                f.tipLabel.Width = f.statusStrip1.Width - f.prevTipButton.Width - f.nextTipButton.Width - f.tipEmojiLabel.Width - 30;
                f.tipLabel.Overflow = ToolStripItemOverflow.Never;
                f.tipLabel.TextAlign = ContentAlignment.MiddleLeft;

                f.timerTip.Enabled = true;
                f.timerTip.Start();
            }
            else
            {
                if (statusWasVisible)
                {
                    f.statusStrip1.Visible = false;

                    int borderHeight = f.Height - f.ClientSize.Height;
                    f.Height = originalClientHeight + borderHeight;

                    // Adjust position again if necessary
                    Rectangle workingArea = Screen.FromControl(f).WorkingArea;
                    if (f.Bottom > workingArea.Bottom)
                    {
                        f.Top = workingArea.Bottom - f.Height;
                        if (f.Top < workingArea.Top) f.Top = workingArea.Top;
                    }
                }

                f.timerTip.Stop();
                f.timerTip.Enabled = false;
            }

            // Round corners
            f.Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, f.Width, f.Height, 20, 20));

            NativeMethods.SendMessage(f.Handle, Constants.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            f.ResumeLayout(true);
            f.PerformLayout();
        }

        internal static void SetNextTip(qbdlxForm f, bool forward)
        {
            // Adjust the index based on the forward parameter
            if (forward)
            {
                f.currentTipIndex++;
            }
            else
            {
                f.currentTipIndex--;
            }

            // Try to get the translation for the tip
            string tipKey = $"tip{f.currentTipIndex}";
            string tipText = f.languageManager.GetTranslation(tipKey);

            // If the tip doesn't exist, reset or loop around
            if (tipText == tipKey) // Translation not found
            {
                if (forward)
                {
                    f.currentTipIndex = 1; // Reset to the first tip
                }
                else
                {
                    // Go to the last tip if we go back past the first
                    int lastTipIndex = 1;
                    while (f.languageManager.GetTranslation($"tip{lastTipIndex}") != $"tip{lastTipIndex}")
                    {
                        lastTipIndex++;
                    }
                    f.currentTipIndex = lastTipIndex - 1;
                }
                tipText = f.languageManager.GetTranslation($"tip{f.currentTipIndex}");
            }

            // Finally, assign the formatted tip text
            f.currentTipText = "                    " + tipText.PadRight(200, ' ');
        }

        internal static string GetShortenedGenreName(string genre)
        {
            if (string.IsNullOrEmpty(genre))
                return string.Empty;

            // Shorten English genre names
            string genreName = genre
                .Replace("Alternative", "Alt.")
                .Replace("Brazilian", "Brazil.")
                .Replace("Film Soundtracks", "OST")
                .Replace("Latin America", "Latin Am.")
                .Replace("Miscellaneous", "Misc.")
                .Replace(" Music", "")
                .Replace(" music", "")
                .Replace("North America", "North Am.");

            // Shorten Spanish genre names
            genreName = genreName
                .Replace("Alternativa", "Alt.")
                .Replace("América latina", "Latina")
                .Replace("Bandas sonoras de", "BSO")
                .Replace("brasileño", "Brasil.")
                .Replace("Chanson ", "")
                .Replace("Música ", "")
                .Replace("música ", "")
                .Replace("Músicas ", "")
                .Replace("músicas ", "")
                .Replace("Música de ", "")
                .Replace("música de ", "")
                .Replace("Músicas de ", "")
                .Replace("músicas de ", "")
                .Replace("Norteamérica", "Norteam.")
                .Replace("World music", "Mundial");

            // Format before truncating
            genreName = genreName.TrimStart(' ', '&', '-', '.', ',').Replace("&", "&&");
            if (!string.IsNullOrEmpty(genreName))
                genreName = char.ToUpper(genreName[0]) + genreName.Substring(1).TrimEnd();

            // Truncate if too long
            if (genreName.Length > 18)
                genreName = genreName.Substring(0, 18) + "…";

            return genreName;
        }

        internal static void ShowFloatingImageFromUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            try
            {
                string fileName = Path.GetFileName(imageUrl);
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = Guid.NewGuid() + ".jpg";
                else
                    fileName = "Qobuz_" + fileName;

                string tempPath = Path.Combine(Path.GetTempPath(), fileName);

                if (!File.Exists(tempPath))
                {
                    var request = (HttpWebRequest)WebRequest.Create(imageUrl);
                    request.Method = "GET";

                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var fileStream = File.Create(tempPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                using (var img = System.Drawing.Image.FromFile(tempPath))
                using (var fpic = new DevFloatingPicture(img)
                {
                    ImageLayout = ImageLayout.Zoom,
                    BackgroundColor = Color.Black,
                    BackgroundOpacity = 0.75,
                    FitBoundsToWorkingArea = true,
                    TopMost = true,
                    TitleBar = false,
                    ImageBorder = true,
                    ImageBorderColor = Color.FromArgb(255, 30, 30, 30),
                    ImageBorderSize = 6,
                    CloseOnEscapeKey = true,
                    CloseOnLeftMouseClick = true
                })
                {
                    fpic.ShowDialog();
                }
            }
            catch
            {
                // swallow or log if needed
            }
        }

        internal static async Task reorderSearchResultsAsync(qbdlxForm f)
        {
            qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;

            f.logger.Debug("Hiding search buttons (sorting)");
            f.searchAlbumsButton.Visible = false;
            f.searchTracksButton.Visible = false;
            f.sortingSearchResultsLabel.Visible = true;
            f.sortingSearchResultsLabel.Update();
            f.searchResultsPanel.Hide();

            try
            {
                f.logger.Debug("Sorting search results started");
                await Task.Run(() =>
                {
                    if (f.searchPanelHelper.lastSearchType == "releases")
                    {
                        if (f.searchPanelHelper.QoAlbumSearch?.Albums != null)
                        {
                            f.searchPanelHelper.QoAlbumSearch.Albums = f.searchPanelHelper.SortAlbums(f.searchPanelHelper.QoAlbumSearch.Albums);
                            f.searchPanelHelper.PopulateTableAlbums(f, f.searchPanelHelper.QoAlbumSearch);
                        }
                    }
                    else if (f.searchPanelHelper.lastSearchType == "tracks")
                    {
                        if (f.searchPanelHelper.QoTrackSearch?.Tracks != null)
                        {
                            f.searchPanelHelper.QoTrackSearch.Tracks = f.searchPanelHelper.SortTracks(f.searchPanelHelper.QoTrackSearch.Tracks);
                            f.searchPanelHelper.PopulateTableTracks(f, f.searchPanelHelper.QoTrackSearch);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                f.logger.Error("Error occured during reorderSearchResultsAsync(), error below:\r\n" + ex);
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
                f.searchResultsPanel.Show();
                f.searchAlbumsButton.Visible = true;
                f.searchTracksButton.Visible = true;
                f.sortingSearchResultsLabel.Visible = false;
                f.sortingSearchResultsLabel.Update();
                return;
            }
            f.logger.Debug("Sorting completed!");
            qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
            f.searchResultsPanel.Show();
            f.searchAlbumsButton.Visible = true;
            f.searchTracksButton.Visible = true;
            f.sortingSearchResultsLabel.Visible = false;
            f.sortingSearchResultsLabel.Update();
            return;
        }

        internal static string GetCheckedDownloadFromArtistTypes()
        {
            if (Settings.Default.downloadAllFromArtist)
                return "all";

            string[] fixedNames = { "album", "epSingle", "live", "compilation", "download", "other" };

            var result = new List<string>();

            for (int i = 0; i < qbdlxForm._qbdlxForm.downloadFromArtistListBox.Items.Count; i++)
            {
                if (qbdlxForm._qbdlxForm.downloadFromArtistListBox.GetItemChecked(i))
                {
                    result.Add(fixedNames[i]);
                }
            }

            return string.Join(",", result);
        }

        internal static bool ShowDownloadFromArtistWarningIfNeeded()
        {
            string selectedDownloadFromArtistTypes = Miscellaneous.GetCheckedDownloadFromArtistTypes();
            if (string.IsNullOrEmpty(selectedDownloadFromArtistTypes))
            {
                qbdlxForm._qbdlxForm.extraSettingsPanel.Show();
                MessageBox.Show(qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadFromArtistWarning"),
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            return false;
        }

        internal static void DeleteTempEmbeddedArtwork()
        {
            string embeddedArtworkPath = Path.Combine(Path.GetTempPath(), qbdlxForm._qbdlxForm.embeddedArtSize + ".jpg");
            try
            {
                if (!string.IsNullOrEmpty(embeddedArtworkPath) &&
                    ZlpIOHelper.FileExists(embeddedArtworkPath))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Deleting embedded artwork");
                    ZlpIOHelper.DeleteFile(embeddedArtworkPath);
                }
            }
            catch
            {
                qbdlxForm._qbdlxForm.logger.Warning("Unable to delete embedded artwork");
            }
        }

        internal static async Task downloadButtonAsyncWork(qbdlxForm f, DownloadStats stats = null)
        {
            qbdlxForm.getLinkTypeIsBusy = true;
            f.abortTokenSource?.Dispose();
            f.abortTokenSource = null;
            f.abortTokenSource = new CancellationTokenSource();

            f.albumPictureBox.Cursor = default;
            f.albumPictureBox.Tag = "";
            f.albumPictureBox.ImageLocation = "";
            if (f.albumPictureBox.Image == null) f.albumPictureBox.Image = Resources.QBDLX_PictureBox;

            Miscellaneous.DeleteTempEmbeddedArtwork();

            if (stats == null)
            {
                stats = new DownloadStats
                {
                    SpeedWatch = qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked ? Stopwatch.StartNew() : null,
                    CumulativeBytesRead = 0,
                    LastUiBytes = 0,
                    LastUiTimeMs = 0
                };
            }

            try
            {
                f.inputTextbox.Enabled = false;
                f.downloadButton.Enabled = false;
                f.batchDownloadButton.Enabled = false;
                f.abortButton.Enabled = true;
                if (!qbdlxForm.isBatchDownloadRunning)
                {
                    f.batchDownloadProgressCountLabel.Text = "";
                    f.batchDownloadProgressCountLabel.Visible = false;
                }

                await Miscellaneous.getLinkTypeAsync(f, stats, f.abortTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                f.logger.Debug("Download aborted by user.");
                f.downloadOutput.AppendText($"\r\n{f.downloadAborted}");
            }
            finally
            {
                f.skipButton.Enabled = false;
                f.abortButton.Enabled = false;
                f.inputTextbox.Enabled = true;
                f.downloadButton.Enabled = true;
                f.batchDownloadButton.Enabled = true;
                qbdlxForm.skipCurrentAlbum = false;
                qbdlxForm.getLinkTypeIsBusy = false;
            }
        }

        internal static async Task DownloadBatchUrls(qbdlxForm f, HashSet<string> batchUrls)
        {
            f.abortTokenSource?.Dispose();
            f.abortTokenSource = null;

            int batchUrlsCount = batchUrls.Count;
            int batchUrlsCurrentIndex = 0;

            f.batchDownloadProgressCountLabel.Text = "";
            f.batchDownloadProgressCountLabel.Visible = true;
            TaskbarHelper.SetProgressState(TaskbarProgressState.Normal);
            TaskbarHelper.SetProgressValue(0, batchUrlsCount);
            f.batchDownloadProgressCountLabel.Text = $"{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
            f.notifyIcon1.Text = $"QobuzDLX\r\n\r\n{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
            qbdlxForm.isBatchDownloadRunning = true;

            var stats = new DownloadStats
            {
                SpeedWatch = qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked ? Stopwatch.StartNew() : null,
                CumulativeBytesRead = 0,
                LastUiBytes = 0,
                LastUiTimeMs = 0
            };
            foreach (string url in batchUrls)
            {
                batchUrlsCurrentIndex++;

                if (f.abortTokenSource != null && f.abortTokenSource.IsCancellationRequested)
                {
                    TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                    qbdlxForm.isBatchDownloadRunning = false;
                    break;
                }

                f.inputTextbox.Text = url;
                f.inputTextbox.ForeColor = Color.FromArgb(200, 200, 200);

                await downloadButtonAsyncWork(f, stats);

                if (f.abortTokenSource != null && f.abortTokenSource.IsCancellationRequested)
                {
                    break;
                }
                f.batchDownloadProgressCountLabel.Text = $"{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
                f.notifyIcon1.Text = $"QobuzDLX\r\n\r\n{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
                TaskbarHelper.SetProgressValue(batchUrlsCurrentIndex, batchUrlsCount);
            }
            if (!f.Visible) f.notifyIcon1.ShowBalloonTip(5000, "QobuzDLX", f.languageManager.GetTranslation("batchDownloadFinished"), ToolTipIcon.Info);
            f.notifyIcon1.Text = $"QobuzDLX";
            qbdlxForm.isBatchDownloadRunning = false;

            f.batchDownloadSelectedRowsButton.Enabled =
                f.downloadButton.Enabled &&
                !qbdlxForm.getLinkTypeIsBusy &&
                SearchPanelHelper.selectedRowindices.Any();
        }

        private static async Task getLinkTypeAsync(qbdlxForm f, DownloadStats stats, CancellationToken abortToken)
        {
            if (!qbdlxForm.isBatchDownloadRunning)
            {
                TaskbarHelper.SetProgressState(TaskbarProgressState.Normal);
                TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
            }

            IntPtr hwnd = Process.GetCurrentProcess().MainWindowHandle;
            var progress = new Progress<int>(value =>
            {
                f.progressBarDownload.Value = value;
                if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(value, qbdlxForm.lastTaskBarProgressMaxValue);
            });

            f.downloadOutput.Focus();
            f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.downloadOutputCheckLink));
            f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = f.progressBarDownload.Minimum));

            // Check if there's no selected path.
            if (f.downloadLocation == null | f.downloadLocation == "" | f.downloadLocation == "no folder selected")
            {
                // If there is NOT a saved path.
                f.logger.Warning("No path has been set! Remember to Choose a Folder!");
                f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = String.Empty));
                f.downloadOutput.Invoke(new Action(() => f.downloadOutput.AppendText($"{f.downloadOutputNoPath}\r\n")));
                f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                return;
            }

            string albumLink = f.inputTextbox.Text.Trim();
            if (albumLink.EndsWith("/releases", StringComparison.OrdinalIgnoreCase))
            {
                albumLink = albumLink.Substring(0, albumLink.Length - "/releases".Length);
            }

            bool isValidUrl = qbdlxForm.qobuzUrlRegEx.IsMatch(albumLink)
                              && Uri.TryCreate(albumLink, UriKind.Absolute, out Uri uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
            {
                string msg = string.Format(f.languageManager.GetTranslation("invalidUrl"), albumLink);
                f.logger.Error(msg);
                f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = msg));
                f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = msg));
                if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                if (qbdlxForm.isBatchDownloadRunning) MessageBox.Show(f, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var qobuzStoreLinkGrab = qbdlxForm.qobuzStoreLinkRegex.Match(albumLink).Groups;
            var linkRegion = qobuzStoreLinkGrab[1].Value;
            var storeLinkType = qobuzStoreLinkGrab[2].Value;
            var linkName = qobuzStoreLinkGrab[3].Value;
            var qobuzStoreLinkId = qobuzStoreLinkGrab[4].Value;

            if (linkRegion != null)
            {
                if (storeLinkType == "album")
                {
                    albumLink = "https://play.qobuz.com/album/" + qobuzStoreLinkId;
                }
                else if (storeLinkType == "interpreter")
                {
                    albumLink = "https://play.qobuz.com/artist/" + qobuzStoreLinkId;
                }
            }

            var qobuzLinkIdGrab = qbdlxForm.qobuzLinkIdGrabRegex.Match(albumLink).Groups;

            var linkType = qobuzLinkIdGrab[1].Value;
            var qobuzLinkId = qobuzLinkIdGrab[2].Value;
            f.qobuz_id = qobuzLinkId;

            f.downloadTrack.clearOutputText();
            f.getInfo.outputText = null;
            f.getInfo.updateDownloadOutput(f.downloadOutputCheckLink);

            f.progressItemsCountLabel.Text = "";
            f.progressItemsCountLabel.Visible = true;

            switch (linkType)
            {
                case "album":
                    f.skipButton.Enabled = true;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                    await Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    f.QoAlbum = f.getInfo.QoAlbum;
                    if (f.QoAlbum == null)
                    {
                        f.getInfo.updateDownloadOutput($"{f.downloadOutputAPIError}");
                        f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                        break;
                    }
                    f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("album")} | {f.QoAlbum.TracksCount:N0} {f.languageManager.GetTranslation("tracks")}";
                    Miscellaneous.updateAlbumInfoLabels(f, f.QoAlbum);
                    await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(f.app_id, f.qobuz_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret, f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum, progress, stats, abortToken));
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;
                case "track":
                    f.skipButton.Enabled = false;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                    await Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    f.QoItem = f.getInfo.QoItem;
                    f.QoAlbum = f.getInfo.QoAlbum;
                    updateAlbumInfoLabels(f, f.QoAlbum);
                    f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("singleTrack")}";
                    await Task.Run(() => f.downloadTrack.DownloadTrackAsync("track", f.app_id, f.qobuz_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret, f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum, f.QoItem, progress, stats, abortToken));
                    // Say the downloading is finished when it's completed.
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(f.progressBarDownload.Maximum, f.progressBarDownload.Maximum);
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = f.progressBarDownload.Maximum));
                    break;
                case "playlist":
                    f.albumPictureBox.Tag = "playlist";
                    f.skipButton.Enabled = false;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                    await Task.Run(() => f.getInfo.getPlaylistInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    f.QoPlaylist = f.getInfo.QoPlaylist;
                    Miscellaneous.updatePlaylistInfoLabels(f, f.QoPlaylist);
                    int totalTracksPlaylist = f.QoPlaylist.Tracks.Items.Count;
                    int trackIndexPlaylist = 0;
                    foreach (var item in f.QoPlaylist.Tracks.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexPlaylist, totalTracksPlaylist);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("playlist")} | {trackIndexPlaylist:N0} / {totalTracksPlaylist:N0} {f.languageManager.GetTranslation("tracks")}";

                        if (Settings.Default.useItemPosInPlaylist)
                        {
                            item.TrackNumber = item.Position;
                        }

                        trackIndexPlaylist++;
                        try
                        {
                            string track_id = item.Id.ToString();
                            await Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, track_id, f.user_auth_token));
                            f.QoItem = item;
                            f.QoAlbum = f.getInfo.QoAlbum;
                            await Task.Run(() => f.downloadTrack.DownloadPlaylistTrackAsync(linkType,
                                f.app_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                f.downloadLocation, f.trackTemplate, f.playlistTemplate, f.QoAlbum, f.QoItem, f.QoPlaylist,
                                new Progress<int>(value =>
                                {
                                    double scaledValue = (trackIndexPlaylist - 1 + value / 100.0) / totalTracksPlaylist * 100.0;
                                    f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                }), stats, abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexPlaylist, totalTracksPlaylist);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("playlist")} | {trackIndexPlaylist:N0} / {totalTracksPlaylist:N0} {f.languageManager.GetTranslation("tracks")}";

                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;
                case "artist":
                    f.skipButton.Enabled = true;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                    await Task.Run(() => f.getInfo.getArtistInfo(f.app_id, f.qobuz_id, f.user_auth_token));
                    f.QoArtist = f.getInfo.QoArtist;
                    if (f.QoArtist == null || f.QoArtist.Albums == null || f.QoArtist.Albums.Items == null)
                    {
                        string msg = string.Format(f.languageManager.GetTranslation("invalidUrl"), albumLink);
                        f.logger.Error(msg);
                        f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = msg));
                        f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = msg));
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                        if (qbdlxForm.isBatchDownloadRunning) MessageBox.Show(f, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int totalAlbumsArtist = f.QoArtist.Albums.Items.Count;
                    int albumIndexArtist = 0;

                    if (totalAlbumsArtist == 0)
                    {
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("artist")} | 0 {f.languageManager.GetTranslation("albums")}";
                    }

                    foreach (var item in f.QoArtist.Albums.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexArtist, totalAlbumsArtist);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("artist")} | {albumIndexArtist:N0} / {totalAlbumsArtist:N0} {f.languageManager.GetTranslation("albums")}";

                        albumIndexArtist++;
                        try
                        {
                            string album_id = item.Id.ToString();
                            await Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                            f.QoAlbum = f.getInfo.QoAlbum;
                            updateAlbumInfoLabels(f, f.QoAlbum);
                            await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                               f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                               f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                               new Progress<int>(value =>
                               {
                                   double scaledValue = ((albumIndexArtist - 1) + value / 100.0) / totalAlbumsArtist * 100.0;
                                   f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                               }), stats, abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexArtist, totalAlbumsArtist);
                    f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("artist")} | {albumIndexArtist:N0} / {totalAlbumsArtist:N0} {f.languageManager.GetTranslation("albums")}";
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;
                case "label":
                    f.skipButton.Enabled = true;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                    await Task.Run(() => f.getInfo.getLabelInfo(f.app_id, f.qobuz_id, f.user_auth_token));
                    f.QoLabel = f.getInfo.QoLabel;
                    int totalAlbumsLabel = f.QoLabel.Albums.Items.Count;
                    int albumIndexLabel = 0;

                    if (totalAlbumsLabel == 0)
                    {
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("recordLabel")} | 0 {f.languageManager.GetTranslation("albums")}";
                    }

                    foreach (var item in f.QoLabel.Albums.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexLabel, totalAlbumsLabel);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("recordLabel")} | {albumIndexLabel:N0} / {totalAlbumsLabel:N0} {f.languageManager.GetTranslation("albums")}";

                        albumIndexLabel++;
                        try
                        {
                            string album_id = item.Id.ToString();
                            await Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                            f.QoAlbum = f.getInfo.QoAlbum;
                            updateAlbumInfoLabels(f, f.QoAlbum);
                            await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                new Progress<int>(value =>
                                {
                                    double scaledValue = ((albumIndexLabel - 1) + value / 100.0) / totalAlbumsLabel * 100.0;
                                    f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                }), stats, abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexLabel, totalAlbumsLabel);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("recordLabel")} | {albumIndexLabel:N0} / {totalAlbumsLabel:N0} {f.languageManager.GetTranslation("albums")}";
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;
                case "user":
                    if (qobuzLinkId.Contains("albums"))
                    {
                        f.skipButton.Enabled = true;
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                        await Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "albums", f.user_auth_token));
                        f.QoFavorites = f.getInfo.QoFavorites;
                        int totalAlbumsUser = f.QoFavorites.Albums.Items.Count;
                        int albumIndexUser = 0;

                        if (totalAlbumsUser == 0)
                        {
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | 0 {f.languageManager.GetTranslation("albums")}";
                        }

                        foreach (var item in f.QoFavorites.Albums.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexUser, totalAlbumsUser);
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {albumIndexUser:N0} / {totalAlbumsUser:N0} {f.languageManager.GetTranslation("albums")}";

                            albumIndexUser++;
                            try
                            {
                                string album_id = item.Id.ToString();
                                await Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                                f.QoAlbum = f.getInfo.QoAlbum;
                                updateAlbumInfoLabels(f, f.QoAlbum);
                                await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                    f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                    f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                    new Progress<int>(value =>
                                    {
                                        double scaledValue = ((albumIndexUser - 1) + value / 100.0) / totalAlbumsUser * 100.0;
                                        f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(f.progressBarDownload.Value, f.progressBarDownload.Maximum);
                                    }), stats, abortToken));
                            }
                            catch
                            {
                                continue;
                            }
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {albumIndexUser:N0} / {totalAlbumsUser:N0} {f.languageManager.GetTranslation("albums")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("tracks"))
                    {
                        f.skipButton.Enabled = false;
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                        await Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "tracks", f.user_auth_token));
                        f.QoFavorites = f.getInfo.QoFavorites;
                        int totalTracksUser = f.QoFavorites.Tracks.Items.Count;
                        int trackIndexUser = 0;

                        if (totalTracksUser == 0)
                        {
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | 0 {f.languageManager.GetTranslation("tracks")}";
                        }

                        foreach (var item in f.QoFavorites.Tracks.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexUser, totalTracksUser);
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {trackIndexUser:N0} / {totalTracksUser:N0} {f.languageManager.GetTranslation("tracks")}";

                            trackIndexUser++;
                            try
                            {
                                string track_id = item.Id.ToString();
                                await Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, track_id, f.user_auth_token));
                                f.QoItem = f.getInfo.QoItem;
                                f.QoAlbum = f.getInfo.QoAlbum;
                                updateAlbumInfoLabels(f, f.QoAlbum);
                                await Task.Run(() => f.downloadTrack.DownloadTrackAsync(
                                    "track", f.app_id, track_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                    f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum, f.QoItem,
                                    new Progress<int>(value =>
                                    {
                                        double scaledValue = ((trackIndexUser - 1) + value / 100.0) / totalTracksUser * 100.0;
                                        f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                    }), stats, abortToken));
                            }
                            catch
                            {
                                continue;
                            }
                            if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexUser, totalTracksUser);
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {trackIndexUser:N0} / {totalTracksUser:N0} {f.languageManager.GetTranslation("tracks")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("artists"))
                    {
                        f.skipButton.Enabled = true;
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);
                        await Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "artists", f.user_auth_token));
                        f.QoFavorites = f.getInfo.QoFavorites;

                        int totalAlbumsUserArtists = 0;
                        int totalArtists = f.QoFavorites.Artists.Items.Count;

                        if (totalAlbumsUserArtists == 0)
                        {
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | 0 {f.languageManager.GetTranslation("artists")}";
                        }

                        foreach (var artist in f.QoFavorites.Artists.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            try
                            {
                                await Task.Run(() => f.getInfo.getArtistInfo(f.app_id, artist.Id.ToString(), f.user_auth_token));
                                f.QoArtist = f.getInfo.QoArtist;
                                totalAlbumsUserArtists += f.QoArtist.Albums.Items.Count;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        int albumIndexUserArtist = 0;
                        int indexUserArtist = 0;

                        foreach (var artist in f.QoFavorites.Artists.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(indexUserArtist, totalArtists);

                            indexUserArtist++;
                            try
                            {
                                string artist_id = artist.Id.ToString();
                                await Task.Run(() => f.getInfo.getArtistInfo(f.app_id, artist_id, f.user_auth_token));
                                f.QoArtist = f.getInfo.QoArtist;

                                foreach (var artistItem in f.QoArtist.Albums.Items)
                                {
                                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                                    f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {indexUserArtist:N0} / {totalArtists:N0} {f.languageManager.GetTranslation("artists")} | {albumIndexUserArtist:N0} / {totalAlbumsUserArtists:N0} {f.languageManager.GetTranslation("albums")}";

                                    albumIndexUserArtist++;
                                    try
                                    {
                                        string album_id = artistItem.Id.ToString();
                                        await Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                                        f.QoAlbum = f.getInfo.QoAlbum;
                                        updateAlbumInfoLabels(f, f.QoAlbum);

                                        await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                            f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret, f.downloadLocation,
                                            f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                            new Progress<int>(value =>
                                            {
                                                double scaledValue = ((albumIndexUserArtist - 1) + value / 100.0) / totalAlbumsUserArtists * 100.0;
                                                f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                                if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(f.progressBarDownload.Value, f.progressBarDownload.Maximum);
                                            }), stats, abortToken));
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {indexUserArtist:N0} / {totalArtists:N0} {f.languageManager.GetTranslation("artists")} | {albumIndexUserArtist:N0} / {totalAlbumsUserArtists:N0} {f.languageManager.GetTranslation("albums")}";
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        // Say what isn't available at the moment.
                        f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = String.Empty));
                        f.downloadOutput.Invoke(new Action(() => f.downloadOutput.AppendText(f.downloadOutputNotImplemented)));
                        f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.downloadFolderPlaceholder));
                        return;
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;
                default:
                    // Say what isn't available at the moment.
                    f.downloadOutput.Invoke(new Action(() => f.downloadOutput.Text = String.Empty));
                    f.downloadOutput.Invoke(new Action(() => f.downloadOutput.AppendText(f.downloadOutputNotImplemented)));
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    return;
            }
        }
    }
}
