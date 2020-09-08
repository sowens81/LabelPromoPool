#!/bin/bash
# This script will start a new mongodb docker container.
echo "Provide Mongo Database Name, followed by [ENTER]:"
read db_name 

if [$db_name == null]
then
    echo "Datbase Name is null or empty!"
    exit 1
fi

echo "Provide Mongo Admin user name to create, followed by [ENTER]:"
read db_admin_username
if [$db_admin_username == null]
then
    echo "Datbase Admin Username is null or empty!"
    exit 1
fi

echo "Provide Mongo Admin password to create, followed by [ENTER]:"
read db_admin_password
if [$db_admin_password == null]
then
    echo "Datbase Admin User Password is null or empty!"
    exit 1
fi

echo "Provide Mongo host listener port, followed by [ENTER]:"
read db_host_port
if [ $db_host_port == null]
then
    echo "Database host listener port is null or empty!"
    exit 1
fi


db_directory_name="${db_name}_db"
echo "Create a MongoDb Data Directory - ${db_directory_name}"
mkdir $db_directory_name

container_name="mongo-${db_name}"
location=$(pwd)
data="${location}/${db_directory_name}"

docker run -d -p ${db_host_port}:27017 -v ${data}:/data/db --name ${container_name} -e MONGO_INITDB_ROOT_USERNAME=${db_admin_username} -e MONGO_INITDB_ROOT_PASSWORD=${db_admin_password} mongo:latest