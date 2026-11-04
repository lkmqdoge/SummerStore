FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /source

COPY --link *.slnx ./*/*.csproj *.props .
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done;
RUN dotnet restore -a $TARGETARCH

COPY --link . .
RUN dotnet publish -a $TARGETARCH -c release -o /app --no-restore

# runtime-stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN mkdir -p /data/attachments && chown -R app:app /data/attachments
EXPOSE 8080
WORKDIR /app
COPY --link --from=build /app .
USER app
ENTRYPOINT ["dotnet", "SummerStore.Api.dll"]
