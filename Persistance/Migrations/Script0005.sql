CREATE TABLE Reviews
(
	Id UUID PRIMARY KEY,
	CreatedAt TIMESTAMP NOT NULL,
	UpdatedAt TIMESTAMP NOT NULL,
	AccountId UUID NOT NULL REFERENCES Accounts,
	ProjectId UUID NOT NULL REFERENCES Projects,
	Rate SMALLINT NOT NULL CONSTRAINT RateValuesConstraint CHECK (Rate >= 1 AND Rate <=5),
	Content TEXT,
	UNIQUE (AccountId, ProjectId)
);