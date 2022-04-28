DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Add Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Add Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Add Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Add Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Add Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Delete Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Delete Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Delete Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Delete Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Delete Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Get Tag1';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Get Tag2';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Get Tag3';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Get Tag4';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Get Tag5';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Doesnt Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager Tag Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager REMOVE Tag Exist';
DELETE FROM NodeTags WHERE TagName = 'Tresearch Manager REMOVE Exist and Tagged';

DELETE FROM Nodes WHERE UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648';
DELETE FROM Nodes WHERE UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89';
DELETE FROM Nodes WHERE UserHash = '820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b';
DELETE FROM Nodes WHERE UserHash = 'e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3';
DELETE FROM Nodes WHERE UserHash = '68bb308dca0f82d611a12327f806fdf7a636ff7c93e29757b8942fd068abaf63631d65ce598df6b77b787b5a9fb8ab8500db6789f3d6b9abd4d98e399cccd8d0';
DELETE FROM Nodes WHERE UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b';

DELETE FROM Tags WHERE TagName = 'Tresearch Manager Add Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Add Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Add Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Add Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Add Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Delete Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Delete Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Delete Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Delete Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Delete Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Get Tag1';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Get Tag2';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Get Tag3';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Get Tag4';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Get Tag5';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Doesnt Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager Tag Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager REMOVE Tag Exist';
DELETE FROM Tags WHERE TagName = 'Tresearch Manager REMOVE Exist and Tagged';

DELETE FROM UserHashTable WHERE UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648';
DELETE FROM UserHashTable WHERE UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89';
DELETE FROM UserHashTable WHERE UserHash = '820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b';
DELETE FROM UserHashTable WHERE UserHash = 'e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3';
DELETE FROM UserHashTable WHERE UserHash = '68bb308dca0f82d611a12327f806fdf7a636ff7c93e29757b8942fd068abaf63631d65ce598df6b77b787b5a9fb8ab8500db6789f3d6b9abd4d98e399cccd8d0';
DELETE FROM UserHashTable WHERE UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b';

DELETE FROM Accounts WHERE Username = 'tagManagerIntegration1@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagManagerIntegration2@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagManagerIntegration3@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagManagerIntegrationNotEnabled@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagManagerIntegrationNotConfirmed@tresearch.system';
DELETE FROM Accounts WHERE Username = 'tagManagerIntegrationAdmin1@tresearch.system';

DROP PROCEDURE IF EXISTS ManagerIntegrationTagInitializeProcedure;

Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('tagManagerIntegration1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('tagManagerIntegration2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('tagManagerIntegrationNotEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 1),
			('tagManagerIntegrationNotConfirmed@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 0),
			('tagManagerIntegrationAdmin1@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'admin', 1, 1),
			('tagManagerIntegration3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648'
		FROM Accounts WHERE Username = 'tagManagerIntegration1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89'
		FROM Accounts WHERE Username = 'tagManagerIntegration2@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b'
		FROM Accounts WHERE Username = 'tagManagerIntegrationNotEnabled@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3'
		FROM Accounts WHERE Username = 'tagManagerIntegrationNotConfirmed@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '68bb308dca0f82d611a12327f806fdf7a636ff7c93e29757b8942fd068abaf63631d65ce598df6b77b787b5a9fb8ab8500db6789f3d6b9abd4d98e399cccd8d0'
		FROM Accounts WHERE Username = 'tagManagerIntegrationAdmin1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b'
		FROM Accounts WHERE Username = 'tagManagerIntegration3@tresearch.system';

INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648', 'CECS 491A', 'Extremely hard class at CSULB', 0),
	('d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648', 'CECS 491B', 'Extremely easy class at CSULB', 1),
	('d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648', 'Cooking with thumbs', 'An artform', 1),
	('f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89', 'C++', 'Garbage collection not found', 0),
	('f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89', 'Tea', 'Black, Earl Gray, Green', 1),
	('f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89', 'Coffee', 'Bean, Bean Bean, Bean', 0),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'My Node', 'Important information', 0),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Dachin', 'May mean Rome', 1),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Henry The Eigth', 'Mary and Elizabeth', 0),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Bloody Mary', 'Is she a queen or a drink?', 1),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Julius Caeser', 'Not the first emporer', 1),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Augustus Caeser', 'The first emporer', 1),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Mark Antony', 'Almost the first emporer', 1),
	('571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b', 'Octavian', 'Also the first emporer', 0);


DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'CECS 491A');
DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'CECS 491B');
DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'Cooking with thumbs');
DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'C++');
DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'Tea');
DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'Coffee');
DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'My Node');
DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Dachin');
DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Henry The Eigth');
DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Bloody Mary');
DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Julius Caeser');
DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Augustus Caeser');
DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Mark Antony');
DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Octavian');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Manager Add Tag1', 0),
	('Tresearch Manager Add Tag2', 0),
	('Tresearch Manager Add Tag3', 0),
	('Tresearch Manager Add Tag4', 0);


INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch Manager Add Tag1'),
	(@node1, 'Tresearch Manager Add Tag1'),
	(@node2, 'Tresearch Manager Add Tag1'),
	(@node0, 'Tresearch Manager Add Tag3'),
	(@node1, 'Tresearch Manager Add Tag3');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Manager Delete Tag1', 0),
	('Tresearch Manager Delete Tag2', 0),
	('Tresearch Manager Delete Tag3', 0),
	('Tresearch Manager Delete Tag4', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(@node0, 'Tresearch Manager Delete Tag1'),
	(@node1, 'Tresearch Manager Delete Tag1'),
	(@node2, 'Tresearch Manager Delete Tag1'),
	(@node0, 'Tresearch Manager Delete Tag3'),
	(@node1, 'Tresearch Manager Delete Tag3');

INSERT Tags (TagName, TagCount) VALUES
	('Tresearch Manager Get Tag1', 0),
	('Tresearch Manager Get Tag2', 0),
	('Tresearch Manager Get Tag3', 0),
	('Tresearch Manager Get Tag4', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(@node6, 'Tresearch Manager Get Tag1'),
	(@node7, 'Tresearch Manager Get Tag1'),
	(@node8, 'Tresearch Manager Get Tag1'),
	(@node6, 'Tresearch Manager Get Tag2'),
	(@node7, 'Tresearch Manager Get Tag2'),
	(@node8, 'Tresearch Manager Get Tag2'),
	(@node8, 'Tresearch Manager Get Tag3'),
	(@node9, 'Tresearch Manager Get Tag2'),
	(@node10, 'Tresearch Manager Get Tag1')

INSERT Tags VALUES ('Tresearch Manager Tag Exist', 0);
INSERT Tags VALUES ('Tresearch Manager REMOVE Tag Exist', 0);
INSERT Tags VALUES ('Tresearch Manager REMOVE Exist and Tagged', 0);

INSERT NodeTags(NodeID, TagName) VALUES
	(@node13, 'Tresearch Manager REMOVE Exist and Tagged'),
	(@node12, 'Tresearch Manager REMOVE Exist and Tagged');


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ManagerIntegrationTagInitializeProcedure]    
AS
BEGIN
	DECLARE @DAOTagTable TABLE(nodeID BIGINT)
	DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'CECS 491A');
	DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'CECS 491B');
	DECLARE @node2 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'd9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648' AND  NodeTitle = 'Cooking with thumbs');
	DECLARE @node3 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'C++');
	DECLARE @node4 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'Tea');
	DECLARE @node5 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89' AND  NodeTitle = 'Coffee');
	DECLARE @node6 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'My Node');
	DECLARE @node7 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Dachin');
	DECLARE @node8 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Henry The Eigth');
	DECLARE @node9 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Bloody Mary');
	DECLARE @node10 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Julius Caeser');
	DECLARE @node11 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Augustus Caeser');
	DECLARE @node12 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Mark Antony');
	DECLARE @node13 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b' AND  NodeTitle = 'Octavian');


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