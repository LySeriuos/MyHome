# Architecture

## Overview

MyHome uses two isolated application environments on one Ubuntu VPS.

```text
Internet
   |
   v
Nginx :80/:443
   |
   +-- myhomemaster.org
   |      |
   |      v
   |   127.0.0.1:7211
   |      |
   |      v
   |   myhome production container
   |
   +-- staging.myhomemaster.org
          |
          v
       127.0.0.1:7212
          |
          v
       myhome-staging container

Nginx terminates HTTPS and forwards requests to Docker containers bound only
to the server's loopback interface. Application ports 7211 and 7212 are
therefore not exposed directly to the internet.
Environment isolation
Resource	Production	Staging
Domain	myhomemaster.org	staging.myhomemaster.org
Branch	master	dev
Docker tag	latest	dev
Container	myhome	myhome-staging
Host port	127.0.0.1:7211	127.0.0.1:7212
Server directory	/home/lyseriuos/MyHome	/home/lyseriuos/MyHome-Staging
Database	MyHome.db	MyHome-Staging.db
Upload storage	Separate directory	Separate directory
Data Protection keys	Separate directory	Separate directory


The environments never mount the same SQLite database or uploaded-file
directory. Testing in staging therefore does not change production data.
Persistent storage
Containers are replaceable. Persistent state is stored on the VPS and bind
mounted into each container:
Host SQLite database       -> /app/BlazorData/MyHome.db
Host uploaded-files        -> /app/Files
Host Data Protection keys  -> /home/app/.aspnet/DataProtection-Keys
The database contains application records and file references. Uploaded
documents remain in the filesystem. Database and uploaded-file backups must
therefore be treated as related data.
Database strategy
SQLite was selected because MyHome runs on a single 2 GB RAM VPS and does not
currently require a separate database server. This reduces operational and
memory overhead.
Entity Framework Core migrations are applied when the application starts:
- A staging deployment migrates only the staging database.
- A production deployment migrates only the production database.
- The database is backed up before the new container starts.
Networking and TLS
- DNS points all application domains to the VPS.
- UFW permits SSH, HTTP, and HTTPS.
- Nginx is the only public web entry point.
- Certbot obtains Let's Encrypt certificates.
- certbot.timer checks for renewal twice daily.
- HTTP requests are redirected to HTTPS.
Resource considerations
The VPS has approximately 2 GB RAM. Docker images are built by GitHub-hosted
runners instead of on the VPS. This keeps deployment memory and CPU usage low.
SQLite also avoids the overhead of an additional database container.