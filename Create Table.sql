CREATE TABLE useraccounts(
	username VARCHAR(25),
	email VARCHAR(40),
	passphrase VARCHAR(40),
	authorization_level VARCHAR(40),
	account_status BIT,
	confirmation BIT,

	CONSTRAINT user_account_pk PRIMARY KEY(username, authorization_level)
);

CREATE TABLE otpclaims(
	username VARCHAR(25) FOREIGN KEY REFERENCES useraccounts(username),
	otp VARCHAR(100),
	time_created TIME,
	fail_count INT,
);

CREATE TABLE nodes(
	node_ID BIGINT PRIMARY KEY,
	node_parent_id BIGINT,
	node_title VARCHAR(50),
	summary VARCHAR(300),
	mode VARCHAR,

	account_own VARCHAR(25) FOREIGN KEY REFERENCES useraccounts(username)
);

CREATE TABLE tags(
	tag_name VARCHAR(15) PRIMARY KEY
);

CREATE TABLE nodetags(
	node_id BIGINT FOREIGN KEY REFERENCES nodes(node_ID),
	tag_name VARCHAR(15) FOREIGN KEY REFERENCES tags(tag_name),
	
	CONSTRAINT node_tags_pk PRIMARY KEY(node_id, tag_name)
);

CREATE TABLE userratings(
	username VARCHAR(25) FOREIGN KEY REFERENCES useraccounts(username),
	node_ID BIGINT FOREIGN KEY REFERENCES nodes(node_id),
	rating INT,
	
	CONSTRAINT user_ratings_pk PRIMARY KEY(username, node_id)
);

CREATE TABLE treehistories(
	edit_date DATE,
	edit_time TIME,
);

CREATE TABLE viewswebpage(
	view_name VARCHAR(25),
	visits INT,
	average_duration FLOAT
);

-- Keep number of registrations on a given date
CREATE TABLE dailyregistrations(
	registration_date DATE PRIMARY KEY,
	registration_count INT
);

-- Keep all of the logins for a given date
CREATE TABLE dailylogins(
	login_date DATE PRIMARY KEY,
	login_count INT
);

CREATE TABLE topsearches(
	top_search_date DATE PRIMARY KEY,
	search_string VARCHAR(50),
	search_count INT
);

CREATE TABLE nodescreated(
	node_creation_date DATE PRIMARY KEY,
	node_creation_count INT,
	
);

CREATE TABLE emailconfirmationlinks(
	username VARCHAR(25) PRIMARY KEY,
	GUID UNIQUEIDENTIFIER,
	timestamp TIME
);