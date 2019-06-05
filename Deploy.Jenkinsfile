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
                checkout([$class: 'GitSCM', branches: [[name: '${env.BRANCH_NAME}']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CleanCheckout']], submoduleCfg: [], userRemoteConfigs: [[]]])
            }
        }
    }
}