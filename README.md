# Gateway management

Management for gateways and divices

## Using Docker

Requirements

* docker

Steps:

Using the terminal of your preference open the fordel of the project and run the fallowing command:

`docker build . -t gateway_management:1.0`

When finish you can access the app using <https://localhost:5001/>

Note: If the application don't start, you can try creating a container like this:

`docker run --name gatewayman gateway_management:1.0`

## Local install

Requirements

* dotnet 3.1
* node 12
* Angular 9

Steps

`cd src/GatewayManagement`
`dotnet restore`
`dotnet build`
`dotnet run`

## Automated build

In the .github folder are an GitHub Actions Workflow automated build, but you must config the deployment platform of your preference.
