#!/bin/bash
set -e

echo "Installing LaserGRBL Native (Mono)..."

# Ensure runtime dependencies
sudo apt-get update
sudo apt-get install -y mono-complete libgdiplus

# Create directories
sudo mkdir -p /opt/lasergrbl

# Copy files
sudo cp -rv LaserGRBL/bin/Release/* /opt/lasergrbl/

# Copy and convert icon if possible, otherwise just copy ico
if command -v convert >/dev/null 2>&1; then
    sudo convert LaserGRBL/Grafica/LaserGrbl.ico[0] /opt/lasergrbl/lasergrbl.png
else
    sudo cp LaserGRBL/Grafica/LaserGrbl.ico /opt/lasergrbl/lasergrbl.png
fi

# Install launcher
sudo cp scripts/lasergrbl /usr/local/bin/lasergrbl
sudo chmod +x /usr/local/bin/lasergrbl

# Install desktop file
sudo cp scripts/lasergrbl.desktop /usr/share/applications/

# Permissions for serial port
sudo usermod -aG dialout $USER

echo "--------------------------------------------------------"
echo "Installation complete!"
echo "Please LOG OUT AND LOG IN (or restart) to apply group changes."
echo "You can launch LaserGRBL from your application menu or command line."
echo "Note: If the application menu entry doesn't show up immediately,"
echo "try running 'update-desktop-database ~/.local/share/applications/'. "
echo "--------------------------------------------------------"
