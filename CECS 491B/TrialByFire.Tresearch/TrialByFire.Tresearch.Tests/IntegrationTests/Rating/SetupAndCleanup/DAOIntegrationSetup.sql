Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) VALUES
			('RateDAOUser1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1),
			('RateAnotherUserDAO1@tresearch.system', '2d1e06f30c62efb95c118ac7adf4f0922d8d47ef1a4b26a51646de990b074cb019761095d391b2f5a33b864ab984f868180d317f1c7aac94b0cc099134a1219a', 'user', 1, 1);

INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, 'a4adfe7c09a5ff48c6d9ad3a0e5b783d8cf566a335ac012a31d8ff605e3c34dcb471fef37457d1e13071d7cbc93242d9e5220a9ee3d880bd25c9a514b0bb0834'
		FROM Accounts WHERE Username = 'RateDAOUser1@tresearch.system';
INSERT UserHashTable(UserID, UserHash)
	SELECT Accounts.UserID, '50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614'
		FROM Accounts WHERE Username = 'RateAnotherUserDAO1@tresearch.system';

INSERT Nodes(UserHash, NodeID, NodeParentID, NodeTitle, Summary, Visibility) VALUES 
	('50a9ae613eb58433a6562ea07695a77063c586e4ad05fa6b327a67dba6a03b4c69e6e3ae47f2ce0fb640decccf721354676e3fda38cc3b161e8f8352e2833614', 5091676250, null, 'CECS 491A', 'Extremely hard class at CSULB', 0);