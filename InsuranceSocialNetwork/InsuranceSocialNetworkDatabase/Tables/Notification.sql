CREATE TABLE [Insurance].[Notification]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ToUserID] NVARCHAR(128) NOT NULL,
	[FromUserID] NVARCHAR(128) NULL,
	[ID_NotificationType] BIGINT NOT NULL,
	[Text] NVARCHAR (64),
	[URL] NVARCHAR (256),
	[CreateDate] DATETIME2 NOT NULL,
	[ReadDate] DATETIME2 NOT NULL,
	[Read] BIT NOT NULL DEFAULT 0,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NotificationToUser_AspNetUsers] FOREIGN KEY([ToUserID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_NotificationFromUser_AspNetUsers] FOREIGN KEY([FromUserID]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Notification_NotificationType] FOREIGN KEY([ID_NotificationType]) REFERENCES [Insurance].[NotificationType] ([ID])
)
