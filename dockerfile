FROM node:12.18-alpine AS client-app
ARG skip_client_build=false
WORKDIR /app
COPY src/ClientApp .
RUN [[ ${skip_client_build} = true ]] && echo "Skipping npm install" || npm install 
RUN [[ ${skip_client_build} = true ]] && mkdir dist || npm run-script build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1
LABEL authors="Elvis Jr Crego" name="gateway_management" tag="1.0"
WORKDIR /app/server
COPY --from=client-app /app/dist ../ClientApp/dist
COPY src/GatewayManagement .
RUN dotnet restore
EXPOSE 5001
CMD ["dotnet", "run"]
