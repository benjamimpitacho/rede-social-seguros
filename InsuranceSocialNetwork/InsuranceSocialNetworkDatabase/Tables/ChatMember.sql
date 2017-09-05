﻿CREATE TABLE [Insurance].[ChatMember]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_Chat] BIGINT NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_ChatMember] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ChatMember_Chat] FOREIGN KEY([ID_Chat]) REFERENCES [Insurance].[Chat] ([ID]),
	CONSTRAINT [FK_ChatMember_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
