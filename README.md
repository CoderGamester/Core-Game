# Core Game Template Project

## 🎮 Overview
Build fast, scalable mobile and WebGL games with a clean architecture, Addressables, and a lightweight service layer.

[![Unity](https://img.shields.io/badge/Unity-6000.0.55f1-black?logo=unity)](ProjectSettings/ProjectVersion.txt)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CoderGamester/Core-Game/blob/master/LICENSE)

Target audience: Unity developers building mobile (iOS/Android) and WebGL games who want a solid starting point with best practices.

---

## 📑 Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Project Architecture](#project-architecture)
- [Folder Structure](#folder-structure)
- [Packages and Dependencies](#packages-and-dependencies)
- [Addressables Setup and Workflow](#addressables-setup-and-workflow)
- [Scenes and Game Flow](#scenes-and-game-flow)
- [Build and Deployment](#build-and-deployment)
- [Performance & Optimization](#performance--optimization)
- [Troubleshooting](#troubleshooting)
- [API References](#api-references)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 🚀 Features
- **SOLID-first architecture** with manual dependency injection.
- **Addressables-first asset pipeline** for dynamic loading and small initial APK/WebGL payloads.
- **Message-driven design** with a simple message broker.
- **State machine–driven game flow**.
- **WebGL-ready** with a working demo and production-friendly hooks for pause/quit.
- Built-in [Compliance Screen](Assets/Addressables/Prefabs/UI/Compliance%20Screen.prefab) prefab.

## 📦 Prerequisites
- Unity Editor: [6000.0.55f1](https://unity.com/releases/editor/whats-new/6000.0.55#installs)
- API Compatibility Level: .NET Standard 2.1 (or .NET 4.x)
- Git LFS (Large File Storage)

## 🧰 Quick Start
1) Clone
```bash
git clone https://github.com/CoderGamester/Core-Game.git
cd Core-Game
```
2) Open in Unity Hub (Unity 6000.0.55f1 recommended)

3) Generate Addressables settings
- Window > Asset Management > Addressables > Groups > Create/Generate Settings

4) Configure UI (optional)
- Locate and open `UiConfigs.asset` (Project search or Tools > Select UiConfigs.asset if available)

5) Open the "Boot" Scene and Play
- Open the scene named by `Constants.Scenes.BOOT` (boots into Main additively)

6) Run the sample and inspect the flow

---

## 🧭 Project Architecture

The entrypoint `Main` (`Assets/Src/Main.cs`) wires services and starts the game state machine.

Key responsibilities of `Main`:
- Bind services via a lightweight `Installer`
- Initialize Unity Services and internal versioning
- Start the `GameStateMachine`
- Handle lifecycle events (pause, focus, quit) and persist data

Example (excerpt from `Main.cs`):

```csharp
var installer = new Installer();

installer.Bind<IMessageBrokerService>(new MessageBrokerService());
installer.Bind<ITimeService>(new TimeService());
installer.Bind<GameUiService, IGameUiServiceInit, IGameUiService>(new GameUiService(new UiAssetLoader()));
installer.Bind<IPoolService>(new PoolService());
installer.Bind<ITickService>(new TickService());
installer.Bind<ICoroutineService>(new CoroutineService());
installer.Bind<AssetResolverService, IAssetResolverService, IAssetAdderService>(new AssetResolverService());
installer.Bind<ConfigsProvider, IConfigsAdder, IConfigsProvider>(new ConfigsProvider());
installer.Bind<DataService, IDataService, IDataProvider>(new DataService());

var gameServices = new GameServicesLocator(installer);
installer.Bind<IGameServicesLocator>(gameServices);

_stateMachine = new GameStateMachine(installer);
```

Lifecycle hooks (excerpt):
```csharp
private void OnApplicationPause(bool isPaused)
{
    if (isPaused)
    {
        _dataService.SaveAllData();
        _services.AnalyticsService.FlushEvents();
    }
    _services.MessageBrokerService.Publish(new ApplicationPausedMessage { IsPaused = isPaused });
}
```

State and logic boundaries are orchestrated by `GameServicesLocator`, `GameLogicLocator`, and `GameStateMachine` instances.

---

## 🗂 Folder Structure
- `Assets/`
  - `Addressables/` — runtime-loadable assets (configs, scenes, prefabs, UI)
    - `Scenes/` — scenes to be loaded (often additively) at runtime
    - `Prefabs/` — UI and gameplay prefabs (e.g., Compliance Screen)
  - `Libs/` — third-party libraries
  - `Resources/` — avoid for game content; reserved for Unity defaults/plugins
  - `Src/` — gameplay code and composition roots (e.g., `Main.cs`, `BootSplashscreen.cs`)
    - `Cheats/` — developer cheats and test toggles (e.g., `SROptions.Cheats.cs`)
    - `Commands/` — commands to trigger game logic actions (e.g., `AcceptComplianceCommand.cs`, `RestartGameCommand.cs`)
    - `Configs/` — ScriptableObject config definitions (e.g., `DataConfigs.cs`, `GameConfigs.cs`, `SceneAssetConfigs.cs`)
    - `Data/` — persistent data models (e.g., `AppData.cs`, `PlayerData.cs`) saved via `IDataService`
    - `Editor/` — editor-only tools (asset/sheet importers, utilities)
    - `Ids/` — strongly-typed IDs and auto-generated Addressable IDs (`AddressableId.cs`, `GameId.cs`, `SceneId.cs`)
    - `Logic/` — domain game logic and facades (`GameLogicLocator.cs`, plus `Client/` and `Server/` folders)
    - `Messages/` — message types/events published via `IMessageBrokerService` (e.g., `ApplicationStateMessages.cs`)
    - `Presenters/` — UI presenters (MVP) for all UI screens managed by the 'GameUiService' (e.g., `MainHudPresenter.cs`, `MainMenuPresenter.cs`)
    - `Services/` — game-specific services/adapters (`GameServicesLocator.cs`, analytics helpers, UI service, world refs)
    - `StateMachines/` — game flow states and orchestrator (e.g., `GameStateMachine.cs`, `InitialLoadingState.cs`)
    - `Utils/` — constants and small helpers (`Constants.cs`)
    - `ViewControllers/` — base/entity view controllers handled by the Presenters (`ViewControllerBase.cs`, `EntityViewController.cs`)
    - `Views/` — view MonoBehaviours (e.g., `TimerView.cs`)
- `Packages/` — Package Manager manifest and lock
- `ProjectSettings/` — Unity project settings and version
- `WebGL_Build/` — example built WebGL output and template files

Rationale: prioritize Addressables over `Resources/` to reduce initial payloads and enable content updates.

---

## 📚 Packages and Dependencies
Declared in `Packages/manifest.json` (selected):
- `com.gamelovers.services` — service locator and installer utilities
- `com.gamelovers.statechart` — state machine
- `com.gamelovers.uiservice` — UI service scaffolding
- `com.gamelovers.assetsimporter` — Addressables import helpers
- `com.gamelovers.configsprovider` — static config provider
- `com.gamelovers.dataextensions` — extensions and data containers
- `com.gamelovers.googlesheetimporter` — Google Sheets data import
- `com.cysharp.unitask` — async/await for Unity without allocations
- `com.acegikmo.mathfs` — math helpers
- Unity packages: Addressables (2.6.0), Input System, UGUI, Cinemachine, Newtonsoft JSON, Analytics, Cloud Diagnostics, etc.

See also:
- [Services](https://github.com/CoderGamester/com.gamelovers.services)
- [Statechart Machine](https://github.com/CoderGamester/com.gamelovers.statechart)
- [UI Service](https://github.com/CoderGamester/com.gamelovers.uiservice)
- [Assets Importer](https://github.com/CoderGamester/Unity-AssetsImporter)
- [Configs Provider](https://github.com/CoderGamester/Unity-ConfigsProvider)
- [Data Extensions](https://github.com/CoderGamester/com.gamelovers.dataextensions)
- [Google Sheet Importer](https://github.com/CoderGamester/Unity-GoogleSheet-Importer)
- [UniTask](https://github.com/Cysharp/UniTask)
- [Mathfs](https://github.com/FreyaHolmer/Mathfs.git)

---

## 🗃 Addressables Setup and Workflow
1. Open Addressables window
   - Window > Asset Management > Addressables > Groups
2. Create/Generate Settings (first time only)
3. Organize groups (Configs, UI, Scenes, etc.) and assign labels as needed
4. Build Addressables
   - Build > New Build > Default Build Script
5. Use generated IDs from `AddressableId` helpers

Example usage:
```csharp
// Example from docs
AddressableId.Addressables_Configs_DataConfigs.GetConfig().Address;
```

Notes
- Prefer Addressables over `Resources/` for gameplay content
- When changing serialized data, rebuild Addressables (and perform content update if using Remote)

---

## 🎬 Scenes and Game Flow
- `Boot` scene contains `BootSplashScreen` and is the initial scene
- `Main` scene hosts `Main` (composition root) and gameplay entry
- Boot loads Main additively, then merges scenes and destroys bootstrapper

---

## 📦 Build and Deployment

### WebGL
1) Build Settings
- Platform: WebGL
- Add Boot and Main scenes to Build Settings
- Build to: `WebGL_Build/`

2) Addressables
- Build Addressables before building player

3) Hosting
- You can host the output (this repo ships a sample at `WebGL_Build/`)
- Demo: https://codergamester.github.io/Core-Game/WebGL_Build/

Tips
- Enable compression and data caching in Player Settings as needed
- On WebGL, `OnApplicationQuit` is not called; this project publishes quit on pause for WebGL

### Mobile (Android/iOS)
1) Build Settings
- Add Boot and Main scenes
- Android: IL2CPP, ARM64, Internet Access: Required if using remote Addressables
- iOS: IL2CPP, set bundle identifiers, enable required capabilities

2) Addressables
- Choose Local or Remote profiles; rebuild Addressables for release

3) Analytics/Diagnostics
- Configure Unity Services as needed (Analytics, Cloud Diagnostics)

---

## ⚡ Performance & Optimization
- Prefer Addressables over `Resources/`
- Pool frequently spawned objects (`IPoolService`)
- Keep WebGL memory size appropriate for your content
- Use IL2CPP and code stripping on mobile release builds
- Defer heavy initialization with async (`UniTask`) during splash/boot

---

## 🧪 Troubleshooting
- Addressables not loading
  - Ensure settings were created and groups built
- WebGL shows blank page
  - Serve via HTTP(S); ensure compression/decompression settings are consistent
- Missing packages after opening project
  - Open Package Manager to resolve; run `Reimport All` if needed
- iOS tracking compile symbols
  - iOS ATT calls are behind `UNITY_IOS`; ensure related package/capabilities only for iOS builds

---

## 🤝 Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m "Add some AmazingFeature"`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License
[MIT License](https://github.com/CoderGamester/Core-Game/blob/master/LICENSE)

---

## 📞 Contact
- GitHub Issues: https://github.com/CoderGamester/Core-Game/issues
- LinkedIn: https://www.linkedin.com/in/miguel-tomas/
- Email: game.gamester@gmail.com
- Discord: gamester7178

---

## Unity Project Keybind Shortcuts

- ALT+R (Windows) / ⌘+R (Mac) — force compile project code
- ALT+1 (Windows) / ⌥+1 (Mac) — open the "Boot" scene
- ALT+2 (Windows) / ⌥+2 (Mac) — open the "Main" scene

### Demo URL

https://codergamester.github.io/Core-Game/WebGL_Build/
