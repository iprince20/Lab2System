# Lab2System

A C# Windows Forms desktop application for the .NET Assignment 2 (Lab 1 problems in a GUI).
The user logs in, chooses a service, and uses either the **Bicycle Rental** calculator or the **Mushroom Identification** quiz.

## Application flow

```
Login  →  Choose Service  →  Bicycle Rental
                          →  Mushroom Identification
            (Logout on any screen returns to Login)
```

## Features

### 1. Login
- Fields: Name/Email and Password (masked).
- Demo credentials: **`admin` / `1234`**.
- Shows validation messages for empty or wrong credentials.

### 2. Choose Service
- Two options: **Bicycle Rental** and **Mushroom Identification**.
- Logout button returns to the login page.

### 3. Bicycle Rental (Lab 1 – Problem 1)
Enter a start hour (0–23) and an end hour (1–24); the start must be less than the end, decimals are rejected.
Each hour is charged at the rate of the time range it falls in:

| Time range | Rate per hour |
|---|---|
| 0–7 and 21–24 | 500 RWF |
| 7–14 and 19–21 | 1000 RWF |
| 14–19 | 1500 RWF |

Example: 6h → 8h = 500 + 1000 = **1500 RWF**.

### 4. Mushroom Identification (Lab 1 – Problem 2)
The user thinks of one of six mushrooms and answers **Yes/No**. The program identifies it in **at most three questions**,
always starting with *"Does your mushroom grow in a forest?"*

```
Grows in a forest?
├─ Yes → Convex cup?
│        ├─ Yes → Has a ring?  Yes: Amanite tue-mouche   No: Pied bleu
│        └─ No  → Has gills?   Yes: Girolle              No: Cèpe de Bordeaux
└─ No  → Convex cup?           Yes: Agaric jaunissant    No: Coprin chevelu
```

## Project structure

| File | Purpose |
|---|---|
| `Program.cs` | Entry point – opens the login form |
| `Form1` | Login page |
| `ChooseForm` | Service selection |
| `BicycleForm` | Bicycle rental calculator |
| `MushroomForm` | Mushroom identification quiz |
| `Navigation.cs` | Shared logout logic (back to login) |
| `Theme.cs` | Shared modern styling: gradient background, card, rounded buttons and inputs |

Each form has a `*.Designer.cs` file (layout of controls) and a `*.cs` file (event handlers / logic).

## How to run

**Requirements:** Windows, .NET Framework, and Visual Studio (or its MSBuild).

- **Visual Studio:** open `Lab2System.slnx` and press **F5**.
- **VS Code:** press **Ctrl + Shift + B** (build and run task is included in `.vscode`).
- **Terminal:**
  ```powershell
  & "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" Lab2System\Lab2System.csproj -v:m
  .\Lab2System\bin\Debug\Lab2System.exe
  ```

## Author
Prince Gasamagera
