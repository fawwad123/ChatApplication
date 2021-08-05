CREATE TABLE [User] (
	Id INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(150) NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50) NULL,
	LastName VARCHAR(50) NOT NULL,
	DateOfBirth DATE NOT NULL,
	Email VARCHAR(255) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	token VARCHAR(MAX) NULL,
	IsActive bit NOT NULL,
	CreatedOn DateTime NOT NULL,
	ModifiedOn DateTime NOT NULL
	CONSTRAINT PK_User PRIMARY KEY (Id)
);

CREATE TABLE [Group](
	Id INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(255) NOT NULL,
	CreatedBy INT NOT NULL,
	CreatedOn DateTime NOT NULL,
	ModifiedBy INT NOT NULL,
	ModifiedOn DateTime NOT NULL,
	CONSTRAINT PK_Group PRIMARY KEY (Id),
	CONSTRAINT FK_User_Group_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [User](Id),
	CONSTRAINT FK_User_Group_ModifiedBy FOREIGN KEY (ModifiedBy) REFERENCES [User](Id)

);

CREATE TABLE [UserGroup](
	Id INT IDENTITY(1,1) NOT NULL,
	UserId INT NOT NULL,
	GroupId INT NOT NULL,
	isAdmin bit null,
	CONSTRAINT PK_UserGroup PRIMARY KEY (Id),
	CONSTRAINT FK_User_UserGroup_UserId FOREIGN KEY (UserId) REFERENCES [User](Id),
	CONSTRAINT FK_User_UserGroup_GroupId FOREIGN KEY (GroupId) REFERENCES [Group](Id)
);

CREATE TABLE [GroupConversation](
	Id INT IDENTITY(1,1) NOT NULL,
	GroupId INT NOT NULL,
	Message NVARCHAR(MAX) NOT NULL,
	MessageBy int NOT NULL,
	MessageOn DATETIME NOT NULL,
	IsDeleted bit NULL,
	CONSTRAINT PK_GroupConversation PRIMARY KEY (Id),
	CONSTRAINT FK_Group_GroupConversation FOREIGN KEY (GroupId) REFERENCES [Group](Id),
	CONSTRAINT FK_User_GroupConversation FOREIGN KEY (MessageBy) REFERENCES [User](Id)
);

CREATE TABLE [UserContact](
	Id INT IDENTITY(1,1) NOT NULL,
	UserId INT NOT NULL,
	ContactId INT NOT NULL,
	IsBlocked bit NULL,
	[Name] VARCHAR(50),
	CONSTRAINT PK_UserContact PRIMARY KEY (Id),
	CONSTRAINT FK_User_UserContact_UserId FOREIGN KEY (UserId) REFERENCES [User](Id),
	CONSTRAINT FK_User_UserContact_ContactId FOREIGN KEY (ContactId) REFERENCES [User](Id),
);

CREATE TABLE [Conversation](
	Id INT IDENTITY(1,1) NOT NULL,
	UserContactId INT NOT NULL,
	Message VARCHAR(MAX) NOT NULL,
	MessageBy int NOT NULL,
	MessageOn DATETIME NOT NULL,
	IsDeleted bit NULL,
	CONSTRAINT PK_Conversation PRIMARY KEY (Id),
	CONSTRAINT FK_Conversation_UserContact FOREIGN KEY (UserContactId) REFERENCES [UserContact](Id),
	CONSTRAINT FK_User_Conversation FOREIGN KEY (MessageBy) REFERENCES [User](Id)
);