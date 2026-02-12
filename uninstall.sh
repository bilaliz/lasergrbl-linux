#!/bin/bash
set -e

echo "Uninstalling LaserGRBL Native (Mono)..."

sudo rm -f /usr/local/bin/lasergrbl
sudo rm -f /usr/share/applications/lasergrbl.desktop
sudo rm -rf /opt/lasergrbl

echo "Uninstallation complete."
