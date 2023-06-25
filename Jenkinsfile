pipeline {
  
  agent none

  environment {
    COMMIT_ID = ''
    BRANCH_NAME = 'main'
    PROJECT_NAME = 'Booking Food'
    DOCKER_IMAGE = 'huynx11/booking-food:latest'
    USER_SERVER_DEPLOY = 'root'
    JENKINS_SERVER = "https://jenkins.code-mega.com"
    IP_SERVER_DEPLOY = '194.233.83.104'
    DEPLOY_FOLDER = './deploy/booking-food/deploy.sh'
    WEBSITE_URL = 'https://client-demo-1.code-mega.com'
    REPO_URL = "https://gitlab.com/huynx11.dev/booking-food"
    DISCORD_WEBHOOK_URL = "https://discordapp.com/api/webhooks/987684086241951794/vhS9u3MyqRP6-oMojbs0_P4tCeZ3Vm4Eg3vKtUjzxdjMEUgRAD_GxBX9_hk2QVgZEoU0"
  }

  // begin stages
  stages {

    // begin checkout branch
    stage('checkout branch') {
        agent {
        node {
          label 'built-in'
        }
      }
        steps{
            script{
              COMMIT_ID = GIT_COMMIT.substring(0, 7)
              echo "${GIT_BRANCH}"
            }
        }
    }
    // end checkout branch
 
    // begin build and push
    stage('build and push') {
      agent {
        node {
          label 'built-in'
        }
      }

      steps {
        withCredentials([usernamePassword(credentialsId: 'docker-hub', usernameVariable: 'DOCKER_USERNAME', passwordVariable: 'DOCKER_PASSWORD')]) {
            sh 'echo $DOCKER_PASSWORD | docker login --username $DOCKER_USERNAME --password-stdin'
            echo 'Login Docker'
        }
        // build push clean docker image and push docker hub
        script {
            sh "docker build -t ${DOCKER_IMAGE} . "
            sh "docker push ${DOCKER_IMAGE}"
            sh "docker rmi ${DOCKER_IMAGE}"
            echo "Build and push docker image"
        }
      }
    }
    // end start build and push

    // begin deploy
    stage('deploy') {
        agent {
          node {
            label 'built-in'
          }
        }
        steps {
            sshagent (credentials: ['ssh-jenkins-server']) {
              script {
                sh "ssh -p 11011 -o StrictHostKeyChecking=no -l ${USER_SERVER_DEPLOY} ${IP_SERVER_DEPLOY} '${DEPLOY_FOLDER}'"
              }
            }
        }
    }
    // end deploy
  }
  // end stages

  // begin post event
 post {
    always {
        echo currentBuild.currentResult
        discordSend description: "Build Number: #${BUILD_NUMBER}\nBranch: ${BRANCH_NAME}\nCommit ID: ${COMMIT_ID}\nProject Name: ${PROJECT_NAME}\nWebsite: ${WEBSITE_URL}\nRepo URL : ${REPO_URL}", footer: "BUILD ${currentBuild.currentResult}", link: "${JENKINS_SERVER}/job/${JOB_NAME}/${BUILD_NUMBER}/", result: currentBuild.currentResult, title: JOB_NAME, webhookURL: "${DISCORD_WEBHOOK_URL}"
        //slackSend color: "good", botUser: true, channel: '#cicd-notice-potoro-all', message: 'Search Service - Build Successfully :white_check_mark:', teamDomain: 'CI-CD Workspace', tokenCredentialId: 'slack-bot-token'
    }
  }
  // end post event
}
