#!/bin/bash

source=( "src/GST.Library.API.REST/GST.Library.API.REST.csproj"
"src/GST.Library.Data/GST.Library.Data.csproj"
"src/GST.Library.Helper/GST.Library.Helper.csproj"
"src/GST.Library.Middleware.HttpOverrides/GST.Library.Middleware.HttpOverrides.csproj",
"src/GST.Library.StoredProcedureHelper/GST.Library.StoredProcedureHelper.csproj"
"src/GST.Library.String/GST.Library.String.csproj"
)

for package in ${source[@]}
do
    dotnet restore $package
    dotnet build $package --configuration Release --framework netcoreapp2.1 --force
    dotnet pack $package --configuration Release --include-source --include-symbols --output ../../nupkgs
done
