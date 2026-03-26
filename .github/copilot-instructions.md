# CnE2PLC – Copilot Instructions

## What This Project Is

**CnE Toolbox** is a cross-platform .NET application for analyzing Allen-Bradley CompactLogix/ControlLogix PLC programs. It parses `.L5X` files (Studio 5000 XML exports) into an in-memory object model, displays tags in a data grid, and generates Excel-based engineering reports (I/O lists, interlocks, valves, instruments, etc.).

## Project Structure

| Project | Target | Role |
|---|---|---|
| `CnE2PLC.Avalonia` | `net10.0` | **Cross-platform UI** — Avalonia + MVVM (primary, macOS/Linux/Windows) |
| `CnE2PLC` | `net10.0-windows` | WinForms UI — legacy Windows-only, kept for reference |
| `CnE2PLC.PLC` | `net10.0` | Domain model: `Controller`, tags, programs, tasks, modules |
| `CnE2PLC.Helpers` | `net10.0` | XML, logging, string, object utility extension methods |
| `CnE2PLC.Reporting` | `net10.0` | NPOI-based Excel report generation |

`CnE2PLC.Avalonia` and the three library projects are fully cross-platform. `CnE2PLC` (WinForms) requires Windows.

## Build

```bash
# Cross-platform Avalonia UI + libraries (macOS/Linux/Windows)
dotnet build CnE2PLC.Avalonia/CnE2PLC.Avalonia.csproj

# Full solution (Windows only — WinForms project requires Windows)
dotnet build CnE2PLC.sln

# Cross-platform library projects only
dotnet build CnE2PLC.PLC/CnE2PLC.PLC.csproj
dotnet build CnE2PLC.Helpers/CnE2PLC.Helpers.csproj
dotnet build CnE2PLC.Reporting/CnE2PLC.Reporting.csproj

# Release
dotnet build CnE2PLC.Avalonia/CnE2PLC.Avalonia.csproj -c Release
```

CI has two jobs in `.github/workflows/dotnet-desktop.yml`:
- **`build-avalonia`** — runs on ubuntu, macOS, and Windows; builds `CnE2PLC.Avalonia`
- **`build`** — runs on `windows-latest`; MSBuild for the WinForms project (Debug + Release)

There are no automated tests.

## Architecture: L5X Parsing Flow

Every domain object deserializes from an `XmlNode` passed to its constructor. The entry point for a loaded file is:

```
Controller(XmlNode) 
  └─ ProcessTags(XmlNode)
       └─ CreateTag(XmlNode) → factory dispatch by DataType string
            └─ new DIData(node) | AIData(node) | Valve(node) | ...
```

`Controller` is the root container holding: `Tags`, `Programs`, `Tasks`, `Modules`, `DataTypes`, `AddOnInstructionDefinitions`. All collections are populated purely from the parsed XML — there is no database or save-back-to-L5X capability.

## Tag Type Hierarchy

```
PLCTag (abstract base)
└── XTO_AOI
    ├── DIData, DOData       (digital I/O)
    ├── AIData, AOData       (analog I/O)
    ├── Pumps, Tanks, Valves
    ├── Interlocks
    └── IO_Map, Compass, Enterflex
```

New tag types go in `CnE2PLC.PLC/Tags/XTO/`, inherit from `XTO_AOI`, and must be registered in `Controller.CreateTag()`.

## XML Parsing Convention

All XML access goes through extension methods on `XmlNode` defined in `CnE2PLC.Helpers/XMLHelper.cs`. **Never access attributes directly** — always use these helpers to get safe null-coalescing behavior:

```csharp
node.GetNamedAttributeItemInnerText("Name")          // string, "" if missing
node.GetNamedAttributeItemInnerTextAsInt("Value")    // int?
node.GetNamedAttributeItemInnerTextAsBool("Enabled") // bool?
node.AttributeExists("SomeAttr")                     // bool
node.GetChildNodeInnerText("Description")            // first child text
```

## Logging Convention

Use `Debug.Print` via `LogHelper.DebugPrint(message)` for all trace output. The `UiTraceListener` bridges `System.Diagnostics.Trace` to the UI's debug panel via an event:

```csharp
LogHelper.DebugPrint($"Loaded {count} tags in {elapsed}ms");
```

Never write directly to the UI from library projects — raise events or use `Trace`.

## Report Generation

`CnE2PLC.Reporting` uses **NPOI** to manipulate `CnE_Template.xlsx`. Reports iterate `Controller.AllTags`, type-switch on `DataType` strings, and call `InsertRow()`/`InsertCol()` helpers with pre-defined RGB cell formatting. Add new report sections by extending `CnE_Report.cs` or adding a new `*_Report.cs` class following the same pattern.

## Key C# Conventions

- **File-scoped namespaces** throughout (`namespace CnE2PLC.PLC;`)
- **Nullable reference types enabled** — all `XmlNode` constructors must handle missing/null attributes
- **Implicit usings enabled** — don't add redundant `using System;` etc.
- `Properties/Settings.Designer.cs` is auto-generated — edit via `Settings.settings` only
- WinForms `.Designer.cs` files are auto-generated — don't hand-edit them

## Avalonia UI Conventions (CnE2PLC.Avalonia)

### Structure
```
CnE2PLC.Avalonia/
  Services/         SettingsService (JSON), TraceService (log bridge), DialogHelper
  ViewModels/       MainWindowViewModel (CommunityToolkit.Mvvm [ObservableProperty]/[RelayCommand])
  Views/            MainWindow.axaml, AboutDialog.axaml
  Converters/       TagRowColorConverter (4 converters), BoolToGridLengthConverter
```

### MVVM Pattern
- All ViewModels extend `ViewModelBase` (`ObservableObject`)
- Use `[ObservableProperty]` for bindable properties — generates `OnXxxChanged` partial method hooks
- Use `[RelayCommand]` for commands — async commands use `async Task`, sync use `void`
- `CnE2PLC.PLC` defines a class named `Task` — always alias: `using SystemTask = System.Threading.Tasks.Task;`

### Tag Row Coloring
`Converters/TagRowColorConverter.cs` contains four `IValueConverter` implementations registered as static resources in `App.axaml`:
- `TagBgConverter` — background brush
- `TagFgConverter` — foreground brush  
- `TagFontWeightConverter` — Bold when Simmed or BypActive
- `TagFontStyleConverter` — Italic when AOICalls == 0

Apply via `DataGrid.Styles` using selector `DataGridRow` binding `$self.DataContext` to the converter. The rules mirror WinForms `CellFormatting` exactly — see `CnE2PLC/frmMain.cs TagsDataView_CellFormatting` for the source of truth.

### Settings Persistence
`SettingsService` saves JSON to `%APPDATA%/CnE2PLC/settings.json` (cross-platform via `Environment.SpecialFolder.ApplicationData`). Loaded in `App.axaml.cs` before window is shown; saved in `MainWindow.axaml.cs` `Closing` handler.

### Logging
`TraceService` subscribes to `UiTraceListener.TraceOutput` and raises `LogMessage` events filtered by `CurrentDebugLevel`. `MainWindowViewModel` appends to `LogText` (string, bound to read-only `TextBox`). `UiTraceListener` is registered in `Program.cs` before Avalonia starts.
