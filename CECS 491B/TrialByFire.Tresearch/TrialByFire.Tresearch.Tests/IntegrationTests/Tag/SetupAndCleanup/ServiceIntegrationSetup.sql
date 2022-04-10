Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('tresearchTagServiceShould1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('tresearchTagServiceShould2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('tresearchTagServiceShould3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4'
		FROM Accounts WHERE Username = 'tresearchTagServiceShould1@tresearch.system';

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84'
		FROM Accounts WHERE Username = 'tresearchTagServiceShould2@tresearch.system';

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5'
		FROM Accounts WHERE Username = 'tresearchTagServiceShould3@tresearch.system';

INSERT Nodes(UserHash, NodeID, NodeParentID, NodeTitle, Summary, Visibility) VALUES 
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 2072942630, null, 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 2072942631, null, 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 2072942632, null, 'Cooking with thumbs', 'An artform', 1),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 2072942633, null, 'C++', 'Garbage collection not found', 0),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 2072942634, null, 'Tea', 'Black, Earl Gray, Green', 1),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 2072942635, null, 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942636, null, 'My Node', 'Important information', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942637, null, 'Dachin', 'May mean Rome', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942638, null, 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942639, null, 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942640, null, 'Julius Caeser', 'Not the first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942641, null, 'Augustus Caeser', 'The first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942642, null, 'Mark Antony', 'Almost the first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 2072942643, null, 'Octavian', 'Also the first emporer', 0);

INSERT Tags(TagName, TagCount) Values ('Tresearch Service Add Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Add Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Add Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Add Tag4', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Delete Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Delete Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Delete Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Delete Tag4', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Get Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Get Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Get Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch Service Get Tag4', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(2072942630, 'Tresearch Service Add Tag1'),
	(2072942631, 'Tresearch Service Add Tag1'),
	(2072942632, 'Tresearch Service Add Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (2072942630, 'Tresearch Service Add Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (2072942631, 'Tresearch Service Add Tag3');

INSERT NodeTags(NodeID, TagName) VALUES
	(2072942633, 'Tresearch Service Delete Tag1'),
	(2072942634, 'Tresearch Service Delete Tag1'),
	(2072942635, 'Tresearch Service Delete Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (2072942633, 'Tresearch Service Delete Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (2072942634, 'Tresearch Service Delete Tag3');

INSERT NodeTags(NodeID, TagName) VALUES
	(2072942636, 'Tresearch Service Get Tag1'),
	(2072942637, 'Tresearch Service Get Tag1'),
	(2072942638, 'Tresearch Service Get Tag1');
INSERT NodeTags(NodeID, TagName) VALUES
	(2072942636, 'Tresearch Service Get Tag2'),
	(2072942637, 'Tresearch Service Get Tag2'),
	(2072942638, 'Tresearch Service Get Tag2');
INSERT NodeTags(NodeID, TagName) VALUES (2072942636, 'Tresearch Service Get Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (2072942639, 'Tresearch Service Get Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(2072942639, 'Tresearch Service Get Tag1'),
	(2072942640, 'Tresearch Service Get Tag2'),
	(2072942641, 'Tresearch Service Get Tag3');

INSERT Tags(TagName, TagCount) VALUES 
	('Tresearch Service Create tag2', 0);
INSERT Tags(TagName,TagCount) VALUES 
	('Tresearch Service Remove Me tag2', 0);
INSERT Tags(TagName,TagCount) VALUES 
	('Tresearch Service Remove Me tag3', 0);

INSERT NodeTags(NodeID, TagName) VALUES (2072942643, 'Tresearch Service Remove Me tag3');