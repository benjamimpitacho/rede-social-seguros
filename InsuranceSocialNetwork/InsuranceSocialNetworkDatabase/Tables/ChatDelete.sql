CREATE TABLE [Insurance].[ChatDelete]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_Chat] BIGINT NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[LastChatDeleteDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_ChatDelete] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ChatDelete_Chat] FOREIGN KEY([ID_Chat]) REFERENCES [Insurance].[Chat] ([ID]),
	CONSTRAINT [FK_ChatDelete_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
