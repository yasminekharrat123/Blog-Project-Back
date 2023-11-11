pm2 stop blog
dotnet publish -c Release -r linux-x64 -o ./publish
pm2 start blog
