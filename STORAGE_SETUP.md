# Move uploaded documents out of bin

## What changes

A shared DeviceFileStorage service now handles paths for upload, protected download and email attachments. Its default root is Files under the application's content root: MyHomeBlazorApp/Files in local development, /app/Files in the existing Docker container. The supplied Docker Compose file already mounts /app/Files as a persistent volume; no volume changes are required.

New database references are portable keys such as 10/102/random-name.pdf. Old Windows and Linux absolute references are interpreted by their Files/user/device/name tail. Old Files/user/name references are also supported for registered receipts. Files are only read from the configured root; the old machine-specific absolute path is not opened. Download requests still require ownership, and the filename must match a receipt or extra-warranty reference for that device.

CheckIfFileExist no longer clears database fields. Details no longer calls that old cleanup method. No entity changes or migrations are needed.

## Apply the small patch

1. Stop the app and back up your current source, database and document folders.
2. Extract the patch. Copy its MyHomeBlazorApp folder over the matching project folder. This updates Program.cs, the DataService constructor/files partial, FilesController, the email page and the Details code-behind, and adds Services/Files/DeviceFileStorage.cs. Compare your Program.cs first if you made your own changes since the organised copy.
3. Copy documents from the OLD project's Files, bin/Debug/net8.0/Files and bin/Release/net8.0/Files into the NEW project's Files directory. Keep the numeric subfolders. Copy, do not move or delete. Do not overwrite differing files.
4. Rebuild and run with the https/http Visual Studio profile. Test an existing receipt, a new upload, extra warranty, reopening after restart, and email attachments to your own test address if you use that feature.

The patch does not contain your database or uploaded documents. Source structure was checked here, but .NET compilation, PowerShell execution and browser testing could not be performed in this environment.

## Optional copy helper (Windows PowerShell)

The accompanying tools/Copy-UploadedFiles.ps1 previews by default. Both arguments are paths to the MyHomeBlazorApp PROJECT folders, not the solution folders. Stop the apps before using it.

Example preview:

```powershell
.\tools\Copy-UploadedFiles.ps1 -OldProject 'C:\Users\shiranco.DESKTOP-HRN41TE\source\repos\MyHomeNew\MyHomeBlazorApp' -NewProject 'C:\Users\shiranco.DESKTOP-HRN41TE\Downloads\MyHome_Organized_Source\MyHome\MyHomeBlazorApp'
```

After checking the paths and preview, add -Apply to the same command. It copies only missing files, leaves original files in place, and reports conflicting destination files without overwriting them. If a file is absent from all source folders, this helper cannot restore it. A preview can show the same proposed destination more than once when several sources contain that file; the applying run skips identical existing copies.

## Optional configuration

No appsettings changes are needed for the default. To choose an absolute directory independent of the project folder, merge this setting into appsettings.Development.json or User Secrets (retain all existing settings):

```json
"FileStorage": {
  "RootPath": "C:\\MyHomeData\\Files"
}
```

For an environment variable, use FileStorage__RootPath. Relative paths are resolved against the application content root. The root must be outside wwwroot so documents cannot bypass the authorised download endpoint. Copy existing document subfolders to whichever root you choose.

This fixes physical storage, not future device/account ownership transfer. Transfers will need a separate document ownership design.
