// ***********************************************************************
// Author   : ElektroStudios
// Modified : 14-January-2026
// ***********************************************************************

// Note: ChatGPT code conversion from VB.NET to C# 7.3 (it may be not equal than original VB.NET implementation)

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;

/// <summary>
/// A settings provider that stores application settings in a configurable directory and filename.
/// Supports optional deterministic directory hashing and version-based folder organization
/// to keep configuration stable and predictable across updates.
/// </summary>
public class DeterministicFileSettingsProvider : SettingsProvider
{
    #region Public Fields

    /// <summary>
    /// Name of the configuration file used to persist settings.
    /// </summary>
    public readonly string ConfigFileName = "user.config";

    /// <summary>
    /// Full path to the directory where the configuration file will be stored.
    /// </summary>
    public readonly string ConfigDirectoryPath = @".\"; // Application's base dir.

    /// <summary>
    /// Determines whether to append the application version to the configuration directory name.
    /// </summary>
    public readonly bool AppendVersion = false;

    /// <summary>
    /// Determines whether to append a deterministic hash suffix to the configuration directory name.
    /// </summary>
    public readonly bool AppendHash = false;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the application name used by the provider.
    /// When gotten, it computes a stable application identifier (optionally with version/hash).
    /// Setting is intentionally ignored.
    /// </summary>
    public override string ApplicationName
    {
        get => GetApplicationNameId(AppendVersion, AppendHash);
        set { /* intentionally ignored */ }
    }

    private readonly string _Description = "A Settings provider that stores application settings in a configurable directory and filename.";

    /// <summary>
    /// Gets a brief, friendly description of this SettingsProvider, suitable for display in UIs.
    /// </summary>
    public override string Description => string.IsNullOrEmpty(_Description) ? Name : _Description;

    /// <summary>
    /// Gets a fallback directory path under the user's LocalApplicationData folder.
    /// Used when writing to the specified application directory is not possible.
    /// </summary>
    public string ConfigDirectoryPathFallback
    {
        get
        {
            string path2 = GetCompanyName();
            path2 = string.IsNullOrEmpty(path2)
                ? GetApplicationNameId(AppendVersion, AppendHash)
                : $"{path2}\\{GetApplicationNameId(AppendVersion, AppendHash)}";

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), path2);
        }
    }

    /// <summary>
    /// Gets the fully qualified configuration file path (directory + filename).
    /// </summary>
    public string ConfigFileFullName => Path.Combine(ConfigDirectoryPath, ConfigFileName);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicFileSettingsProvider"/> class.
    /// Ensures default values for file name and directory are set.
    /// </summary>
    public DeterministicFileSettingsProvider()
    {
        // Allow the readonly fields to be assigned only in the constructor if needed.
        // If a consumer changes the default at compile-time, they can modify the field initializer above.
        if (string.IsNullOrWhiteSpace(ConfigFileName))
        {
            // Note: ConfigFileName is readonly but can be assigned here if necessary.
            // (Left as-is because we provide a default initializer.)
        }

        // If ConfigDirectoryPath is not meaningful, default to fallback (LocalAppData).
        if (string.IsNullOrWhiteSpace(ConfigDirectoryPath) || ConfigDirectoryPath == ".")
        {
            // keep "." as default; actual write location will be validated in EnsureConfigDirectory()
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the provider with the specified name and configuration collection.
    /// </summary>
    /// <param name="name">Friendly name of the provider.</param>
    /// <param name="config">Provider-specific attributes from configuration.</param>
    public override void Initialize(string name, NameValueCollection config)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = nameof(DeterministicFileSettingsProvider);
        }
        base.Initialize(name, config);
    }

    /// <summary>
    /// Retrieves the values for the given settings property collection for the current context.
    /// If the configuration file does not exist or is unreadable, returns default values.
    /// </summary>
    /// <param name="context">SettingsContext describing the current application use.</param>
    /// <param name="properties">Collection of Setting properties to retrieve.</param>
    /// <returns>A SettingsPropertyValueCollection containing the values.</returns>
    public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
    {
        var values = new SettingsPropertyValueCollection();
        XDocument doc = null;
        string configPath = ConfigFileFullName;

        // If ConfigDirectoryPath is ".", combine with current directory explicitly.
        if (ConfigDirectoryPath == ".")
        {
            configPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigFileName);
        }

        if (File.Exists(configPath))
        {
            try
            {
                using (var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    doc = XDocument.Load(fs);
                }
            }
            catch
            {
                // If file is corrupt/unreadable, create a fresh document.
                doc = new XDocument(new XElement("settings"));
            }
        }
        else
        {
            doc = new XDocument(new XElement("settings"));
        }

        if (doc.Root == null)
            doc = new XDocument(new XElement("settings"));

        foreach (SettingsProperty prop in properties)
        {
            var el = doc.Root.Element(prop.Name);
            object value = el != null ? (object)el.Value : prop.DefaultValue;

            var spv = new SettingsPropertyValue(prop)
            {
                SerializedValue = value
            };
            values.Add(spv);
        }

        return values;
    }

    /// <summary>
    /// Persists the provided property values to the configuration file.
    /// Ensures the configuration directory exists and is writable before saving.
    /// </summary>
    /// <param name="context">SettingsContext describing the current application use.</param>
    /// <param name="values">Collection of Setting values to persist.</param>
    public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
    {
        EnsureConfigDirectory();

        string targetDirectory = ConfigDirectoryPath == "." ? Directory.GetCurrentDirectory() : ConfigDirectoryPath;
        string targetPath = Path.Combine(targetDirectory, ConfigFileName);

        var root = new XElement("settings");

        foreach (SettingsPropertyValue val in values)
        {
            string nodeName = val.Property != null && !string.IsNullOrEmpty(val.Property.Name)
                ? val.Property.Name
                : "unknown";

            string nodeValue = val.SerializedValue?.ToString() ?? "";
            root.Add(new XElement(nodeName, nodeValue));
        }

        var doc = new XDocument(root);

        using (var fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            doc.Save(fs);
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Returns a stable, unique name identifier for the current application.
    /// Can append version and/or deterministic hash.
    /// </summary>
    /// <param name="appendVersion">If true, append assembly version.</param>
    /// <param name="appendHash">If true, append deterministic hash suffix.</param>
    /// <param name="hashLength">Desired hash length (min 4, default 8).</param>
    /// <returns>Non-empty application identifier string.</returns>
    [DebuggerStepThrough]
    private string GetApplicationNameId(bool appendVersion, bool appendHash, int hashLength = 8)
    {
        const string fallbackApplicationName = "Application";

        string version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? string.Empty;

        if (hashLength <= 4) hashLength = 4;

        // Try to get product name from AssemblyProductAttribute
        Assembly asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        string name = asm?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

        if (string.IsNullOrWhiteSpace(name))
            name = asm?.GetName().Name;

        if (string.IsNullOrWhiteSpace(name))
            name = asm?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

        if (!string.IsNullOrWhiteSpace(name))
            name = name.Replace(' ', '_');

        if (appendHash || string.IsNullOrWhiteSpace(name))
        {
            if (asm != null)
            {
                var guidAttr = asm.GetCustomAttribute<GuidAttribute>();
                Guid guid = guidAttr != null ? new Guid(guidAttr.Value) : asm.ManifestModule.ModuleVersionId;

                if (guid != Guid.Empty)
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        string seed = guid.ToString("N");
                        hashLength = Math.Min(hashLength, md5.HashSize / 4);
                        string hash = ComputeDeterministicHashOfString(md5, seed, hashLength);

                        if (string.IsNullOrWhiteSpace(name))
                            name = fallbackApplicationName;

                        return appendVersion ? $"{name}_{version}_{hash}" : $"{name}_{hash}";
                    }
                }
            }
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            if (appendHash)
            {
                using (MD5 md5 = MD5.Create())
                {
                    string seed = GetType().FullName;
                    hashLength = Math.Min(hashLength, md5.HashSize / 4);
                    string hash = ComputeDeterministicHashOfString(md5, seed, hashLength);
                    return appendVersion ? $"{fallbackApplicationName}_{version}_{hash}" : $"{fallbackApplicationName}_{hash}";
                }
            }
            else
            {
                return appendVersion ? $"{fallbackApplicationName}_{version}" : fallbackApplicationName;
            }
        }

        return appendVersion ? $"{name}_{version}" : name;
    }

    /// <summary>
    /// Returns the company name specified in assembly attributes, or empty string when undefined.
    /// </summary>
    [DebuggerStepThrough]
    private string GetCompanyName()
    {
        return Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? string.Empty;
    }

    /// <summary>
    /// Computes a deterministic hex string of exactly <paramref name="length"/> characters
    /// using the provided hash algorithm and input string.
    /// </summary>
    [DebuggerStepThrough]
    private string ComputeDeterministicHashOfString(HashAlgorithm algorithm, string value, int length)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
        byte[] hash = algorithm.ComputeHash(bytes);

        var sb = new StringBuilder(length);

        for (int i = 0; i <= Math.Min((length / 2) - 1, hash.Length - 1); i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        if (length % 2 == 1 && hash.Length > (length / 2))
        {
            sb.Append((hash[length / 2] >> 4).ToString("X"));
        }

        int remaining = length - sb.Length;
        if (remaining > 0)
            sb.Append(new string('0', remaining));

        return sb.ToString();
    }

    /// <summary>
    /// Checks whether the current Windows identity has write permission to the given directory.
    /// </summary>
    /// <param name="dir">Directory path to check. Directory must exist.</param>
    /// <returns>True if write permission is granted; otherwise false.</returns>
    private bool CanWriteToDirectory(string dir)
    {
        if (!Directory.Exists(dir))
            throw new ArgumentNullException(nameof(dir));

        try
        {
            var directoryInfo = new DirectoryInfo(dir);
            var acl = directoryInfo.GetAccessControl();

            var rules = acl.GetAccessRules(true, true, typeof(SecurityIdentifier));
            var identity = WindowsIdentity.GetCurrent();

            var sids = new List<SecurityIdentifier>();
            if (identity?.User != null) sids.Add(identity.User);
            if (identity?.Groups != null)
            {
                foreach (var group in identity.Groups)
                {
                    if (group is SecurityIdentifier sid) sids.Add(sid);
                }
            }

            foreach (FileSystemAccessRule rule in rules)
            {
                var sid = rule.IdentityReference as SecurityIdentifier;
                if (sid != null && sids.Contains(sid))
                {
                    if ((rule.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData
                        && rule.AccessControlType == AccessControlType.Allow)
                        return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Ensures the configuration directory exists and is writable.
    /// If the configured directory fails, falls back to LocalApplicationData.
    /// Throws InvalidOperationException when no writable directory can be found.
    /// </summary>
    private void EnsureConfigDirectory()
    {
        string tempConfigDirectoryPath = ConfigDirectoryPath == "." ? Directory.GetCurrentDirectory() : ConfigDirectoryPath;

        if (string.IsNullOrWhiteSpace(tempConfigDirectoryPath))
        {
            tempConfigDirectoryPath = ConfigDirectoryPathFallback;
        }

        try
        {
            Directory.CreateDirectory(tempConfigDirectoryPath);
        }
        catch
        {
            // Fallback to LocalAppData
            tempConfigDirectoryPath = ConfigDirectoryPathFallback;
            try
            {
                Directory.CreateDirectory(tempConfigDirectoryPath);
            }
            catch
            {
                // ignore - permission check below will detect the issue
            }
        }

        if (!CanWriteToDirectory(tempConfigDirectoryPath))
        {
            if (tempConfigDirectoryPath != ConfigDirectoryPathFallback)
                tempConfigDirectoryPath = ConfigDirectoryPathFallback;

            if (!CanWriteToDirectory(tempConfigDirectoryPath))
                throw new InvalidOperationException($"Cannot write application's configuration file in directory: {tempConfigDirectoryPath}. Check user permissions.");
        }
    }

    #endregion
}
