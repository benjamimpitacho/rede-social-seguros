CREATE TABLE [Insurance].[ConstructionCompanyFavorite]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_ConstructionCompany] BIGINT NOT NULL, 
    [ID_User] NVARCHAR(128) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_ConstructionCompanyFavorite] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ConstructionCompanyFavorite_ConstructionCompany] FOREIGN KEY([ID_ConstructionCompany]) REFERENCES [Insurance].[ConstructionCompany] ([ID]),
	CONSTRAINT [FK_ConstructionCompanyFavorite_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

