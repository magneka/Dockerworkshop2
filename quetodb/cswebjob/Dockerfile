FROM mcr.microsoft.com/dotnet/sdk:7.0

# Required inside Docker, otherwise file-change events may not trigger
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENV DOTNET_RUNNING_IN_CONTAINER true

# Set a working dir at least 2 deep. The output and intermediate output folders will be /code/obj and /code/bin
WORKDIR /source/Worker

# By copying these into the image when building it, we don't have to re-run restore everytime we launch a new container
COPY ./source/Worker/worker.csproj .
RUN dotnet restore

#COPY ./minimalapi/Directory.Build.props .
#COPY . .
RUN dotnet dev-certs https
#RUN dotnet build

#EXPOSE 3000 3001

# This will build and launch the server in a loop, restarting whenever a *.cs file changes
#ENTRYPOINT dotnet watch run 