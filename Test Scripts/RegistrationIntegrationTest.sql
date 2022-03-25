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