# MyHome: organised source and learning guide

This is a separate source copy of your uploaded solution. Open MyHome.sln in Visual Studio. The first completed refactor is the Details page, plus a solution-wide navigation cleanup.

## Before using this copy

1. Keep your original project as a backup. Extract this ZIP into a NEW folder; do not merge it over the old folder, because old duplicate source files would remain.
2. Open MyHome.sln. Set MyHomeBlazorApp as the startup project.
3. Restore NuGet packages and build the solution in Visual Studio.
4. Configure the development database and email settings locally. Your database files, uploaded documents, XML data and Data Protection keys are deliberately not in this source archive. User Secrets are associated with the existing project UserSecretsId and remain on your own computer.
5. If you want to test existing data, use a COPY of your database and document storage with development configuration. Startup still runs EF migrations as in the original app.
6. Check the four Details tabs, add/edit dialogs, QR selection/printing, receipts, account pages and mobile layout.

This environment has no .NET SDK or Docker and could not download the SDK. The result is source-reviewed and structurally checked, NOT compile- or runtime-verified. Do not treat it as a verified release.

## 1. Understand the two projects

| Project | Meaning | Put here |
|---|---|---|
| MyHome | Shared C# class library, not the web startup application | Business objects and calculations that do not need Blazor, HTTP, Identity or EF Core |
| MyHomeBlazorApp | Running web application | Pages, components, database access, login, file handling, email and startup configuration |

The web app references the library. The library must not reference the web app. The uploaded MyHome.csproj already had OutputType=Library; its old Program.cs was actually a ManualTester helper.

## 2. Where to find things

| Folder | Responsibility | Example |
|---|---|---|
| MyHome/Models | Existing business entities and enum | DeviceProfile, RealEstate, DeviceWarranty |
| MyHome/Services | Shared operations without web/database dependencies | WarrantyQueries, DeviceQueries, QrCodeGenerator |
| MyHome/Catalog | Device category/detail suggestions | DeviceDetails |
| MyHome/Utilities | Small reusable technical helpers | Existing URL helper Util |
| MyHome/Legacy | Earlier console, XML, sample-data and unused model code | ManualTester, UI, XMLFileUitilities, LegacyLogic |
| Web/Data | Identity database context and application user | MyHomeBlazorAppContext, MyHomeBlazorAppUser |
| Web/Migrations | EF-generated database change history | Leave generated files alone during folder cleanup |
| Web/Authentication | Existing impersonation claim constants | ImpersonationClaimTypes |
| Web/Services/Inventory | Current stateful data service, divided into partial files | DataService.Devices.cs |
| Web/Services/Email | Email delivery and options | EmailSender, AuthMessageSenderOptions |
| Web/ViewModels | Models used by web forms | EmailRequestModel |
| Web/Pages | Screens with URLs | Inventory/Details.razor |
| Web/Components | Reusable UI and feature sections | Devices/AddEditDeviceForm.razor |
| Web/Controllers | HTTP endpoints | FilesController for protected downloads |
| Web/DemoData | EF demo-data creation | DemoDataSeeder |
| Web/wwwroot | Public CSS, JavaScript, images and libraries | Public assets only |
| Web/Program.cs | Dependency injection, middleware, endpoints and startup | Register services here |

Web means MyHomeBlazorApp in the table above. Physical document storage and SQLite storage paths are runtime concerns; they do not change just because source files move.

## 3. Pages: choose by the screen's purpose

| Location under Pages | Screen |
|---|---|
| Home/Index.razor | Public landing page |
| Home/WalkThrough.razor | Product walkthrough |
| Dashboard/MainDashboard.razor | Signed-in summary |
| Inventory/Details.razor | Combined Estates, Devices, Warranties and Shops workspace |
| Devices/MobileDeviceInfo.razor | Device view opened from QR links |
| QrCodes/QrCoder.razor | QR creation |
| QrCodes/QrPrinting.razor | HTML QR print page |
| Email/CreateEmail.razor | Compose a device-related email |
| Admin/UserManagement.razor | User administration |
| Admin/Danger.razor | Existing destructive administration screen; review before using |
| Error.cshtml + Error.cshtml.cs | Framework error page; kept at its existing route |

Details is under Inventory because it covers both properties and devices. Changing the folder does not change a route: the @page directive defines the URL.

Account pages remain under Components/Account/Pages, as in the Identity template. Some components therefore have URLs too; don't move generated authentication files merely to force one folder convention.

## 4. Components: choose by responsibility

| Components folder | Contents |
|---|---|
| Details | Four extracted table/tab sections |
| Devices | Device form, placement display and device navigation |
| Devices/Modals | Existing device dialogs |
| RealEstates/Modals | Property editing/deletion and reassignment dialogs |
| Warranties | Warranty form and its modal |
| Shops | Shop form and its modal |
| Addresses | Address form fields |
| Common/Buttons | Generic Save, Cancel and Previous/Next controls |
| Common/Display | Cards and empty-value display |
| Common/Modals | Generic modal shell and success/confirmation dialogs |
| Layout | Main layout, navigation menu and login display |
| Navigation | Existing LoginRedirect helper |
| TabComponents | Existing TabControl and TabPage |
| Account | Identity pages, layouts and supporting authentication helpers |

A reusable component accepts Parameters and raises EventCallbacks. Feature components may currently use services. Don't inject the whole data service into a generic button or display component when it doesn't use it; removing these old unused injections is a later, small cleanup.

## 5. Walk through the Details refactor

Open these files in order:

1. Pages/Inventory/Details.razor: routes, four tabs and the modal declarations. Reduced from 636 lines to 94 lines of markup/directives.
2. Components/Details/EstatesTab.razor: renders the estate grid. Items is an input. OnAdd, OnEdit and OnDelete are outputs.
3. Pages/Inventory/Details.razor.cs: loads data and handles those callbacks. The active device-edit flow still uses CustomReusableModal and AddEditDeviceForm.
4. The other three tab components: same idea, applied to devices, warranties and shops.
5. Pages/Inventory/Details.razor.css: parent styles use ::deep where necessary to reach markup now rendered by child components.

Example: an Edit button in EstatesTab calls OnEdit.InvokeAsync(estateId). The parent binds OnEdit to OnShowModalRealEstateClick. That method selects the estate and opens the existing modal. The modal saves through DataService and reports that data changed, so the page refreshes its inputs.

The parent owns the workflow. The tab owns its table. This is the pattern to repeat on other large pages.

## 6. What happened to Logic.cs?

| New class | Methods/responsibility |
|---|---|
| WarrantyQueries | ExpiringDevicesWarrantiesInDays, GetUserDevicesWarranties |
| DeviceQueries | GetAllUserDevices, GetDeviceInfoByID |
| QrCodeGenerator | Device QR bytes and Wi-Fi QR generation |
| LegacyLogic | Old XML deletion, manual ID calculations and old device-moving helper |

Call sites were updated. Method bodies were retained so organisation and business-behaviour changes are not mixed together. These methods still have opportunities for correction and simplification; the names do not imply their behaviour has been fully validated.

## 7. Understand partial DataService files honestly

DataService is still ONE object registered in Program.cs. The partial keyword allows one class to occupy several source files. This improves navigation; it does not create independent services or solve its state/lifetime design.

| File under Services/Inventory | Contains |
|---|---|
| DataService.cs | Constructor, shared state and SaveChanges wrapper |
| DataService.Users.cs | Current user and profile loading, ownership lookup |
| DataService.RealEstates.cs | Property operations |
| DataService.Devices.cs | Device operations and existing manual lookup |
| DataService.DevicePlacement.cs | Existing bulk move to unassigned devices |
| DataService.Warranties.cs | Warranty selection/calculation helpers |
| DataService.QrQueue.cs | QR print selection |
| DataService.Files.cs | Existing file writes, deletion and file URL helpers |

The public method inventory (38 methods) was preserved. New independent services should be extracted after the organised copy builds and existing flows have been tested. The best first extraction is the print queue, followed by document storage. User, property and device operations share tracked EF data today, so splitting those into independent services needs a deliberate transaction and DbContext lifetime design.

## 8. Why some namespaces still have old names

Existing namespaces, entity names and component names were deliberately retained during moves. For example a file physically under Services/Inventory still declares MyHomeBlazorApp.BlazorData. The existing Identity model and migrations retain their types. Explicit @namespace directives preserve moved Razor component identities.

For a NEW project, normally make namespaces match folders. For an EXISTING project, physical organisation and namespace renaming can be separate refactors. This copy demonstrates that incremental approach; it is not a recommendation to invent mismatched namespaces.

The new Details tab namespace is MyHomeBlazorApp.Components.DetailsTabs. This avoids a namespace named Details shadowing the existing Details component used by TabPage.

## 9. What was removed and what remains for review

The upload included both original and moved copies of DataService, Identity classes, mobile device view and QR pages. The moved/latest locations were kept. Empty DbSet and EmailService classes were removed. See docs/refactor-moves.json for the exact file move/removal list.

The old untracked PdfController was removed from this reference copy: it called a no-longer-existing two-argument QR method and its no-devices test was reversed. Current QR printing uses Pages/QrCodes/QrPrinting.razor. If any external client uses /api/pdf/generate-qr, restore and redesign that endpoint separately; it is not included here.

Legacy code was preserved, not declared safe to delete. The Legacy folder is still compiled in the shared project. A separate legacy project or removal can follow after confirming that no external consumers rely on it.

## 10. Existing issues to address next

These are source-review findings, not claims of tested exploits or runtime failures:

- Several write operations load entities by ID without an ownership condition inside the operation. Centralise authorisation on every write before introducing service companies or transfers. The presence of IsDeviceOwnedByUserAsync elsewhere is not a substitute.
- The server-side stateful DataService holds an EF context and caches the current user. Review Blazor circuit lifetime, concurrent operations and impersonation/cache reset before extracting independent database services.
- MoveDeviceToUnnassignedList uses an OR null-check that can dereference null; guard the argument and source property explicitly.
- Existing uploaded-file paths include user IDs. Future ownership transfer needs a document identity/storage model independent of the old owner; moving a device row does not move its files.
- CreateEmail still contains SMTP composition/delivery logic in the page. Move that workflow behind an email application service after the structural cleanup is verified.
- WarrantyQueries currently includes already-expired warranties and omits unassigned devices, matching the old behaviour. Define the desired rule and test it before changing it.
- Legacy device-moving/ID helpers are not the place to build the new transfer workflow.
- Some modals overlap (ModalEditAddDevice versus AddEditDeviceForm) and generic controls have unused service injections. Keep or delete based on actual references and behaviour, not filename alone.
- Admin/Danger is destructive functionality. Review its authorisation and intended availability before using it.

No new HouseBook entities, database schema changes, package upgrades or production deployment are included.

## 11. Reuse these rules on your next project

Ask in order:

1. Is it business data? Put the entity in Models.
2. Is it a business calculation independent of the web/database? Put it in a focused shared class.
3. Does it query/save the database, write files or send email? Put the operation in a focused application/infrastructure service.
4. Is it a screen with a URL? Put it in a feature group under Pages (or keep the framework's Account convention).
5. Is it a section used by a page? Put it under Components and give it parameters and callbacks.
6. Is it used across unrelated features? Common is appropriate; otherwise keep it with its feature.
7. Is it startup wiring? Program.cs.
8. Is it an earlier experiment? Clearly identify it as Legacy; verify dependencies before deletion.

Do not add more projects just to get more folders. A useful boundary is about responsibility and dependencies, not file count.

## Validation completed here

- DataService public method inventory preserved across partial files.
- 44 distinct Razor route strings with no duplicate strings.
- All four Details tab parameter sets and parent callback names checked.
- Existing remaining model, Identity and migration definitions preserved apart from text encoding/newlines.
- Project XML parsed and duplicate public non-partial types checked.

Not performed: NuGet restore, C#/Razor compilation, CSS/browser rendering, database integration, Docker build or deployment. Run those in your own development environment before adoption.
