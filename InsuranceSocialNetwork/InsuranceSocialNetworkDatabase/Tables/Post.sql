CREATE TABLE [Insurance].[Post]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_PostType] BIGINT NOT NULL,
	[ID_PostSubject] BIGINT NOT NULL,
	[Text] NVARCHAR (64),
	--[Image] NVARCHAR (256),
	[Video] NVARCHAR (256),
	[URL] NVARCHAR (256),
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2,
	[Sticky] BIT NOT NULL DEFAULT 0,
	[Sponsored] BIT NOT NULL DEFAULT 0,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Post_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Post_PostType] FOREIGN KEY([ID_PostType]) REFERENCES [Insurance].[PostType] ([ID]),
	CONSTRAINT [FK_Post_PostSubject] FOREIGN KEY([ID_PostSubject]) REFERENCES [Insurance].[PostSubject] ([ID])
)
