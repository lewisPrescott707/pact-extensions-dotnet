FROM microsoft/dotnet:sdk AS build-env
ARG nugetPassword
ARG nugetUsername="ASSEENONSCREEN\lewis.prescott"
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN apt-get update && apt-get install mono-complete -y
RUN wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

COPY ./Asos.Customer.Update.Tool.Api.PactTests.csproj .
COPY ./NuGet.Config /root/.nuget/NuGet/NuGet.Config
RUN mono nuget.exe source update -ConfigFile /root/.nuget/NuGet/NuGet.Config -Name ASOSPackages -username $nugetUsername -StorePasswordInClearText -password $nugetPassword
RUN mono nuget.exe source update -ConfigFile /root/.nuget/NuGet/NuGet.Config -Name ASOSProget -username $nugetUsername -StorePasswordInClearText -password $nugetPassword
RUN dotnet restore ./Asos.Customer.Update.Tool.Api.PactTests.csproj --configfile /root/.nuget/NuGet/NuGet.Config

COPY . .
RUN dotnet build ./Asos.Customer.Update.Tool.Api.PactTests.csproj
ENTRYPOINT [ "dotnet", "test" ]