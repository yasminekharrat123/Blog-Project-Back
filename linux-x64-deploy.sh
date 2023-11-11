cd Blog-Project-Back
pm2 stop blog

git checkout master
git pull

dotnet publish -c Release -r win-x64 --self-contained -o dist
pm2 start blog
