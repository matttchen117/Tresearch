CREATE TABLE Accounts(
	Username VARCHAR(25),
	Email VARCHAR(40),
	Passphrase VARCHAR(40),
	AuthorizationLevel VARCHAR(40),
	AccountStatus BIT,
	Confirmed BIT,

	CONSTRAINT user_account_pk PRIMARY KEY(Username, AuthorizationLevel)
);

CREATE TABLE OTPClaims(
	Username VARCHAR(25) FOREIGN KEY REFERENCES Accounts(Username),
	OTP VARCHAR(100),
	AuthorizationLevel VARCHAR(40) FOREIGN KEY REFERENCES Accounts(AuthorizationLevel),
	TimeCreated DATETIME,
	FailCount INT,

	CONSTRAINT otp_claims_pk PRIMARY KEY(Username, AuthorizationLevel)
);

CREATE TABLE Nodes(
	node_ID BIGINT PRIMARY KEY,
	node_parent_id BIGINT,
	node_title VARCHAR(50),
	summary VARCHAR(300),
	mode VARCHAR,

	account_own VARCHAR(25) FOREIGN KEY REFERENCES Accounts(Username)
);

CREATE TABLE Tags(
	tag_name VARCHAR(15) PRIMARY KEY
);

CREATE TABLE NodeTags(
	node_id BIGINT FOREIGN KEY REFERENCES nodes(node_ID),
	tag_name VARCHAR(15) FOREIGN KEY REFERENCES tags(tag_name),
	
	CONSTRAINT node_tags_pk PRIMARY KEY(node_id, tag_name)
);

CREATE TABLE UserRatings(
	username VARCHAR(25) FOREIGN KEY REFERENCES Accounts(Username),
	node_ID BIGINT FOREIGN KEY REFERENCES nodes(node_id),
	rating INT,
	
	CONSTRAINT user_ratings_pk PRIMARY KEY(username, node_id)
);

CREATE TABLE TreeHistories(
	edit_date DATE,
	edit_time TIME,
);

CREATE TABLE ViewsWebPages(
	view_name VARCHAR(25),
	visits INT,
	average_duration FLOAT
);

-- Keep number of registrations on a given date
CREATE TABLE DailyRegistrations(
	registration_date DATE PRIMARY KEY,
	registration_count INT
);

-- Keep all of the logins for a given date
CREATE TABLE DailyLogins(
	login_date DATE PRIMARY KEY,
	login_count INT
);

CREATE TABLE TopSeraches(
	top_search_date DATE PRIMARY KEY,
	search_string VARCHAR(50),
	search_count INT
);

CREATE TABLE NodesCreated(
	node_creation_date DATE PRIMARY KEY,
	node_creation_count INT,
);

CREATE TABLE EmailConfirmationLinks(
	username VARCHAR(25) PRIMARY KEY,
	GUID UNIQUEIDENTIFIER,
	timestamp TIME
);