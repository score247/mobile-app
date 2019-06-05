node("slave108"){
    properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '', numToKeepStr: '5')), disableConcurrentBuilds(), gitLabConnection('')])
    
    def scmInfo

    stage("Checkout"){
	    scmInfo = checkout scm
    }

    stage("Checkout MAC"){             
        node 'slaveMAC'
        
        checkout([$class: 'GitSCM', branches: [[name: '${scmInfo.GIT_COMMIT}']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CleanCheckout']], submoduleCfg: [], userRemoteConfigs: [[url: 'git@gitlab.nexdev.net:livescore/LiveScoreApp.git']]])        
    }    
}