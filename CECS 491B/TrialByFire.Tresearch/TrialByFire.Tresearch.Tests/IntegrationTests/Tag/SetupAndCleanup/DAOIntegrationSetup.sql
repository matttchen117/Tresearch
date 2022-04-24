DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO test tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO test tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO This Tag Exists Already';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO This Tag Exists Already';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Me Tag';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Me Tag';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag5';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag5';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag4';

DELETE Nodes WHERE UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501';
DELETE Nodes WHERE UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807';
DELETE Nodes WHERE UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf';

DELETE UserHashTable WHERE UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501';
DELETE UserHashTable WHERE UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807';
DELETE UserHashTable WHERE UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf';

DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould1@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould2@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould3@tresearch.system';

DROP PROCEDURE IF EXISTS DAOIntegrationTagInitializeProcedure;

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



INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501', 'Cooking with thumbs', 'An artform', 1),
	
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 'C++', 'Garbage collection not found', 0),
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 'Tea', 'Black, Earl Gray, Green', 1),
	('1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807', 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 'My Node', 'Important information', 0),
	
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 'Dachin', 'May mean Rome', 1),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf', 'Julius Caeser', 'Not the first emporer', 1);


DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'CECS 491A');
DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'CECS 491B');
DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'Cooking with thumbs');
DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'C++');
DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'Tea');
DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'Coffee');
DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'My Node');
DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Dachin');
DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Henry The Eigth');
DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Bloody Mary');
DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Julius Caeser');


INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch SqlDAO Add Tag1'),
	(@node1, 'Tresearch SqlDAO Add Tag1'),
	(@node2, 'Tresearch SqlDAO Add Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (@node0, 'Tresearch SqlDAO Add Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(@node3, 'Tresearch SqlDAO Delete Tag1'),
	(@node4, 'Tresearch SqlDAO Delete Tag1'),
	(@node5, 'Tresearch SqlDAO Delete Tag1');
INSERT NodeTags(NodeID, TagName) VALUES (@node3, 'Tresearch SqlDAO Delete Tag4');
INSERT NodeTags(NodeID, TagName) VALUES
	(@node7, 'Tresearch SqlDAO Get Tag1'),
	(@node8, 'Tresearch SqlDAO Get Tag1'),
	(@node9, 'Tresearch SqlDAO Get Tag1');
INSERT NodeTags(NodeID, TagName) VALUES
	(@node7, 'Tresearch SqlDAO Get Tag2'),
	(@node8, 'Tresearch SqlDAO Get Tag2'),
	(@node9, 'Tresearch SqlDAO Get Tag2');
INSERT NodeTags(NodeID, TagName) VALUES (@node7, 'Tresearch SqlDAO Get Tag3');
INSERT NodeTags(NodeID, TagName) VALUES (@node10, 'Tresearch SqlDAO Get Tag4');


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DAOIntegrationTagInitializeProcedure]    
AS
BEGIN
	DECLARE @DAOTagTable TABLE(nodeID BIGINT)
	
	DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'CECS 491A');
	DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'CECS 491B');
	DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501' AND  NodeTitle = 'Cooking with thumbs');
	DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'C++');
	DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'Tea');
	DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807' AND  NodeTitle = 'Coffee');
	DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'My Node');
	DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Dachin');
	DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Henry The Eigth');
	DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Bloody Mary');
	DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf' AND  NodeTitle = 'Julius Caeser');

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
		(@node10);

	SELECT * FROM @DAOTagTable
END