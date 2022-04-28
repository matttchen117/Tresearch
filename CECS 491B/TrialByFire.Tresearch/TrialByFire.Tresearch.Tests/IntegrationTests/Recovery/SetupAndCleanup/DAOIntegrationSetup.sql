INSERT INTO Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES 
	('sqlShouldRegUser1@tresearch.systems', 'e355f3593633a62cb2db3b66a3576688ebfaf5aab904634eeebf5093daca30f0a8d734d3a844adbd87d8a3160aae51b0690085c94e9b34add2f9a5b9ccc10835', 'user', 0, 1);


INSERT INTO EmailRecoveryLinksCreated(Username, AuthorizationLevel, LinkCount) VALUES 
	('sqlShouldRegUser1@tresearch.systems', 'user', 2);