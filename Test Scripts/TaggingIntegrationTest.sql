----- Tagging Service

DELETE NodeTags where NodeID = 67892
DELETE NodeTags where NodeID = 67891
DELETE NodeTags where NodeID = 67890
DELETE NodeTags where NodeID = 67895
DELETE NodeTags where NodeID = 67894
DELETE NodeTags where NodeID = 67893
DELETE NodeTags where NodeID = 67896
DELETE NodeTags where NodeID = 67897

DELETE Nodes WHERE NodeID = 67892
DELETE Nodes WHERE NodeID = 67891
DELETE Nodes WHERE NodeID = 67890
DELETE Nodes where NodeID = 67895
DELETE Nodes where NodeID = 67894
DELETE Nodes where NodeID = 67893
DELETE Nodes where NodeID = 67896
DELETE Nodes where NodeID = 67897

DELETE Accounts WHERE Username = '82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b'
DELETE Accounts WHERE Username = 'd1de2d978dc3d116c99227968b482f6fd07784125a9db4eb9b0f4e6227301159a9fabf98138b883ad22f3308e0b0e2fe6d5f0898f3320ca6a93ea563a409a540'
DELETE Accounts WHERE Username = '24629aea2c70e9164d7cc6a785a4be9767ccdf0677b08165225d082b4c42b5f31bfd3ad95d8954e4e2849d8ed7ec68781f26c47383a42b7ad22132a83db067f7'
DELETE Accounts WHERE Username = '2f702a4a224896d858a402b8745075661bf1783beb4deb5a8786e427ec0f17b1b82c03f20a02d60243cae36be4bcb96d2c84b8bf6017989e09aa9f5b87c228ad'
DELETE Accounts WHERE Username = '662cb1b5e1b520d14d4eba7e24c27203b0650c170200fdff83faa9f088e345c8fd33ef425c485f39219329eeba92830b61951b06ee33225352bb1ed56add22b7'
DELETE Accounts WHERE Username = 'fcf4e639bb166ec8bac11283c8fbd82a147bfb3c0c2ee53b0dd8cc1d8ece71d425e95262a049299b37131f1b75524af7da53c3c88f23c6e578016ff2473e75cc'
DELETE Accounts WHERE Username = '26dbe1b5e112224c44d2316e51732cfa57a33e6031e54158c464e137905d9f955cca17f0832a4625297f9010bb1a8a262727f6da5d71528dea4566688062b0fe'
DELETE Accounts WHERE Username = '922af9995cba547f23f3cd24d5fb8f901d24e656ff0726e2b10bd6edf03c13186ea8e3bad192930950423f2d39afef51ad9447962b96adf179e916c56707e1bf'
DELETE Accounts WHERE Username = '3abc1413abbe2d2b92ecebb8a18dbd093e87e7c3fb80d266df3bb7639b127da39bd2cf4ff15b8a0f27a30aed08e229b41f21a20d63db7dcc9f14398c6f0a734a'
DELETE Accounts WHERE Username = '501887a12f175f45dc7c80e21f5ffc1f8ab46e8004e755595d3d0e08c851dacab130573f4f531353eefcd7ed3314bd0aded194c3934387db986e8c80ea7fb356'

DELETE UserHashTable WHERE UserID = 'tagService1@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService2@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService3@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService4@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService5@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService6@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService7@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService8@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService9@gmail.com'
DELETE UserHashTable WHERE UserID = 'tagService10@gmail.com'



Insert UserHashTable(UserID, UserHash) 
	Values('tagService1@gmail.com', '82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b'),
		  ('tagService2@gmail.com', 'd1de2d978dc3d116c99227968b482f6fd07784125a9db4eb9b0f4e6227301159a9fabf98138b883ad22f3308e0b0e2fe6d5f0898f3320ca6a93ea563a409a540'),
		  ('tagService3@gmail.com', '24629aea2c70e9164d7cc6a785a4be9767ccdf0677b08165225d082b4c42b5f31bfd3ad95d8954e4e2849d8ed7ec68781f26c47383a42b7ad22132a83db067f7'),
		  ('tagService4@gmail.com', '2f702a4a224896d858a402b8745075661bf1783beb4deb5a8786e427ec0f17b1b82c03f20a02d60243cae36be4bcb96d2c84b8bf6017989e09aa9f5b87c228ad'),
		  ('tagService5@gmail.com', '662cb1b5e1b520d14d4eba7e24c27203b0650c170200fdff83faa9f088e345c8fd33ef425c485f39219329eeba92830b61951b06ee33225352bb1ed56add22b7'),
		  ('tagService6@gmail.com', 'fcf4e639bb166ec8bac11283c8fbd82a147bfb3c0c2ee53b0dd8cc1d8ece71d425e95262a049299b37131f1b75524af7da53c3c88f23c6e578016ff2473e75cc'),
		  ('tagService7@gmail.com', '26dbe1b5e112224c44d2316e51732cfa57a33e6031e54158c464e137905d9f955cca17f0832a4625297f9010bb1a8a262727f6da5d71528dea4566688062b0fe'),
		  ('tagService8@gmail.com', '922af9995cba547f23f3cd24d5fb8f901d24e656ff0726e2b10bd6edf03c13186ea8e3bad192930950423f2d39afef51ad9447962b96adf179e916c56707e1bf'),
		  ('tagService9@gmail.com', '3abc1413abbe2d2b92ecebb8a18dbd093e87e7c3fb80d266df3bb7639b127da39bd2cf4ff15b8a0f27a30aed08e229b41f21a20d63db7dcc9f14398c6f0a734a'),
		  ('tagService10@gmail.com', '501887a12f175f45dc7c80e21f5ffc1f8ab46e8004e755595d3d0e08c851dacab130573f4f531353eefcd7ed3314bd0aded194c3934387db986e8c80ea7fb356')


Insert Accounts(Username, Passphrase, AuthorizationLevel, AccountStatus, Confirmed)
	VALUES ('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
		   ('d1de2d978dc3d116c99227968b482f6fd07784125a9db4eb9b0f4e6227301159a9fabf98138b883ad22f3308e0b0e2fe6d5f0898f3320ca6a93ea563a409a540', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
	       ('24629aea2c70e9164d7cc6a785a4be9767ccdf0677b08165225d082b4c42b5f31bfd3ad95d8954e4e2849d8ed7ec68781f26c47383a42b7ad22132a83db067f7', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
	       ('2f702a4a224896d858a402b8745075661bf1783beb4deb5a8786e427ec0f17b1b82c03f20a02d60243cae36be4bcb96d2c84b8bf6017989e09aa9f5b87c228ad', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
	       ('662cb1b5e1b520d14d4eba7e24c27203b0650c170200fdff83faa9f088e345c8fd33ef425c485f39219329eeba92830b61951b06ee33225352bb1ed56add22b7', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
	       ('fcf4e639bb166ec8bac11283c8fbd82a147bfb3c0c2ee53b0dd8cc1d8ece71d425e95262a049299b37131f1b75524af7da53c3c88f23c6e578016ff2473e75cc', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
	       ('26dbe1b5e112224c44d2316e51732cfa57a33e6031e54158c464e137905d9f955cca17f0832a4625297f9010bb1a8a262727f6da5d71528dea4566688062b0fe', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
		   ('922af9995cba547f23f3cd24d5fb8f901d24e656ff0726e2b10bd6edf03c13186ea8e3bad192930950423f2d39afef51ad9447962b96adf179e916c56707e1bf', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
		   ('3abc1413abbe2d2b92ecebb8a18dbd093e87e7c3fb80d266df3bb7639b127da39bd2cf4ff15b8a0f27a30aed08e229b41f21a20d63db7dcc9f14398c6f0a734a', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1),
		   ('501887a12f175f45dc7c80e21f5ffc1f8ab46e8004e755595d3d0e08c851dacab130573f4f531353eefcd7ed3314bd0aded194c3934387db986e8c80ea7fb356', 'c6a2d4e73f1544ab7d81eb0946aac9e575809fb7aa72ecb530f6413cf3b69fa2880abb702b80809a5df090afb97b094f31ae1bf6eeef88ed28bb7e663a055208', 'user', 1, 1)

	--tagService1@gmail.com
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67890,
			67890,
			'Underwater basket weaving',
			'hardest class at csulb',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67891,
			67890,
			'Subject 1',
			'easy starter subject',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67892,
			67890,
			'Subject 2',
			'not so easy starter subject',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67893,
			67893,
			'Above water weaving',
			'hardest class at csulb',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67894,
			67893,
			'Subject 1',
			'easy starter subject',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67895,
			67894,
			'Subject 2',
			'not so easy starter subject',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b',
			'user',
			67896,
			67895,
			'Subject 3',
			'hard subject',
			1)
INSERT Nodes(Username, AuthorizationLevel, NodeID, NodeParentID, NodeTitle, Summary, Visibility)
	VALUES('26dbe1b5e112224c44d2316e51732cfa57a33e6031e54158c464e137905d9f955cca17f0832a4625297f9010bb1a8a262727f6da5d71528dea4566688062b0fe',
			'user',
			67897,
			67897,
			'CECS 491B',
			'interesting class',
			1)
-- service add case 0
INSERT NodeTags(NodeID, TagName)
	VALUES(67890, 'music'), (67891, 'music'), (67892, 'music')
--service remove case 0
INSERT NodeTags(NodeID, TagName)
	VALUES(67890, 'gym'), (67891, 'gym'), (67892, 'gym')
--service remove case 1
INSERT NodeTags(NodeID, TagName) VALUES (67890, 'myth')


INSERT NodeTags(NodeID, TagName)
	VALUES(67890, 'comedy'), (67891, 'comedy')

INSERT NodeTags(NodeID, TagName)
	VALUES(67893, 'craft'), (67894, 'craft'), (67895, 'craft'),
		  (67893, 'crypto'), (67894, 'crypto'),
		  (67893, 'fries'), 
		  (67894, 'gear')

SELECT * FROM Accounts
SELECT * FROM UserHashTable
SELECT * FROM Nodes
SELECT * FROM NodeTags
SELECT * FROM Tags