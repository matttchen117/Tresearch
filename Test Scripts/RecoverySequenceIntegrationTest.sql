--Service Controller Should scripts
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService1@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService2@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService3@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService4@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService5@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService6@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService7@gmail.com';
  DELETE EmailRecoveryLinks WHERE Username = 'IntegrationRecoveryService8@gmail.com';

  DELETE Accounts WHERE Username='IntegrationRecoveryService1@gmail.com' AND AuthorizationLevel = 'user';
  DELETE Accounts WHERE Username='IntegrationRecoveryService2@gmail.com' AND AuthorizationLevel = 'admin';
  DELETE Accounts WHERE Username='IntegrationRecoveryService3@gmail.com' AND AuthorizationLevel = 'user';
  DELETE Accounts WHERE Username='IntegrationRecoveryService4@gmail.com' AND AuthorizationLevel = 'user';
  DELETE Accounts WHERE Username='IntegrationRecoveryService5@gmail.com' AND AuthorizationLevel = 'user';
  DELETE Accounts WHERE Username='IntegrationRecoveryService6@gmail.com' AND AuthorizationLevel = 'admin';
  DELETE Accounts WHERE Username='IntegrationRecoveryService7@gmail.com' AND AuthorizationLevel = 'user';
  DELETE Accounts WHERE Username='IntegrationRecoveryService8@gmail.com' AND AuthorizationLevel = 'user';

  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService1@gmail.com', 'IntegrationRecoveryService1@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService2@gmail.com', 'IntegrationRecoveryService2@gmail.com', 'myPassphrase', 'admin', '1', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService3@gmail.com', 'IntegrationRecoveryService3@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService4@gmail.com', 'IntegrationRecoveryService4@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService5@gmail.com', 'IntegrationRecoveryService5@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService6@gmail.com', 'IntegrationRecoveryService6@gmail.com', 'myPassphrase', 'admin', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService7@gmail.com', 'IntegrationRecoveryService7@gmail.com', 'myPassphrase', 'user', '0', '1');
  INSERT INTO Accounts(Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
		 VALUES('IntegrationRecoveryService8@gmail.com', 'IntegrationRecoveryService8@gmail.com', 'myPassphrase', 'user', '0', '1');

--Service Controller Get RecoveryLink
  INSERT INTO EmailRecoveryLinks(Username, AuthorizationLevel, TimeCreated, GUIDLink)
		 VALUES('IntegrationRecoveryService3@gmail.com', 'user', 
		 GETDATE(), (Convert(uniqueidentifier, N'4bfa5d46-d891-4648-b3bc-fd52c6905bd1')));
