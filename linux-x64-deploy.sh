cd /root/Blog-Project-Back
pm2 stop blog

git checkout master
git pull

dotnet publish -c Release -r linux-x64 -o ./publish
pm2 start blog
