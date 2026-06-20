# CodexUsageTray

CodexUsageTray는 Codex app-server의 rate limit 정보를 Windows tray popup으로 보여주는 개인 운영 도구다.

## 목적

- Codex 사용 중 남은 rate limit percent를 빠르게 확인한다.
- Codex app이나 CLI 화면을 열지 않아도 5시간/1주 window 상태와 reset time을 본다.
- `E:\Business`와 분리된 `E:\Tools` 아래에 두어 business project와 개인 운영 도구를 섞지 않는다.

## 실행

```powershell
cd E:\Tools\CodexUsageTray\app
dotnet run
```

PowerShell execution policy 때문에 `codex.ps1` shim이 막히는 환경에서는 앱이 내부적으로 `cmd.exe /c codex app-server`를 사용한다.

## Codex 연결

현재 앱은 별도 API key를 직접 쓰지 않는다.

1. 앱이 `cmd.exe /c codex app-server`를 child process로 실행한다.
2. stdio JSON-RPC로 `initialize`를 보낸다.
3. `account/rateLimits/read`를 호출한다.
4. `account/rateLimits/updated` notification을 받으면 UI를 갱신한다.

## UI

- tray icon click: popup 열기/닫기
- `Ctrl+Alt+U`: popup 열기/닫기
- 우측 상단 pin icon: pinned 상태를 토글한다.
- pinned 상태에서는 popup이 항상 위에 있고, focus를 잃거나 `Esc`를 눌러도 자동으로 닫히지 않는다.
- 상단 header 영역은 drag handle이며 borderless popup을 이동하는 데 사용한다.
- tray icon 또는 열린 popup에서 우클릭하면 같은 menu를 연다:
  - `Refresh`
  - `Toggle`
  - `Settings > Codex Connection > Reconnect`
  - `Settings > Position`
  - `Settings > Time Display`
  - `Settings > Usage Rows > GPT-5.3 Spark`
  - `Settings > Shape Theme`
  - `Settings > Color Theme`
  - `Exit`
- popup은 titlebar 없는 dark/glass style이다.
- color theme은 `Dark Blue Purple`과 `Glassmorphism`을 지원한다.
- 가장 바깥쪽 canvas는 transparent key로 투명하게 처리한다.
- 표시 정보는 기본적으로 `5h`, `1w` 두 window를 사용한다. `Spark 5h`, `Spark 1w`는 setting에서 선택적으로 표시한다.
- user-visible popup labels and connection status messages are written in English.
- popup의 time text를 클릭하면 `Clock Time`과 `Remaining Time` 표시가 전환된다.
- `Bento Circles` theme은 좌상단 label, 중앙 large circular gauge, 하단 reset/remaining time 구조의 taller card layout을 사용한다. Spark 표시가 켜지면 2x2 circle layout을 사용한다.
- 기본 font는 Pretendard/Pretendard Variable을 우선 사용하고, 설치되어 있지 않으면 Segoe UI로 fallback한다.
- DPI 흐림 방지를 위해 앱 시작 시 `Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)`를 설정한다.

## 설정

앱 실행 폴더의 `settings.json`이 있으면 읽는다. 없으면 기본값을 사용한다. Settings menu에서 위치나 theme을 고르면 이 파일에 저장된다.

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

`timeDisplayMode`는 `ClockTime` 또는 `RemainingTime`을 사용한다.

현재 `hotkey` 문자열은 향후 변경 UI를 위한 준비값이며 실제 등록은 `Ctrl+Alt+U`로 고정한다.





