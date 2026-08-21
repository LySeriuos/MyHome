# CI/CD and Deployment

## Overview

MyHome uses GitHub Actions to build Docker images, publish them to Docker Hub,
and deploy them to a Linux VPS through SSH.

The workflow is defined in:

```text
.github/workflows/ci.yml

Branch behavior
Event	Build	Publish image	Deploy
Push to dev	Yes	:dev and commit SHA	Staging
Push to master	Yes	:latest and commit SHA	Production
Pull request to dev or master	Yes	No	No
Manual workflow run	Yes	No	No


A pull request therefore validates that the Docker image can be built without
being allowed to publish an image or connect to the VPS.
Build process
The GitHub-hosted Ubuntu runner:
1. Checks out the repository.
2. Configures Docker Buildx.
3. Logs into Docker Hub for branch pushes.
4. Builds MyHomeBlazorApp/Dockerfile using the repository root as context.
5. Targets the linux/amd64 platform.
6. Uses GitHub Actions build caching.
7. Publishes a branch tag and an immutable commit-SHA tag.
Example image tags:
lyseriuos/myhomeblazorapp:dev
lyseriuos/myhomeblazorapp:latest
lyseriuos/myhomeblazorapp:<git-commit-sha>
The commit-SHA tag identifies the exact source revision used for an image and
can support diagnosis or rollback.
Deployment process
A deployment job runs only after the Docker build job succeeds.
GitHub Actions:
1. Loads a deployment SSH private key from GitHub Secrets.
2. Pins the VPS SSH host key through known_hosts.
3. Connects as the non-root deployment user.
4. Runs the appropriate server-side deployment script.
Server scripts:
/home/lyseriuos/bin/deploy-staging.sh
/home/lyseriuos/bin/deploy-production.sh
Each script:
1. Pulls the new Docker image before causing downtime.
2. Stops its application container.
3. Creates a consistent SQLite backup.
4. Recreates the container with Docker Compose.
5. Retries a local HTTP health check.
6. Reports container logs if the health check fails.
Staging deployment
Push to dev
  -> build image
  -> publish :dev
  -> run deploy-staging.sh
  -> back up MyHome-Staging.db
  -> recreate myhome-staging
  -> check http://127.0.0.1:7212
Staging URL:
https://staging.myhomemaster.org
Production deployment
Merge dev into master
  -> build image
  -> publish :latest
  -> run deploy-production.sh
  -> back up MyHome.db
  -> recreate myhome
  -> check http://127.0.0.1:7211
Production URL:
https://myhomemaster.org
Production promotion should happen through a reviewed pull request from dev
to master after the same changes have been tested in staging.
GitHub Actions secrets
The workflow expects the following secret names:
Secret	Purpose
DOCKERHUB_USERNAME	Docker Hub account name
DOCKERHUB_TOKEN	Read/write Docker Hub access token
SERVER_HOST	VPS IP address or hostname
SERVER_USER	Non-root SSH deployment account
SERVER_PORT	SSH port
SERVER_KNOWN_HOSTS	Pinned public SSH host key
STAGING_SSH_PRIVATE_KEY	Deployment SSH private key


Secret values must never be committed, printed in logs, or included in
documentation.
The current STAGING_SSH_PRIVATE_KEY name is historical: the same deployment
key is used by both environments. Renaming it to DEPLOY_SSH_PRIVATE_KEY
would describe its scope more accurately.
Concurrency
The workflow allows one active run per Git reference. A newer push cancels an
older in-progress run for the same branch. This prevents an older commit from
finishing after and replacing a newer deployment.
Failure behavior
- Build failure: no image is published and no deployment runs.
- Registry login/push failure: the existing environment remains running.
- SSH failure: the image may be published, but the server is unchanged.
- Health-check failure: the workflow fails and displays recent container logs.
- Database migration failure: the deployment fails; use the pre-deployment
  database backup for diagnosis or restoration.
A failed workflow does not automatically mean the currently running website
is offline. Always check the environment URL and server container status.