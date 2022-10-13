set -uvx
set -e
#gh auth login --hostname github.com --git-protocol https --web
#gh release upload 64bit "spider-next-boot.json" --repo spider-explorer/spider-next --clobber
rm -rf obj bin 64bit
dotnet publish -c Release -r win-x64 -f net6.0-windows --self-contained false
rm -rf $HOME/cmd/run-exe
cp -rp bin/Release/net6.0-windows/win-x64/publish $HOME/cmd/run-exe
