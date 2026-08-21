# Backup and Restore

## Data that must be protected

MyHome stores persistent data in two places:

1. SQLite stores users, homes, devices, warranties, and file references.
2. The filesystem stores uploaded receipts, warranties, manuals, and other
   documents.

ASP.NET Core Data Protection keys must also be retained so authentication and
protected application data remain usable across container replacements.

A complete environment backup includes:

```text
Database
uploaded-files/
dp-keys/
.env (stored securely and separately)

Automated deployment backups

Before replacing a container, each deployment script:
1. Stops the relevant application container.
2. Copies its SQLite database into the environment's backups directory.
3. Starts the new application version.
Stopping the application briefly ensures the SQLite copy is consistent.
Production backups:
/home/lyseriuos/MyHome/backups/MyHome-<UTC timestamp>.db
Staging backups:
/home/lyseriuos/MyHome-Staging/backups/MyHome-Staging-<UTC timestamp>.db
List recent backups:
ls -lht backups | head
Manual full backup
Run the commands in the correct environment directory. The following example
uses production.
cd /home/lyseriuos/MyHome
docker compose stop myhome

backup_date=$(date -u +%Y%m%d-%H%M%S)

cp MyHome.db "backups/MyHome-$backup_date.db"

sudo tar -czf "backups/files-$backup_date.tar.gz" \
  uploaded-files dp-keys

docker compose up -d
Verify:
ls -lh backups
tar -tzf "backups/files-$backup_date.tar.gz" | head
curl --fail --silent https://myhomemaster.org >/dev/null
Database restore
Restoring production changes live data. Confirm the environment and backup
filename before proceeding.
cd /home/lyseriuos/MyHome
pwd
docker compose stop myhome
Preserve the current database before replacing it:
restore_date=$(date -u +%Y%m%d-%H%M%S)
cp MyHome.db "backups/MyHome-before-restore-$restore_date.db"
Restore the selected backup:
cp "backups/SELECTED-BACKUP.db" MyHome.db
sudo chown 1654:1654 MyHome.db
sudo chmod 664 MyHome.db
docker compose up -d
Verify:
curl --fail --silent https://myhomemaster.org >/dev/null
docker compose logs --tail 100 myhome
The numeric UID/GID must match the application user in the current image.
Confirm it when necessary:
docker exec myhome id
File restore
Inspect an archive before extracting it:
tar -tzf backups/SELECTED-FILES-BACKUP.tar.gz | head
Stop the application, preserve current files, and then extract the selected
archive into the environment directory. Database and file storage should be
restored from compatible backup times because database rows contain paths to
uploaded files.
Staging refresh
Refreshing staging means replacing its database with a separate snapshot of
production:
Production MyHome.db
       -> safe snapshot
Staging MyHome-Staging.db
Staging must never mount the live production database.
A refresh should:
1. Create a consistent production SQLite snapshot.
2. Stop staging.
3. Preserve the current staging database.
4. Copy the snapshot to MyHome-Staging.db.
5. Sanitize personal data if the database contains real users.
6. Correct ownership and permissions.
7. Start staging and run a health check.
Staging email delivery should be disabled, redirected, or use separate test
credentials before using copied production data.
Retention
Deployment backups accumulate over time. A retention policy should keep a
limited number of recent backups and selected longer-term backups.
Backups stored only on the same VPS do not protect against complete server or
disk loss. Important backups should also be copied to encrypted storage
outside the VPS.
Migration and rollback warning
Entity Framework Core migrations can change the database schema. Reverting
only the Docker image may not be sufficient if an older application version
cannot read the migrated schema.
For deployments containing migrations:
- Verify migrations in staging first.
- Keep the pre-deployment database backup.
- Treat application rollback and database rollback as one coordinated
  operation.