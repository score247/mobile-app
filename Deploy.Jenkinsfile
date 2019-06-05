pipeline{
    agent{
        label 'slave108'
    }

    options{
        buildDiscarder(logRotator(numToKeepStr: '5'))    
        disableConcurrentBuilds()
    }

    stages{
        stage("Checkout"){
            agent { 
                label 'slaveMAC'
            }
            steps{
                withEnv(['PATH=/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/share/dotnet:~/.dotnet/tools:/Library/Frameworks/Mono.framework/Versions/Current/Commands:/Applications/Xamarin Workbooks.app/Contents/SharedSupport/path-bin']) {
                    sh "msbuild /p:Configuration=Test /p:Platform=iPhoneSimulator /t:Restore $WORKSPACE/src/Platforms/LiveScore.iOS/LiveScore.iOS.csproj /v:minimal"

                    sh "msbuild /p:Configuration=Test /p:Platform=iPhoneSimulator /t:ReBuild $WORKSPACE/src/Platforms/LiveScore.iOS/LiveScore.iOS.csproj /p:MtouchArch=x86_64 /v:minimal"
                }
            }
        }
    }
}