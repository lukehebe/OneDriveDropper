This PoC exploits the Windows DLL search order mechanism to achieve code execution through DLL proxying. The dropper:

1. Locates the legitimate OneDrive.exe binary on the target system
2. Copies it to a temporary directory alongside a malicious version.dll
3. Launches OneDrive.exe, which loads the malicious DLL instead of the legitimate system version.dll
4. The proxy DLL forwards all exports to the legitimate version.dll while executing a payload

Components:
Program.cs: C# dropper that orchestrates the sideloading attack
dllmain.cpp: Malicious proxy DLL that:
Forwards all version.dll exports to the legitimate system DLL
Executes a payload (calc.exe by default) in a separate thread
onedrivedropper.csproj: Build configuration for the dropper

Compile Instructions:
PS C:\RedTeam\OneDriveDropper> dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=truedotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
