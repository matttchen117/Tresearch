----- Tagging Service
	DELETE NodeTags WHERE NodeID = 1
	DELETE NodeTags WHERE NodeID = 2
	DELETE NodeTags WHERE NodeID = 3

	Insert NodeTags(NodeID, TagName) VALUES (1, 'archery'), (2, 'archery'), (3, 'archery');
	Insert NodeTags(NodeID, TagName) VALUES (1, 'calculus');




	DELETE Tags where TagName = 'CECS 491B';
	DELETE Tags where TagName = 'Trial By Fire';
	DELETE Tags where TagName = 'CSULB';
	DELETE Tags where TagName = 'Long Beach';
	DELETE NodeTags where TagName = 'Oh no Im tagged';
	DELETE Tags where TagName = 'Oh no Im tagged';

	Insert Tags(TagName) VALUES ('Trial By Fire');
	Insert Tags(TagName) VALUES ('CSULB');
	Insert Tags(TagName) VALUES ('Oh no Im tagged');

	INSERT NodeTags(NodeID, TagName) VALUES (1, 'Oh no Im tagged');
