set -uvx
set -e
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
#version="${ts}-beta"
version="${ts}"
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" run.csproj
rm -rf obj bin 64bit
dotnet build -c Release run.csproj
rm -rf $HOME/cmd/run.exe
cp -rp bin/Release/net472/*.exe $HOME/cmd/

cp -rp run.csproj runw.csproj
sed -i -e "s/<OutputType>.*<\/OutputType>/<OutputType>WinExe<\/OutputType>/g" runw.csproj
rm -rf obj bin 64bit
dotnet build -c Release runw.csproj
rm -rf $HOME/cmd/runw.exe
cp -rp bin/Release/net472/*.exe $HOME/cmd/
