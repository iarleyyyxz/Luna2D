<img width="1920" height="700" alt="Luna-Banner2" src="https://github.com/user-attachments/assets/fd1dd760-06e2-4052-b009-28705b8acca6" />

# 🚀 Luna2D - Game Engine

[![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://dotnet.microsoft.com/languages/csharp)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![SDL2](https://img.shields.io/badge/UI%20&%20Input-SDL2-123456?style=for-the-badge&logo=sdl&logoColor=white)](https://www.libsdl.org/)
[![OpenTK](https://img.shields.io/badge/Rendering-OpenTK-000000?style=for-the-badge&logo=opengl&logoColor=white)](https://opentk.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

## 🌟 About Luna2D

Luna2D is a **2D game engine** currently under development, designed to be simple, performant, and flexible. The goal is to provide a robust toolset for creating 2D games, with a special focus on integrating an intuitive User Interface (UI) and all the functionalities expected of a modern engine.

### Project Status

> ⚠️ **Actively Under Construction** - This repository reflects continuous, early-stage development. New features and refactoring are being implemented constantly. The engine is not yet ready for production use.

## ✨ Key Features (Under Development)

* **Optimized 2D Rendering:** Utilizes **OpenTK** (which is an OpenGL wrapper) for low-level, high-performance graphics rendering.
* **Window and Input Handling:** Uses **SDL (Simple DirectMedia Layer)** to manage windows, input events (keyboard, mouse, gamepad), and basic audio.
* **Integrated UI System:** A custom User Interface (UI) system, designed to facilitate the creation of menus, HUDs, and editing tools within the engine itself.
* **Component-Based Architecture (ECS-Like):** A flexible structure for composing game entities.
* **Basic Editor (Next Steps):** The plan is to develop a basic integrated editor for scene visualization and object manipulation.
* **Asset Pipeline:** Management of textures, spritesheets, fonts, and other resources.

## 💻 Technologies Used

| Technology | Primary Role |
| :--- | :--- |
| **C#** | Primary programming language for the project. |
| **.NET 9.0** | High-performance framework for development. |
| **SDL2** | Window, input, and audio handling. |
| **OpenTK** | OpenGL bindings for graphics rendering. |

## 🛠️ How to Clone and Run

### Prerequisites

1.  **.NET 9.0 SDK** (or newer):
    * Check if the correct SDK is installed: `dotnet --version`
2.  **Visual Studio** (or another C# and .NET compatible IDE).
3.  **Native Dependencies (SDL2):** Since SDL is a native dependency, check the project's libs folder or specific setup instructions to ensure the native DLLs/files are accessible to the C# project (usually via NuGet or manually).

### Steps

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/iarleyyyxz/Luna2D.git](https://github.com/iarleyyyxz/Luna2D.git)
    cd Luna2D
    ```
2.  **Restore Dependencies:**
    ```bash
    dotnet restore
    ```
3.  **Build the Project:**
    ```bash
    dotnet build
    ```
4.  **Run the Engine/Example:**
    ```bash
    dotnet run --project [MainEngineProjectName]
    ```
    * *(Replace `[MainEngineProjectName]` with the actual name of the startup project in your solution)*.

## 🤝 Contributions

Contributions, suggestions, and bug reports are highly welcome!

1.  **Fork** the project.
2.  Create a **Branch** for your feature (`git checkout -b feature/MyNewFeature`).
3.  **Commit** your changes (`git commit -m 'feat: Add feature X'`).
4.  **Push** to the branch (`git push origin feature/MyNewFeature`).
5.  Open a **Pull Request**.

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for more details.

## 📞 Contact

* **iarleyyyxz** - [https://github.com/iarleyyyxz](https://github.com/iarleyyyxz)

---

## 🗺️ Road Map (Next Steps)

* [ ] Implementation of the Engine life cycle (Initialization, Update, Draw).
* [ ] Refactoring the input system based on SDL events.
* [ ] Initial development of the Scene Editor (Main editor window).
* [ ] Support for SpriteSheets and simple animations.
* [ ] Basic documentation of core classes.
