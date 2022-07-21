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

echo f | xcopy /s /f /y %CopyFromDir%\DOTweenEditor.dll %CopyToDir%\Package\DOTween\Editor\DOTweenEditor.dll
echo f | xcopy /s /f /y %CopyFromDir%\DOTweenEditor.dll.mdb %CopyToDir%\Package\DOTween\Editor\DOTweenEditor.dll.mdb
echo f | xcopy /s /f /y %CopyFromDir%\DOTweenEditor.xml %CopyToDir%\Package\DOTween\Editor\DOTweenEditor.xml
echo f | xcopy /s /y %CopyFromDir%\Imgs %CopyToDir%\Package\DOTween\Editor\Imgs

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