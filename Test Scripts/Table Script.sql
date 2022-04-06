DROP TABLE IF EXISTS EmailRecoveryLinks
DROP TABLE IF EXISTS EmailRecoveryLinksCreated
DROP TABLE IF EXISTS EmailConfirmationLinks
DROP TABLE IF EXISTS TreeHistories
DROP TABLE IF EXISTS DailyRegistrations
DROP TABLE IF EXISTS DailyLogins
DROP TABLE IF EXISTS NodesCreated
DROP TABLE IF EXISTS ViewsWebPages
DROP TABLE IF EXISTS TopSearches
DROP TABLE IF EXISTS UserRatings
DROP TABLE IF EXISTS NodeTags
DROP TABLE IF EXISTS Tags
DROP TABLE IF EXISTS Nodes
DROP TABLE IF EXISTS OTPClaims
DROP TABLE IF EXISTS Accounts
DROP TABLE IF EXISTS UserHashTable
DROP TABLE IF EXISTS Logs

DROP PROCEDURE IF EXISTS GetLogs
DROP PROCEDURE IF EXISTS DeleteLogs
DROP PROCEDURE IF EXISTS AddTagToNode
DROP PROCEDURE IF EXISTS ConfirmAccount
DROP PROCEDURE IF EXISTS CreateAccount
DROP PROCEDURE IF EXISTS CreateConfirmationLink	
DROP PROCEDURE IF EXISTS CreateNode
DROP PROCEDURE IF EXISTS CreateRecoveryLink
DROP PROCEDURE IF EXISTS CreateTag
DROP PROCEDURE IF EXISTS CreateUserHash
DROP PROCEDURE IF EXISTS DecrementRecoveryLinksCreated
DROP PROCEDURE IF EXISTS DecrementTagCount
DROP PROCEDURE IF EXISTS DeleteAccountStoredProcedure
DROP PROCEDURE IF EXISTS DisableAccount
DROP PROCEDURE IF EXISTS EnableAccount
DROP PROCEDURE IF EXISTS GetAccount
DROP PROCEDURE IF EXISTS GetAmountOfAdmins
DROP PROCEDURE IF EXISTS GetConfirmationLink
DROP PROCEDURE IF EXISTS GetNodeTags
DROP PROCEDURE IF EXISTS GetNodeTagsDesc
DROP PROCEDURE IF EXISTS GetRecoveryLink
DROP PROCEDURE IF EXISTS GetTags
DROP PROCEDURE IF EXISTS GetTagsDesc
DROP PROCEDURE IF EXISTS GetTagNames
DROP PROCEDURE IF EXISTS IncrementRecoveryLinksCreated
DROP PROCEDURE IF EXISTS IncrementTagCount
DROP PROCEDURE IF EXISTS IsAuthorizedNodeChanges
DROP PROCEDURE IF EXISTS RemoveConfirmationLink
DROP PROCEDURE IF EXISTS RemoveRecoveryLink
DROP PROCEDURE IF EXISTS RemoveTag
DROP PROCEDURE IF EXISTS RemoveTagFromNode
DROP PROCEDURE IF EXISTS UnconfirmAccount

CREATE TABLE UserHashTable(
	UserID VARCHAR(100),
	UserRole VARCHAR(40),
	UserHash VARCHAR(128),
	CONSTRAINT user_hashtable_pk PRIMARY KEY(UserHash)
);
 
CREATE TABLE Accounts(
    Username VARCHAR(100),
    Passphrase VARCHAR(128),
    AuthorizationLevel VARCHAR(40),
    AccountStatus BIT,
    Confirmed BIT,
	Token VARCHAR(64) NULL,
	CONSTRAINT user_account_pk PRIMARY KEY(Username, AuthorizationLevel),
	CONSTRAINT user_account_hash_fk  FOREIGN KEY(Username, AuthorizationLevel) REFERENCES UserHashTable(UserID, UserRole)
);

CREATE TABLE OTPClaims(
	Username VARCHAR(100),
	OTP VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	TimeCreated DATETIME,
	FailCount INT,

	CONSTRAINT otp_claims_fk_01 FOREIGN KEY (Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel),
	CONSTRAINT otp_claims_pk PRIMARY KEY(Username, AuthorizationLevel)
);

CREATE TABLE Nodes(
	UserHash VARCHAR(128),
	NodeID BIGINT PRIMARY KEY,
	NodeParentID BIGINT,
	NodeTitle VARCHAR(100),
	Summary VARCHAR(750),
	Visibility BIT,
	CONSTRAINT node_owner_fk FOREIGN KEY(UserHash) REFERENCES UserHashTable(UserHash)
);

CREATE TABLE Tags(
	TagName VARCHAR(100) PRIMARY KEY,
	TagCount BIGINT
);

CREATE TABLE NodeTags(
    TagName VARCHAR(100),
    NodeID BIGINT,
	CONSTRAINT tag_name_fk FOREIGN KEY (TagName) REFERENCES Tags(TagName),
	CONSTRAINT node_tags_pk PRIMARY KEY(NodeID, TagName),
	CONSTRAINT node_id_fk FOREIGN KEY (NodeID) REFERENCES Nodes(NodeID)
);

CREATE TABLE UserRatings(
	UserHash VARCHAR(128)
	NodeID BIGINT,
	Rating INT,
	CONSTRAINT user_ratings_fk FOREIGN KEY (UserHash) REFERENCES UserHashTable(UserHash),
	CONSTRAINT user_ratings_node_fk FOREIGN KEY (NodeID) REFERENCES Nodes(NodeID),
	CONSTRAINT user_ratings_pk PRIMARY KEY(UserHash, NodeID)
);

--shouldn�t you combine both EditDate and EditTime into one attribute so the datatype can be DateTime
CREATE TABLE TreeHistories(
	EditDate DATE,
	EditTime TIME,
);

CREATE TABLE ViewsWebPages(
	ViewName VARCHAR(100),
	Visits INT,
	AverageDuration FLOAT
);

-- Keep number of registrations on a given date
CREATE TABLE DailyRegistrations(
	RegistrationDate DATE PRIMARY KEY,
	RegistrationCount INT
);

-- Keep all of the logins for a given date
CREATE TABLE DailyLogins(
	LoginDate DATE PRIMARY KEY,
	LoginCount INT
);

CREATE TABLE TopSearches(
	TopSearchDate DATE PRIMARY KEY,
	SearchString VARCHAR(100),
	SearchCount INT
);

CREATE TABLE NodesCreated(
	NodeCreationDate DATE PRIMARY KEY,
	NodeCreationTime INT,
);

CREATE TABLE EmailConfirmationLinks(
	Username VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	GuidLink UNIQUEIDENTIFIER PRIMARY KEY,
	TimeCreated DateTime,
	CONSTRAINT confirmation_links_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel)
);

CREATE TABLE EmailRecoveryLinks(
	Username VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	GuidLink UNIQUEIDENTIFIER PRIMARY KEY,
	TimeCreated DateTime,
	CONSTRAINT recovery_links_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel),
);

CREATE TABLE EmailRecoveryLinksCreated(
	Username VARCHAR(128),
	AuthorizationLevel VARCHAR(40),
	LinkCount int,
	CONSTRAINT recovery_count_pk PRIMARY KEY(Username, AuthorizationLevel),
	CONSTRAINT recovery_count_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel)
);

CREATE TABLE Logs(
	Timestamp DATETIME,
	Level VARCHAR(30),
	UserHash VARCHAR(128),
	Category VARCHAR(30),
	Description TEXT,
	CONSTRAINT logs_fk FOREIGN KEY (UserHash) REFERENCES UserHashTable(UserHash)
);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/19/22
-- Description:	Grabs all logs older than the specified DateTime
-- =============================================
CREATE PROCEDURE GetLogs 
	-- Add the parameters for the stored procedure here
	@Timestamp DateTime = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Logs
	WHERE Timestamp <= @Timestamp

	RETURN;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/19/2022
-- Description:	Deletes all logs older than the given DateTime
-- =============================================
CREATE PROCEDURE DeleteLogs 
	-- Add the parameters for the stored procedure here
	@Timestamp DateTime = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE
	FROM Logs
	WHERE Timestamp <= @Timestamp

	RETURN;
END
GO

-- =============================================
-- Author:		Pammy Poor
-- Description:	Adds tag to node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[AddTagToNode]    
(
	@NodeID BIGINT,
	@TagName VARCHAR(100)
)
as
begin
	IF NOT EXISTS(SELECT * FROM NodeTags WHERE NodeID = @NodeID AND TagName = @TagName)
	BEGIN
		INSERT INTO NodeTags(Nodeid, TagName) VALUES(@NodeID, @TagName);
	END
end


-- =============================================
-- Author:		Pammy Poor
-- Description:	Changes confirmed status to true
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConfirmAccount]    
(
	@Username VARCHAR(128),
	@AuthorizationLevel VARCHAR(40)
)
as
begin
	UPDATE Accounts SET Confirmed = 1 WHERE (Username = @Username AND AuthorizationLevel = @AuthorizationLevel)
end


-- =============================================
-- Author:		Pammy Poor
-- Description:	Creates an account 
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateAccount]    
(
	@Username VARCHAR(100),
	@Passphrase VARCHAR(128),
	@AuthorizationLevel VARCHAR(40),
	@AccountStatus bit,
	@Confirmed bit
)
as
begin
	INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES(@Username, @Passphrase, @AuthorizationLevel, @AccountStatus, @Confirmed);
end


-- =============================================
-- Author:		Pammy Poor
-- Description:	Creates a confirmation link 
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateConfirmationLink]    
(
	@Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40),
	@GUIDLink UNIQUEIDENTIFIER,
	@TimeCreated DATETIME
)
as
begin
Insert into EmailConfirmationLinks(Username, GUIDLink, AuthorizationLevel, TimeCreated) values (@Username, @GUIDLink, @AuthorizationLevel, @TimeCreated)
end

-- =============================================
--Author:        Jessie Lazo
-- Description:    Creates a Node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateNode]
(
    @Username VARCHAR(100),
    @AuthorizationLevel VARCHAR(40),
    @NodeID BIGINT,
    @NodeParentID BIGINT,
    @NodeTitle VARCHAR(100),
    @Summary VARCHAR(750),
    @Visibility BIT
)
as
begin
    INSERT INTO Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
         VALUES(@Username, @AuthorizationLevel, @NodeID, @NodeParentID, @NodeTitle, @Summary, @Visibility);
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Creates a user id 
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUserHash]    
(
	@UserID VARCHAR(100),
	@UserRole VARCHAR(40),
	@UserHash VARCHAR(128)
)
as
begin
	IF EXISTS( SELECT * FROM UserHashTable with (updlock, serializable) WHERE UserHash = @UserHash)
		BEGIN
			UPDATE UserHashTable SET UserID = @UserID WHERE UserHash = @UserHash;
			UPDATE UserHashTable SET UserRole = @UserRole WHERE UserHash = @UserHash;
		END
	ELSE
		BEGIN
			INSERT INTO UserHashTable(UserID, UserRole, UserHash) VALUES(@UserID, @UserRole, @UserHash);
		END
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Creates a recovery link
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CreateRecoveryLink]
(
	@Username VARCHAR(100),
	@GUIDLink UNIQUEIDENTIFIER,
	@TimeCreated DATETIME,
	@AuthorizationLevel VARCHAR(40)
)
as
begin
Insert into EmailRecoveryLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel) values (@Username, @GUIDLink, @TimeCreated, @AuthorizationLevel)
end


-- =============================================
-- Author:		Pammy Poor
-- Description:	Creates a user tag
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateTag]    
(
	@TagName VARCHAR(100)
	@TagCount BIGINT 
)
as
begin
	INSERT Tags(TagName, TagCount) VALUES (@TagName, @TagCount)
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Decrements amount of recovery links user has created
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DecrementRecoveryLinksCreated]
	@Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40)

as
begin
	UPDATE EmailRecoveryLinksCreated SET LinkCount = LinkCount - 1 WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel 
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Decrements amount of recovery links user has created
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DecrementTagCount]
	@TagName VARCHAR(100)

as
begin
	UPDATE Tags SET TagCount = TagCount - 1 WHERE TagName = @TagName
end

-- =============================================
-- Author:		Viet Nguyen
-- Description:	Deletes a user and relevant data
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAccountStoredProcedure]
@Username VARCHAR(100), @AuthorizationLevel VARCHAR(40)
AS 
BEGIN 
	DELETE FROM NodeTags WHERE NodeTags.NodeID IN 
    (SELECT NodeTags.NodeID
    FROM Nodes  
    INNER JOIN NodeTags ON NodeTags.NodeID = Nodes.NodeID
    WHERE Nodes.Username = @Username AND Nodes.AuthorizationLevel = @AuthorizationLevel);

    DELETE FROM UserRatings 
    WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;

    DELETE FROM EmailConfirmationLinks     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM EmailRecoveryLinks     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM EmailConfirmationLinksCreated     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM EmailRecoveryLinksCreated     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;

    DELETE FROM Nodes WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;

    DELETE FROM OTPClaims WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;

    DELETE FROM Accounts WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Updates users status to false
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[DisableAccount]
(
	@Username Varchar(100),
	@AuthorizationLevel VARCHAR(40)
)
AS
BEGIN
	UPDATE Accounts SET AccountStatus = 0 Where Username = @Username AND AuthorizationLevel = @AuthorizationLevel;
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Updates users status to True
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[EnableAccount] 
(
	@Username VARCHAR(100), 
	@AuthorizationLevel VARCHAR(40)
)
AS
BEGIN
	UPDATE Accounts SET AccountStatus = 1 WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel;
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns user account matching credentials
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetAccount] @Username VARCHAR(100), @AuthorizationLevel VARCHAR(40)
as
BEGIN
SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel
END

-- =============================================
-- Author:		Viet Nguyen
-- Description:	Gets amount of users with admin role
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAmountOfAdmins] 
AS
BEGIN 
	SELECT * FROM Accounts WHERE AuthorizationLevel = 'admin';
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns confirmation link based on guid passed in
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetConfirmationLink]
	@GUIDLink VARCHAR(36)
as
begin
	SELECT * FROM EmailConfirmationLinks where GUIDLink = (Convert(uniqueidentifier,@GUIDLink));
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns list of tags a node has tagged
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNodeTags]    
(
	@NodeID BIGINT
)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM Nodes where Nodeid = @NodeID)
		BEGIN
			SELECT '-1 invalid'
		END
	ELSE
		BEGIN
			SELECT TagName FROM NodeTags where Nodeid = @NodeID
		END
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns list of tags a node has tagged
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNodeTagsDesc]    
(
	@NodeID BIGINT
)
as
begin
	IF NOT EXISTS(SELECT * FROM Nodes where Nodeid = @NodeID)
		BEGIN
			SELECT '-1 invalid'
		END
	ELSE
		BEGIN
			SELECT TagName FROM NodeTags where Nodeid = @NodeID ORDER BY TagName DESC
		END
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns recovery link based on guid passed in
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetRecoveryLink]
	@GUIDLink uniqueidentifier
as
begin
	SELECT * FROM EmailRecoveryLinks where GUIDLink = @GUIDLink;
end


-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns tag bank
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTags]    
as
begin
	SELECT * FROM Tags
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns tag bank
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTagsDesc]    
as
begin
	SELECT * FROM Tags ORDER BY TagName DESC
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Returns tag bank
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTagNames]    
as
begin
	SELECT TagName FROM Tags
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Increments number of recovers if exists
-- =============================================
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[IncrementRecoveryLinksCreated]
	@Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40)

as
begin
	if exists(
		SELECT * FROM EmailRecoveryLinksCreated with (updlock, serializable)
		WHERE Username = @Username AND AuthorizationLevel =@AuthorizationLevel
		)
		BEGIN 
			UPDATE EmailRecoveryLinksCreated SET LinkCount = LinkCount + 1 WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel 
		END
	else
		begin
			INSERT EmailRecoveryLinksCreated (Username, AuthorizationLevel, LinkCount) VALUES(@Username, @AuthorizationLevel, 1)
		end
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Decrements amount of recovery links user has created
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[IncrementTagCount]
	@TagName VARCHAR(100)
as
begin
	UPDATE Tags SET TagCount = TagCount + 1 WHERE TagName = @TagName
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Removes confirmation link
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveConfirmationLink]    
(
	@GUIDLink UNIQUEIDENTIFIER
)
as
begin
	  DELETE FROM EmailConfirmationLinks WHERE GUIDLink = @GUIDLink;
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Checks if user is authorized to make changes to a node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsAuthorizedNodeChanges]
(
	@NodeID BIGINT,
	@UserHash VARCHAR(128)
)
AS
BEGIN
	  SELECT CASE WHEN EXISTS ( SELECT * FROM Nodes WHERE UserHash = @UserHash AND NodeID = @NodeID)
	  THEN CAST(1 AS BIT)
	  ELSE CAST(0 AS BIT)
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Removes recovery link
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveRecoveryLink]
(
	@GUIDLink VARCHAR(36)
)
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM EmailRecoveryLinks where GUIDLink = (Convert(uniqueidentifier, @GUIDLink));
END

-- =============================================
-- Author:		Pammy Poor
-- Description:	Removes tag from tag bank
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveTag]    
(
	@TagName VARCHAR(100)
)
as
begin
	IF EXISTS(SELECT * FROM Tags WHERE TagName = @TagName)
		BEGIN
			DELETE NodeTags WHERE TagName = @TagName
			DELETE Tags WHERE TagName = @TagName
		END
	
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Removes tag from node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveTagFromNode]    
(
	@NodeID BIGINT,
	@TagName VARCHAR(100)
)
as
begin
	DELETE FROM NodeTags where NodeID = @NodeID AND TagName = @TagName;
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Updates confirmation status to false 
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UnconfirmAccount]    
(
	@Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40)
)
as
begin
	UPDATE Accounts SET Confirmed = 0 WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel
end