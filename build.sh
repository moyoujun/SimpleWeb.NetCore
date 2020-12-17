#!/bin/sh
cd ./client
npm install
npm run build

cd ../SimpleWeb.NetCore
dotnet publish -c Release -o out

cd ..
docker-compose up -d
