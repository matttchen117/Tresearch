 
 
 -- SQLDAO SHOULD SCRIPTS
  

  DELETE UserHashTable WHERE UserID = 'pammypoor+daoReg1@gmail.com'
  DELETE UserHashTable WHERE UserID = 'pammypoor+daoReg2@gmail.com'
  DELETE UserHashTable WHERE UserID = 'sqlDAORegTest1@gmail.com'
  DELETE UserHashTable WHERE UserID = 'sqlDAORegTest2@gmail.com'

  DELETE Accounts WHERE Username = 'sqlDAORegTest1@gmail.com'
  DELETE Accounts WHERE Username = 'sqlDAORegTest2@gmail.com'

  INSERT INTO Accounts (Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES ('sqlDAORegTest2@gmail.com', '1e5645a39fd80695b37f8fd4cf96bef2632e0d0aaf78ca13c42066196a2838535414e82ffa24adf950683d528deed5757675870b7d4708a2f814ea9000237de8', 'user', 1, 1);

  
 --Recovery Service Should scripts

  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService1@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService2@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService3@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService4@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService5@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService6@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService7@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService8@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'IntegrationRegistrationService9@gmail.com';

  DELETE Accounts WHERE Username='IntegrationRegistrationService1@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService2@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService3@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService4@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService5@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService6@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService7@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService8@gmail.com';
  DELETE Accounts WHERE Username='IntegrationRegistrationService9@gmail.com';

  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService1@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService2@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService3@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService4@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService5@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService6@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService7@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService8@gmail.com';
  DELETE UserHashTable WHERE UserID = 'IntegrationRegistrationService9@gmail.com';

  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService1@gmail.com', 'user', '2dc3e209e4d8a1d977ee8369390088e82b424787135bebacf0ee8d755d98b4715a4f5c1941d5c3d08026e6deee69dea339bc92e7b41b6b8026ddb53332ba9f26');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService2@gmail.com', 'user', 'b456e69280632e91c2526df4b9e25e7a261fbe1e3bc11e06ca2b6eccbc52f2c3f54242ce71418178f000fcb6047c9cd250cb9aa947c1e33d24f550a80c810d4b');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService3@gmail.com', 'user', 'e74272538ed4842c1ababcf8a3813fefef58bbf01dac37944987c9f8e2f17c0759ebe570fb587cc7300d8a12fae0ead4fa72ed43cd5fd9c825526070923c6339');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService4@gmail.com', 'user', 'ec2e4d75f40fcd3f97be5d118a2ac8a8635bcef5b3fc026d51c70d27f0c6e798370f7706ec7dfa2bc1840f63b02bb061b4b2bce209e84c022ba7852109b93de5');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService5@gmail.com', 'user', '35286486711bdbda757cad87719748a333af0025cd8515ae27b7ea51621d4b2765a7e6d023ace4dc56d2c450ac871a784aff9c4f01c15deae967b8e763046fc2');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService6@gmail.com', 'user', '085ab15a30e330554d3eb2ee358b92c491fa7026edb5c601740708bdf8214ffb0762054dddebebb95f4350ac1ad766260946a7c09abc8e2bcd4e7debb044f92c');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService7@gmail.com', 'user', '369f08fdf4b8b13547a0fdd8aa3e91a253c95b27a691e2c1c49e9252797d9b990630e5d98bb149aec870a5502825a83862a1afa7c443f05f79cf286d5179fc25');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService8@gmail.com', 'user', '6986d1f3b479587873fd71f8df96058dddc5e78b7dd04c3443c4c789a0a2dcfe8358ff0259172af329f8d519b3fd90729ddb235cfe8812ffb8309cd6db4ba13b');
  INSERT INTO UserHashTable (UserID, UserRole, UserHash) VALUES ('IntegrationRegistrationService9@gmail.com', 'user', '61f545f91e9b3d93657a719117d68d74f2068b250dbb8a3a18eca246214074b1ff991b70ad67b64bb6d43a4786eb5c525b1c807f1affe46b585bceb7d8975439');


  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService3@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 0);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService4@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 0);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService5@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 0);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService6@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 0);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService7@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 1);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService8@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 1, 1);
  INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService9@gmail.com', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 0, 1);

  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('IntegrationRegistrationService5@gmail.com', (Convert(uniqueidentifier, N'd3592438-07a9-462c-a6c7-db4b9b99cf45')), GETDATE(), 'user')
  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('IntegrationRegistrationService8@gmail.com', (Convert(uniqueidentifier, N'7eeb0847-f9f7-4ff4-b7e1-de4a4160c965')), GETDATE(), 'user')

--Recovery Manager Should scripts
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegMan1@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegMan2@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegMan3@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegMan4@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegMan5@gmail.com';

  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegMan1@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegMan2@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegMan3@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegMan4@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegMan5@gmail.com';

  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegMan2@gmail.com', 'trialbyfire.tresearch+IntRegMan2@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegMan3@gmail.com', 'trialbyfire.tresearch+IntRegMan3@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegMan4@gmail.com', 'trialbyfire.tresearch+IntRegMan4@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegMan5@gmail.com', 'trialbyfire.tresearch+IntRegMan5@gmail.com', 'myPassphrase', 'user', '0', '1');

  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('trialbyfire.tresearch+IntRegMan4@gmail.com', (Convert(uniqueidentifier, N'7f5c634a-ef48-49c2-a20c-adde83b6837d')), GETDATE(), 'user')
  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('trialbyfire.tresearch+IntRegMan5@gmail.com', (Convert(uniqueidentifier, N'5278f32c-353f-487d-9145-4320125fc433')), GETDATE(), 'user')


--Recovery Controller Should scripts
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegCon1@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegCon2@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegCon3@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegCon4@gmail.com';
  DELETE EmailConfirmationLinks WHERE Username = 'trialbyfire.tresearch+IntRegCon5@gmail.com';

  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegCon1@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegCon2@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegCon3@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegCon4@gmail.com';
  DELETE Accounts WHERE Username='trialbyfire.tresearch+IntRegCon5@gmail.com';

  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegCon2@gmail.com', 'trialbyfire.tresearch+IntRegCon2@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegCon3@gmail.com', 'trialbyfire.tresearch+IntRegCon3@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegCon4@gmail.com', 'trialbyfire.tresearch+IntRegCon4@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('trialbyfire.tresearch+IntRegCon5@gmail.com', 'trialbyfire.tresearch+IntRegCon5@gmail.com', 'myPassphrase', 'user', '0', '1');

  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('trialbyfire.tresearch+IntRegCon4@gmail.com', (Convert(uniqueidentifier, N'f92c1817-a3b0-4bce-b96a-5deb38ccf05d')), GETDATE(), 'user')
  INSERT INTO EmailConfirmationLinks(Username, GUIDLink, TimeCreated, AuthorizationLevel)
		 VALUES('trialbyfire.tresearch+IntRegCon5@gmail.com', (Convert(uniqueidentifier, N'3404629b-af83-4d70-8cd2-bd5cfd65e9ea')), GETDATE(), 'user')