$pakages = @(
    "src/GST.Library.API.REST/GST.Library.API.REST.csproj",
    "src/GST.Library.Data/GST.Library.Data.csproj",
	"src/GST.Library.Middleware.HttpOverrides/GST.Library.Middleware.HttpOverrides.csproj",
    "src/GST.Library.Helper/GST.Library.Helper.csproj",
    "src/GST.Library.StoredProcedureHelper/GST.Library.StoredProcedureHelper.csproj",
	"src/GST.Library.String/GST.Library.String.csproj"
    )
For ($i=0; $i -lt $pakages.Length; $i++) {
    dotnet restore $pakages[$i]
    dotnet build $pakages[$i] --configuration Release --framework netcoreapp2.0 --force
    dotnet pack $pakages[$i] --configuration Release --include-source --include-symbols --output ../../nupkgs
}
