FROM mcr.microsoft.com/dotnet/sdk:7.0

# Required inside Docker, otherwise file-change events may not trigger
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENV DOTNET_RUNNING_IN_CONTAINER true

# Kopierer csproj filen source katalogen og kjør dotnet restore
# Da får du et layer, dvs cachet alle dependencies og slipper på
# Kjøre disse hver gang du starter containeren
WORKDIR /source/Worker
COPY ./source/Worker/worker.csproj .
RUN dotnet restore

# Lag et utviklersertifikat
RUN dotnet dev-certs https
