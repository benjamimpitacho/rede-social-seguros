﻿CREATE TABLE [Insurance].[Post]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_PostType] BIGINT NOT NULL,
	[ID_PostSubject] BIGINT NOT NULL,
	[Title] NVARCHAR (256),
	[Text] NVARCHAR (MAX),
	[Video] NVARCHAR (256),
	[URL] NVARCHAR (MAX),
	[URL_Domain] NVARCHAR (MAX),
	[URL_Title] NVARCHAR (MAX),
	[URL_Description] NVARCHAR (MAX),
	[URL_Image_Address] NVARCHAR (MAX),
	[IsRepost] BIT NOT NULL DEFAULT 0,
	[Repost_Text] NVARCHAR (MAX),
	[Repost_PostID] BIGINT NULL,
	[Repost_ProfileID] BIGINT NULL,  
    [ID_County] BIGINT NULL, 
    [ID_District] BIGINT NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2,
	[Sticky] BIT NOT NULL DEFAULT 0,
	[Sponsored] BIT NOT NULL DEFAULT 0,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Post_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Post_PostType] FOREIGN KEY([ID_PostType]) REFERENCES [Insurance].[PostType] ([ID]),
	CONSTRAINT [FK_Post_PostSubject] FOREIGN KEY([ID_PostSubject]) REFERENCES [Insurance].[PostSubject] ([ID]),
	CONSTRAINT [FK_Post_District] FOREIGN KEY([ID_District]) REFERENCES [Insurance].[District] ([ID]),
	CONSTRAINT [FK_Post_County] FOREIGN KEY([ID_County]) REFERENCES [Insurance].[County] ([ID]),
	CONSTRAINT [FK_Post_Post] FOREIGN KEY([Repost_PostID]) REFERENCES [Insurance].[Post] ([ID])
)
