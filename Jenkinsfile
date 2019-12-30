@Library("JenkinsPipelineLibrary") _

pipeline{
    agent{
        label 'slave108'
    }

	parameters {
		string(name: 'SimulatorUDID', defaultValue: '13C278BF-A473-4654-B7AE-D1569ADA54E4', description: 'Simulator iPhone 8 UDID')
	}

    options{
        buildDiscarder logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '1', numToKeepStr: '5')
        disableConcurrentBuilds()
        gitLabConnection('Gitlab')
    }

    triggers{
        gitlab(triggerOnPush: true, triggerOnMergeRequest: true, branchFilterType: 'All')
        cron('H 20 * * *')
    }

    stages{
        stage('Build') {
            steps {
                script{
                    pipelineLib.beginSonarQubeForMsBuild("livescore", "Score247 / Score247 App", "/d:sonar.cs.opencover.reportsPaths=\"${WORKSPACE}\\CoverageReports\\*.xml\" /d:sonar.cs.vstest.reportsPaths=\"${WORKSPACE}\\TestResults\\*.trx\"")

                    pipelineLib.msBuild16("LiveScore.App.sln")
                }
            }
        }

        stage("C# Unit Test"){
            steps{
                script{
                    pipelineLib.xUnitForNetCore()
                }
            }
        }

        stage("SonarQube Analysis"){
            steps{
                script{
                    pipelineLib.endSonarQubeForMsBuild()
                }
            }

            post{
                always{
                    script{
                        if(manager.logContains(".*Quality gate status.*")){
                            pipelineLib.generateSonarQubeReport("livescore")
                        }
                    }

                    archiveArtifacts "*.xml,*.email"

                    step([$class: 'ACIPluginPublisher', name: '*.xml', shownOnProjectPage: false])

                    mstest failOnError: false
                }
            }
        }

        stage("Deploy to Local"){
            when {
                allOf {
                    triggeredBy 'TimerTrigger'
                    expression { BRANCH_NAME ==~ /^(origin\/)*\d+\-Sprint\d+$/ }
                }
            }
            parallel{
                stage("Deploy Api"){
                    steps{
                        script{
                            pipelineLib.deployByRocketor("11156,11095,11114", "$BRANCH_NAME", "", "", "86c94ed8b8ed4fad95da4c9961992ff7")
                        }
                    }
                }

                stage("Deploy App"){
                    agent {
                        label 'slaveNewMAC'
                    }
                    steps{
                        withEnv(['PATH=/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/share/dotnet:~/.dotnet/tools:/Library/Frameworks/Mono.framework/Versions/Current/Commands:/Applications/Xamarin Workbooks.app/Contents/SharedSupport/path-bin']) {
                            sh label: "Restore nuget", script: "msbuild /p:Configuration=AutoTest /p:Platform=iPhoneSimulator /t:Restore $WORKSPACE/src/Platforms/LiveScore.iOS/LiveScore.iOS.csproj /v:minimal"

                            sh label: "Build IOS App", script: "msbuild /p:Configuration=AutoTest /p:Platform=iPhoneSimulator /t:ReBuild $WORKSPACE/src/Platforms/LiveScore.iOS/LiveScore.iOS.csproj /p:MtouchArch=x86_64 /v:minimal"

							sh label: "Shutdown Simulator", script: "/usr/bin/xcrun simctl shutdown all"

							sh label: "Boot Simulator", script: "/usr/bin/xcrun simctl boot ${params.SimulatorUDID}"

                            sh label: "Uninstall App", script: "/usr/bin/xcrun simctl uninstall ${params.SimulatorUDID} Score247.LiveScore"

                            sh label: "Install App", script: "/usr/bin/xcrun simctl install ${params.SimulatorUDID} $WORKSPACE/src/Platforms/LiveScore.iOS/bin/iPhoneSimulator/AutoTest/LiveScoreApp.iOS.app"
                        }
                    }
                }
            }
        }
    }
    post{
        unsuccessful{
            emailext body: '$DEFAULT_CONTENT', subject: '$DEFAULT_SUBJECT', to: 'vivian.nguyen@starixsoft.com, harrison.nguyen@starixsoft.com, anders.le@starixsoft.com, ricky.nguyen@starixsoft.com'
        }
    }
}
