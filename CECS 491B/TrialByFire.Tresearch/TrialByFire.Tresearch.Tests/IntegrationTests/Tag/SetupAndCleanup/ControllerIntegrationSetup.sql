DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Add Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Add Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Add Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Add Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Add Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Delete Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Delete Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Delete Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Delete Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Delete Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Get Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Get Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Get Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Get Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Get Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Doesnt Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller Tag Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller REMOVE Tag Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Controller REMOVE Exist and Tagged';

DELETE FROM Nodes WHERE UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5';
DELETE FROM Nodes WHERE UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb';
DELETE FROM Nodes WHERE UserHash = '3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee';
DELETE FROM Nodes WHERE UserHash = '89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c';
DELETE FROM Nodes WHERE UserHash = 'e85986ed95080a27feb37943a125a4a5bb6ceaca81842e1ddd97c28556da5e1ff1e02b08f3f0001510671688176141d5646779d1e5b11432fbe647dc9f6c36ff';
DELETE FROM Nodes WHERE UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f';

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

UPDATE  UserHashTable SET UserID = null WHERE UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5';
UPDATE  UserHashTable SET UserID = null WHERE UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb';
UPDATE  UserHashTable SET UserID = null WHERE UserHash = '3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee';
UPDATE  UserHashTable SET UserID = null WHERE UserHash = '89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c';
UPDATE  UserHashTable SET UserID = null WHERE UserHash = 'e85986ed95080a27feb37943a125a4a5bb6ceaca81842e1ddd97c28556da5e1ff1e02b08f3f0001510671688176141d5646779d1e5b11432fbe647dc9f6c36ff';
UPDATE  UserHashTable SET UserID = null WHERE UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f';

DELETE FROM Accounts WHERE Username = 'tagControllerIntegration1@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegration2@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegration3@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationNotEnabled@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationNotConfirmed@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagControllerIntegrationAdmin1@tresearch.system';

DROP PROCEDURE IF EXISTS ControllerIntegrationTagInitializeProcedure;

Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('tagControllerIntegration1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('tagControllerIntegration2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('tagControllerIntegrationNotEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 1),
			('tagControllerIntegrationNotConfirmed@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 0),
			('tagControllerIntegrationAdmin1@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'admin', 1, 1),
			('tagControllerIntegration3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

DECLARE @User1 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegration1@tresearch.system');
DECLARE @User2 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegration2@tresearch.system');
DECLARE @User3 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegrationNotEnabled@tresearch.system');
DECLARE @User4 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegrationNotConfirmed@tresearch.system');
DECLARE @User5 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegrationAdmin1@tresearch.system');
DECLARE @User6 INT = (SELECT UserID from Accounts Where Username = 'tagControllerIntegration3@tresearch.system');

EXEC CreateUserHash @User1, '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5';
EXEC CreateUserHash @User2, '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb';
EXEC CreateUserHash @User3, '3e5c76fdaaa3dbdc12ecf59f01028284632d7a5289656eede6680c582a9e71eb082dafe0fb99411e6a220f4c9b1937a7e8d9317b3a0006051265590a166043ee';
EXEC CreateUserHash @User4, '89ff4ea1982c9a201348d5ad6522ab72dc81084199596fdc7790c670a79bf86b3312c2d521ec6b7dc73b2eaa0698e54c17dddf47ecd6ef0b1f54f1b68552ca9c';
EXEC CreateUserHash @User5, 'e85986ed95080a27feb37943a125a4a5bb6ceaca81842e1ddd97c28556da5e1ff1e02b08f3f0001510671688176141d5646779d1e5b11432fbe647dc9f6c36ff';
EXEC CreateUserHash @User6, '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f';

INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5', 'Cooking with thumbs', 'An artform', 1),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 'C++', 'Garbage collection not found', 0),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 'Tea', 'Black, Earl Gray, Green', 1),
	('20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb', 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'My Node', 'Important information', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Dachin', 'May mean Rome', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Julius Caeser', 'Not the first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Augustus Caeser', 'The first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Mark Antony', 'Almost the first emporer', 1),
	('0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f', 'Octavian', 'Also the first emporer', 0);

DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'CECS 491A');
DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'CECS 491B');
DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'Cooking with thumbs');
DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'C++');
DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'Tea');
DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'Coffee');
DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'My Node');
DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Dachin');
DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Henry The Eigth');
DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Bloody Mary');
DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Julius Caeser');
DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Augustus Caeser');
DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Mark Antony');
DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Octavian');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Controller Add Tag1', 0),
	('Tresearch Controller Add Tag2', 0),
	('Tresearch Controller Add Tag3', 0),
	('Tresearch Controller Add Tag4', 0),
	('Tresearch Controller Get Tag1', 0),
	('Tresearch Controller Get Tag2', 0),
	('Tresearch Controller Get Tag3', 0),
	('Tresearch Controller Get Tag4', 0),
	('Tresearch Controller Delete Tag1', 0),
	('Tresearch Controller Delete Tag2', 0),
	('Tresearch Controller Delete Tag3', 0),
	('Tresearch Controller Delete Tag4', 0),
	('Tresearch Controller Tag Exist', 0),
	('Tresearch Controller REMOVE Tag Exist', 0),
	('Tresearch Controller REMOVE Exist and Tagged', 0);


INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch Controller Add Tag1'),
	(@node1, 'Tresearch Controller Add Tag1'),
	(@node2, 'Tresearch Controller Add Tag1'),
	(@node0, 'Tresearch Controller Add Tag3'),
	(@node1, 'Tresearch Controller Add Tag3');
	

INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch Controller Delete Tag1'),
	(@node1, 'Tresearch Controller Delete Tag1'),
	(@node2, 'Tresearch Controller Delete Tag1'),
	(@node0, 'Tresearch Controller Delete Tag3'),
	(@node1, 'Tresearch Controller Delete Tag3');
	

INSERT NodeTags(NodeID, TagName) VALUES
	(@node6, 'Tresearch Controller Get Tag1'),
	(@node7, 'Tresearch Controller Get Tag1'),
	(@node8, 'Tresearch Controller Get Tag1'),
	(@node6, 'Tresearch Controller Get Tag2'),
	(@node7, 'Tresearch Controller Get Tag2'),
	(@node8, 'Tresearch Controller Get Tag2'),
	(@node8, 'Tresearch Controller Get Tag3'),
	(@node9, 'Tresearch Controller Get Tag2'),
	(@node10, 'Tresearch Controller Get Tag1')

INSERT NodeTags(NodeID, TagName) VALUES
	(@node13, 'Tresearch Controller REMOVE Exist and Tagged'),
	(@node12, 'Tresearch Controller REMOVE Exist and Tagged');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ControllerIntegrationTagInitializeProcedure]    
AS
BEGIN
	DECLARE @DAOTagTable TABLE(nodeID BIGINT)
	DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'CECS 491A');
	DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'CECS 491B');
	DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '09bdb27005ebc8c2f3894957ece9703d2d2c7b848d5175da7181af2841e35be54708d3faf6b16e7ee29eef8bb71e2debebc619401a118849435368da610c20f5' AND  NodeTitle = 'Cooking with thumbs');
	DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'C++');
	DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'Tea');
	DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '20b5738a239a937e6e04375836610a07f6380581bd295ea57b9da041981527c832aaffdb0f67dc9dc4d31754e3faa4bf486079076e9340e96d14310c654a17bb' AND  NodeTitle = 'Coffee');
	DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'My Node');
	DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Dachin');
	DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Henry The Eigth');
	DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Bloody Mary');
	DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Julius Caeser');
	DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Augustus Caeser');
	DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Mark Antony');
	DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '0e6ed0cb983d0dd8cf8d96ae9ea44fb5d11659cba04b7e6ec120334f8f5315350bf66a9a981b3d68ac7f0c4425b855feb97df11d64883cca0f8ffd242deb7b4f' AND  NodeTitle = 'Octavian');

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