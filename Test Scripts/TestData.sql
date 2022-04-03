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
DROP TABLE IF EXISTS Logs
DROP PROCEDURE IF EXISTS GetLogs
DROP PROCEDURE IF EXISTS DeleteLogs
DROP PROCEDURE IF EXISTS VerifyAccount
DROP PROCEDURE IF EXISTS Authenticate
DROP PROCEDURE IF EXISTS StoreOTP
DROP PROCEDURE IF EXISTS Logout
DROP PROCEDURE IF EXISTS StoreLog
 
CREATE TABLE Accounts(
    Username VARCHAR(50),
    Email VARCHAR(50),
    Passphrase VARCHAR(100),
    AuthorizationLevel VARCHAR(40),
    AccountStatus BIT,
    Confirmed BIT,
	Token VARCHAR(64) NULL,
    CONSTRAINT user_account_pk PRIMARY KEY(Username, AuthorizationLevel)
);


CREATE TABLE OTPClaims(
	Username VARCHAR(50),
	OTP VARCHAR(10),
	AuthorizationLevel VARCHAR(40),
	TimeCreated DATETIME,
	FailCount INT,

	CONSTRAINT otp_claims_fk_01 FOREIGN KEY (Username, AuthorizationLevel) REFERENCES Accounts(Username, AuthorizationLevel),
	CONSTRAINT otp_claims_pk PRIMARY KEY(Username, AuthorizationLevel)
);


CREATE TABLE Nodes(

	Username VARCHAR(50),
	AuthorizationLevel VARCHAR(40),

	NodeID BIGINT PRIMARY KEY,
	NodeParentID BIGINT,
	NodeTitle VARCHAR(50),
	Summary VARCHAR(300),
	Mode VARCHAR,

	CONSTRAINT account_own FOREIGN KEY(Username, AuthorizationLevel)
		REFERENCES Accounts(Username, AuthorizationLevel)
);




CREATE TABLE Tags(
	TagName VARCHAR(15) PRIMARY KEY
);




CREATE TABLE NodeTags(
	Nodeid BIGINT FOREIGN KEY REFERENCES Nodes(NodeID),
	TagName VARCHAR(15) FOREIGN KEY REFERENCES Tags(TagName),
	
	CONSTRAINT node_tags_pk PRIMARY KEY(Nodeid, TagName)
);



CREATE TABLE UserRatings(
	Username VARCHAR(50),
	AuthorizationLevel VARCHAR(40),


	NodeID BIGINT FOREIGN KEY REFERENCES Nodes(Nodeid),
	Rating INT,
	

	CONSTRAINT user_ratings_fk FOREIGN KEY(Username, AuthorizationLevel) 
		REFERENCES Accounts(Username, AuthorizationLevel),
	CONSTRAINT user_ratings_pk PRIMARY KEY(Username, Nodeid)
);

CREATE TABLE TreeHistories(
	EditDate DATE,
	EditTime TIME,
);

CREATE TABLE ViewsWebPages(
	ViewName VARCHAR(25),
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
	SearchString VARCHAR(50),
	SearchCount INT
);

CREATE TABLE NodesCreated(
	NodeCreationDate DATE PRIMARY KEY,
	NodeCreationTime INT,
);

CREATE TABLE EmailConfirmationLinks(
	Username VARCHAR(25) PRIMARY KEY,
	GUID UNIQUEIDENTIFIER,
	timestamp TIME
);

CREATE TABLE Logs(
	Timestamp DATETIME,
	Level VARCHAR(30),
	Username VARCHAR(50),
	Category VARCHAR(30),
	Description TEXT,
	Hash VARCHAR(64)
);

--for otp tests
INSERT INTO Accounts (Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
('drakat7@gmail.com', 'drakat7@gmail.com', 'abcDEF123', 'user', 1, 1),
('drakat7@gmail.com', 'drakat7@gmail.com', 'abcDEF123', 'admin', 1, 1),
('aarry@gmail.com', 'aarry@gmail.com', 'abcDEF123', 'user', 1, 1),
('barry@gmail.com', 'barry@gmail.com', 'abcDEF123', 'admin', 1, 1),
('carry@gmail.com', 'carry@gmail.com', 'abcDEF123', 'user', 1, 1),
('darry@gmail.com', 'darry@gmail.com', 'abcDEF123', 'user', 0, 1),
('earry@gmail.com', 'earry@gmail.com', 'abcDEF123', 'user', 0, 0),
('farry@gmail.com', 'farry@gmail.com', 'abcDEF123', 'user', 1, 1);
--for authentication tests
INSERT INTO Accounts (Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
('garry@gmail.com', 'garry@gmail.com', 'abcDEF123', 'user', 1, 1),
('harry@gmail.com', 'harry@gmail.com', 'abcDEF123', 'admin', 1, 1),
('iarry@gmail.com', 'iarry@gmail.com', 'abcDEF123', 'admin', 1, 1),
('sarry@gmail.com', 'sarry@gmail.com', 'abcDEF123', 'admin', 1, 1),
('jarry@gmail.com', 'jarry@gmail.com', 'abcDEF123', 'admin', 1, 1),
('karry@gmail.com', 'karry@gmail.com', 'abcDEF123', 'user', 1, 1),
('larry@gmail.com', 'larry@gmail.com', 'abcDEF123', 'user', 0, 1),
('marry@gmail.com', 'marry@gmail.com', 'abcDEF123', 'user', 0, 0),
('narry@gmail.com', 'narry@gmail.com', 'abcDEF123', 'user', 1, 1),
('oarry@gmail.com', 'oarry@gmail.com', 'abcDEF123', 'user', 1, 1),
('parry@gmail.com', 'parry@gmail.com', 'abcDEF123', 'user', 1, 1),
('rarry@gmail.com', 'rarry@gmail.com', 'abcDEF123', 'user', 1, 1),
--api test
('qarry@gmail.com', 'qarry@gmail.com', 'abcDEF123', 'user', 1, 1);

--for otp tests
INSERT INTO OTPClaims (Username, OTP, AuthorizationLevel, TimeCreated, FailCount) VALUES
('drakat7@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('drakat7@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('aarry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('barry@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('carry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('darry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('earry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('farry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 4);
--for authentication tests
INSERT INTO OTPClaims (Username, OTP, AuthorizationLevel, TimeCreated, FailCount) VALUES
('garry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('harry@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('iarry@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('sarry@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('jarry@gmail.com', 'ABCdef123', 'admin', '2022-03-04T05:06:00', 0),
('karry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('larry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('marry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 0),
('narry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 4),
('oarry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 4),
('parry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 4),
('rarry@gmail.com', 'ABCdef123', 'user', '2022-03-04T05:06:00', 4),
--api test
('qarry@gmail.com', 'ABCdef123', 'user', '2022-03-06T18:43:00', 0);

INSERT INTO Logs (Timestamp, Level, Username, Category, Description) VALUES
('2022-02-06T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.'),
('2022-02-07T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.'),
('2022-02-08T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.'),
('2022-02-09T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.'),
('2022-02-10T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.'),
('2022-02-11T18:43:00', 'Server', 'drakat7@gmail.com', 'Info', 'This is a test.');


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
				FROM Logs
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
	@Username VARCHAR(50),
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
	@Username VARCHAR(50),
    @OTP VARCHAR(10),
    @AuthorizationLevel VARCHAR(40),
	@TimeCreated DateTime,
	@Token VARCHAR(64) NULL,
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

				IF(@Result = 1)
					BEGIN
						UPDATE Accounts 
						SET Token = @Token 
						WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel

						SELECT @RowCount = @@ROWCOUNT

						IF(@RowCount != 1)
							SET @Result = @RowCount
					END

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
	@Username VARCHAR(50),
	@AuthorizationLevel VARCHAR(40),
	@Passphrase VARCHAR(100),
	@OTP VARCHAR(10),
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
CREATE PROCEDURE Logout 
	-- Add the parameters for the stored procedure here
	@Username VARCHAR(50),
	@AuthorizationLevel VARCHAR(40),
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

				-- 0 = No Account Found, 1 = Success, 2 = Duplicate account found, 3 = Rollback occurred

				UPDATE Accounts
				SET Token = null
				WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel 

				SELECT @RowCount = @@ROWCOUNT

				IF(@RowCount = 0)
					SET @Result = 0
				ELSE IF(@RowCount = 1)
					SET @Result = 1
				ELSE
					SET @Result = 2

				COMMIT TRANSACTION
			END TRY
		BEGIN CATCH
			IF @TranCounter = 0
				BEGIN
					SET @Result = 3
					ROLLBACK TRANSACTION
				END
			ELSE
				IF XACT_STATE() <> -1
					BEGIN
						SET @Result = 3
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
	@Username VARCHAR(50),
	@Category VARCHAR(30),
	@Description TEXT,
	@Hash VARCHAR(64),
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

				INSERT INTO Logs (Timestamp, Level, Username, Category, Description, Hash) VALUES
				(@Timestamp, @Level, @Username, @Category, @Description, @Hash);

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