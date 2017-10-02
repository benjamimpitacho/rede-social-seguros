CREATE TABLE [Insurance].[HomeApplianceRepairFavorite]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_HomeApplianceRepair] BIGINT NOT NULL, 
    [ID_User] NVARCHAR(128) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_HomeApplianceRepairFavorite] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_HomeApplianceRepairFavorite_HomeApplianceRepair] FOREIGN KEY([ID_HomeApplianceRepair]) REFERENCES [Insurance].[HomeApplianceRepair] ([ID]),
	CONSTRAINT [FK_HomeApplianceRepairFavorite_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

