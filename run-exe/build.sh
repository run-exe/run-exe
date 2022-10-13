cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
#version="${ts}-beta"
version="${ts}"
sed -i -e "s/<Version>.*<\/Version>/<Version>${version}<\/Version>/g" run-exe.csproj
rm -rf obj bin
dotnet build -c Release run-exe.csproj
rm -rf *.nupkg
cp -rp bin/Release/*.nupkg .
