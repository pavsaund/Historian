FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

VOLUME [ "/app/data" ]

COPY Build/MSBuild/* ./Build/MSBuild/
COPY modules/TimeSeriesHistorian/*.csproj ./modules/TimeSeriesHistorian/

WORKDIR /app/modules/TimeSeriesHistorian

RUN dotnet restore

COPY modules/TimeSeriesHistorian/ ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime-stretch-slim
WORKDIR /app
COPY --from=build-env /app/modules/TimeSeriesHistorian/out ./
COPY --from=build-env /app/modules/TimeSeriesHistorian/data ./data

#RUN useradd -ms /bin/bash moduleuser
#USER moduleuser

ENTRYPOINT ["dotnet", "Dolittle.Edge.TimeSeriesHistorian.dll"]