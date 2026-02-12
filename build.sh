#!/bin/bash
set -e

# Build LaserGRBL natively for Linux using Mono

echo "Step 1: Installing dependencies..."
sudo apt-get update
sudo apt-get install -y mono-complete mono-devel nuget libgdiplus libc6-dev ca-certificates-mono

echo "Step 2: Restoring NuGet packages..."
nuget restore LaserGRBL.sln

echo "Step 3: Compiling LaserGRBL via xbuild..."
# Note: We target Release|Any CPU
xbuild LaserGRBL.sln /p:Configuration=Release

echo "Build complete. Artifacts are in LaserGRBL/bin/Release/"
