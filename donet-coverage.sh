#/bin/bash

#dotnet tool install --global dotnet-coverage
#dotnet tool install -g dotnet-reportgenerator-globaltool
#dotnet test /p:CollectCoverage=true

dotnet-coverage collect 'dotnet test' -f xml  -o 'coverage.xml'

reportgenerator -reports:"./coverage.xml" -targetdir:"coveragereport" -reporttypes:Html