CREATE TABLE [Insurance].[PostCommentLike]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NOT NULL,
	[ID_Comment] BIGINT NOT NULL,
	[Date] DATETIME2 NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_PostCommentLike] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_PostCommentLike_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_PostCommentLike_Comment] FOREIGN KEY([ID_Comment]) REFERENCES [Insurance].[PostComment] ([ID])
)
