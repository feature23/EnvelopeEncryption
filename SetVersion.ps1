param([Parameter(Mandatory)] $version);

# Requires the dotnet-setversion tool installed:
#   dotnet tool install -g dotnet-setversion
Get-ChildItem -Recurse -Filter *.csproj | 
    Where-Object Name -NotLike "*.Tests.csproj" |
    ForEach-Object {
        setversion $version $_.FullName
    }