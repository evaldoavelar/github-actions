FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY TodoAPI/. ./TodoAPI
WORKDIR /source/TodoAPI
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /source/TodoAPI/publish ./
RUN dir -s
ENTRYPOINT [ "./TodoAPI" ]