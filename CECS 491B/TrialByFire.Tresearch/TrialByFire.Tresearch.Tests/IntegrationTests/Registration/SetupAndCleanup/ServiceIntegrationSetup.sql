Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('regServiceUser1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('regServiceUser2@tresearch.system', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('regServiceUser3@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1),
			('regServiceUserNotConfirmedEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 0),
			('regServiceUserConfirmedNotEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'admin', 0, 1),
			('regServiceUserConfirmedEnabled@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1),
			('regServiceCreateUser2@tresearch.system', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '96501f5803a5f00ccfc8900ac8369165824f7d40bd77579fe4b08ef16ed9f3b582d3dbf0a4ec1f047c4a64e66d5d1e1e3b02327c9a1890f89d8f58321de795d8'
		FROM Accounts WHERE Username = 'regServiceUser1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '8bd2b7d575828b9deed468583fae9474ea2cab22965f0ce3ca81874a70e5eb1a22aa667e9172bde0c28b9491eb48979bd39c64db3dced5651579b85584ac9488'
		FROM Accounts WHERE Username = 'regServiceUser2@gmail.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '8dbf6b1ec7d0bb582327dc655371bb4432601ccde32c93584a17103fb9bfff11405bdd06bf104a70bd6a2f7e2c1604d18a660035493c5b214e54f3674dc4b333'
		FROM Accounts WHERE Username = 'regServiceUser3@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'b4cd967228f1b62d3445db33d45bb0614109f3d09eb0153be01d1ef7df85723861a93ec535eaa951195f35ffbcc98616de68ead9c96dc61f70fc014c7e7d1163'
		FROM Accounts WHERE Username = 'regServiceUserNotConfirmedEnabled@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '0f8247112bd2a1fce9dd127aff00c2bcd854d8a2e6fd4d7d8932f44ce57bc9f00fbd5909a88ded2820ad80df9e8e065cd8dd3c3fadae51f9e8a21d51c9371799'
		FROM Accounts WHERE Username = 'regServiceUserConfirmedNotEnabled@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '857298369e2f0a56577650b45e01cde183fb301042090b8dc00f790cfbb6ea9e4bc142c91413185fd9ea0a5b20e587647e4fcfc132d792320d9872715dfb53d4'
		FROM Accounts WHERE Username = 'regServiceUserConfirmedEnabled@tresearch.system';


INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('regServiceUser2@tresearch.system', (Convert(uniqueidentifier, N'd3592438-08a9-462c-a6c7-db4b9b99cf45')), GETDATE(), 'user')
INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('regServiceUser3@tresearch.system', (Convert(uniqueidentifier, N'd3592438-07a9-462c-a6c7-db4b9b99cf45')), GETDATE(), 'user')

