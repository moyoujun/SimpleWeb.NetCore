docker run -d -p 3380:3306 mysql -e MYSQL_ROOT_PASSWORD=mysql1234 \
--command mysqld --default-authentication-plugin=mysql_native_password \
--volumes  ./mysql:/var/lib/mysql