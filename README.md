# Core Game Template Project

## 🎮 Overview
A Unity game template project to develop your own mobile or web game, designed with scalability, performance, and best practices in mind.

## 🚀 Features
- Follow game development best practices with SOLID principles foundation
- Optimized performance and architecture design
- Contains a [Compliance Screen](https://github.com/CoderGamester/Core-Game/blob/master/Assets/Addressables/Prefabs/UI/Compliance%20Screen.prefab) for mobile and web-based games with terms of service and privacy policy

## 📦 Prerequisites
- [Unity 2022.3.51f1](https://unity.com/releases/editor/whats-new/2022.3.51)
- .NET 4.x or .NET Standard 2.1 compatible
- Git LFS (Large File Storage)

## 🛠️ Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/CoderGamester/Core-Game/blob/master/README.md
   ```
2. Open the project in Unity Hub
3. Open Addressables Groups page and generate Addressables settings. Window > Asset Management > Addressables > Groups > Generate Settings
4. Open the UiConfigs scriptable object. Search in the project or Tools > Select UiConfigs.asset
5. Open the "Boot" Scene to play the game demo
6. Run the project

## 🧩 Project Structure
- `Assets/`: Main project assets
  - `Addressables/`: Static game resources to be loaded at runtime via addressables
  - `Libs/`: Third-party libraries
  - `Resources/`: **AVOID THIS FOLDER**. Only used by deafult Unity assets or some plugins. Use Addressables instead
  - `Src/`: All game relacted code
  - `Scenes/`: Game scenes and that are not dynamic loaded at runtime via addressables

## 📚 Package Dependencies
- [Game Services](https://github.com/CoderGamester/Services) to provide the intricate connection between game logic and different modules of the game
- [Game State Machine](https://github.com/CoderGamester/StatechartMachine) to control the game's flow
- [Ui Service](https://github.com/CoderGamester/UiService) to provide the necessary MVP architecture to control the game's UI flow and object visibility
- [Asset Importer](https://github.com/CoderGamester/AssetsImporter) to import addressable assets from the Unity Hub
- [Configs Provider](https://github.com/CoderGamester/ConfigsProvider) to provide the necessary static configuration for the game. Useful for game settings and to use in par with the [Asset Importer](https://github.com/CoderGamester/AssetsImporter)
- [Google Sheet Importer](https://github.com/CoderGamester/GoogleSheetImporter) to import data from Google Sheets
- [Data Extensions](https://github.com/CoderGamester/DataExtensions) to provide the necessary Unity's data extensions to use in a game. E.g. [Observable Collections](https://github.com/CoderGamester/Unity-DataTypeExtensions/blob/master/Runtime/ObservableList.cs)
- [Notification Service](https://github.com/CoderGamester/NotificationService) to provide the necessary notification system to use in a game running on a notification based device (e.g web browser & mobile)
- [Mobile UI Service](https://github.com/CoderGamester/Unity-Mobile-NativeUI) to provide the default mobile app UI system
- [Unitask](https://github.com/Cysharp/UniTask) to provide the necessary async extensions to use in a game
- [Mathfs](https://github.com/FreyaHolmer/Mathfs) to provide the necessary math extensions to use in a game

## 🔧 Dependency Injection
This project uses manual dependency injection through the [Main Installer](https://github.com/CoderGamester/Services/blob/master/Runtime/MainInstaller.cs), providing:
- Loose coupling between game modules
- Easy testability
- Flexible and modular architecture

## 🌐 Asset Management
Leverages Unity Addressables with [Asset Importer](https://github.com/CoderGamester/AssetsImporter) for:
- Efficient asset loading
- Dynamic resource management
- Reduced initial load times
- Generate Addressables loading paths and groups for easier coding in [AddressableId](https://github.com/CoderGamester/Core-Game/blob/master/Assets/Src/Ids/AddressableId.cs). Can be used in the following way:
```csharp
AddressableId.Addressables_Configs_DataConfigs.GetConfig().Address;
```
- Open Addressables Groups page and generate Addressables settings. Window > Asset Management > Addressables > Groups > Generate Settings

## 🤝 Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License
[MIT License](https://github.com/CoderGamester/Core-Game/blob/master/LICENSE)

## 📞 Contact
Create new issues and pull requests at or contact us via Discord for any questions or suggestions:
- [GitHub](https://github.com/CoderGamester/Core-Game/issues)
- [Discord](https://discord.gg/MaDymKtKWy)

## Unity Project Keybind Shortcuts

- ALT+R to force compile all project code
- ALT+1 to open the "Boot" scene
- ALT+2 to open the "Main" scene

### Demo URL

https://codergamester.github.io/Core-Game/WebGL_Build/
