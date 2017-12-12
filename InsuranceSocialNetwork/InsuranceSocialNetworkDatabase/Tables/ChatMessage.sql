﻿CREATE TABLE [Insurance].[ChatMessage]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_Chat] BIGINT NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[Text] NVARCHAR (MAX) NULL,
	[Image] IMAGE NULL,
	[ReadDate] DATETIME2 NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ChatMessage_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_ChatMessage_Chat] FOREIGN KEY([ID_Chat]) REFERENCES [Insurance].[Chat] ([ID])
)
