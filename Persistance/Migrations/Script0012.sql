ALTER TABLE Posts RENAME COLUMN ImageId TO ImageResourceId;
ALTER TABLE Posts RENAME COLUMN ProjectId TO TargetProjectId;
ALTER TABLE ProjectImages RENAME COLUMN ImageId TO ResourceId;