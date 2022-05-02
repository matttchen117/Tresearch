DELETE FROM NodeRatings Where UserHash = 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834';
DELETE FROM NodeRatings Where UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614';

DELETE FROM Nodes Where UserHash = 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834';
DELETE FROM Nodes Where UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614';

DELETE FROM UserHashTable WHERE UserHash = 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834';
DELETE FROM UserHashTable WHERE UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614';

DELETE From Accounts WHERE Username = 'RateDAOUser1@tresearch.system';
DELETE From Accounts WHERE Username = 'RateAnotherUserDAO1@tresearch.system';


DROP PROCEDURE IF EXISTS DAOIntegrationRateInitializeProcedure;


Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('RateDAOUser1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('RateAnotherUserDAO1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834'
		FROM Accounts WHERE Username = 'RateDAOUser1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614'
		FROM Accounts WHERE Username = 'RateAnotherUserDAO1@tresearch.system';

INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614', 'CECS 491A', 'Extremely hard class at CSULB', 0);
INSERT Nodes(UserHash, NodeTitle, Summary, Visibility) VALUES 
	('a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834', 'CECS 491A', 'Extremely hard class at CSULB', 0);

DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614' AND  NodeTitle = 'CECS 491A');
DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834' AND  NodeTitle = 'CECS 491A');

EXEC RateNode @UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614', @NodeID = @node1, @Rating = 1;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DAOIntegrationRateInitializeProcedure]    
AS
BEGIN
	DECLARE @DAOTagTable TABLE(nodeID BIGINT)
	
	DECLARE @node0 BIGINT = (SELECT NodeID FROM Nodes where UserHash = '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614' AND  NodeTitle = 'CECS 491A');
	DECLARE @node1 BIGINT = (SELECT NodeID FROM Nodes where UserHash = 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834' AND  NodeTitle = 'CECS 491A');


	INSERT INTO @DAOTagTable VALUES
		(@node0),
		(@node1);

	SELECT * FROM @DAOTagTable
END