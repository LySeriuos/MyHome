namespace MyHomeBlazorApp.Services.Files;

/// <summary>Private document storage shared by uploads, downloads and email attachments.</summary>
public sealed class DeviceFileStorage
{
    public string RootPath { get; }

    public DeviceFileStorage(IWebHostEnvironment environment, IConfiguration configuration)
    {
        var configured = configuration["FileStorage:RootPath"];
        RootPath = Path.GetFullPath(Path.IsPathRooted(configured ?? "")
            ? configured! : Path.Combine(environment.ContentRootPath,
                string.IsNullOrWhiteSpace(configured) ? "Files" : configured));
        var publicRoot = Path.GetFullPath(environment.WebRootPath
            ?? Path.Combine(environment.ContentRootPath, "wwwroot"));
        if (IsWithin(RootPath, publicRoot))
            throw new InvalidOperationException("File storage must be outside wwwroot.");
    }

    public async Task<string> SaveAsync(int userId, int deviceId, byte[] bytes, string originalName)
    {
        if (userId <= 0 || deviceId <= 0) throw new ArgumentOutOfRangeException(nameof(deviceId));
        if (bytes.Length == 0) throw new ArgumentException("File is empty.", nameof(bytes));
        var extension = Path.GetExtension(GetFileName(originalName));
        var name = Guid.NewGuid().ToString("N") + extension;
        var key = $"{userId}/{deviceId}/{name}";
        var destination = PathForKey(key);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        await File.WriteAllBytesAsync(destination, bytes);
        return key; // Portable reference; never store a machine-specific absolute path.
    }

    public string? ResolveExisting(string? reference, int userId, int deviceId)
    {
        if (string.IsNullOrWhiteSpace(reference) || userId <= 0 || deviceId <= 0) return null;
        var key = GetStorageKey(reference);
        if (key is null) return null;
        var parts = key.Split('/');
        // Existing receipts used both Files/user/device/name and Files/user/name.
        var matches = parts.Length == 3 && parts[0] == userId.ToString()
            && parts[1] == deviceId.ToString();
        var legacy = parts.Length == 2 && parts[0] == userId.ToString();
        if (!matches && !legacy) return null;
        var path = PathForKey(key);
        return File.Exists(path) ? path : null;
    }

    public static string GetFileName(string reference) =>
        Path.GetFileName(reference.Replace('\\', '/'));

    private static string? GetStorageKey(string reference)
    {
        var value = reference.Replace('\\', '/');
        // Extract only the old storage-relative tail, never open its original absolute path.
        var marker = value.LastIndexOf("/Files/", StringComparison.OrdinalIgnoreCase);
        if (marker >= 0) value = value[(marker + 7)..];
        else if (value.StartsWith("Files/", StringComparison.OrdinalIgnoreCase)) value = value[6..];
        var parts = value.Split('/');
        if (parts.Length is not (2 or 3) || parts.Any(p => string.IsNullOrWhiteSpace(p)
            || p is "." or ".." || p.Contains(':') || p.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
            return null;
        return string.Join('/', parts);
    }

    private string PathForKey(string key)
    {
        var path = Path.GetFullPath(Path.Combine(RootPath, key.Replace('/', Path.DirectorySeparatorChar)));
        if (!IsWithin(path, RootPath) || path == RootPath)
            throw new InvalidOperationException("Invalid file reference.");
        return path;
    }

    private static bool IsWithin(string path, string root)
    {
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return path.Equals(root, comparison) || path.StartsWith(
            root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, comparison);
    }
}
