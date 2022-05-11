DROP TABLE IF EXISTS EmailRecoveryLinks
DROP TABLE IF EXISTS EmailRecoveryLinksCreated
DROP TABLE IF EXISTS EmailConfirmationLinks
DROP TABLE IF EXISTS TreeHistories
DROP TABLE IF EXISTS DailyRegistrations
DROP TABLE IF EXISTS DailyLogins
DROP TABLE IF EXISTS NodesCreated
DROP TABLE IF EXISTS ViewsWebPages
DROP TABLE IF EXISTS TopSearches
DROP TABLE IF EXISTS NodeRatings
DROP TABLE IF EXISTS NodeTags
DROP TABLE IF EXISTS Tags
DROP TABLE IF EXISTS Nodes
DROP TABLE IF EXISTS OTPClaims
DROP TABLE IF EXISTS AnalyticLogs
DROP TABLE IF EXISTS ArchiveLogs
DROP TABLE IF EXISTS UserHashTable
DROP TABLE IF EXISTS Accounts
DROP TABLE IF EXISTS Searches

DROP PROCEDURE IF EXISTS GetLogs
DROP PROCEDURE IF EXISTS DeleteLogs
DROP PROCEDURE IF EXISTS AddTagToNode
DROP PROCEDURE IF EXISTS ConfirmAccount
DROP PROCEDURE IF EXISTS CreateAccount
DROP PROCEDURE IF EXISTS CreateConfirmationLink	
DROP PROCEDURE IF EXISTS CreateNode
DROP PROCEDURE IF EXISTS CreateOTP
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
DROP PROCEDURE IF EXISTS RateNode
DROP PROCEDURE IF EXISTS RemoveConfirmationLink
DROP PROCEDURE IF EXISTS RemoveRecoveryLink
DROP PROCEDURE IF EXISTS RemoveTag
DROP PROCEDURE IF EXISTS RemoveTagFromNode
DROP PROCEDURE IF EXISTS UnconfirmAccount
DROP PROCEDURE IF EXISTS Authenticate
DROP PROCEDURE IF EXISTS VerifyAccount
DROP PROCEDURE IF EXISTS StoreOTP
DROP PROCEDURE IF EXISTS GetArchiveableLogs
DROP PROCEDURE IF EXISTS DeleteArchiveableLogs
DROP PROCEDURE IF EXISTS Logout
DROP PROCEDURE IF EXISTS StoreLog
DROP PROCEDURE IF EXISTS GetUserHash
DROP PROCEDURE IF EXISTS SearchNodes
DROP PROCEDURE IF EXISTS RefreshSession
DROP PROCEDURE IF EXISTS GetNodes
DROP PROCEDURE IF EXISTS GetNodeRating
DROP PROCEDURE IF EXISTS GetUserNodeRating
DROP PROCEDURE IF EXISTS GetNodesRatings
DROP PROCEDURE IF EXISTS GetUserNodesRatings
DROP PROCEDURE IF EXISTS VerifyAuthorizedToView
DROP PROCEDURE IF EXISTS RateNodes
DROP PROCEDURE IF EXISTS UpdateNodeContent

CREATE TABLE [dbo].Accounts(
	UserID INT IDENTITY(1,1) NOT NULL,
    Username VARCHAR(100),
    Passphrase VARCHAR(128),
    AuthorizationLevel VARCHAR(40),
    AccountStatus BIT,
    Confirmed BIT,
	CONSTRAINT user_accounts_ck UNIQUE (UserID),
    CONSTRAINT user_account_pk PRIMARY KEY(Username, AuthorizationLevel)
);

CREATE TABLE [dbo].UserHashTable(
	UserID INT NULL,
	UserHash VARCHAR(128),
	CONSTRAINT user_hashtable_fk FOREIGN KEY (UserID) REFERENCES Accounts(UserID),
	CONSTRAINT user_hashtable_pk PRIMARY KEY(UserHash)
);
 
CREATE TABLE [dbo].OTPClaims(
	Username VARCHAR(100),
	OTP VARCHAR(128),
	AuthorizationLevel VARCHAR(40),
	TimeCreated DATETIME,
	FailCount INT,

	CONSTRAINT otp_claims_fk_01 FOREIGN KEY (Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel),
	CONSTRAINT otp_claims_pk PRIMARY KEY(Username, AuthorizationLevel)
);

-- In order to not allow duplicate titled nodes per user, PK needs to be UserHash + NodeTitle
-- Parent will be determined by the same thing
CREATE TABLE [dbo].Nodes(
    UserHash VARCHAR(128),
    NodeID BIGINT Identity(1,1) PRIMARY KEY,
    ParentNodeID BIGINT,
    NodeTitle VARCHAR(100),
    Summary VARCHAR(750),
	TimeModified DATETIME,
    Visibility BIT,
    Deleted BIT,
    CONSTRAINT node_owner_fk FOREIGN KEY(UserHash) REFERENCES UserHashTable(UserHash),
	CONSTRAINT nodes_parent_fk FOREIGN KEY(NodeID) REFERENCES Nodes(NodeID),
	UNIQUE (UserHash, NodeTitle)
);

CREATE TABLE [dbo].Tags(
	TagName VARCHAR(100) PRIMARY KEY,
	TagCount BIGINT
);

CREATE TABLE [dbo].NodeTags(
    TagName VARCHAR(100),
    NodeID BIGINT,
	CONSTRAINT tag_name_fk FOREIGN KEY (TagName) REFERENCES Tags(TagName),
	CONSTRAINT node_id_fk FOREIGN KEY (NodeID) REFERENCES Nodes(NodeID),
	CONSTRAINT node_tags_pk PRIMARY KEY(NodeID, TagName)
);

CREATE TABLE [dbo].NodeRatings(
	UserHash VARCHAR(128),
	NodeID BIGINT,
	Rating INT,
	CONSTRAINT user_ratings_fk FOREIGN KEY (UserHash) REFERENCES UserHashTable(UserHash),
	CONSTRAINT user_ratings_node_fk FOREIGN KEY (NodeID) REFERENCES Nodes(NodeID),
	CONSTRAINT user_ratings_pk PRIMARY KEY(UserHash, NodeID)
);

--shouldn’t you combine both EditDate and EditTime into one attribute so the datatype can be DateTime
CREATE TABLE [dbo].TreeHistories(
	EditDate DATE,
	EditTime TIME,
);

CREATE TABLE [dbo].ViewsWebPages(
	ViewName VARCHAR(100),
	Visits INT,
	AverageDuration FLOAT
);

-- Keep number of registrations on a given date
CREATE TABLE [dbo].DailyRegistrations(
	RegistrationDate DATE PRIMARY KEY,
	RegistrationCount INT
);

-- Keep all of the logins for a given date
CREATE TABLE [dbo].DailyLogins(
	LoginDate DATE PRIMARY KEY,
	LoginCount INT
);

CREATE TABLE [dbo].TopSearches(
	TopSearchDate DATE PRIMARY KEY,
	SearchString VARCHAR(100),
	SearchCount INT
);

CREATE TABLE [dbo].NodesCreated(
	NodeCreationDate DATE PRIMARY KEY,
	NodeCreationTime INT,
);

CREATE TABLE [dbo].EmailConfirmationLinks(
	Username VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	GuidLink UNIQUEIDENTIFIER PRIMARY KEY,
	TimeCreated DateTime,
	CONSTRAINT confirmation_unique UNIQUE (Username, AuthorizationLevel),
	CONSTRAINT confirmation_links_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel)
);

CREATE TABLE EmailRecoveryLinks(
	Username VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	GuidLink UNIQUEIDENTIFIER PRIMARY KEY,
	TimeCreated DateTime,
	CONSTRAINT recovery_links_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel),
);

CREATE TABLE [dbo].EmailRecoveryLinksCreated(
	Username VARCHAR(100),
	AuthorizationLevel VARCHAR(40),
	LinkCount int,
	CONSTRAINT recovery_count_pk PRIMARY KEY(Username, AuthorizationLevel),
	CONSTRAINT recovery_count_fk FOREIGN KEY(Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel)
);


CREATE TABLE [dbo].AnalyticLogs(
	Timestamp DATETIME,
	Level VARCHAR(30),
	UserHash VARCHAR(128),
	Category VARCHAR(30),
	Description TEXT,
	Hash VARCHAR(64)
	CONSTRAINT analytic_logs_fk_01 FOREIGN KEY (UserHash) REFERENCES UserHashTable(UserHash)
);

CREATE TABLE [dbo].ArchiveLogs(
	Timestamp DATETIME,
	Level VARCHAR(30),
	UserHash VARCHAR(128),
	Category VARCHAR(30),
	Description TEXT,
	Hash VARCHAR(64)
	CONSTRAINT archive_logs_fk_01 FOREIGN KEY (UserHash) REFERENCES UserHashTable(UserHash)
);

CREATE TABLE [dbo].Searches(
	Search VARCHAR(100) PRIMARY KEY,
	Times INT
);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 5/6/22
-- Description:	Changes the content and/or title of the specified Node
-- =============================================
CREATE PROCEDURE UpdateNodeContent 
	-- Add the parameters for the stored procedure here
	@NodeID BIGINT,
	@NodeTitle VARCHAR(100),
	@Summary VARCHAR(750),
	@Result INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here

				-- 1 = success, 2 = Rollback occurred
				UPDATE Nodes SET NodeTitle = @NodeTitle, Summary = @Summary WHERE NodeID = @NodeID;

				SET @Result = 1

				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 2
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 2
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/19/22
-- Description:	Grabs all logs older than the specified DateTime
-- =============================================
CREATE PROCEDURE GetArchiveableLogs 
	-- Add the parameters for the stored procedure here
	@Timestamp DateTime = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM ArchiveLogs
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
CREATE PROCEDURE DeleteArchiveableLogs 
	-- Add the parameters for the stored procedure here
	@Timestamp DateTime = NULL,
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here

				-- 1 = success, 2 = Rollback occurred
				DELETE
				FROM ArchiveLogs
				WHERE Timestamp <= @Timestamp

				SET @Result = 1

				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 2
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 2
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN @Result;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/27/2022
-- Description:	Verify that the account is confirmed and enabled
-- =============================================
CREATE PROCEDURE VerifyAccount 
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(128),
    @AuthorizationLevel VARCHAR(40),
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here 

				-- 0 = No Account found, 1 = Success, 2 = Not Confirmed, 3 = Not Enabled, 4 = Rollback occurred
				SET @Result = (SELECT COALESCE(
					(SELECT CASE 
						WHEN Confirmed = 1 AND AccountStatus = 1 THEN 1
						WHEN Confirmed = 0 AND AccountStatus = 0 THEN 2
						WHEN Confirmed = 1 AND AccountStatus = 0 THEN 3
					END AS Verified
					FROM Accounts
					WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel), 0)
				)
				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 4
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 4
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN @Result;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/27/2022
-- Description:	Authenticates the user
-- =============================================
CREATE PROCEDURE Authenticate 
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(128),
    @OTP VARCHAR(128),
    @AuthorizationLevel VARCHAR(40),
	@TimeCreated DateTime,
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @FailCount int;
	DECLARE @RowCount int;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here

				-- 0 = No OTP Claim found, 1 = Success, 2 = Expired OTP, 3 = Bad OTP, 4 = Too many fails, 5 = Duplicate OTP Claims found, 6 = Duplicate Accounts found
				-- 7 = Rollback occurred
				SET @Result = (
					SELECT COALESCE(
						(SELECT CASE 
							WHEN CAST(OTP as BINARY) = CAST(@OTP as BINARY) AND TimeCreated <= @TimeCreated AND DATEADD(minute, 2, TimeCreated) >= @TimeCreated THEN 1
							WHEN CAST(OTP as BINARY) = CAST(@OTP as BINARY) THEN 2
							ELSE 3
						END AS Result
						FROM OTPClaims
						WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel), 0
					)
				)

				IF(@Result = 3)
					BEGIN
						UPDATE OTPClaims 
						SET FailCount = FailCount+1 
						WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel

						SELECT @RowCount = @@ROWCOUNT

						SET @FailCount = (
							SELECT COALESCE(
							(SELECT FailCount
							FROM OTPClaims
							WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel), 0)
						)

						IF(@RowCount = 1)
							BEGIN
								IF(@FailCount >= 5)
									BEGIN
										SET @Result = 4
										UPDATE Accounts
										SET AccountStatus = 0
										WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel

										SET @RowCount = @@ROWCOUNT

										IF(@RowCount != 1)
											SET @Result = 6
									END
							END
						ELSE
							SET @Result = 5
					END
					COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 7
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 7
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN @Result;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/27/2022
-- Description:	Stores the otp for the User
-- =============================================
CREATE PROCEDURE StoreOTP 
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(128),
	@AuthorizationLevel VARCHAR(40),
	@Passphrase VARCHAR(128),
	@OTP VARCHAR(128),
	@TimeCreated DateTime,
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RowCount int;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here

				-- 0 = No Account Found, 1 = Success, 2 = Bad Passphrase, 3 = No OTP Claim found, 4 = Duplicate OTP Claims found, 5 = Rollback occurred

				SET @Result = (SELECT COALESCE(
					(SELECT CASE 
							WHEN CAST(Passphrase as BINARY) = CAST(@Passphrase as BINARY) THEN 1
							ELSE 2
						END AS Result
						FROM Accounts
						WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel), 0
					)
				)

				IF(@Result = 1)
					BEGIN
						UPDATE OTPClaims
						SET OTP = @OTP, TimeCreated = @TimeCreated, 
							FailCount = CASE 
											WHEN TimeCreated >= DATEADD(day, 1, @TimeCreated) THEN 0
											ELSE FailCount
										END
						WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel

						SELECT @RowCount = @@ROWCOUNT
			
						IF(@RowCount = 1)
							SET @Result = 1
						ELSE IF (@RowCount = 0)
							SET @Result = 3
						ELSE
							SET @Result = 4
					END
				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 5
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 5
						ROLLBACK TRANSACTION ProcedureSave
					END
			END CATCH
	RETURN @Result;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/29/2022
-- Description:	Stores the otp for the User
-- =============================================
CREATE PROCEDURE StoreLog 
	-- Add the parameters for the stored procedure here
	@Timestamp DATETIME,
	@Level VARCHAR(30),
	@UserHash VARCHAR(128),
	@Category VARCHAR(30),
	@Description TEXT,
	@Hash VARCHAR(64),
	@Destination VARCHAR(40),
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RowCount int;
	DECLARE @TranCounter int;
	SET @TranCounter = @@TRANCOUNT;
	IF @TranCounter > 0
		SAVE TRANSACTION ProcedureSave
	ELSE
		BEGIN TRAN
			BEGIN TRY;
				-- Insert statements for procedure here

				-- 0 = Fail, 1 = success, 2 = Rollback occurred
				IF @Destination = 'AnalyticLogs'
					INSERT INTO AnalyticLogs(Timestamp, Level, UserHash, Category, Description, Hash) VALUES
					(@Timestamp, @Level, @UserHash, @Category, @Description, @Hash);
				ELSE IF @Destination = 'ArchiveLogs'
					INSERT INTO ArchiveLogs(Timestamp, Level, UserHash, Category, Description, Hash) VALUES
					(@Timestamp, @Level, @UserHash, @Category, @Description, @Hash);
				
				-- need to include schema for the table
				--@sql = 'INSERT INTO dbo.'+@DESTINATION+''

				-- if error, return line that caused error
				-- it is fine as is though, so decide what to settle on

				SELECT @RowCount = @@ROWCOUNT

				IF(@RowCount = 1)
					SET @Result = 1
				ELSE
					SET @Result = 0

				COMMIT TRANSACTION
			END TRY
		BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					-- output could be a table
					SET @Result = 2
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 2
						ROLLBACK TRANSACTION ProcedureSave
					END
		END CATCH
	RETURN @Result;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matthew Chen
-- Create date: 3/29/2022
-- Description:	Stores the otp for the User
-- ============================================= 
CREATE PROCEDURE SearchNodes 
	-- Add the parameters for the stored procedure here
	@Search VARCHAR(100) -- match NodeTitle length
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT * FROM Searches WHERE UPPER(Search) = UPPER(@Search) OR LOWER(Search) = LOWER(@Search))
		BEGIN
			UPDATE Searches SET Times = Times+1;
		END
	ELSE
		BEGIN
			INSERT INTO Searches(Search, Times) VALUES (@Search, 1)
		END
	
	SELECT Nodes.UserHash, Nodes.NodeID, NodeTitle, TimeModified, TagName, COALESCE(AVG(Rating),0) AS Rating
		FROM (Nodes LEFT JOIN NodeTags ON Nodes.NodeID = NodeTags.NodeID) LEFT JOIN NodeRatings ON Nodes.NodeID = NodeRatings.NodeID
		WHERE (UPPER(NodeTitle) LIKE ('%' + UPPER(@Search) + '%') OR LOWER(NodeTitle) LIKE ('%' + LOWER(@Search) + '%')) AND Visibility = 1 AND Deleted = 0
		GROUP BY Nodes.UserHash, Nodes.NodeID, NodeTitle, TimeModified, TagName
	-- right now just seeing if word is in title at all (exact match, is a word in it, is a substring of a word in it)
	-- can pass in a List, where the list is made of up the individual words of the searched phrase, keeping it simple for now, in future should optimize with this but take out filler words
	-- (the, and, etc.)

	RETURN;
END
GO


-- =============================================
CREATE PROCEDURE GetUserHash 
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40),
	@Result VARCHAR(128) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY;
		--IF(EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'TrialByFire.Tresearch.IntegrationTestDB' AND TABLE_NAME = 'UserHashTable'))
		SET @Result = (SELECT UserHash 
					   FROM UserHashTable 
					   WHERE UserID =  (SELECT UserID FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel));

		IF(@Result IS NOT NULL)
			RETURN 1;

	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH
	RETURN 0;
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
		UPDATE Tags SET TagCount = TagCount + 1 WHERE TagName= @TagName
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
AS
BEGIN
	INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES(@Username, @Passphrase, @AuthorizationLevel, @AccountStatus, @Confirmed);
	SELECT UserID FROM Accounts WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
END


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
    @UserHash VARCHAR(128),
    @NodeID BIGINT,
    @ParentNodeID BIGINT,
    @NodeTitle VARCHAR(100),
    @Summary VARCHAR(750),
    @Visibility BIT
)
as
begin
    INSERT INTO Nodes(UserHash, NodeID, ParentNodeID, NodeTitle, Summary, Visibility)
         VALUES(@UserHash, @NodeID, @ParentNodeID, @NodeTitle, @Summary, @Visibility);
end

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Jessie Lazo 
-- Description: Return list of Nodes pertaining to the UserHash
-- =============================================
CREATE PROCEDURE [dbo].[GetNodes]
(
    -- Add the parameters for the stored procedure here
    @UserHash VARCHAR(128),
	@AccountHash VARCHAR(128)
)
AS
BEGIN
    IF (@UserHash = @AccountHash)
		BEGIN
			SELECT UserHash, NodeID, ParentNodeID, NodeTitle, Summary, TimeModified, Visibility, Deleted FROM Nodes	
				WHERE Userhash = @UserHash AND Deleted = 0
		END

	ELSE
		BEGIN
			SELECT UserHash, NodeID, ParentNodeID, NodeTitle, Summary, TimeModified, Visibility, Deleted FROM Nodes
				WHERE UserHash = @UserHash AND Visibility = 1 AND Deleted = 0
		END
END

-- =============================================
--Author:        Pammy Poor
-- Description:    Create OTP
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateOTP]
(
    @Username VARCHAR(100),
	@AuthorizationLevel VARCHAR(40),
	@FailCount INT
)
as
begin
    INSERT INTO OTPClaims(Username, AuthorizationLevel, FailCount)
         VALUES(@Username, @AuthorizationLevel, @FailCount);
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
	@UserID INT,
	@UserHash VARCHAR(128)
)
as
begin
	IF EXISTS( SELECT * FROM UserHashTable with (updlock, serializable) WHERE UserHash = @UserHash)
		BEGIN
			UPDATE UserHashTable SET UserID = @UserID WHERE UserHash = @UserHash;
		END
	ELSE
		BEGIN
			INSERT INTO UserHashTable(UserID, UserHash) VALUES(@UserID, @UserHash);
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
	@TagName VARCHAR(100),
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

    DELETE FROM EmailConfirmationLinks     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM EmailRecoveryLinks     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM EmailRecoveryLinksCreated     WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
    DELETE FROM OTPClaims WHERE Username = @Username and AuthorizationLevel = @AuthorizationLevel;
	UPDATE UserHashTable SET UserID = NULL WHERE UserID = (SELECT UserID FROM Accounts Where Username = @Username and AuthorizationLevel = @AuthorizationLevel);
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
			DECLARE @TagCount BIGINT = (SELECT TagCount FROM Tags WHERE TagName = @TagName)
			DELETE NodeTags WHERE TagName = @TagName
			DELETE Tags WHERE TagName = @TagName
			SELECT @TagCount
		END
END

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
	UPDATE Tags SET TagCount = TagCount - 1 WHERE TagName= @TagName
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

-- =============================================
-- Author:		Pammy Poor
-- Description:	Get Node's Ratings
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNodeRating]    
(
	@NodeID BIGINT
)
as
begin
	SELECT Avg(Rating) FROM NodeRatings Where NodeID = @NodeID
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Get Node's Ratings
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserNodeRating]    
(
	@NodeID BIGINT,
	@UserHash VARCHAR(128)
)
as
begin
	SELECT Avg(Rating) FROM NodeRatings Where NodeID = @NodeID AND UserHash = @UserHash
end



-- =============================================
-- Author:		Pammy Poor
-- Description:	Get Nodes Ratings
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNodesRatings](@InNodes VARCHAR(MAX))
as
begin
SELECT Nodes.UserHash, Nodes.NodeID, NodeTitle, ParentNodeID, Summary, TimeModified, Visibility, Deleted, COALESCE(AVG(1.0 * Rating),0) AS RatingScore
        FROM Nodes LEFT JOIN NodeRatings ON Nodes.NodeID = NodeRatings.NodeID
        WHERE Nodes.NodeID IN (SELECT Value from STRING_SPLIT(@InNodes, ','))
		GROUP BY Nodes.UserHash, Nodes.NodeID, NodeTitle, ParentNodeID, Summary, TimeModified, Visibility, Deleted
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Get Nodes Ratings
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserNodesRatings]    
(
	@InNodes VARCHAR(MAX),
	@UserHash VARCHAR(128)
)
as
begin
	SELECT Nodes.UserHash, Nodes.NodeID, NodeTitle, Rating FROM Nodes LEFT JOIN NodeRatings on Nodes.NodeID = NodeRatings.NodeID 
	WHERE Nodes.NodeID IN (SELECT Value from STRING_SPLIT(@InNodes, ',')) AND NodeRatings.UserHash = @UserHash

end



-- =============================================
-- Author:		Pammy Poor
-- Description:	Verify nodes are public or owned by user
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VerifyAuthorizedToView]    
(
	@InNodes VARCHAR(MAX),
	@UserHash VARCHAR(128)
)
as
begin
	SELECT CASE WHEN (SELECT COUNT(*) FROM Nodes WHERE NodeID IN (SELECT Value from STRING_SPLIT(@InNodes, ',')) AND (Visibility = 1 OR UserHash = @UserHash)) = (SELECT Count(*) from STRING_SPLIT(@InNodes, ','))
	  THEN CAST(1 AS BIT)
	  ELSE CAST(0 AS BIT)
	END
end

-- =============================================
-- Author:		Pammy Poor
-- Description:	Rates Node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RateNode]    
(
	@UserHash VARCHAR(128),
	@NodeID BIGINT,
	@Rating INT
)
as
BEGIN
	IF EXISTS (SELECT * FROM NodeRatings WHERE UserHash = @UserHash AND NodeID = @NodeID)
		BEGIN
			UPDATE NodeRatings SET Rating = @Rating WHERE UserHash = @UserHash AND NodeID = @NodeID;
		END
	ELSE
		BEGIN
			INSERT NodeRatings(UserHash, NodeID, Rating) VALUES (@UserHash, @NodeID, @Rating);
		END
END


-- =============================================
-- Author:		Pammy Poor
-- Description:	Rates Node
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RateNodes]    
(
	@UserHash VARCHAR(128),
	@InNodes VARCHAR(MAX),
	@Rating INT
)
as
BEGIN
	DECLARE @Temp Table(NodeID BIGINT);
	INSERT INTO @Temp(NodeID) SELECT Value from STRING_SPLIT(@InNodes, ',')

	Declare @TempID BIGINT = 0
	
	WHILE (1 = 1)
		BEGIN
			Select Top 1 @TempID = NodeID from @Temp Where NodeID > @TempId Order by NodeID

			if @@ROWCOUNT = 0 BREAK;

			EXEC RateNode @UserHash, @TempID, @Rating
		END
END

