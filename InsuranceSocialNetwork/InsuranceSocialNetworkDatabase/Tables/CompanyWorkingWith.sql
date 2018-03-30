CREATE TABLE [Insurance].[CompanyWorkingWith]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_User] NVARCHAR(128) NULL,
	[ID_CompanyType] BIGINT NOT NULL,
	[ID_Company] BIGINT NOT NULL,
	[Order] INT NOT NULL,
	[CreateDate] DATETIME2 NOT NULL,
	[LastChangeDate] DATETIME2 NOT NULL,
	[DeleteDate] DATETIME2 NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_CompanyWorkingWith] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_CompanyWorkingWith_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_CompanyWorkingWith_CompanyType] FOREIGN KEY([ID_CompanyType]) REFERENCES [Insurance].[CompanyType] ([Id])
)
