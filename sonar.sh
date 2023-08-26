dotnet sonarscanner begin /k:"TodoAPI" /d:sonar.host.url="http://172.17.0.3:9000"  /d:sonar.token="sqp_bc77773160c3235eafc79ffeab73bd069a1b9f17"  /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml

dotnet build

dotnet-coverage collect 'dotnet test' -f xml  -o 'coverage.xml'
dotnet sonarscanner end /d:sonar.token="sqp_bc77773160c3235eafc79ffeab73bd069a1b9f17"