CREATE TABLE [Insurance].[Notification]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_NotificationType] BIGINT NOT NULL,
	[Text] NVARCHAR (64),
	[URL] NVARCHAR (256),
	[CreateDate] DATETIME2 NOT NULL,
	[ReadDate] DATETIME2 NOT NULL,
	[Read] BIT NOT NULL DEFAULT 0,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Notification_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([ID_NotificationType]) REFERENCES [Insurance].[NotificationType] ([ID])
)
