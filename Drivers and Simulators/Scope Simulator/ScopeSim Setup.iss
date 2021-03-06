;;    Remarks for the template are prefixed like this

;;    2007-sep-08  cdr  Add registry code for DCOM
;;                      Add RegASCOM if COM dll or exe

;;    2007-Sep-09  rbd  Comment for CLSID, make more obvious :-)

; Script generated by the ASCOM Driver Installer Script Generator 1.0.1.0
; Generated by me on 9/9/2007 (UTC)
;
[Setup]
AppName=ASCOM ScopeSim Telescope Driver
AppVerName=ASCOM ScopeSim Telescope Driver 1.0
AppVersion=1.0
AppPublisher=me you
AppPublisherURL=http://ascom-standards.org/
AppSupportURL=http://ascom-standards.org/
AppUpdatesURL=http://ascom-standards.org/
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Telescope"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="ScopeSim Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Documents and Settings\Robert B. Denny\My Documents\Visual Studio 2005\Projects\ASCOM\Driver Inst\InstallerGen\bin\Debug\Resources\WizardImage.bmp"
LicenseFile="C:\Documents and Settings\Robert B. Denny\My Documents\Visual Studio 2005\Projects\ASCOM\Driver Inst\InstallerGen\bin\Debug\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Telescope folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Telescope\ScopeSim"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Telescope\ScopeSim"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Files]
Source: "C:\Program Files\Common Files\ASCOM\Telescope\ScopeSim.exe"; DestDir: "{app}"; AfterInstall: RegASCOM();;AfterInstall: RegASCOM()
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "C:\Program Files\Common Files\ASCOM\Telescope\"; DestDir: "{app}"; Flags: isreadme
; TODO: Add other files needed by your driver here (add subfolders above)

;Only if COM Local Server
[Run]
Filename: "{app}\ScopeSim.exe"; Parameters: "/regserver"

;Only if COM Local Server
[UninstallRun]
Filename: "{app}\ScopeSim.exe"; Parameters: "/unregserver"

;  DCOM setup for COM local Server, needed for TheSky
[Registry]
; TODO: set this value to the ClsID of your aplication
#define AppClsid "{{5549eb07-3808-4172-8358-210a1ac9f847}"

; set the DCOM access control for TheSky
Root: HKCR; Subkey: CLSID\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueData: "ASCOM ScopeSim Telescope Driver"
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: string; ValueName: AppID; ValueData: {#AppClsid}
Root: HKCR; Subkey: AppId\{#AppClsid}; ValueType: dword; ValueName: AuthenticationLevel; ValueData: 1

; code to register and unregister the driver with the chooser
; modified by CR to use with all COM servers (dll and exe) 
[Code]
procedure RegASCOM();
var
   Helper: Variant;
begin
   Helper := CreateOleObject('DriverHelper.Profile');
   Helper.DeviceType := 'Telescope';
   Helper.Register('ScopeSim.Telescope', 'ScopeSim');
end;

// CR this is never called
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
   Helper: Variant;
begin
   if CurUninstallStep = usUninstall then
   begin
     Helper := CreateOleObject('DriverHelper.Profile');
     Helper.DeviceType := 'Telescope';
     Helper.Unregister('ScopeSim.Telescope');
  end;
end;
