; example2.nsi
;
; This script is based on example1.nsi, but it remember the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "fs2_open#Launcher"

; The file to write
OutFile "fs2open-sharp-launcher-0.2-1.exe"

; The default installation directory
InstallDir $PROGRAMFILES\fs2open-sharp-launcher

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "fs2_open# Launcher (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "fs2open-sharp-launcher.exe"
  File "fs2open-sharp-launcher.ico"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fs2open-sharp-launcher" "DisplayName" "fs2_open#Launcher"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fs2open-sharp-launcher" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fs2open-sharp-launcher" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fs2open-sharp-launcher" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\fs2_open#Launcher"
  CreateShortCut "$SMPROGRAMS\fs2_open#Launcher\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\fs2_open#Launcher\fs2_open#Launcher.lnk" "$INSTDIR\fs2open-sharp-launcher.exe" "" "$INSTDIR\fs2open-sharp-launcher.ico" 0
  
SectionEnd

Section "GTK# for .NET"

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "gtk-sharp-2.12.9-2.win32.msi"
  ExecWait '"msiexec" /i "$INSTDIR\gtk-sharp-2.12.9-2.win32.msi"  /passive'  
SectionEnd


;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fs2open-sharp-launcher"
  DeleteRegKey HKLM SOFTWARE\fs2open-sharp-launcher

  ; Remove files and uninstaller
  Delete $INSTDIR\fs2open-sharp-launcher.exe
  Delete $INSTDIR\fs2open-sharp-launcher.ico
  Delete $INSTDIR\uninstall.exe
  Delete $INSTDIR\gtk-sharp-2.12.9-2.win32.msi
  
  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\fs2_open#Launcher\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\fs2_open#Launcher"
  RMDir "$INSTDIR"

SectionEnd
