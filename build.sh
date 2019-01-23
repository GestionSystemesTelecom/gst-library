#!/bin/sh

RED='\033[0;31m'
GREEN='\033[0;32m'
NC='\033[0m' # No Color
print_warning(){
    MESSAGE=$1
    shift;
    echo -e "${RED}

 ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄                 ▄  ▄  ▄ 
▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░▌               ▐░▌▐░▌▐░▌
▐░█▀▀▀▀▀▀▀▀▀ ▐░█▀▀▀▀▀▀▀█░▌ ▀▀▀▀█░█▀▀▀▀ ▐░▌               ▐░▌▐░▌▐░▌
▐░▌          ▐░▌       ▐░▌     ▐░▌     ▐░▌               ▐░▌▐░▌▐░▌
▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄█░▌     ▐░▌     ▐░▌               ▐░▌▐░▌▐░▌
▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌     ▐░▌     ▐░▌               ▐░▌▐░▌▐░▌
▐░█▀▀▀▀▀▀▀▀▀ ▐░█▀▀▀▀▀▀▀█░▌     ▐░▌     ▐░▌               ▐░▌▐░▌▐░▌
▐░▌          ▐░▌       ▐░▌     ▐░▌     ▐░▌                ▀  ▀  ▀ 
▐░▌          ▐░▌       ▐░▌ ▄▄▄▄█░█▄▄▄▄ ▐░█▄▄▄▄▄▄▄▄▄       ▄  ▄  ▄ 
▐░▌          ▐░▌       ▐░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌     ▐░▌▐░▌▐░▌
 ▀            ▀         ▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀       ▀  ▀  ▀ 
                                                                  
\033[90;091m ------------------------------------------------------------------------------------------
${GREEN} $MESSAGE
\033[90;091m ------------------------------------------------------------------------------------------
    ${NC}"

exit 1
}

source=(
"src/GST.Library.Middleware.HttpOverrides/GST.Library.Middleware.HttpOverrides.csproj"
"src/GST.Library.API.REST/GST.Library.API.REST.csproj"
"src/GST.Library.Data/GST.Library.Data.csproj"
"src/GST.Library.Helper/GST.Library.Helper.csproj"
"src/GST.Library.StoredProcedureHelper/GST.Library.StoredProcedureHelper.csproj"
"src/GST.Library.String/GST.Library.String.csproj"
"src/GST.Library.TimeZone/GST.Library.TimeZone.csproj"
)

for package in ${source[@]}
do
	echo "---------------------------------------------------------------------------------------"
	echo "----- $package -----"
	echo "---------------------------------------------------------------------------------------"

    dotnet restore $package
	if [ $? -ne 0 ]; then
		print_warning "Restore failed $package"
	fi
    dotnet build $package --configuration Release --framework netcoreapp2.2 --force
	if [ $? -ne 0 ]; then
		print_warning "Build failed $package"
	fi
    dotnet pack $package --configuration Release --include-source --include-symbols --output ../../nupkgs
	if [ $? -ne 0 ]; then
		print_warning "Pack failed $package"
	fi

	echo -e "\n\n"
done
