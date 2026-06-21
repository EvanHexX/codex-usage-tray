# Codex Usage Tray

Codex Usage Tray is a small Windows tray utility for monitoring Codex app-server rate limit status without keeping the Codex app, CLI, or dashboard view open.

The current release target is a lightweight Windows Forms version. After the WinForms release is stabilized, the project is planned to move to a WinUI 3 implementation.

> Current product direction: keep the WinForms version small, clear, and practical; remove the experimental Glassmorphism theme before release; then use the release as the baseline for a WinUI 3 port.

## What it does

Codex Usage Tray runs locally, starts `codex app-server`, reads the current Codex rate limit data through stdio JSON-RPC, and displays the result in a compact tray popup.

It is designed for people who use Codex frequently and want a quick view of remaining usage windows while working.

## Features

- Windows system tray utility
- Compact popup for Codex usage windows
- 5-hour and 1-week usage gauges
- Optional Spark usage gauges
- Pinned popup mode
- Global hotkey: `Ctrl+Alt+U`
- Manual refresh and reconnect controls
- Position, time display, shape theme, and color theme settings
- Local `settings.json` persistence
- No separate OpenAI API key required

## Current UI

The current WinForms UI is a compact dark tray popup with circular usage cards.

Default rows:

- `5h`
- `1w`

Optional Spark rows:

- `Spark 5h`
- `Spark 1w`

The popup supports two common layouts:

- Standard Codex usage only: 2 cards
- Codex + Spark usage: 4 cards

The current UI is intentionally simple and will be used as the functional baseline before the WinUI 3 port.

## How it works

1. The app starts `codex app-server` as a child process.
2. It initializes a stdio JSON-RPC session.
3. It calls `account/rateLimits/read`.
4. It listens for `account/rateLimits/updated` notifications.
5. The tray popup updates when new rate limit data arrives.

This app uses the local Codex runtime session. It does not ask for or store a separate OpenAI API key.

## Requirements

- Windows
- Codex CLI / Codex app-server available through the `codex` command
- .NET 10 SDK for local development

The current project targets:

```text
net10.0-windows
Windows Forms
```

## Run locally

From the repository root:

```powershell
dotnet run --project app/CodexUsageTray.csproj
```

Or from the app directory:

```powershell
cd app
dotnet run
```

Run the built-in mapper self-test:

```powershell
dotnet run --project app/CodexUsageTray.csproj -- --self-test
```

## Settings

The app stores local settings next to the running app output:

```text
settings.json
```

Example settings:

```json
{
  "hotkey": "Ctrl+Alt+U",
  "refreshSeconds": 60,
  "warningThresholdPercent": 20,
  "popupGraph": "half-circle",
  "codexCommand": "codex",
  "popupPosition": "BottomRight",
  "shapeTheme": "Bars",
  "colorTheme": "DarkBluePurple",
  "timeDisplayMode": "ClockTime",
  "isPinned": false,
  "showSparkUsage": false
}
```

Notes:

- `timeDisplayMode` can be `ClockTime` or `RemainingTime`.
- `showSparkUsage` enables the Spark rows.
- The `hotkey` setting exists in the settings file, but the current registered hotkey is fixed to `Ctrl+Alt+U`.

## Current limitations

- Windows-only.
- The current UI is Windows Forms, not WinUI 3.
- The app depends on Codex app-server behavior and available rate limit fields.
- It is a local tray utility, not a cloud dashboard or analytics product.
- Settings are stored locally in the app output folder.

## Roadmap

Near-term:

1. Remove the experimental Glassmorphism theme.
2. Finalize the current Windows Forms release.
3. Improve README, release notes, and screenshots.
4. Tag a WinForms baseline release.

Next major step:

1. Port the UI to WinUI 3.
2. Keep the Codex app-server integration and rate limit mapping behavior stable.
3. Rebuild the UI with a more maintainable native Windows app structure.
4. Re-evaluate packaging and distribution after the WinUI 3 port.

Possible future rename:

- `QuotaScope`

For now, the repository remains `Codex Usage Tray` because the current implementation is Codex-first.

## Documentation

Project notes live under `docs/`.

- `docs/README.md`: operating notes and behavior details
- `docs/PROJECT_MAP.md`: module and file map
- `docs/MODERNIZATION_PLAN.md`: .NET / WinUI modernization plan
- `docs/modules/codex_rate_limits.md`: Codex app-server rate limit schema and mapping notes

## Korean README

한국어 README는 [`README.ko.md`](README.ko.md)를 참고하세요.
