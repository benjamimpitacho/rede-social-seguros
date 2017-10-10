﻿CREATE TABLE [Insurance].[AuthorizedEmail]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NULL,
	[Email] NVARCHAR(512) NOT NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2 NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_AuthorizedEmail] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_AuthorizedEmail_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
