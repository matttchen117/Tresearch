DELETE FROM NodeTags Where NodeID = 9019303350;
DELETE FROM NodeTags Where NodeID = 9019303351;
DELETE FROM NodeTags Where NodeID = 9019303352;
DELETE FROM NodeTags Where NodeID = 9019303353;
DELETE FROM NodeTags Where NodeID = 9019303354;
DELETE FROM NodeTags Where NodeID = 9019303355;
DELETE FROM NodeTags Where NodeID = 9019303356;
DELETE FROM NodeTags Where NodeID = 9019303357;
DELETE FROM NodeTags Where NodeID = 9019303358;
DELETE FROM NodeTags Where NodeID = 9019303359;
DELETE FROM NodeTags Where NodeID = 9019303360;
DELETE FROM NodeTags Where NodeID = 9019303361;
DELETE FROM NodeTags Where NodeID = 9019303362;
DELETE FROM NodeTags Where NodeID = 9019303363;

DELETE FROM Nodes Where NodeID = 9019303350;
DELETE FROM Nodes Where NodeID = 9019303351;
DELETE FROM Nodes Where NodeID = 9019303352;
DELETE FROM Nodes Where NodeID = 9019303353;
DELETE FROM Nodes Where NodeID = 9019303354;
DELETE FROM Nodes Where NodeID = 9019303355;
DELETE FROM Nodes Where NodeID = 9019303356;
DELETE FROM Nodes Where NodeID = 9019303357;
DELETE FROM Nodes Where NodeID = 9019303358;
DELETE FROM Nodes Where NodeID = 9019303359;
DELETE FROM Nodes Where NodeID = 9019303360;
DELETE FROM Nodes Where NodeID = 9019303361;
DELETE FROM Nodes Where NodeID = 9019303362;
DELETE FROM Nodes Where NodeID = 9019303363;

DELETE FROM Tags WHERE TagName = 'Tresearch Controller Add Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Add Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Add Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Add Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Add Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Delete Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Delete Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Delete Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Delete Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Delete Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Get Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Get Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Get Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Get Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Get Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Doesnt Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller Tag Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller REMOVE Tag Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Controller REMOVE Exist and Tagged';

DELETE FROM UserHashTable WHERE UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5';
DELETE FROM UserHashTable WHERE UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb';
DELETE FROM UserHashTable WHERE UserHash = '3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee';
DELETE FROM UserHashTable WHERE UserHash = '89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c';
DELETE FROM UserHashTable WHERE UserHash = 'e85986ed95080a27feb37943a125a4a5bb6ceaca81842e1ddd97c28556da5e1ff1e02b08f3f0001510671688176141d5646779d1e5b11432fbe647dc9f6c36ff';
DELETE FROM UserHashTable WHERE UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f';

DELETE FROM Accounts WHERE Username = 'tagControllerIntegration1@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegration2@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegration3@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationNotEnabled@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationNotConfirmed@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationAdmin1@tresearch.system';

Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('tagControllerIntegration1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('tagControllerIntegration2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('tagControllerIntegrationNotEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 1),
			('tagControllerIntegrationNotConfirmed@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 0),
			('tagControllerIntegrationAdmin1@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'admin', 1, 1),
			('tagControllerIntegration3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5'
		FROM Accounts WHERE Username = 'tagControllerIntegration1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb'
		FROM Accounts WHERE Username = 'tagControllerIntegration2@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee'
		FROM Accounts WHERE Username = 'tagControllerIntegrationNotEnabled@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c'
		FROM Accounts WHERE Username = 'tagControllerIntegrationNotConfirmed@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'e85986ed95080a27feb37943a125a4a5bb6ceaca81842e1ddd97c28556da5e1ff1e02b08f3f0001510671688176141d5646779d1e5b11432fbe647dc9f6c36ff'
		FROM Accounts WHERE Username = 'tagControllerIntegrationAdmin1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f'
		FROM Accounts WHERE Username = 'tagControllerIntegration3@tresearch.system';

INSERT Nodes(UserHash, NodeID, NodeParentID, NodeTitle, Summary, Visibility) VALUES 
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 9019303350, null, 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 9019303351, null, 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 9019303352, null, 'Cooking with thumbs', 'An artform', 1),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 9019303353, null, 'C++', 'Garbage collection not found', 0),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 9019303354, null, 'Tea', 'Black, Earl Gray, Green', 1),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 9019303355, null, 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303356, null, 'My Node', 'Important information', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303357, null, 'Dachin', 'May mean Rome', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303358, null, 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303359, null, 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303360, null, 'Julius Caeser', 'Not the first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303361, null, 'Augustus Caeser', 'The first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303362, null, 'Mark Antony', 'Almost the first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 9019303363, null, 'Octavian', 'Also the first emporer', 0);

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Controller Add Tag1', 0),
	('Tresearch Controller Add Tag2', 0),
	('Tresearch Controller Add Tag3', 0),
	('Tresearch Controller Add Tag4', 0);


INSERT NodeTags(NodeID, TagName) VALUES
	(9019303350, 'Tresearch Controller Add Tag1'),
	(9019303351, 'Tresearch Controller Add Tag1'),
	(9019303352, 'Tresearch Controller Add Tag1'),
	(9019303350, 'Tresearch Controller Add Tag3'),
	(9019303351, 'Tresearch Controller Add Tag3');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Controller Delete Tag1', 0),
	('Tresearch Controller Delete Tag2', 0),
	('Tresearch Controller Delete Tag3', 0),
	('Tresearch Controller Delete Tag4', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(9019303350, 'Tresearch Controller Delete Tag1'),
	(9019303351, 'Tresearch Controller Delete Tag1'),
	(9019303352, 'Tresearch Controller Delete Tag1'),
	(9019303350, 'Tresearch Controller Delete Tag3'),
	(9019303351, 'Tresearch Controller Delete Tag3');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Controller Get Tag1', 0),
	('Tresearch Controller Get Tag2', 0),
	('Tresearch Controller Get Tag3', 0),
	('Tresearch Controller Get Tag4', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(9019303356, 'Tresearch Controller Get Tag1'),
	(9019303357, 'Tresearch Controller Get Tag1'),
	(9019303358, 'Tresearch Controller Get Tag1'),
	(9019303356, 'Tresearch Controller Get Tag2'),
	(9019303357, 'Tresearch Controller Get Tag2'),
	(9019303358, 'Tresearch Controller Get Tag2'),
	(9019303358, 'Tresearch Controller Get Tag3'),
	(9019303359, 'Tresearch Controller Get Tag2'),
	(9019303360, 'Tresearch Controller Get Tag1')

INSERT Tags VALUES ('Tresearch Controller Tag Exist', 0);
INSERT Tags VALUES ('Tresearch Controller REMOVE Tag Exist', 0);
INSERT Tags VALUES ('Tresearch Controller REMOVE Exist and Tagged', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(9019303363, 'Tresearch Controller REMOVE Exist and Tagged'),
	(9019303362, 'Tresearch Controller REMOVE Exist and Tagged');