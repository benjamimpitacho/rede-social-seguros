CREATE TABLE [Insurance].[ProfileSettings]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_Profile] BIGINT UNIQUE NOT NULL, 
    [ShowDisplayName] BIT NOT NULL DEFAULT 1, 
    [ShowBirthDate] BIT NOT NULL DEFAULT 1, 
    [ShowContactInformation] BIT NOT NULL DEFAULT 1,
	[ShowSocialNetworks] BIT NOT NULL DEFAULT 1,
    [LikesOnYourPosts] BIT NOT NULL DEFAULT 1, 
	[CommentsOnYourPosts] BIT NOT NULL DEFAULT 1,
	[ReceiveNotificationsByEmail] BIT NOT NULL DEFAULT 1, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_ProfileSettings] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProfileSettings_Profile] FOREIGN KEY([ID_Profile]) REFERENCES [Insurance].[Profile] ([ID])
)

