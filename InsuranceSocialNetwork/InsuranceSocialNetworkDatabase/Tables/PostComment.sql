﻿CREATE TABLE [Insurance].[PostComment]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_Post] BIGINT NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL,
	[Date] DATETIME2 NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_PostComment] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_PostComment_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_PostComment_Post] FOREIGN KEY([ID_Post]) REFERENCES [Insurance].[Post] ([ID])
)
