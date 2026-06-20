# Public Release Status

Last updated: 2026-06-20

## Completed

- Added repository-level `AGENTS.md` for Codex work rules.
- Added `docs/MODERNIZATION_PLAN.md` for .NET and optional WinUI 3 planning.
- Refined `.gitignore` for .NET desktop build outputs, editor state, logs, and local settings.
- Rewrote `docs/README.md` to remove maintainer-local absolute paths and make the notes more public-safe.

## Current technical status

- Current app project: `app/CodexUsageTray.csproj`
- Current target framework: `net6.0-windows`
- Current UI framework: Windows Forms
- Recommended near-term direction: upgrade to a supported modern .NET LTS target while keeping Windows Forms.
- Recommended WinUI 3 direction: prototype later in a separate branch; do not rewrite the main app before the first stable public release.

## Remaining before first public release polish

- Add `LICENSE`.
- Update `README.md` with install, build, usage, privacy, limitations, and disclaimer sections.
- Add screenshots after redacting private account details.
- Upgrade the app target from `net6.0-windows` to the selected modern LTS target and verify locally.
- Add a Windows GitHub Actions build workflow.
- Decide release packaging: framework-dependent zip, self-contained zip, or installer/MSIX later.

## Recommended next Codex task

Ask Codex to do the .NET target upgrade only, with no UI framework rewrite:

```text
Read AGENTS.md and docs/MODERNIZATION_PLAN.md first.
Upgrade the existing Windows Forms app from net6.0-windows to net10.0-windows.
Do not migrate to WinUI 3.
Run dotnet build app/CodexUsageTray.csproj and dotnet run --project app/CodexUsageTray.csproj -- --self-test.
Report exact results and any local SDK requirement.
```
