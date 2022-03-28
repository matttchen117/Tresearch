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

  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService3@gmail.com', 'IntegrationRegistrationService3@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService4@gmail.com', 'IntegrationRegistrationService4@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService5@gmail.com', 'IntegrationRegistrationService5@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService6@gmail.com', 'IntegrationRegistrationService6@gmail.com', 'myPassphrase', 'user', '0', '0');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService7@gmail.com', 'IntegrationRegistrationService7@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService8@gmail.com', 'IntegrationRegistrationService8@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRegistrationService9@gmail.com', 'IntegrationRegistrationService9@gmail.com', 'myPassphrase', 'user', '0', '1');

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