--CREATE TABLE user_profiles();

CREATE TABLE user_accounts(
	username VARCHAR(25) PRIMARY KEY,
	email VARCHAR(40),
	passphrase VARCHAR(40),
	authorization_level VARCHAR,
	account_status BIT
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
	rating INT,

	account_own VARCHAR(25) FOREIGN KEY REFERENCES user_accounts(username)
);

CREATE TABLE tags(
	tag_name VARCHAR(15) PRIMARY KEY
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

-- Keep all of the registrations for a given date
CREATE TABLE daily_registrations(
	registration_date DATE,
	registration_count INT,


);

-- Keep all of the logins for a given date
CREATE TABLE daily_login(
	login_date DATE,
	login_count INT,


);

CREATE TABLE top_search(
	top_search_date DATE,
	search_string VARCHAR(50),
	search_count INT
);

CREATE TABLE nodes_created(
	node_creation_date DATE,
	node_creation_count INT,
	
);