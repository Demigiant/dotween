:: %1 = $(SolutionDir) ► Solution dir path - with final slash
:: %2 = Bin dir name ► (ex: "bin", "bin_pro")

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

echo :::::: Starting export...

set CopyFromDir=%1%2
set CopyToDir=%1..

echo f | xcopy /s /f /y %CopyFromDir%\DOTween.dll %CopyToDir%\Package\DOTween\DOTween.dll
echo f | xcopy /s /f /y %CopyFromDir%\DOTween.dll.mdb %CopyToDir%\Package\DOTween\DOTween.dll.mdb
echo f | xcopy /s /f /y %CopyFromDir%\DOTween.xml %CopyToDir%\Package\DOTween\DOTween.xml
echo f | xcopy /s /y %CopyFromDir%\Modules %CopyToDir%\Package\DOTween\Modules

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