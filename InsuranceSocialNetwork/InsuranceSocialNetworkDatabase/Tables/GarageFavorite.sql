﻿CREATE TABLE [Insurance].[GarageFavorite]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_Garage] BIGINT NOT NULL, 
    [ID_User] NVARCHAR(128) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_GarageFavorite] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_GarageFavorite_Garage] FOREIGN KEY([ID_Garage]) REFERENCES [Insurance].[Garage] ([ID]),
	CONSTRAINT [FK_GarageFavorite_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
