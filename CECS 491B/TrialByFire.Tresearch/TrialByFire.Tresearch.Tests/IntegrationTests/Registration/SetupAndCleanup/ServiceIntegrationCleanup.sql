DELETE OTPClaims WHERE Username = 'regServiceUser1@tresearch.system';
DELETE OTPClaims WHERE Username = 'regServiceUser2@tresearch.system';
DELETE OTPClaims WHERE Username = 'regServiceUser3@tresearch.system';
DELETE OTPClaims WHERE Username = 'regServiceUserNotConfirmedEnabled@tresearch.system';
DELETE OTPClaims WHERE Username = 'regServiceUserConfirmedNotEnabled@tresearch.system';
DELETE OTPClaims WHERE Username = 'regServiceUserConfirmedEnabled@tresearch.system';

DELETE EmailConfirmationLinks WHERE Username = 'regServiceUser1@tresearch.system';
DELETE EmailConfirmationLinks WHERE Username = 'regServiceUser2@tresearch.system';
DELETE EmailConfirmationLinks WHERE Username = 'regServiceUser3@tresearch.system';
DELETE EmailConfirmationLinks WHERE Username = 'regServiceUserNotConfirmedEnabled@tresearch.system';
DELETE EmailConfirmationLinks WHERE Username = 'regServiceUserConfirmedNotEnabled@tresearch.system';
DELETE EmailConfirmationLinks WHERE Username = 'regServiceUserConfirmedEnabled@tresearch.system';


DELETE UserHashTable WHERE UserHash = '96501f5803a5f00ccfc8900ac8369165824f7d40bd77579fe4b08ef16ed9f3b582d3dbf0a4ec1f047c4a64e66d5d1e1e3b02327c9a1890f89d8f58321de795d8';
DELETE UserHashTable WHERE UserHash = '8bd2b7d575828b9deed468583fae9474ea2cab22965f0ce3ca81874a70e5eb1a22aa667e9172bde0c28b9491eb48979bd39c64db3dced5651579b85584ac9488';
DELETE UserHashTable WHERE UserHash = '8dbf6b1ec7d0bb582327dc655371bb4432601ccde32c93584a17103fb9bfff11405bdd06bf104a70bd6a2f7e2c1604d18a660035493c5b214e54f3674dc4b333';
DELETE UserHashTable WHERE UserHash = 'b4cd967228f1b62d3445db33d45bb0614109f3d09eb0153be01d1ef7df85723861a93ec535eaa951195f35ffbcc98616de68ead9c96dc61f70fc014c7e7d1163';
DELETE UserHashTable WHERE UserHash = '0f8247112bd2a1fce9dd127aff00c2bcd854d8a2e6fd4d7d8932f44ce57bc9f00fbd5909a88ded2820ad80df9e8e065cd8dd3c3fadae51f9e8a21d51c9371799';
DELETE UserHashTable WHERE UserHash = '857298369e2f0a56577650b45e01cde183fb301042090b8dc00f790cfbb6ea9e4bc142c91413185fd9ea0a5b20e587647e4fcfc132d792320d9872715dfb53d4';

DELETE Accounts WHERE Username = 'regServiceUser1@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceUser2@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceUser3@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceUserNotConfirmedEnabled@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceUserConfirmedNotEnabled@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceUserConfirmedEnabled@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceCreateUser1@tresearch.system';
DELETE Accounts WHERE Username = 'regServiceCreateUser2@tresearch.system';