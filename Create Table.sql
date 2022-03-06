--CREATE TABLE user_profiles();

CREATE TABLE user_accounts(
	username VARCHAR(25),
	email VARCHAR(40),
	passphrase VARCHAR(40),
	authorization_level VARCHAR,
	account_status BIT,
	confirmation BIT,

	CONSTRAINT user_account_pk PRIMARY KEY(username, authorization_level)
);

CREATE TABLE otp_claims(
	username VARCHAR(25) FOREIGN KEY REFERENCES user_accounts(username),
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

	account_own VARCHAR(25) FOREIGN KEY REFERENCES user_accounts(username)
);

CREATE TABLE tags(
	tag_name VARCHAR(15) PRIMARY KEY
);

CREATE TABLE node_tags(
	node_id BIGINT FOREIGN KEY REFERENCES nodes(node_ID),
	tag_name VARCHAR(15) FOREIGN KEY REFERENCES tags(tag_name),
	
	CONSTRAINT node_tags_pk PRIMARY KEY(node_id, tag_name)
);

CREATE TABLE user_ratings(
	username VARCHAR(25) FOREIGN KEY REFERENCES user_accounts(username),
	node_ID BIGINT FOREIGN KEY REFERENCES nodes(node_id),
	rating INT,
	
	CONSTRAINT user_ratings_pk PRIMARY KEY(username, node_id)
);

CREATE TABLE tree_histories(
	edit_date DATE,
	edit_time TIME,
);

CREATE TABLE views_webpage(
	view_name VARCHAR(25),
	visits INT,
	average_duration FLOAT
);

-- Keep number of registrations on a given date
CREATE TABLE daily_registrations(
	registration_date DATE PRIMARY KEY,
	registration_count INT
);

-- Keep all of the logins for a given date
CREATE TABLE daily_login(
	login_date DATE PRIMARY KEY,
	login_count INT
);

CREATE TABLE top_search(
	top_search_date DATE PRIMARY KEY,
	search_string VARCHAR(50),
	search_count INT
);

CREATE TABLE nodes_created(
	node_creation_date DATE PRIMARY KEY,
	node_creation_count INT,
	
);

CREATE TABLE email_confirmation_links(
	username VARCHAR(25) PRIMARY KEY,
	GUID UNIQUEIDENTIFIER,
	timestamp TIME
);
