FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN apt-get update
RUN apt-get install mono-complete -y
RUN wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

COPY . .
COPY ./NuGet.Config /root/.nuget/NuGet/NuGet.Config
RUN mono nuget.exe source update -ConfigFile /root/.nuget/NuGet/NuGet.Config -Name ASOSPackages -username "ASSEENONSCREEN\lewis.prescott" -StorePasswordInClearText -password "Nopu5912@"
RUN mono nuget.exe source update -ConfigFile /root/.nuget/NuGet/NuGet.Config -Name ASOSProget -username "ASSEENONSCREEN\lewis.prescott" -StorePasswordInClearText -password "Nopu5912@"
RUN cat /root/.nuget/NuGet/NuGet.Config
RUN dotnet restore ./Asos.Customer.Update.Tool.Api.PactTests.csproj --configfile /root/.nuget/NuGet/NuGet.Config
RUN dotnet test ./Asos.Customer.Update.Tool.Api.PactTests.csproj