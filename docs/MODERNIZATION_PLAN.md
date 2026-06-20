# Modernization Plan

Last updated: 2026-06-21

## Current repository state

Codex Usage Tray is currently a small Windows tray utility. The implementation is Codex-first and uses `codex app-server` over stdio JSON-RPC to read rate limit data.

Current technical baseline:

- App project: `app/CodexUsageTray.csproj`
- Current target: `net10.0-windows`
- Current UI stack: Windows Forms
- Current entry point: `app/Program.cs`
- Current self-test path: `dotnet run --project app/CodexUsageTray.csproj -- --self-test`
- Current documentation map: `docs/PROJECT_MAP.md`

## Modernization decision

### Recommended near-term direction

Keep the app on Windows Forms for the first public release, but move the target framework from `.NET 6` to a supported modern .NET LTS release.

Recommended near-term target:

```xml
<TargetFramework>net10.0-windows</TargetFramework>
<UseWindowsForms>true</UseWindowsForms>
```

Rationale:

- `.NET 6` is out of support.
- `.NET 10` is an LTS release with a longer support window.
- The current UI is already a custom tray/popup implementation using Windows Forms, `NotifyIcon`, GDI+ drawing, and Win32-style hotkey/window behavior.
- A .NET target upgrade is much smaller and safer than a UI framework rewrite.

### WinUI 3 decision

Do not migrate the main app to WinUI 3 before the first stable public release.

WinUI 3 is a valid future option, but it should be treated as a separate v2 prototype/rewrite because the current app depends heavily on Windows Forms concepts:

- `NotifyIcon`
- `ApplicationContext`
- borderless `Form` popup behavior
- custom GDI+ painting
- `TransparencyKey`
- native-window global hotkey handling
- context menus shared between tray icon and popup

Expected WinUI 3 benefits:

- modern Fluent UI controls and styling
- better fit with current Windows app design language
- stronger foundation for animation-heavy UI
- Windows App SDK APIs for modern app lifecycle/windowing scenarios

Expected WinUI 3 costs/risks:

- popup and tray behavior may require extra Win32 interop or dependencies
- current custom drawing must be replaced with XAML/Win2D/composition equivalents
- packaging/runtime deployment becomes more complex
- migration may delay the public release without improving the core Codex usage-monitoring value

## Phase 0 — Public repository hygiene

Checklist:

- [x] Add `AGENTS.md` for Codex working rules.
- [x] Add this modernization plan.
- [ ] Add or verify `.gitignore` for .NET, Visual Studio, logs, and local settings.
- [ ] Add a license file before promoting the repository publicly.
- [ ] Remove or avoid maintainer-local absolute paths from docs.
- [ ] Add public README sections for installation, usage, privacy, limitations, and disclaimer.
- [ ] Add screenshots only after redacting private account information.

Exit criteria:

- A new contributor can understand what the app does and how to build it.
- Public documentation does not imply OpenAI affiliation.
- Public documentation does not expose local machine paths, secrets, or private account details.

## Phase 1 — Move from .NET 6 to supported .NET LTS while keeping Windows Forms

Goal:

Upgrade runtime support without changing UI architecture.

Checklist:

- [x] Check installed SDKs with `dotnet --list-sdks`.
- [x] Confirm the selected modern .NET SDK is available locally.
- [x] Change `app/CodexUsageTray.csproj` from `net6.0-windows` to the selected modern target, preferably `net10.0-windows`.
- [x] Keep `<UseWindowsForms>true</UseWindowsForms>`.
- [x] Run `dotnet build app/CodexUsageTray.csproj`.
- [x] Run `dotnet run --project app/CodexUsageTray.csproj -- --self-test`.
- [ ] Run the Windows tray smoke test:
  - [ ] tray icon appears
  - [ ] popup opens from tray click
  - [ ] `Ctrl+Alt+U` toggles popup
  - [ ] Refresh works
  - [ ] Reconnect works
  - [ ] pinned popup behavior works
  - [ ] settings persist after restart
- [x] Update `README.md` build requirements.
- [x] Update docs if target framework or commands changed.

Exit criteria:

- App builds and runs on the selected modern target.
- Self-test passes.
- Existing tray behavior is preserved.
- No WinUI 3 migration is included in this phase.

Rollback:

- Revert only the target-framework change and any directly related documentation updates.

## Phase 2 — Settings and architecture hardening

Goal:

Make the app safer and easier to maintain before packaging/release work.

Checklist:

- [ ] Move default settings storage away from the app output folder to a per-user application data path.
- [ ] Preserve backward compatibility by reading existing output-folder `settings.json` if present.
- [ ] Add a small settings migration note to docs.
- [ ] Consider extracting Codex rate-limit reading behind an interface, but keep only the Codex provider implemented.
- [ ] Avoid adding non-Codex providers until explicitly requested.
- [ ] Add unit-test project or keep the current self-test if a full test project is not worth the overhead yet.
- [ ] Add validation for unusual/missing Codex app-server payload fields.
- [ ] Improve error messages for missing `codex`, expired login, app-server timeout, and JSON-RPC errors.

Exit criteria:

- Settings are stored in a user-safe location.
- Failure states are clearer for users.
- Provider-ready structure exists only where it improves maintainability.

## Phase 3 — Public release readiness

Goal:

Prepare a clean first public GitHub release.

Checklist:

- [ ] Add `LICENSE`.
- [ ] Add GitHub Actions build workflow on `windows-latest`.
- [ ] Add release/publish instructions.
- [ ] Decide deployment mode:
  - [ ] framework-dependent zip
  - [ ] self-contained zip
  - [ ] MSIX or installer later
- [ ] Add a privacy section explaining that the app launches local `codex app-server` and does not use a separate OpenAI API key.
- [ ] Add a limitations section explaining dependence on Codex app-server behavior.
- [ ] Add screenshots after redaction.
- [ ] Add disclaimer:

> This project is not affiliated with, endorsed by, or sponsored by OpenAI. Codex is a product/service of OpenAI.

Exit criteria:

- A user can download/build/run the app from the README.
- Build instructions are reproducible.
- The release does not include local artifacts or private data.

## Phase 4 — Optional WinUI 3 prototype

Goal:

Evaluate whether WinUI 3 improves the app enough to justify a rewrite.

Status: TODO. Do not migrate the main Windows Forms app to WinUI 3 as part of the .NET target upgrade.

Branch recommendation:

```text
prototype/winui3-popup
```

Checklist:

- [ ] Do not change the Codex app-server client during the prototype.
- [ ] Build a minimal WinUI 3 shell that can show the same usage data.
- [ ] Solve tray icon integration deliberately; do not assume WinUI 3 has a direct `NotifyIcon` equivalent.
- [ ] Reproduce global hotkey behavior.
- [ ] Reproduce popup positioning and pinning behavior.
- [ ] Reproduce dark popup visual style.
- [ ] Compare app startup time, memory use, packaging complexity, and maintenance cost.
- [ ] Document the result before replacing the main app.

WinUI 3 migration can replace the main app only if all parity items pass and the maintenance/deployment cost is acceptable.

## Suggested task order for Codex

Use small tasks like these:

1. "Audit public repo hygiene and report only. Do not modify files."
2. "Add or refine .gitignore for a .NET Windows desktop app. Do not change source code."
3. "Upgrade the Windows Forms project from net6.0-windows to net10.0-windows. Run build and self-test. Do not change UI framework."
4. "Move settings storage to per-user AppData with backward-compatible migration. Keep UI behavior unchanged."
5. "Add GitHub Actions build workflow for Windows. Do not publish artifacts yet."
6. "Draft README sections for install, usage, privacy, limitations, and disclaimer using provided screenshots."
7. "Create a separate WinUI 3 prototype branch and report feasibility. Do not merge into main."

## Source notes

- Microsoft .NET support policy: https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core
- Windows App SDK release channels: https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/release-channels
- WinUI 3 overview: https://learn.microsoft.com/en-us/windows/apps/winui/winui3/
