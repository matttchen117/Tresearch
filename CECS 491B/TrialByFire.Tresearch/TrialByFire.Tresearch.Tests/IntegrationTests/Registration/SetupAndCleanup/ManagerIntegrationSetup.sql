Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('pammypoor+RegManagerIntegration1@gmail.com', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('pammypoor+RegManagerIntegration2@gmail.com', '30472ac011fe1a7c0ec6ba98686f0fd21a8e2a8d72c074b2e4d60bdf2555bd82e4ad866adef6a2ee4b5a6dc3b2d4fadfae1128e4e658dc2901d83fd5571b436c', 'user', 1, 1),
			('pammypoor+RegManagerIntegration3@gmail.com', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1),
			('pammypoor+regManagereUserNotConfirmedEnabled@gmail.com', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 0, 0),
			('pammypoor+regManagerUserConfirmedNotEnabled@gmail.com', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'admin', 0, 1),
			('pammypoor+regManagerUserConfirmedEnabled@gmail.com', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1),
			('pammypoor+regManagerCreateUser2@gmail.com', 'cb118e46dbebd6b9033c9fd02376dc7cb715f81ea33010612594e3c8989ef2fd42ef59c64e7205659d2a7a48021f4246fcfd920fa93f9bc32f342088c42d3e3f', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'ceef03d444c0c33847d796ced4ed6cd95e89dbe64a9fbe7dcebcdb6b630d2c341d0c40ef3dcf365ab544ffa5a6fd6463d81a0c224f92fef88e7c4d084e2b6a1c'
		FROM Accounts WHERE Username = 'pammypoor+RegManagerIntegration1@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '0cdc67dd32e50cfaf715c7db51941f3fe7c9bddf3526036bc21d5948f59f3fa31560a60b112098837c35c5e3089a6f882a4ad887116b8aaa9301f4dcc6948348'
		FROM Accounts WHERE Username = 'pammypoor+RegManagerIntegration2@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '0ccf19be55f89223ef481d5876c07c4cd4f5418163d5fe6f63f0d405dab777bd0962435ef1b6182d30a394cb7a043f9b8f1892375863b30314ebd41f6b215ca5'
		FROM Accounts WHERE Username = 'pammypoor+RegManagerIntegration3@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'de4ec0e25b2de49bd2b5819eada79e5d2259bf95290c24317103d10054afaf4ae27ea2a77244af6bf4a824cdc5267e621f4331ede8ec2bbd27ac11c381fac655'
		FROM Accounts WHERE Username = 'pammypoor+regManagereUserNotConfirmedEnabled@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'fa09c2c24b7ecf48b61248cd437dfaa6421f5894d3c43c89c7853965762f8a2936d976a45723fd26e2eac021a230ded129fd01e91c70059f65c310b55741120c'
		FROM Accounts WHERE Username = 'pammypoor+regManagerUserConfirmedNotEnabled@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '06c36ca312bf34f0567038cb6de859ed27f8b1905a54edc9f5dd9a9ff6967bb50b02e439ac27d65b357f6aea0f6e601562edb85cea623567860d15a214e42b84'
		FROM Accounts WHERE Username = 'pammypoor+regManagerUserConfirmedEnabled@gmail.com';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'c5d62f5300bda6c4e2b57961b00855070412893c5d4933eb3dd0745a7375040980043e63c760c5c1f41db36bf565299c8618c1ade81289cf5785b1418f6f32d6'
		FROM Accounts WHERE Username = 'pammypoor+regManagerCreateUser2@gmail.com';