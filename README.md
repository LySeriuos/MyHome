# MyHome

MyHome is an ASP.NET Core Blazor application for organizing homes, devices,
warranties, receipts, manuals, and related information.

The project is deployed to a Linux VPS using Docker, Nginx, HTTPS, and a
GitHub Actions CI/CD pipeline. It has separate staging and production
environments with independent SQLite databases and uploaded-file storage.

## Live environments

| Environment | Branch | URL | Docker tag |
|---|---|---|---|
| Staging | `dev` | https://staging.myhomemaster.org | `dev` |
| Production | `master` | https://myhomemaster.org | `latest` |

Staging and production run on the same VPS but use separate containers,
ports, databases, uploaded files, and ASP.NET Core Data Protection keys.

## Technology

- ASP.NET Core 8
- Blazor
- Entity Framework Core
- ASP.NET Core Identity
- SQLite
- Docker and Docker Compose
- Nginx reverse proxy
- Let's Encrypt certificates through Certbot
- GitHub Actions
- Docker Hub

## Deployment flow

```text
Feature development
        |
        v
      dev branch
        |
        v
GitHub Actions builds the Docker image
        |
        v
Docker Hub :dev
        |
        v
staging.myhomemaster.org
        |
        | tested and merged through a pull request
        v
    master branch
        |
        v
GitHub Actions builds the production image
        |
        v
Docker Hub :latest
        |
        v
myhomemaster.org


Every deployment creates a SQLite database backup before replacing the
application container. Persistent data is mounted from the host and is not
included in Docker images.
Documentation
- [Architecture](docs/architecture.md)
- [CI/CD and deployment](docs/deployment.md)
- [Server setup](docs/server-setup.md)
- [Backup and restore](docs/backup-and-restore.md)
- [Troubleshooting](docs/troubleshooting.md)
Security and data handling
Runtime data and secrets are not committed to Git:
- SQLite databases
- Database journal files
- Uploaded documents
- ASP.NET Core Data Protection keys
- .env files
- SSH private keys
- Docker Hub access tokens
GitHub Actions secrets provide deployment credentials. Application secrets
remain in environment files on the server.
Repository workflow
1. Create a feature branch or work on dev.
2. Push changes to dev.
3. Wait for the staging workflow to succeed.
4. Test the change at the staging URL.
5. Open a pull request from dev to master.
6. Review the changes and successful build.
7. Merge the pull request to deploy production.
Do not commit live SQLite databases or uploaded user files. Database schema
changes must be represented by Entity Framework Core migrations.