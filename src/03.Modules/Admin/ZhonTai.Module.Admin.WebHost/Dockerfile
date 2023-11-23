FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["/src/03.Modules/Admin/ZhonTai.Module.Admin.WebHost/ZhonTai.Module.Admin.WebHost.csproj", "/src/03.Modules/Admin/ZhonTai.Module.Admin.WebHost/ZhonTai.Module.Admin.WebHost.csproj"]
RUN dotnet restore "/src/03.Modules/Admin/ZhonTai.Module.Admin.WebHost/ZhonTai.Module.Admin.WebHost.csproj" -nowarn:NETSDK1138
COPY . .
WORKDIR "/src/03.Modules/Admin/ZhonTai.Module.Admin.WebHost"
ENV NUGET_XMLDOC_MODE none
RUN dotnet publish "ZhonTai.Module.Admin.WebHost.csproj" -nowarn:NETSDK1138 -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 8000
ENTRYPOINT ["dotnet", "ZhonTai.Module.Admin.WebHost.dll"]