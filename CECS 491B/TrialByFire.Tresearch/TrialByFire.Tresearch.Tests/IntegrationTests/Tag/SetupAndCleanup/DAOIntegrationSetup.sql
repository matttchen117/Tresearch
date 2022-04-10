Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('tresearchTagServiceSQLDAOshould1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('tresearchTagServiceSQLDAOshould2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('tresearchTagServiceSQLDAOshould3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501'
		FROM Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould1@tresearch.system';

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807'
		FROM Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould2@tresearch.system';

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf'
		FROM Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould3@tresearch.system';

INSERT Tags(TagName, TagCount) VALUES ('Tresearch SqlDAO This Tag Exists Already', 0);
INSERT Tags(TagName, TagCount) VALUES ('Tresearch SqlDAO Delete Me Tag', 22);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Add Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Add Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Add Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Add Tag4', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Delete Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Delete Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Delete Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Delete Tag4', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Get Tag1', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Get Tag2', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Get Tag3', 0);
INSERT Tags(TagName, TagCount) Values ('Tresearch SqlDAO Get Tag4', 0);

INSERT Nodes(UserHash, NodeID, NodeParentID, NodeTitle, Summary, Visibility) VALUES 
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 2022030533, null, 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 2022030534, null, 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 2022030535, null, 'Cooking with thumbs', 'An artform', 1),
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 2022030536, null, 'C++', 'Garbage collection not found', 0),
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 2022030537, null, 'Tea', 'Black, Earl Gray, Green', 1),
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 2022030538, null, 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 2022030539, null, 'My Node', 'Important information', 0),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 2022030540, null, 'Dachin', 'May mean Rome', 1),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 2022030541, null, 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 2022030542, null, 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 2022030543, null, 'Julius Caeser', 'Not the first emporer', 1);

INSERT NodeTags(NodeID, TagName) VALUES
	(2022030533, 'Tresearch SqlDAO Add Tag1'),
	(2022030534, 'Tresearch SqlDAO Add Tag1'),
	(2022030535, 'Tresearch SqlDAO Add Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (2022030533, 'Tresearch SqlDAO Add Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(2022030536, 'Tresearch SqlDAO Delete Tag1'),
	(2022030537, 'Tresearch SqlDAO Delete Tag1'),
	(2022030538, 'Tresearch SqlDAO Delete Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (2022030536, 'Tresearch SqlDAO Delete Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(2022030539, 'Tresearch SqlDAO Get Tag1'),
	(2022030540, 'Tresearch SqlDAO Get Tag1'),
	(2022030541, 'Tresearch SqlDAO Get Tag1');
INSERT NodeTags(NodeID, TagName) VALUES
	(2022030539, 'Tresearch SqlDAO Get Tag2'),
	(2022030540, 'Tresearch SqlDAO Get Tag2'),
	(2022030541, 'Tresearch SqlDAO Get Tag2');
INSERT NodeTags(NodeID, TagName) VALUES (2022030539, 'Tresearch SqlDAO Get Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (2022030542, 'Tresearch SqlDAO Get Tag4');