CREATE TABLE [Insurance].[ChatNotification]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_Chat] BIGINT NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ReceiveNotifications] BIT NOT NULL DEFAULT 1,
	[LastUpdateDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_ChatNotification] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ChatNotification_Chat] FOREIGN KEY([ID_Chat]) REFERENCES [Insurance].[Chat] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ChatNotification_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)
