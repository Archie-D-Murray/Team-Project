git pull
if (Test-Path D:\Unity\Hub\Editor\2022.3.1f1\Editor\Unity.exe --PathType Leaf) {
    echo "Starting Unity..."
    D:\Unity\Hub\Editor\2022.3.1f1\Editor\Unity.exe
    echo "Starting Visual Studio..."
    start devenv Team-Project.sln
} else if (Test-Path 'C:\Program Files\Unity\Hub\Editor\2022.3.1f1\Editor\Unity.exe' --PathType Leaf) {
    echo "Starting Unity..."
    'C:\Program Files\Unity\Hub\Editor\2022.3.1f1\Editor\Unity.exe' --projectPath .
    echo "Starting Visual Studio..."
    start devenv Team-Project.sln
} else {
    echo "Could not find Unity installation message me on Teams to add a check for your install path..."
    echo "Starting Visual Studio..."
    start devenv Team-Project.sln
}

