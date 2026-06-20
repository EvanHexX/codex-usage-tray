# Codex Usage Tray

Codex Usage Tray is a small Windows tray app that shows Codex app-server rate limit status without keeping the Codex app or CLI view open.

## Concept

- App name: `Codex Usage Tray`
- Internal concept: provider-ready, but Codex-first
- Future rebrand candidate: `QuotaScope`

The current implementation is intentionally Codex-first. It launches `codex app-server`, reads `account/rateLimits/read` over stdio JSON-RPC, and updates the popup when `account/rateLimits/updated` notifications arrive. It does not require a separate OpenAI API key.

## Run

```powershell
cd app
dotnet run
```

The app targets `net10.0-windows` and uses Windows Forms.

Local build requirement: install the .NET 10 SDK.

## Features

- Tray popup for 5-hour and 1-week Codex usage windows
- Optional Spark usage rows
- `Ctrl+Alt+U` global hotkey
- Pinned popup mode
- Position, time display, shape theme, and color theme settings
- Local `settings.json` persistence in the app output folder

## Documentation

Project memory lives under `docs/`.

- `docs/README.md`: 운영/동작 메모
- `docs/PROJECT_MAP.md`: 주요 module과 실제 file path map
- `docs/modules/codex_rate_limits.md`: Codex app-server rate limit schema와 mapping 규칙
