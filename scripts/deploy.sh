DEPLOY_DIR="$1"

BUILD_DIR="deploy\\"

#cd $PROJECT_DIR
#'d:\nuget.exe' restore packages.config  -SolutionDirectory .
#[[ ! -d bin ]] && mkdir "bin"

#find packages -name "*.dll" -exec cp {} bin/ \;

pushd "$DEPLOY_DIR" &>/dev/null
pwd
rm -rf *
popd &>/dev/null
pwd

#echo "$BUILD_DIR"
cp -a "$BUILD_DIR." "$DEPLOY_DIR"
#"c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" SDonline.sln /p:Configuration=Production;ProjectName=ODataRestierDynamic;DeployIisAppPath="Default Web Site/odata_unified_svc";DeployServiceUrl="krr-app-pacnt08"
#"c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" SDonline.sln /p:Configuration=Production;ProjectName=ODataRestierDynamic;ProjectFile=ODataRestierDynamic\ODataRestierDynamic\ODataRestierDynamic.csproj;DeployIisAppPath="Default Web Site/odata_unified_svc";DeployServiceUrl="krr-app-pacnt08";UserName=${DEPLOY_USER};Password=${DEPLOY_PASS}

#command build
#VisualBof\Visualbof.csproj /t:WebPublish /p:Configuration=Release;WebPublishMethod=FileSystem;publishUrl=..\deploy;VisualStudioVersion=14.0


