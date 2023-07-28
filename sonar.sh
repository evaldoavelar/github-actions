dotnet sonarscanner begin /k:"TodoAPI" /d:sonar.host.url="http://172.17.0.2:9000"  /d:sonar.token="sqp_0fc82c389757c4921a631ff9037ac7576421502e"
dotnet build
dotnet test
dotnet sonarscanner end /d:sonar.token="sqp_0fc82c389757c4921a631ff9037ac7576421502e"