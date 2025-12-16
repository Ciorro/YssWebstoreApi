BEGIN;

ALTER TABLE Reviews DROP CONSTRAINT reviews_projectid_fkey;
ALTER TABLE Reviews ADD CONSTRAINT reviews_projectid_fkey  
	FOREIGN KEY (ProjectId)
	REFERENCES Projects
	ON DELETE CASCADE;

ALTER TABLE Posts DROP CONSTRAINT posts_projectid_fkey;
ALTER TABLE Posts ADD CONSTRAINT posts_projectid_fkey  
	FOREIGN KEY (TargetProjectId)
	REFERENCES Projects
	ON DELETE SET NULL;

COMMIT;