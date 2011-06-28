; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ASCOM Client Toolbox Update"
#define MyAppVerName "ASCOM Client Toolbox Update 1.0.5"
#define MyAppPublisher "The ASCOM Initiative"
#define MyAppURL "http://ascom-standards.org/"
#define MyAppVersion "1.0.5"

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={cf}\ASCOM\.net
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=ClientToolkitUpdate(1.0.5)
Compression=lzma
SolidCompression=yes
Uninstallable=no
DirExistsWarning=no
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"


[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\bin\Release\ASCOM.DriverAccess.dll"; DestDir: "{app}";
Source: "..\bin\Release\ASCOM.DriverAccess.XML"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\PublisherPolicy\bin\Release\policy.1.0.ASCOM.DriverAccess.dll"; DestDir: "{app}";
Source: "..\GACInstaller\bin\release\GACInstaller.exe"; DestDir: "{app}"; Flags: deleteafterinstall

[Run]
Filename: "{app}\GACInstaller.exe"; Parameters: """{app}\ASCOM.DriverAccess.dll"""; Flags: runhidden
Filename: "{app}\GACInstaller.exe"; Parameters: """{app}\policy.1.0.ASCOM.DriverAccess.dll"""; Flags: runhidden
