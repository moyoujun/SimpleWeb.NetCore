# Dockerize And Deploy ASP.NET Core 3.1 API + Mysql + Nginx + vue With Docker-Compose

This is an example web application to show how to use ASP.Net Core with MySQL database and use Nginx as reverse proxy, and deploy using Docker-Compose.
.NetCore will provide the backend api, and will use the jwt auth to protect the api resources.
The vue 3.0 Application, we will use the composite api to build our vue application.


## How to build the back-end and front-end app.
- Prerequisite:
    - docker or docker desktop
    - visual studio
    - vscode 
    - npm

- Build the VUE App:
     
        'cd ./client'  
        'npm install'  
        'npm run build'

- Run docker compose container:
    - cd the root folder of this solution. Run
        
            'docker-compose up -d'

Now enjoy this project!

The back-end web app may exit at the first start. 
-  use vscode to attach web.db container 
-  bash the code:  

        'mysql -uroot -p mysql123456'
        'use mysql;'
        'select Host, User from user;'

-  you will see the table content like this:  

        | Host | User |
        | %    | root |
-  if the record is not like this, try modify this record.

-  quit the attached bash;
-  use docker-desktop or vscode extensions to start the app container again