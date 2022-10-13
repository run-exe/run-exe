set -uvx
set -e
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
#version="${ts}-beta"
version="${ts}"
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" run-exe.csproj
rm -rf obj bin 64bit
dotnet publish -c Release -r win-x64 -f net6.0-windows --self-contained false
#dotnet build -c Release
rm -rf $HOME/cmd/run-exe
cp -rp bin/Release/net6.0-windows/win-x64/publish $HOME/cmd/run-exe
#cp -rp bin/Release/net472 $HOME/cmd/run-exe
