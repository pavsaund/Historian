FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

VOLUME [ "/app/data" ]

COPY Build/MSBuild/* ./Build/MSBuild/
COPY modules/Historian/*.csproj ./modules/Historian/

WORKDIR /app/modules/Historian

RUN dotnet restore

COPY modules/Historian/ ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime-stretch-slim
WORKDIR /app
COPY --from=build-env /app/modules/Historian/out ./
COPY --from=build-env /app/modules/Historian/data ./data

#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser

ENTRYPOINT ["dotnet", "Dolittle.TimeSeries.Historian.dll"]