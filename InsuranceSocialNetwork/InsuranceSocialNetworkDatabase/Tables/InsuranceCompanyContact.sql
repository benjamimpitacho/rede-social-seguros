CREATE TABLE [Insurance].[InsuranceCompanyContact]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
    [Description] NVARCHAR(256) NULL,
	[NIF] NVARCHAR(16) NOT NULL, 
    [ContactEmail] NVARCHAR(256) NULL, 
    [MobilePhone_1] NVARCHAR(20) NOT NULL, 
    [MobilePhone_2] NVARCHAR(20) NULL, 
    [Telephone_1] NVARCHAR(20) NOT NULL, 
    [Telephone_2] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(256) NULL, 
    [ID_PostalCode] BIGINT NULL, 
    [LogoPhoto] IMAGE NULL, 
    [Website] NVARCHAR(256) NULL,
    [OficialPartner] NVARCHAR(256) NULL, 
    [OficialAgent] NVARCHAR(256) NULL,  
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_InsuranceCompanyContact] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_InsuranceCompanyContact_PostalCode] FOREIGN KEY([ID_PostalCode]) REFERENCES [Insurance].[PostalCode] ([ID])
)

