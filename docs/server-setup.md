# Server Setup

## Platform

MyHome is hosted on an Ubuntu 24.04 LTS VPS with approximately:

- 2 GB RAM
- 60 GB storage
- Docker Engine
- Docker Compose
- Nginx
- Certbot
- UFW firewall
- 1 GB swap

Docker images are built by GitHub Actions rather than on the VPS.

## DNS

The following DNS names resolve to the VPS:

```text
myhomemaster.org
www.myhomemaster.org
staging.myhomemaster.org

notepad ".\docs\server-setup.md"
Paste:
# Server Setup

## Platform

MyHome is hosted on an Ubuntu 24.04 LTS VPS with approximately:

- 2 GB RAM
- 60 GB storage
- Docker Engine
- Docker Compose
- Nginx
- Certbot
- UFW firewall
- 1 GB swap

Docker images are built by GitHub Actions rather than on the VPS.

## DNS

The following DNS names resolve to the VPS:

```text
myhomemaster.org
www.myhomemaster.org
staging.myhomemaster.org

Nginx selects an application based on the requested hostname.

Directory layout
/home/lyseriuos/
├── MyHome/
│   ├── docker-compose.yml
│   ├── .env
│   ├── MyHome.db
│   ├── uploaded-files/
│   ├── dp-keys/
│   └── backups/
│
├── MyHome-Staging/
│   ├── docker-compose.yml
│   ├── .env
│   ├── MyHome-Staging.db
│   ├── uploaded-files/
│   ├── dp-keys/
│   └── backups/
│
└── bin/
    ├── deploy-production.sh
    └── deploy-staging.sh

Production and staging must never share a database, upload directory, or Data
Protection key directory.

Docker Compose

Production uses:
lyseriuos/myhomeblazorapp:latest
127.0.0.1:7211 -> container port 8080
Staging uses:
lyseriuos/myhomeblazorapp:dev
127.0.0.1:7212 -> container port 8080
Binding to 127.0.0.1 prevents users from bypassing Nginx and accessing the
application containers directly.
Persistent mounts provide:
SQLite database       -> /app/BlazorData/MyHome.db
Uploaded files        -> /app/Files
Data Protection keys  -> /home/app/.aspnet/DataProtection-Keys
Environment configuration
Application secrets are stored in .env files on the VPS. These files must
have restricted permissions and must not be committed:
chmod 600 .env
Environment-specific configuration includes:
Production QrCode__BaseUrl=https://myhomemaster.org
Staging    QrCode__BaseUrl=https://staging.myhomemaster.org
Secret values such as email credentials are referenced by Docker Compose but
are not stored in the repository.
Nginx
Nginx listens publicly on ports 80 and 443 and proxies requests to the
appropriate loopback port.
Production:
myhomemaster.org and www.myhomemaster.org
  -> http://127.0.0.1:7211
Staging:
staging.myhomemaster.org
  -> http://127.0.0.1:7212
Proxy configuration includes forwarded host/protocol headers, WebSocket
upgrade headers for interactive Blazor connections, upload-size limits, and
extended proxy timeouts.
Configuration files are stored under:
/etc/nginx/sites-available/
/etc/nginx/sites-enabled/
Validate Nginx before reloading:
sudo nginx -t
sudo systemctl reload nginx
HTTPS
Let's Encrypt certificates are managed by Certbot's Nginx integration.
Verify renewal scheduling:
systemctl status certbot.timer --no-pager
Test renewal safely:
sudo certbot renew --dry-run
Firewall
UFW permits only the intended public services:
OpenSSH
Nginx Full
Application ports 7211 and 7212 are not public.
Check the firewall:
sudo ufw status verbose
Check listening ports:
sudo ss -tulpn
Permissions
The application runs inside its containers as a non-root user. Bind-mounted
database, upload, and Data Protection directories must be writable by the
container user's numeric UID/GID.
Do not use chmod 777 to solve permission errors. Compare ownership with the
working production environment and grant only the required access.
SSH deployment
GitHub Actions connects using a dedicated SSH key and the non-root
lyseriuos account. The account can manage Docker through membership in the
docker group.
The server SSH host key is pinned in the SERVER_KNOWN_HOSTS GitHub secret.
This prevents the workflow from silently accepting an unknown server identity.
The private deployment key exists only in GitHub Secrets. Its matching public
key remains in:
/home/lyseriuos/.ssh/authorized_keys