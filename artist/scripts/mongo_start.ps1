
<#
    .DESCRIPTION
        Powershell script to deploy a MongDB Docker Container.
    
    .NOTES
        AUTHOR: Steve Owens
        LASTEDIT: Dec 02, 2020
        VERSION: 1.0.0.1
#>

# This script will start a new mongodb docker container.
$db_name = Read-Host "Provide Mongo Database Name, followed by [ENTER):"
if($db_name) {
    Write-Host "Database Name is valid!"
}
else {
    Write-Host "Datbase Name is null or empty!"
    exit
}

$db_admin_username = Read-Host "Provide Mongo Admin user name to create, followed by [ENTER):"docker 
if($db_admin_username) {
    Write-Host "Database Admin Username is valid!"
}
else {
    Write-Host "Datbase Admin Username is null or empty!"
    exit
}

$db_admin_password = Read-Host "Provide Mongo Admin password to create, followed by [ENTER):"
if($db_admin_password) {
    Write-Host "Database Admin Password is valid!"
}
else {
    Write-Host "Datbase Admin Password is null or empty!"
    exit
}

$db_host_port = Read-Host "Provide Mongo host listener port, followed by [ENTER):"
if($db_host_port) {
    Write-Host "Mongo host listener port is valid!"
}
else {
    Write-Host "Mongo host listener port is null or empty!"
    exit
}

$db_directory_name= ${db_name} + "_db"
Write-Host "Create a MongoDb Data Directory - ${db_directory_name}"
mkdir $db_directory_name

$container_name="mongo-${db_name}"
$location= (Get-Location).Path
$data="${location}/${db_directory_name}"

write-host "docker run -d -p ${db_host_port}:27017 -v ${data}:/data/db --name ${container_name} -e MONGO_INITDB_ROOT_USERNAME=${db_admin_username} -e MONGO_INITDB_ROOT_PASSWORD=${db_admin_password} mongo:latest"