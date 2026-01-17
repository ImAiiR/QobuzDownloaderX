using QobuzDownloaderX.Helpers.QobuzDownloaderXMOD;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.UI.DevCase.UI.Components;
using QobuzDownloaderX.Win32;
using QopenAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            f.artistTemplate = f.artistTemplateTextBox.Text;
            f.albumTemplate = f.albumTemplateTextBox.Text;
            f.trackTemplate = f.trackTemplateTextBox.Text;
            f.playlistTemplate = f.playlistTemplateTextBox.Text;
            f.favoritesTemplate = f.favoritesTemplateTextBox.Text;
        }

        internal static void LoadSavedTemplates(qbdlxForm f)
        {
            f.artistTemplateTextBox.Text = Settings.Default.savedArtistTemplate;
            f.albumTemplateTextBox.Text = Settings.Default.savedAlbumTemplate;
            f.trackTemplateTextBox.Text = Settings.Default.savedTrackTemplate;
            f.vaTrackTemplateTextBox.Text = Settings.Default.savedVaTrackTemplate;
            f.playlistTemplateTextBox.Text = Settings.Default.savedPlaylistTemplate;
            f.favoritesTemplateTextBox.Text = Settings.Default.savedFavoritesTemplate;
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
            f.albumTitleCheckBox.Checked = Settings.Default.albumTag;
            f.albumArtistCheckBox.Checked = Settings.Default.albumArtistTag;
            f.trackArtistCheckBox.Checked = Settings.Default.artistTag;
            f.composerCheckBox.Checked = Settings.Default.composerTag;
            f.copyrightCheckBox.Checked = Settings.Default.copyrightTag;
            f.labelCheckBox.Checked = Settings.Default.labelTag;
            f.discNumberCheckBox.Checked = Settings.Default.discTag;
            f.discTotalCheckBox.Checked = Settings.Default.totalDiscsTag;
            f.genreCheckBox.Checked = Settings.Default.genreTag;
            f.isrcCheckBox.Checked = Settings.Default.isrcTag;
            f.urlCheckBox.Checked = Settings.Default.urlTag;
            f.releaseTypeCheckBox.Checked = Settings.Default.typeTag;
            f.explicitCheckBox.Checked = Settings.Default.explicitTag;
            f.trackTitleCheckBox.Checked = Settings.Default.trackTitleTag;
            f.trackNumberCheckBox.Checked = Settings.Default.trackTag;
            f.trackTotalCheckBox.Checked = Settings.Default.totalTracksTag;
            f.upcCheckBox.Checked = Settings.Default.upcTag;
            f.releaseDateCheckBox.Checked = Settings.Default.releaseDateTag;
            f.yearCheckBox.Checked = Settings.Default.yearTag;
            f.coverArtCheckBox.Checked = Settings.Default.imageTag;
            f.commentCheckBox.Checked = Settings.Default.commentTag;
            f.commentTextBox.Text = Settings.Default.commentText;
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
            f.streamableCheckBox.Checked = Settings.Default.streamableCheck;
            f.useTLS13CheckBox.Checked = Settings.Default.useTLS13;
            f.fixMD5sCheckBox.Checked = Settings.Default.fixMD5s;
            f.downloadGoodiesCheckBox.Checked = Settings.Default.downloadGoodies;
            f.downloadSpeedCheckBox.Checked = Settings.Default.showDownloadSpeed;
            f.clearOldLogsCheckBox.Checked = Settings.Default.clearOldLogs;
            f.downloadAllFromArtistCheckBox.Checked = Settings.Default.downloadAllFromArtist;
            f.mergeArtistNamesCheckBox.Checked = Settings.Default.mergeArtistNames;
            f.artistNamesSeparatorsPanel.Enabled = f.mergeArtistNamesCheckBox.Checked;
            f.useItemPosInPlaylistCheckBox.Checked = Settings.Default.useItemPosInPlaylist;
            f.showTipsCheckBox.Checked = Settings.Default.showTips;
            f.logFailedDownloadsCheckBox.Checked = Settings.Default.logFailedDownloadsToErrorTxt;

            int duplicateFileMode = Settings.Default.duplicateFileMode;
            switch (duplicateFileMode)
            {
                case 1: // AutoRename
                    f.autoRenameDuplicatesRadioButton.Checked = true;
                    break;
                case 2: // Overwrite Existing Files
                    f.overwriteDuplicatesRadioButton.Checked = true;
                    break;
                default: // Skip downloads
                    f.skipDuplicatesRadioButton.Checked = true;
                    break;
            }

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
            f.downloadFolderTextBox.Text = !string.IsNullOrEmpty(f.downloadLocation) ? f.downloadLocation : f.downloadFolderPlaceholder;
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
            f.skipDuplicatesRadioButton.Text = f.languageManager.GetTranslation("skipDuplicatesButton");
            f.autoRenameDuplicatesRadioButton.Text = f.languageManager.GetTranslation("autoRenameDuplicatesButton");
            f.overwriteDuplicatesRadioButton.Text = f.languageManager.GetTranslation("overwriteDuplicatesButton");

            /* Center additional settings button to center of panel */
            //f.additionalSettingsButton.Location = new Point((f.settingsPanel.Width - f.additionalSettingsButton.Width) / 2, f.additionalSettingsButton.Location.Y);

            /* Center quality panel to center of quality button */
            f.qualitySelectPanel.Location = new Point(f.qualitySelectButton.Left + (f.qualitySelectButton.Width / 2) - (f.qualitySelectPanel.Width / 2), f.qualitySelectPanel.Location.Y);

            // Labels
            f.aboutLabel.Text = f.languageManager.GetTranslation("aboutButton") + "                                                                                                 ";
            f.advancedOptionsLabel.Text = f.languageManager.GetTranslation("advancedOptionsLabel");
            f.albumTemplateLabel.Text = f.languageManager.GetTranslation("albumTemplateLabel");
            f.artistTemplateLabel.Text = f.languageManager.GetTranslation("artistTemplateLabel");
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
            f.searchSortingLabel.Text = f.languageManager.GetTranslation("searchSortingLabel");
            f.sortReleaseDateButton.Text = f.languageManager.GetTranslation("sortReleaseDateLabel");
            f.sortGenreButton.Text = f.languageManager.GetTranslation("sortGenreLabel");
            f.sortArtistNameButton.Text = f.languageManager.GetTranslation("sortArtistNameLabel");
            f.sortAlbumTrackNameButton.Text = f.languageManager.GetTranslation("sortAlbumTrackNameLabel");
            f.sortingSearchResultsLabel.Text = f.languageManager.GetTranslation("sortingSearchResultsLabel");
            f.selectedRowsCountLabel.Text = string.Empty;
            f.primaryListSeparatorLabel.Text = f.languageManager.GetTranslation("primaryListSeparatorLabel");
            f.listEndSeparatorLabel.Text = f.languageManager.GetTranslation("listEndSeparatorLabel");
            f.playlistSectionLabel.Text = f.languageManager.GetTranslation("playlistSectionLabel");
            f.prevTipButton.Text = f.languageManager.GetTranslation("prevTipLabel");
            f.nextTipButton.Text = f.languageManager.GetTranslation("nextTipLabel");
            f.duplicateFilesLabel.Text = f.languageManager.GetTranslation("duplicateFilesLabel");

            // CheckBoxes
            f.albumArtistCheckBox.Text = f.languageManager.GetTranslation("albumArtistCheckBox");
            f.albumTitleCheckBox.Text = f.languageManager.GetTranslation("albumTitleCheckBox");
            f.commentCheckBox.Text = f.languageManager.GetTranslation("commentCheckBox");
            f.trackArtistCheckBox.Text = f.languageManager.GetTranslation("trackArtistCheckBox");
            f.trackTitleCheckBox.Text = f.languageManager.GetTranslation("trackTitleCheckBox");
            f.yearCheckBox.Text = f.languageManager.GetTranslation("yearCheckBox");
            f.releaseDateCheckBox.Text = f.languageManager.GetTranslation("releaseDateCheckBox");
            f.releaseTypeCheckBox.Text = f.languageManager.GetTranslation("releaseTypeCheckBox");
            f.genreCheckBox.Text = f.languageManager.GetTranslation("genreCheckBox");
            f.trackNumberCheckBox.Text = f.languageManager.GetTranslation("trackNumberCheckBox");
            f.trackTotalCheckBox.Text = f.languageManager.GetTranslation("trackTotalCheckBox");
            f.discNumberCheckBox.Text = f.languageManager.GetTranslation("discNumberCheckBox");
            f.discTotalCheckBox.Text = f.languageManager.GetTranslation("discTotalCheckBox");
            f.composerCheckBox.Text = f.languageManager.GetTranslation("composerCheckBox");
            f.explicitCheckBox.Text = f.languageManager.GetTranslation("explicitCheckBox");
            f.coverArtCheckBox.Text = f.languageManager.GetTranslation("coverArtCheckBox");
            f.copyrightCheckBox.Text = f.languageManager.GetTranslation("copyrightCheckBox");
            f.labelCheckBox.Text = f.languageManager.GetTranslation("labelCheckBox");
            f.upcCheckBox.Text = f.languageManager.GetTranslation("upcCheckBox");
            f.isrcCheckBox.Text = f.languageManager.GetTranslation("isrcCheckBox");
            f.urlCheckBox.Text = f.languageManager.GetTranslation("urlCheckBox");
            f.mergeArtistNamesCheckBox.Text = f.languageManager.GetTranslation("mergeArtistNamesCheckBox");
            f.streamableCheckBox.Text = f.languageManager.GetTranslation("streamableCheckBox");
            f.fixMD5sCheckBox.Text = f.languageManager.GetTranslation("fixMD5sCheckBox");
            f.downloadSpeedCheckBox.Text = f.languageManager.GetTranslation("downloadSpeedCheckBox");
            f.sortAscendantCheckBox.Text = f.languageManager.GetTranslation("sortAscendantCheckBox");
            f.downloadGoodiesCheckBox.Text = f.languageManager.GetTranslation("downloadGoodiesCheckBox");
            f.useTLS13CheckBox.Text = f.languageManager.GetTranslation("useTLS13CheckBox");
            f.dontSaveArtworkToDiskCheckBox.Text = f.languageManager.GetTranslation("dontSaveArtworkToDiskCheckBox");
            f.downloadAllFromArtistCheckBox.Text = f.languageManager.GetTranslation("downloadAllFromArtistCheckBox");
            f.clearOldLogsCheckBox.Text = f.languageManager.GetTranslation("clearOldLogsCheckBox");
            f.logFailedDownloadsCheckBox.Text = f.languageManager.GetTranslation("logFailedDownloadsCheckBox");
            f.useItemPosInPlaylistCheckBox.Text = f.languageManager.GetTranslation("useItemPosInPlaylistCheckBox");
            f.showTipsCheckBox.Text = f.languageManager.GetTranslation("showTipsCheckBox");

            // downloadFromArtistListBox
            string translatedNames = f.languageManager.GetTranslation("downloadFromArtistListBox");
            string[] names = translatedNames.Split(',');
            for (int i = 0; i < names.Length && i < f.downloadFromArtistListBox.Items.Count; i++)
            {
                f.downloadFromArtistListBox.Items[i] = names[i].Trim();
            }

            /* Center certain checkboxes in panels */
            f.fixMD5sCheckBox.Location = new Point((f.extraSettingsPanel.Width - f.fixMD5sCheckBox.Width) / 2, f.fixMD5sCheckBox.Location.Y);
            f.downloadSpeedCheckBox.Location = new Point((f.extraSettingsPanel.Width - f.downloadSpeedCheckBox.Width) / 2, f.downloadSpeedCheckBox.Location.Y);

            f.streamableCheckBox.Location = new Point(f.fixMD5sCheckBox.Left - 100, f.streamableCheckBox.Location.Y);
            f.useTLS13CheckBox.Location = new Point(f.streamableCheckBox.Right + 16, f.streamableCheckBox.Location.Y);
            f.downloadGoodiesCheckBox.Location = new Point(f.useTLS13CheckBox.Right + 16, f.streamableCheckBox.Location.Y);

            // Context menu items
            f.showWindowToolStripMenuItem.Text = f.languageManager.GetTranslation("showWindowCmItem");
            f.hideWindowToolStripMenuItem.Text = f.languageManager.GetTranslation("hideWindowCmItem");
            f.closeProgramToolStripMenuItem.Text = f.languageManager.GetTranslation("closeProgramCmItem");

            // Placeholders
            f.albumLabelPlaceholder = f.languageManager.GetTranslation("albumLabelPlaceholder");
            f.artistLabelPlaceholder = f.languageManager.GetTranslation("artistLabelPlaceholder");
            f.infoLabelPlaceholder = f.languageManager.GetTranslation("infoLabelPlaceholder");
            f.inputTextBoxPlaceholder = f.languageManager.GetTranslation("inputTextBoxPlaceholder");
            f.searchTextBoxPlaceholder = f.languageManager.GetTranslation("searchTextBoxPlaceholder");
            f.downloadFolderPlaceholder = f.languageManager.GetTranslation("downloadFolderPlaceholder");
            f.userInfoTextBoxPlaceholder = f.languageManager.GetTranslation("userInfoTextBoxPlaceholder");
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
            f.inputTextBox.Text = f.inputTextBoxPlaceholder;
            f.searchTextBox.Text = f.searchTextBoxPlaceholder;
        }

        internal static void updateAlbumInfoLabels(qbdlxForm f, Album QoAlbum)
        {
            // Use translated singular/plural strings instead of hardcoded English.
            string trackLabelSingular = f.languageManager.GetTranslation("track");
            string trackLabelPlural = f.languageManager.GetTranslation("tracks");
            string trackOrTracks = QoAlbum.TracksCount == 1 ? trackLabelSingular : trackLabelPlural;
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
            }
            catch
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

        internal static void CenterLeftAlignedRichTextBoxText(RichTextBox rtb)
        {
            if (string.IsNullOrEmpty(rtb.Text))
                return;

            // Get all lines
            string[] lines = rtb.Lines;

            int maxLineWidth = 0;

            using (Graphics g = rtb.CreateGraphics())
            {
                foreach (string line in lines)
                {
                    // Measure real pixel width of the line
                    Size size = TextRenderer.MeasureText(
                        g,
                        line,
                        rtb.Font,
                        new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.NoPadding
                    );

                    if (size.Width > maxLineWidth)
                        maxLineWidth = size.Width;
                }
            }

            // Usable width of the RichTextBox (no scrollbars)
            int usableWidth = rtb.ClientSize.Width;

            int indent = Math.Max(0, (usableWidth - maxLineWidth) / 2);

            // Apply indentation
            rtb.SelectAll();
            rtb.SelectionIndent = indent;
            rtb.SelectionRightIndent = indent;
            rtb.DeselectAll();
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

        internal static void EnforceTextBoxToSingleLine(TextBox tb)
        {
            if (!tb.Multiline) return;

            if (tb.TextLength == 0) return;

            string text = tb.Text;

            int index = text.IndexOfAny(new char[] { '\r', '\n' });
            if (index != -1)
            {
                tb.Text = text.Substring(0, index);
                tb.SelectionStart = tb.Text.Length;
            }
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

        internal static void LogNotStreamableAlbumEntry(string downloadLocation, Album QoAlbum, string msg)
        {
            // Artist - Album Title (Date) [Album ID] [UPC]
            string entryTitle = $"{QoAlbum?.Artist?.Name?.Trim()} - {QoAlbum?.Title?.Trim()} ({QoAlbum?.ReleaseDateOriginal ?? QoAlbum?.ReleaseDateStream}) [ID-{QoAlbum?.Id}] [UPC-{QoAlbum?.UPC}]";

            Miscellaneous.LogFailedDownloadEntry(downloadLocation, entryTitle, msg);
        }

        internal static void LogNotStreamableTrackEntry(string downloadLocation, Item QoItem, string msg)
        {
            // Artist - Album Title (Date) -> Track Number. Track Title [Track ID] [UPC]
            string entryTitle =
                $"{QoItem?.Artist?.Name?.Trim() ?? QoItem?.Album?.Artist?.Name?.Trim()} - {QoItem?.Album?.Title?.Trim()} ({QoItem?.ReleaseDateOriginal ?? QoItem?.ReleaseDateStream}) -> {QoItem?.TrackNumber}. {QoItem?.Title?.Trim()} [ID-{QoItem?.Id ?? QoItem?.Album?.Id}] [UPC-{QoItem?.UPC ?? QoItem?.Album?.UPC}]";

            Miscellaneous.LogFailedDownloadEntry(downloadLocation, entryTitle, msg);
        }

        internal static void LogNotDownloadableTrackEntry(string downloadLocation, Item QoItem, string msg)
        {
            // Artist - Album Title (Date) -> Track Number. Track Title [Track ID] [UPC]
            string entryTitle =
                $"{QoItem?.Artist?.Name?.Trim() ?? QoItem?.Album?.Artist?.Name?.Trim()} - {QoItem?.Album?.Title?.Trim()} ({QoItem?.ReleaseDateOriginal ?? QoItem?.ReleaseDateStream}) -> {QoItem?.TrackNumber}. {QoItem?.Title?.Trim()} [ID-{QoItem?.Id ?? QoItem?.Album?.Id}] [UPC-{QoItem?.UPC ?? QoItem?.Album?.UPC}]";

            Miscellaneous.LogFailedDownloadEntry(downloadLocation, entryTitle, msg);
        }

        internal static void LogFailedDownloadStreamEntry(string downloadLocation, Item QoItem, string msg)
        {
            // Artist - Album Title (Date) -> Track Number. Track Title [Track ID] [UPC]
            string entryTitle =
                $"{QoItem?.Artist?.Name?.Trim() ?? QoItem?.Album?.Artist?.Name?.Trim()} - {QoItem?.Album?.Title?.Trim()} ({QoItem?.ReleaseDateOriginal ?? QoItem?.ReleaseDateStream}) -> {QoItem?.TrackNumber}. {QoItem?.Title?.Trim()} [ID-{QoItem?.Id ?? QoItem?.Album?.Id}] [UPC-{QoItem?.UPC ?? QoItem?.Album?.UPC}]";

            Miscellaneous.LogFailedDownloadEntry(downloadLocation, entryTitle, msg);
        }

        internal static void LogFailedDownloadEntry(string downloadLocation, string entryTitle, string msg)
        {
            if (Settings.Default.logFailedDownloadsToErrorTxt)
            {
                try
                {
                    if (msg.Contains(':'))
                    {
                        msg = msg.Substring(0, msg.IndexOf(':'));
                    }
                    msg = msg?.TrimEnd(' ', '.', '\r', '\n');

                    // Fallback to root download directory.
                    if (!Directory.Exists(downloadLocation))
                    {
                        downloadLocation = qbdlxForm._qbdlxForm.downloadLocation;
                    }

                    using (StreamWriter sw = File.AppendText(Path.Combine(downloadLocation, qbdlxForm.failedDownloadsLogFilename)))
                    {
                        sw.WriteLine($"{msg}: {entryTitle}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error writing to file: " + ex.Message);
                }
            }
        }

        internal static string GetDuplicateFileName(string fullPath)
        {
            string folder = Path.GetDirectoryName(fullPath) ?? throw new ArgumentException("Invalid path: " + fullPath);
            string file = Path.GetFileName(fullPath);
            string pszPathForApi = fullPath;

            const int MAX_PATH = 260;
            StringBuilder sb = new StringBuilder(MAX_PATH);

            bool ok = NativeMethods.PathYetAnotherMakeUniqueName(
                sb,
                pszPathForApi, // full path + name
                null,          // pszShort = null -> base on long name
                null           // optional pszFileSpec, can be left null
            );

            if (!ok)
            {
                // The function can return FALSE in case of truncation or other failure.
                throw new IOException("PathYetAnotherMakeUniqueName failed.");
            }

            string result = sb.ToString();

            // Extra safety: if the API returns exactly the same name
            // and the file already exists, use a manual fallback.
            if (string.Equals(result, fullPath, StringComparison.OrdinalIgnoreCase) && ZlpIOHelper.FileExists(fullPath))
            {
                string baseName = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);
                int count = 1;
                string candidate;
                do
                {
                    candidate = Path.Combine(folder, string.Format("{0} ({1}){2}", baseName, count, ext));
                    count++;
                } while (ZlpIOHelper.FileExists(candidate));
                return candidate;
            }

            return result;
        }

        private static async Task RunTaskWithTimeoutAsync(qbdlxForm form, Task workTask, TimeSpan timeout, string timeoutMessage = "Task has timed out.")
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (workTask == null)
                throw new ArgumentNullException(nameof(workTask));

            if (timeout == TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeout));

            try
            {
                var timeoutTask = Task.Delay(timeout);

                var completedTask = await Task.WhenAny(workTask, timeoutTask);

                if (completedTask == workTask)
                {
                    // workTask finished in time, re-throw any exception if there was one.
                    await workTask;
                }
                else
                {
                    // Timeout reached
                    form.Invoke((MethodInvoker)(() =>
                    {
                        form.downloadOutput.Text += $"\r\n[Timeout {timeout.TotalSeconds:F1}s] {timeoutMessage}";
                    }));
                    // Note: the background task keeps running, but we continue execution here.
                    throw new OperationCanceledException();
                }
            }
            catch (Exception ex)
            {
                // Any exception from the work action.
                form.Invoke((MethodInvoker)(() =>
                {
                    form.downloadOutput.Text += $"\r\nError: {ex.Message}";
                }));
                throw;
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
                    SpeedWatch = qbdlxForm._qbdlxForm.downloadSpeedCheckBox.Checked ? Stopwatch.StartNew() : null,
                    CumulativeBytesRead = 0,
                    LastUiBytes = 0,
                    LastUiTimeMs = 0
                };
            }

            try
            {
                f.inputTextBox.Enabled = false;
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
                f.inputTextBox.Enabled = true;
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
            f.batchDownloadProgressCountLabel.Text = $"{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount} {f.languageManager.GetTranslation("completed")}";
            f.notifyIcon1.Text = $"QobuzDLX\r\n\r\n{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount} {f.languageManager.GetTranslation("completed")}";
            qbdlxForm.isBatchDownloadRunning = true;

            var stats = new DownloadStats
            {
                SpeedWatch = qbdlxForm._qbdlxForm.downloadSpeedCheckBox.Checked ? Stopwatch.StartNew() : null,
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

                f.inputTextBox.Text = url;
                f.inputTextBox.ForeColor = Color.FromArgb(200, 200, 200);

                await downloadButtonAsyncWork(f, stats);

                if (f.abortTokenSource != null && f.abortTokenSource.IsCancellationRequested)
                {
                    break;
                }
                f.batchDownloadProgressCountLabel.Text = $"{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount} {f.languageManager.GetTranslation("completed")}";
                f.notifyIcon1.Text = $"QobuzDLX\r\n\r\n{f.languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount} {f.languageManager.GetTranslation("completed")}";
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

            string albumLink = f.inputTextBox.Text.Trim().TrimEnd('/');
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

            TimeSpan getInfosTimeOut = TimeSpan.FromSeconds(30);

            switch (linkType)
            {
                case "album":
                    f.skipButton.Enabled = true;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);

                    // [FETCH INFO] case "album" -> getAlbumInfoLabels
                    var albumTask = Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    try
                    {
                        await RunTaskWithTimeoutAsync(f, albumTask, getInfosTimeOut, "Q(Open)API 'getAlbumInfoLabels' task has timed out.");
                    }
                    catch { return; }

                    f.QoAlbum = f.getInfo.QoAlbum;
                    if (f.QoAlbum == null)
                    {
                        f.getInfo.updateDownloadOutput($"{f.downloadOutputAPIError}");
                        // Not useful at all to log this entry since we can't retrieve any album information:
                        // Miscellaneous.LogFailedDownloadEntry(f.downloadLocation, entryTitle: null, $"{f.downloadOutputAPIError}");
                        f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                        break;
                    }
                    string albumTrackLabel = f.QoAlbum.TracksCount == 1
                        ? f.languageManager.GetTranslation("track")
                        : f.languageManager.GetTranslation("tracks");
                    f.progressItemsCountLabel.Text = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("album"))} | {f.QoAlbum.TracksCount:N0} {albumTrackLabel}";
                    Miscellaneous.updateAlbumInfoLabels(f, f.QoAlbum);

                    var albumTrackCounter = new Progress<(int current, int total)>(t =>
                    {
                        f.progressItemsCountLabel.BeginInvoke(new Action(() =>
                        {
                            f.progressItemsCountLabel.Text =
                                $"{f.languageManager.GetTranslation("album")} | {t.total:N0} {albumTrackLabel} ({t.current:N0}/{t.total:N0} {f.languageManager.GetTranslation("completed")})";
                        }));
                    });

                    // [DOWNLOAD] case "album" -> DownloadAlbumAsync
                    await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(f.app_id, f.qobuz_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret, f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum, progress, albumTrackCounter, stats, abortToken));
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                  
                    // Say the downloading is finished when it's completed.
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;

                case "track":
                    f.skipButton.Enabled = false;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);

                    // [FETCH INFO] case "track" -> getTrackInfoLabels
                    var trackTask = Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    try
                    {
                        await RunTaskWithTimeoutAsync(f, trackTask, getInfosTimeOut, "Q(Open)API 'getTrackInfoLabels' task has timed out.");
                    }
                    catch { return; }
                   
                    f.QoItem = f.getInfo.QoItem;
                    f.QoAlbum = f.getInfo.QoAlbum;
                    updateAlbumInfoLabels(f, f.QoAlbum);
                    f.progressItemsCountLabel.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("track"));

                    // [DOWNLOAD] case "track" -> DownloadTrackAsync
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

                    // [FETCH INFO] case "playlist" -> getPlaylistInfoLabels
                    var playlistTask = Task.Run(() => f.getInfo.getPlaylistInfoLabels(f.app_id, f.qobuz_id, f.user_auth_token));
                    try
                    {
                        await RunTaskWithTimeoutAsync(f, playlistTask, getInfosTimeOut, "Q(Open)API 'getPlaylistInfoLabels' task has timed out.");
                    }
                    catch { return; }
                  
                    f.QoPlaylist = f.getInfo.QoPlaylist;
                    Miscellaneous.updatePlaylistInfoLabels(f, f.QoPlaylist);
                    int totalTracksPlaylist = f.QoPlaylist.Tracks.Items.Count;
                    int trackIndexPlaylist = 0;
                    foreach (var item in f.QoPlaylist.Tracks.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexPlaylist, totalTracksPlaylist);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("playlist")} | {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("track"))} {trackIndexPlaylist:N0}/{totalTracksPlaylist:N0} {f.languageManager.GetTranslation("completed")}";

                        if (Settings.Default.useItemPosInPlaylist)
                        {
                            item.TrackNumber = item.Position;
                        }

                        trackIndexPlaylist++;
                        try
                        {
                            string track_id = item.Id.ToString();
                           
                            // [FETCH INFO] case "playlist" -> getTrackInfoLabels
                            var playlistTrackInfoTask = Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, track_id, f.user_auth_token));
                            await RunTaskWithTimeoutAsync(f, playlistTrackInfoTask, getInfosTimeOut, "Q(Open)API 'getTrackInfoLabels' task has timed out.");
                            f.QoItem = item;
                            f.QoAlbum = f.getInfo.QoAlbum;

                            // [DOWNLOAD] case "playlist" -> DownloadPlaylistTrackAsync
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
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("playlist")} | {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("track"))} {trackIndexPlaylist:N0}/{totalTracksPlaylist:N0} {f.languageManager.GetTranslation("completed")}";

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

                    // [FETCH INFO] case "artist" -> getArtistInfo
                    var artistTask = Task.Run(() => f.getInfo.getArtistInfo(f.app_id, f.qobuz_id, f.user_auth_token));
                    try
                    {
                        await RunTaskWithTimeoutAsync(f, artistTask, getInfosTimeOut, "Q(Open)API 'getArtistInfo' task has timed out.");
                    }
                    catch { return; }

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
                        // f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("artist")} | {f.languageManager.GetTranslation("album")} {albumIndexArtist:N0}/{totalAlbumsArtist:N0} {f.languageManager.GetTranslation("completed")}";

                        var artistTrackCounter = new Progress<(int current, int total)>(tuple =>
                        {
                            f.progressItemsCountLabel.BeginInvoke(new Action(() =>
                            {
                                f.progressItemsCountLabel.Text =
                                    $"{f.languageManager.GetTranslation("artist")} | {f.languageManager.GetTranslation("album")} {albumIndexArtist:N0}/{totalAlbumsArtist:N0} ({f.languageManager.GetTranslation("track")} {tuple.current:N0}/{tuple.total:N0} {f.languageManager.GetTranslation("completed")})";
                            }));
                        });

                        albumIndexArtist++;
                        try
                        {
                            string album_id = item.Id.ToString();
                           
                            // [FETCH INFO] case "artist" -> getAlbumInfoLabels
                            var artistAlbumInfoTask = Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                            await RunTaskWithTimeoutAsync(f, artistAlbumInfoTask, getInfosTimeOut, "Q(Open)API 'getAlbumInfoLabels' task has timed out.");
                            f.QoAlbum = f.getInfo.QoAlbum;
                            updateAlbumInfoLabels(f, f.QoAlbum);

                            // [DOWNLOAD] case "artist" -> DownloadAlbumAsync
                            await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                               f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                               f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                               new Progress<int>(value =>
                               {
                                   double scaledValue = ((albumIndexArtist - 1) + value / 100.0) / totalAlbumsArtist * 100.0;
                                   f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                               }), artistTrackCounter, stats, abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                   
                    // Say the downloading is finished when it's completed.
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexArtist, totalAlbumsArtist);
                    f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("artist")} | {f.languageManager.GetTranslation("album")} {albumIndexArtist:N0}/{totalAlbumsArtist:N0} {f.languageManager.GetTranslation("completed")}";
                    f.getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    f.getInfo.updateDownloadOutput("\r\n" + f.downloadOutputCompleted);
                    f.progressLabel.Invoke(new Action(() => f.progressLabel.Text = f.progressLabelInactive));
                    break;

                case "label":
                    f.skipButton.Enabled = true;
                    if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);

                    // [FETCH INFO] case "label" -> getLabelInfo
                    var labelTask = Task.Run(() => f.getInfo.getLabelInfo(f.app_id, f.qobuz_id, f.user_auth_token));
                    try
                    {
                        await RunTaskWithTimeoutAsync(f, labelTask, getInfosTimeOut, "Q(Open)API 'getLabelInfo' task has timed out.");
                    }
                    catch { return; }

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
                        // f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("recordLabel")} | {f.languageManager.GetTranslation("album")} {albumIndexLabel:N0}/{totalAlbumsLabel:N0} {f.languageManager.GetTranslation("completed")}";

                        var labelTrackCounter = new Progress<(int current, int total)>(tuple =>
                        {
                            f.progressItemsCountLabel.BeginInvoke(new Action(() =>
                            {
                                f.progressItemsCountLabel.Text =
                                    $"{f.languageManager.GetTranslation("recordLabel")} | {f.languageManager.GetTranslation("album")} {albumIndexLabel:N0}/{totalAlbumsLabel:N0} ({f.languageManager.GetTranslation("track")} {tuple.current:N0}/{tuple.total:N0} {f.languageManager.GetTranslation("completed")})";
                            }));
                        });

                        albumIndexLabel++;
                        try
                        {
                            string album_id = item.Id.ToString();

                            // [FETCH INFO] case "label" -> getAlbumInfoLabels
                            var labelAlbumInfoTask = Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                            await RunTaskWithTimeoutAsync(f, labelAlbumInfoTask, getInfosTimeOut, "Q(Open)API 'getAlbumInfoLabels' task has timed out.");
                            f.QoAlbum = f.getInfo.QoAlbum;
                            updateAlbumInfoLabels(f, f.QoAlbum);

                            // [DOWNLOAD] case "label" -> DownloadAlbumAsync
                            await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                new Progress<int>(value =>
                                {
                                    double scaledValue = ((albumIndexLabel - 1) + value / 100.0) / totalAlbumsLabel * 100.0;
                                    f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                }), labelTrackCounter, stats, abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexLabel, totalAlbumsLabel);
                        f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("recordLabel")} | {f.languageManager.GetTranslation("album")} {albumIndexLabel:N0}/{totalAlbumsLabel:N0} {f.languageManager.GetTranslation("completed")}";
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

                        // [FETCH INFO] case "user" ("albums") -> getFavoritesInfo
                        var userFavAlbumsTask = Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "albums", f.user_auth_token));
                        try
                        {
                            await RunTaskWithTimeoutAsync(f, userFavAlbumsTask, getInfosTimeOut, "Q(Open)API 'getFavoritesInfo' task has timed out.");
                        }
                        catch { return; }

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
                            // f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("album")} {albumIndexUser:N0}/{totalAlbumsUser:N0} {f.languageManager.GetTranslation("completed")}";

                            var userAlbumTrackCounter = new Progress<(int current, int total)>(tuple =>
                            {
                                f.progressItemsCountLabel.BeginInvoke(new Action(() =>
                                {
                                    f.progressItemsCountLabel.Text =
                                        $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("album")} {albumIndexUser:N0}/{totalAlbumsUser:N0} ({f.languageManager.GetTranslation("track")} {tuple.current:N0}/{tuple.total:N0} {f.languageManager.GetTranslation("completed")})";
                                }));
                            });

                            albumIndexUser++;
                            try
                            {
                                string album_id = item.Id.ToString();

                                // [FETCH INFO] case "user" ("albums") -> getAlbumInfoLabels
                                var userAlbumInfoTask = Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                                await RunTaskWithTimeoutAsync(f, userAlbumInfoTask, getInfosTimeOut, "Q(Open)API 'getAlbumInfoLabels' task has timed out.");
                                f.QoAlbum = f.getInfo.QoAlbum;
                                updateAlbumInfoLabels(f, f.QoAlbum);

                                // [DOWNLOAD] case "user" ("albums") -> DownloadAlbumAsync
                                await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                    f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret,
                                    f.downloadLocation, f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                    new Progress<int>(value =>
                                    {
                                        double scaledValue = ((albumIndexUser - 1) + value / 100.0) / totalAlbumsUser * 100.0;
                                        f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(f.progressBarDownload.Value, f.progressBarDownload.Maximum);
                                    }), userAlbumTrackCounter, stats, abortToken));
                            }
                            catch
                            {
                                continue;
                            }
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("album")} {albumIndexUser:N0}/{totalAlbumsUser:N0} {f.languageManager.GetTranslation("completed")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("tracks"))
                    {
                        f.skipButton.Enabled = false;
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);

                        // [FETCH INFO] case "user" ("tracks") -> getFavoritesInfo
                        var userFavTracksTask = Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "tracks", f.user_auth_token));
                        try
                        {
                            await RunTaskWithTimeoutAsync(f, userFavTracksTask, getInfosTimeOut, "Q(Open)API 'getFavoritesInfo' task has timed out.");
                        }
                        catch { return; }

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
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("track"))} {trackIndexUser:N0}/{totalTracksUser:N0} {f.languageManager.GetTranslation("completed")}";

                            trackIndexUser++;
                            try
                            {
                                string track_id = item.Id.ToString();

                                // [FETCH INFO] case "user" ("tracks") -> getTrackInfoLabels
                                var userAlbumInfoTask = Task.Run(() => f.getInfo.getTrackInfoLabels(f.app_id, track_id, f.user_auth_token));
                                await RunTaskWithTimeoutAsync(f, userAlbumInfoTask, getInfosTimeOut, "Q(Open)API 'getTrackInfoLabels' task has timed out.");
                                f.QoItem = f.getInfo.QoItem;
                                f.QoAlbum = f.getInfo.QoAlbum;
                                updateAlbumInfoLabels(f, f.QoAlbum);

                                // [DOWNLOAD] case "user" ("tracks") -> DownloadTrackAsync
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
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(f.languageManager.GetTranslation("track"))} {trackIndexUser:N0}/{totalTracksUser:N0} {f.languageManager.GetTranslation("completed")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("artists"))
                    {
                        f.skipButton.Enabled = true;
                        if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, f.progressBarDownload.Maximum);

                        // [FETCH INFO] case "user" ("artists") -> getFavoritesInfo
                        var userFavArtistsTask = Task.Run(() => f.getInfo.getFavoritesInfo(f.app_id, f.user_id, "artists", f.user_auth_token));
                        try
                        {
                            await RunTaskWithTimeoutAsync(f, userFavArtistsTask, getInfosTimeOut, "Q(Open)API 'getFavoritesInfo' task has timed out.");
                        }
                        catch { return; }

                        f.QoFavorites = f.getInfo.QoFavorites;

                        int totalAlbumsUserArtists = 0;
                        int totalArtists = f.QoFavorites.Artists.Items.Count;

                        if (totalAlbumsUserArtists == 0)
                        {
                            f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | 0 {f.languageManager.GetTranslation("albums")}";
                        }

                        var artistInfoCache = new Dictionary<string, Artist>();

                        foreach (var artist in f.QoFavorites.Artists.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }

                            try
                            {
                                string artistId = artist.Id.ToString();

                                // [FETCH INFO] case "user" ("artists") -> getArtistInfo
                                var userArtistInfoTask = Task.Run(() => f.getInfo.getArtistInfo(f.app_id, artist.Id.ToString(), f.user_auth_token));
                                await RunTaskWithTimeoutAsync(f, userArtistInfoTask, getInfosTimeOut, "Q(Open)API 'getArtistInfo' task has timed out.");

                                f.QoArtist = f.getInfo.QoArtist;
                                artistInfoCache[artistId] = f.QoArtist;

                                if (f.QoArtist.Albums != null )
                                {
                                    totalAlbumsUserArtists += f.QoArtist.Albums.Items.Count;
                                }
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

                                if (!artistInfoCache.TryGetValue(artist_id, out var qoArtist))
                                    continue;

                                f.QoArtist = qoArtist;

                                foreach (var artistItem in f.QoArtist.Albums.Items)
                                {
                                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                                    // f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("artist")} {indexUserArtist:N0}/{totalArtists:N0} | {f.languageManager.GetTranslation("album")} {albumIndexUserArtist:N0}/{totalAlbumsUserArtists:N0} {f.languageManager.GetTranslation("completed")}";

                                    var userArtistTrackCounter = new Progress<(int current, int total)>(tuple =>
                                    {
                                        f.progressItemsCountLabel.BeginInvoke(new Action(() =>
                                        {
                                            f.progressItemsCountLabel.Text =
                                                $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("artist")} {indexUserArtist:N0}/{totalArtists:N0} | {f.languageManager.GetTranslation("album")} {albumIndexUserArtist:N0}/{totalAlbumsUserArtists:N0} ({f.languageManager.GetTranslation("track")} {tuple.current:N0}/{tuple.total:N0} {f.languageManager.GetTranslation("completed")})";
                                        }));
                                    });

                                    albumIndexUserArtist++;
                                    try
                                    {
                                        string album_id = artistItem.Id.ToString();

                                        // [FETCH INFO] case "user" ("artists") -> getAlbumInfoLabels
                                        var userArtistAlbumInfoTask = Task.Run(() => f.getInfo.getAlbumInfoLabels(f.app_id, album_id, f.user_auth_token));
                                        await RunTaskWithTimeoutAsync(f, userArtistAlbumInfoTask, getInfosTimeOut, "Q(Open)API 'getAlbumInfoLabels' task has timed out.");
                                        f.QoAlbum = f.getInfo.QoAlbum;
                                        updateAlbumInfoLabels(f, f.QoAlbum);

                                        // [DOWNLOAD] case "user" ("artists") -> DownloadAlbumAsync
                                        await Task.Run(() => f.downloadAlbum.DownloadAlbumAsync(
                                            f.app_id, album_id, f.format_id, f.audio_format, f.user_auth_token, f.app_secret, f.downloadLocation,
                                            f.artistTemplate, f.albumTemplate, f.trackTemplate, f.QoAlbum,
                                            new Progress<int>(value =>
                                            {
                                                double scaledValue = ((albumIndexUserArtist - 1) + value / 100.0) / totalAlbumsUserArtists * 100.0;
                                                f.progressBarDownload.Invoke(new Action(() => f.progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                                if (!qbdlxForm.isBatchDownloadRunning) TaskbarHelper.SetProgressValue(f.progressBarDownload.Value, f.progressBarDownload.Maximum);
                                            }), userArtistTrackCounter, stats, abortToken));
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                f.progressItemsCountLabel.Text = $"{f.languageManager.GetTranslation("user")} | {f.languageManager.GetTranslation("artist")} {indexUserArtist:N0}/{totalArtists:N0} | {f.languageManager.GetTranslation("album")} {albumIndexUserArtist:N0}/{totalAlbumsUserArtists:N0} {f.languageManager.GetTranslation("completed")}";
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
