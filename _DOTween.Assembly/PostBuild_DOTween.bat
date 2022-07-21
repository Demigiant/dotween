:: %1 = $(SolutionDir) ► Solution dir path - with final slash
:: %2 = $(TargetDir) ► Bin folder dir path - with final slash
:: %3 = $(TargetFileName) ► DLL filename with extension
:: %4 = $(TargetName) ► DLL filename without extension
:: %5 = Main export dir ► Main folder inside bin.Global/etc where to copy the files (ex: "DOTween", "DOTweenPro")
:: %6 = Bin dir name ► (ex: "bin", "bin_pro")
:: %7 = $(UnityVersionInstallPath) ► Unity editor path (ex: "C:\Program Files\Unity\Hub\Editor\2020.3.36f1")
:: %8 = Eventual export subdir (can be NULL) ► Eventual subdirectory inside the main export dir (ex: "Editor")

echo :
echo :
echo :
echo :
echo :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo :::::::::::::::::: EXECUTING BATCH FILE :::::::::::::::::::::::
echo :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo :
echo :
echo :
echo :
echo :::::: TARGET: %5 %8

echo :::::: Deleting TMPs...
IF EXIST %2*.tmp DEL %2*.tmp

echo :::::: Converting PDB to MDB and deleting PDB...
CD %2
"%~7\Editor\Data\MonoBleedingEdge\bin\mono.exe" "%~7\Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe" %3
echo ::: Deleting %4.pdb file: %4.pdb
DEL %4.pdb
echo ::: %4.pdb file deleted, PAUSE for 0.5 second
waitfor pdbFilesToBeDeletedIHope /t 0.5 2>NUL || type nul>nul

echo ::: Deleting %4.deps.json files: %4.deps.json
DEL %4.deps.json

echo :::::: Starting export...

set SubDir=%8
if not "%SubDir%"=="" ( set SubDir=\%8 )
set CopyFromDir=%1%6%SubDir%

set CopyToDir=%1..\..\bin.Global\%5%SubDir%
set CopyToDirNoMeta=%1..\..\bin.Global_no_meta\%5%SubDir%
set CopyToDirUnityTests=%1..\UnityTests.Unity4\Assets\Demigiant\%5%SubDir%
set CopyToDirUnity5Tests=%1..\UnityTests.Unity5\Assets\Plugins\Demigiant\%5%SubDir%
set CopyToDirUnity2019Tests=%1..\UnityTests.Unity2019\Assets\Demigiant\%5%SubDir%
set CopyToDirCompatibilityTests=%1..\UnityCompatibilityTests.Unity35\Assets\Demigiant\%5%SubDir%
set CopyToDirExtPluginsTests=%1..\ExternalPluginsTestsAndExamples.Unity5\Assets\Demigiant\%5%SubDir%
set CopyToDirModulesTest=%1..\ModulesTest.Unity2018\Assets\Demigiant\%5%SubDir%

:: Modules test project
echo ::: Exporting from %CopyFromDir% to %CopyToDirModulesTest%
echo f | xcopy %CopyFromDir% %CopyToDirModulesTest% /Y /I /E
:: bin.Global - Ignore, not good anymore
:: echo ::: Exporting from %CopyFromDir% to %CopyToDir%
:: echo f | xcopy %CopyFromDir% %CopyToDir% /Y /I /E
:: bin.Global_no_meta
echo ::: Exporting from %CopyFromDir% to %CopyToDirNoMeta%
echo f | xcopy %CopyFromDir% %CopyToDirNoMeta% /Y /I /E
:: UnityTests.Unity5
echo ::: Exporting from %CopyFromDir% to %CopyToDirUnity5Tests%
echo f | xcopy %CopyFromDir% %CopyToDirUnity5Tests% /Y /I /E
:: UnityTests.Unity2019
echo ::: Exporting from %CopyFromDir% to %CopyToDirUnity2019Tests%
echo f | xcopy %CopyFromDir% %CopyToDirUnity2019Tests% /Y /I /E
exit /B
:: UnityTests.Unity4
echo ::: Exporting from %CopyFromDir% to %CopyToDirUnityTests%
echo f | xcopy %CopyFromDir% %CopyToDirUnityTests% /Y /I /E
:: UnityCompatibilityTests.Unity35
echo ::: Exporting from %CopyFromDir% to %CopyToDirCompatibilityTests%
echo f | xcopy %CopyFromDir% %CopyToDirCompatibilityTests% /Y /I /E
:: ExternalPluginsTestsAndExamples.Unity5
echo ::: Exporting from %CopyFromDir% to %CopyToDirExtPluginsTests%
echo f | xcopy %CopyFromDir% %CopyToDirExtPluginsTests% /Y /I /E

echo :
echo :
echo :
echo :
echo :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo ::::::::::::::::::::: BATCH FILE END ::::::::::::::::::::::::::
echo :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo :
echo :
echo :
echo :