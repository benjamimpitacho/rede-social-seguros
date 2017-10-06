CREATE TABLE [Insurance].[CompanyService]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ID_CompanyType] BIGINT NOT NULL,
	[Description] NVARCHAR (256) NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1,	
    CONSTRAINT [PK_CompanyService] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Company_Type] FOREIGN KEY([ID_CompanyType]) REFERENCES [Insurance].[CompanyType] ([ID])
)
