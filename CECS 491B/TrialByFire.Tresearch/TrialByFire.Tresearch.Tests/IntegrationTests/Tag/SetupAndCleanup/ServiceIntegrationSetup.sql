DELETE NodeTags WHERE TagName = 'Tresearch Service Create tag1';
DELETE NodeTags WHERE TagName = 'Tresearch Service Create tag2';
DELETE NodeTags WHERE TagName = 'Tresearch Service Add Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch Service Add Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch Service Add Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch Service Add Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch Service Add Tag5';
DELETE NodeTags WHERE TagName = 'Tresearch Service Delete Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch Service Delete Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch Service Delete Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch Service Delete Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch Service Delete Tag5';
DELETE NodeTags WHERE TagName = 'Tresearch Service Get Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch Service Get Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch Service Get Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch Service Get Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch Service Get Tag5';
DELETE NodeTags WHERE TagName = 'Tresearch Service Remove Me tag1';
DELETE NodeTags WHERE TagName = 'Tresearch Service Remove Me tag2';
DELETE NodeTags WHERE TagName = 'Tresearch Service Remove Me tag3';

DELETE Tags WHERE TagName = 'Tresearch Service Create tag1';
DELETE Tags WHERE TagName = 'Tresearch Service Create tag2';
DELETE Tags WHERE TagName = 'Tresearch Service Add Tag1';
DELETE Tags WHERE TagName = 'Tresearch Service Add Tag2';
DELETE Tags WHERE TagName = 'Tresearch Service Add Tag3';
DELETE Tags WHERE TagName = 'Tresearch Service Add Tag4';
DELETE Tags WHERE TagName = 'Tresearch Service Add Tag5';
DELETE Tags WHERE TagName = 'Tresearch Service Delete Tag1';
DELETE Tags WHERE TagName = 'Tresearch Service Delete Tag2';
DELETE Tags WHERE TagName = 'Tresearch Service Delete Tag3';
DELETE Tags WHERE TagName = 'Tresearch Service Delete Tag4';
DELETE Tags WHERE TagName = 'Tresearch Service Delete Tag5';
DELETE Tags WHERE TagName = 'Tresearch Service Get Tag1';
DELETE Tags WHERE TagName = 'Tresearch Service Get Tag2';
DELETE Tags WHERE TagName = 'Tresearch Service Get Tag3';
DELETE Tags WHERE TagName = 'Tresearch Service Get Tag4';
DELETE Tags WHERE TagName = 'Tresearch Service Get Tag5';
DELETE Tags WHERE TagName = 'Tresearch Service Remove Me tag1';
DELETE Tags WHERE TagName = 'Tresearch Service Remove Me tag2';
DELETE Tags WHERE TagName = 'Tresearch Service Remove Me tag3';

DELETE Nodes WHERE UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4';
DELETE Nodes WHERE UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84';
DELETE Nodes WHERE UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5';

DELETE UserHashTable WHERE UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4';
DELETE UserHashTable WHERE UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84';
DELETE UserHashTable WHERE UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5';

DELETE Accounts WHERE Username = 'tresearchTagServiceShould1@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceShould2@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceShould3@tresearch.system';

DROP PROCEDURE IF EXISTS ServiceIntegrationTagInitializeProcedure;

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

INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4', 'Cooking with thumbs', 'An artform', 1),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 'C++', 'Garbage collection not found', 0),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 'Tea', 'Black, Earl Gray, Green', 1),
	('08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84', 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'My Node', 'Important information', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Dachin', 'May mean Rome', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Julius Caeser', 'Not the first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Augustus Caeser', 'The first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Mark Antony', 'Almost the first emporer', 1),
	('bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5', 'Octavian', 'Also the first emporer', 0);

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

DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'CECS 491A');
DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'CECS 491B');
DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'Cooking with thumbs');
DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'C++');
DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'Tea');
DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'Coffee');
DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'My Node');
DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Dachin');
DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Henry The Eigth');
DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Bloody Mary');
DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Julius Caeser');
DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Augustus Caeser');
DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Mark Antony');
DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Octavian');

INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch Service Add Tag1'),
	(@node1, 'Tresearch Service Add Tag1'),
	(@node2, 'Tresearch Service Add Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (@node0, 'Tresearch Service Add Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (@node1, 'Tresearch Service Add Tag3');

INSERT NodeTags(NodeID, TagName) VALUES
	(@node3, 'Tresearch Service Delete Tag1'),
	(@node4, 'Tresearch Service Delete Tag1'),
	(@node5, 'Tresearch Service Delete Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (@node3, 'Tresearch Service Delete Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (@node4, 'Tresearch Service Delete Tag3');

INSERT NodeTags(NodeID, TagName) VALUES
	(@node6, 'Tresearch Service Get Tag1'),
	(@node7, 'Tresearch Service Get Tag1'),
	(@node8, 'Tresearch Service Get Tag1');
INSERT NodeTags(NodeID, TagName) VALUES
	(@node6, 'Tresearch Service Get Tag2'),
	(@node7, 'Tresearch Service Get Tag2'),
	(@node8, 'Tresearch Service Get Tag2');
INSERT NodeTags(NodeID, TagName) VALUES (@node6, 'Tresearch Service Get Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (@node9, 'Tresearch Service Get Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(@node9, 'Tresearch Service Get Tag1'),
	(@node10, 'Tresearch Service Get Tag2'),
	(@node11, 'Tresearch Service Get Tag3');

INSERT Tags(TagName, TagCount) VALUES 
	('Tresearch Service Create tag2', 0);
INSERT Tags(TagName,TagCount) VALUES 
	('Tresearch Service Remove Me tag2', 0);
INSERT Tags(TagName,TagCount) VALUES 
	('Tresearch Service Remove Me tag3', 0);

INSERT NodeTags(NodeID, TagName) VALUES (@node13, 'Tresearch Service Remove Me tag3');


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ServiceIntegrationTagInitializeProcedure]    
AS
BEGIN
	DECLARE @DAOTagTable TABLE(nodeID BIGINT)
	DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'CECS 491A');
	DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'CECS 491B');
	DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bea194e10097fe79e4d3ff45729d97ac8f8be25aab9f7ceb2303bb367f3ef93c436974a4fb5fea1dd336284e02d05e5ae9b26b2bfb1c81f9250d5cd73e1a20f4' AND  NodeTitle = 'Cooking with thumbs');
	DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'C++');
	DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'Tea');
	DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '08f97d42a09f5e92e4bb33fbcc43a1a9470bfba79bcc69b8b08171c810c6a8880598414116dbee5dc554dc211b3e0f0c81b6fa064d55767bf9f0d5961fb1ff84' AND  NodeTitle = 'Coffee');
	DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'My Node');
	DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Dachin');
	DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Henry The Eigth');
	DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Bloody Mary');
	DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Julius Caeser');
	DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Augustus Caeser');
	DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Mark Antony');
	DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'bb9ac3bc77bcea87226826840635f8dee58cfcecabf4858c36ad10d120696533df6d8fab786765f20f287bcc8a7d0cfc6a0db0347c8715357f888182e1dbd9d5' AND  NodeTitle = 'Octavian');

	INSERT INTO @DAOTagTable VALUES
		(@node0),
		(@node1),
		(@node2),
		(@node3),
		(@node4),
		(@node5),
		(@node6),
		(@node7),
		(@node8),
		(@node9),
		(@node10),
		(@node11),
		(@node12),
		(@node13);
	SELECT * FROM @DAOTagTable
END