
pipeline {
  agent {
    dockerfile {
      filename 'Dockerfile.build'
      dir 'jenkins'
    }
  }
  stages {
    stage('checkout') {
      steps {
        checkout scm: [
          $class: 'GitSCM',
          branches: scm.branches,
          doGenerateSubmoduleConfigurations: false,
          extensions: [[$class: 'SubmoduleOption',
                        disableSubmodules: false,
                        parentCredentials: true,
                        recursiveSubmodules: false,
                        reference: '',
                        trackingSubmodules: false]],
          submoduleCfg: [],
          userRemoteConfigs: scm.userRemoteConfigs
        ]
      }
    }

    stage('protobuf gen') {
      steps {
        sh "protoc -I=/usr/local/include -I=RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/events --csharp_out=RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/events RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/events/events.proto"
        sh "protoc -I=/usr/local/include -I=RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/commands --csharp_out=RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/commands RhinoDox.JobDefinition.Domain/Contracts/RhinoDox.JobDefinition.Proto/commands/commands.proto"        
      }
    }
    stage('restore') {
      environment {
        HOME = '.'
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
      }
      steps {
        sh "dotnet restore RhinoDox.JobDefinition.sln"
        sh "dotnet restore RhinoDox.JobDefinition.Hosts.Worker/RhinoDox.JobDefinition.Hosts.Worker.csproj"
      }
    }
    stage('build') {
      environment {
        HOME = '.'
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
      }
      steps {
        sh "rm -rf RhinoDox.JobDefinition-*.tar.gz"
        sh "rm -rf RhinoDox.JobDefinition.Hosts.Worker/RhinoDox.JobDefinition.Hosts.Worker/bin"
        sh "dotnet build -c Release RhinoDox.JobDefinition.Domain/RhinoDox.JobDefinition.Domain.csproj"
        sh "dotnet build -c Release RhinoDox.JobDefinition.Hosts.Worker/RhinoDox.JobDefinition.Hosts.Worker.csproj"
        sh "dotnet publish -c Release RhinoDox.JobDefinition.Hosts.Worker/RhinoDox.JobDefinition.Hosts.Worker.csproj"
      }
    }
    stage('package') {
      steps {
        sh "tar czf RhinoDox.JobDefinition-${env.BUILD_NUMBER}.tar.gz -C RhinoDox.JobDefinition.Hosts.Worker/bin/Release/netcoreapp2.1/publish ."
      }
    }
    stage('upload') {
      when {
        expression {
          BRANCH_NAME ==~ /develop/
        }
      }
      steps {
        sh "aws s3 mv RhinoDox.JobDefinition-${env.BUILD_NUMBER}.tar.gz s3://rhinodox-build-artifacts/rhinodox.job-definition/${BRANCH_NAME}/"
      }
    }
    stage('deploy: dev') {
      when {
        expression {
          BRANCH_NAME ==~ /develop/
        }
      }
      steps {
        build 'ansible/dev/configure-linux-app-server'
      }
    }
  }
}