# Troubleshooting

## Start with environment identification

Before running commands, confirm the environment:

```bash
pwd

Paste:
# Troubleshooting

## Start with environment identification

Before running commands, confirm the environment:

```bash
pwd
Expected production directory:
/home/lyseriuos/MyHome
Expected staging directory:
/home/lyseriuos/MyHome-Staging
Accidentally using the production directory while diagnosing staging can
modify real data.
Check container state
docker compose ps
docker compose logs --tail 100
Check both applications:
docker ps --format 'table {{.Names}}\t{{.Image}}\t{{.Status}}\t{{.Ports}}'
Expected ports:
myhome          127.0.0.1:7211 -> 8080
myhome-staging  127.0.0.1:7212 -> 8080
Check local application health
Production:
curl -sS -o /dev/null \
  -w "Production local: %{http_code}\n" \
  http://127.0.0.1:7211
Staging:
curl -sS -o /dev/null \
  -w "Staging local: %{http_code}\n" \
  http://127.0.0.1:7212
A 200 response confirms that Kestrel is responding through the Docker port
mapping.
curl -I sends a HEAD request. If the application accepts only GET and
POST, it may return 405 Method Not Allowed even when it is healthy. Use
the normal GET-based checks above.
Check public HTTPS
curl -sS -o /dev/null \
  -w "Production HTTPS: %{http_code}\n" \
  https://myhomemaster.org

curl -sS -o /dev/null \
  -w "Staging HTTPS: %{http_code}\n" \
  https://staging.myhomemaster.org
If local health succeeds but public HTTPS fails, investigate Nginx, DNS,
firewall rules, and certificates rather than the application container.
Check DNS
getent ahostsv4 myhomemaster.org
getent ahostsv4 staging.myhomemaster.org
Both should resolve to the VPS address.
Check Nginx
sudo nginx -t
sudo systemctl status nginx --no-pager
sudo journalctl -u nginx --since "15 minutes ago"
Do not reload Nginx if nginx -t reports an error.
Check certificates
sudo certbot certificates
systemctl status certbot.timer --no-pager
sudo certbot renew --dry-run
GitHub Actions build failure
A final message such as:
buildx failed
dotnet build exited with code 1
is only a summary. Search earlier in the build log for:
error CS
error MSB
The first compiler error normally identifies the real file, line, and cause.
If the build fails:
- No new image is published.
- The deployment job does not run.
- The currently running staging or production container remains unchanged.
Reproduce locally:
dotnet build ".\MyHomeBlazorApp\MyHomeBlazorApp.csproj" `
  --configuration Release
Docker registry failure
If docker compose pull fails, the deployment script pulls before stopping
the application. The current container should therefore remain running.
Inspect the configured image:
docker compose config --images
Inspect the running container:
docker inspect CONTAINER_NAME \
  --format 'Image={{.Config.Image}} Started={{.State.StartedAt}}'
Verify the deployed image
For staging:
docker inspect myhome-staging --format '{{.Image}}'
docker image inspect lyseriuos/myhomeblazorapp:dev --format '{{.Id}}'
For production:
docker inspect myhome --format '{{.Image}}'
docker image inspect lyseriuos/myhomeblazorapp:latest --format '{{.Id}}'
Matching IDs confirm that the container uses the locally downloaded branch
image.
SQLite permission failure
Typical symptoms include:
SQLite Error: attempt to write a readonly database
Permission denied
Inspect ownership:
docker exec CONTAINER_NAME id
stat -c '%u:%g %A %n' DATABASE_FILE uploaded-files dp-keys
The mounted paths must be writable by the container application's UID/GID.
Do not solve the problem with chmod 777.
Data Protection key permission failure
Typical log message:
UnauthorizedAccessException
Access to DataProtection-Keys is denied
Compare directory ownership with the container user:
docker exec CONTAINER_NAME id
stat -c '%u:%g %A %n' dp-keys
Staging and production should use different Data Protection key directories.
Staging login does not accept production account
This is expected when staging has its own empty or independently populated
database. User accounts are stored in SQLite and are not automatically shared
between environments.
Create a staging test account or perform a controlled, sanitized staging
database refresh. Never mount the production database into staging.
Missing environment configuration
Inspect a non-secret environment value:
docker exec myhome printenv QrCode__BaseUrl
docker exec myhome-staging printenv QrCode__BaseUrl
Expected:
Production: https://myhomemaster.org
Staging:    https://staging.myhomemaster.org
Do not print secret environment variables into terminal history or CI logs.
SSH deployment failure
Common errors:
Permission denied (publickey)
Host key verification failed
Check:
- The private key secret includes its complete BEGIN and END lines.
- The matching public key exists in the deployment user's authorized_keys.
- SERVER_KNOWN_HOSTS contains the correct server host key.
- SERVER_USER, SERVER_HOST, and SERVER_PORT are correct.
- The deployment script exists and is executable.
Server resource checks
free -h
df -h /
docker stats --no-stream
swapon --show
Low free memory alone is not necessarily a problem because Linux uses RAM
for filesystem cache. The available value is more useful.
Useful log commands
Recent application logs:
docker compose logs --since 15m
Follow logs:
docker compose logs --follow
Recent Nginx service logs:
sudo journalctl -u nginx --since "15 minutes ago"
Listening ports:
sudo ss -tulpn