﻿CREATE TABLE [Insurance].[HomeApplianceRepair]
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
    [ID_District] BIGINT NULL, 
    [ID_County] BIGINT NULL, 
    [ID_Parish] BIGINT NULL, 
    [LogoPhoto] IMAGE NULL, 
    [Website] NVARCHAR(256) NULL,
    [OfficialPartner] NVARCHAR(256) NULL, 
    [OfficialAgent] NVARCHAR(256) NULL,  
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_HomeApplianceRepair] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_HomeApplianceRepair_District] FOREIGN KEY([ID_District]) REFERENCES [Insurance].[District] ([ID]),
	CONSTRAINT [FK_HomeApplianceRepair_County] FOREIGN KEY([ID_County]) REFERENCES [Insurance].[County] ([ID]),
	CONSTRAINT [FK_HomeApplianceRepair_Parish] FOREIGN KEY([ID_Parish]) REFERENCES [Insurance].[Parish] ([ID])
)
