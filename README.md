# LaserGRBL Native for Linux (Ubuntu 20.04+)

This repository provides a **native Linux build** of [LaserGRBL](https://github.com/arkypita/LaserGRBL), compiled using the Mono runtime. No Wine is required.

## Features
- **Native Execution**: Runs directly via Mono on Linux.
- **Improved Performance**: Better integration with the Linux serial port stack compared to Wine.
- **Easy Installation**: Includes an automated installer script for Ubuntu-based distributions.
- **Desktop Integration**: Appears in your application menu with a launcher and icon.

## Prerequisites
- **Ubuntu 20.04 LTS** or newer (tested on Pop!_OS 22.04).
- **Mono Runtime**: The installation script will automatically install this if missing.
- **User Permissions**: You must be in the `dialout` group to access serial ports (handled by `install.sh`).

## Quick Install

1. Clone this repository:
   ```bash
   git clone https://github.com/USER_OR_ORG/lasergrbl-linux.git
   cd lasergrbl-linux
   ```

2. Run the installer:
   ```bash
   chmod +x install.sh
   ./install.sh
   ```

3. **Log out and log back in** for group permissions (serial port access) to take effect.

## Usage
Launch LaserGRBL from your application menu or by typing `lasergrbl` in your terminal.

## Build From Source
If you wish to recompile from source:
```bash
chmod +x build.sh
./build.sh
```

## Patches & Modifications
The following modifications were made to the original LaserGRBL source to enable native Linux compilation:
- **Case-Sensitivity Fixes**: Adjusted `.csproj` and file references for Linux filesystems.
- **C# Language Version**: Forced `LangVersion=Latest` for pattern matching support in Mono's compiler.
- **Ambiguity Fixes**: Resolved ambiguous method calls in `MainForm.cs`.
- **Windows-Only Code Removal**: Commented out Windows-specific ETW (`System.Diagnostics.Eventing`) references in `PreviewForm.cs`.

## Known Issues
- **UI Layout**: Mono's `TableLayoutPanel` rendering differs slightly from Windows, leading to minor UI alignment quirks.
- **SVG Import**: Some complex SVGs might not render perfectly in Mono.

## License
Original LaserGRBL code is GPLv3. See [LICENSE](LICENSE) for details.
