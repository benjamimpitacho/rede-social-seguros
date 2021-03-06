﻿CREATE TABLE [Insurance].[NotificationType]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[Token] NVARCHAR (64) NOT NULL,
	[Description] NVARCHAR (256) NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED ([ID] ASC)
)
