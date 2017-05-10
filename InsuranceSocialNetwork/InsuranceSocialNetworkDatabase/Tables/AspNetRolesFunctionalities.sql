CREATE TABLE [dbo].[AspNetRolesFunctionalities] (
    [Id]		BIGINT IDENTITY NOT NULL,
	[RoleId]	NVARCHAR (128) NOT NULL,
    [Token]		NVARCHAR (256) NOT NULL,
	[Active]	BIT NOT NULL DEFAULT 0
);


GO
ALTER TABLE [dbo].[AspNetRolesFunctionalities]
    ADD CONSTRAINT [PK_dbo.AspNetRolesFunctionalities] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[AspNetRolesFunctionalities]
    ADD CONSTRAINT [FK_dbo.AspNetUserRolesFunctionalities_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;
GO