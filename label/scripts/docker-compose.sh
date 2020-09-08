#!/bin/bash
# This script will start a docker compose service

echo "Provide a Build Version Number to create, followed by [ENTER]:"
read build_number
if [$build_number == null]
then
    echo "The Build Version Number is null or empty!"
    exit 1
fi

echo "Provide a Docker Hub Id, followed by [ENTER]:"
read docker_hub_id
if [$docker_hub_id == null]
then
    echo "The Docker Hub Id is null or empty!"
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
if [$db_host_port == null]
then
    echo "Database host listener port is null or empty!"
    exit 1
fi

cd ../src
docker-compose up --build
