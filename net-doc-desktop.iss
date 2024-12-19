[Setup]
AppName=Net Doc Desktop
AppVersion=2.0
ArchitecturesInstallIn64BitMode=x64compatible
DefaultDirName={autopf}\Net Doc Desktop
DefaultGroupName=Net Doc Desktop
AppPublisher=Longridge High School I.T. Support
UninstallDisplayIcon={app}\net-doc-desktop.exe
OutputBaseFilename=net-doc-desktop-setup

[Files]
Source: "bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Excludes: "*.zip"; Flags: recursesubdirs

[Dirs]
Name: "{localappdata}\net-doc-desktop"
Name: "{app}\net-doc-desktop.exe.WebView2"; Permissions: users-modify

[Icons] 
Name: "{userstartup}\Net Doc Desktop"; Filename: "{app}\net-doc-desktop.exe"
Name: "{group}\Net Doc Desktop"; Filename: "{app}\net-doc-desktop.exe"