CREATE TABLE [Insurance].[Friend]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_User_Friend] NVARCHAR(128) NOT NULL,
	[ID_FriendStatus] BIGINT NOT NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_Friend] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_FriendUser_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_FriendUserFriend_AspNetUsers] FOREIGN KEY([ID_User_Friend]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_FriendStatus_FriendStatus] FOREIGN KEY([ID_FriendStatus]) REFERENCES [Insurance].[FriendStatus] ([ID])
)
