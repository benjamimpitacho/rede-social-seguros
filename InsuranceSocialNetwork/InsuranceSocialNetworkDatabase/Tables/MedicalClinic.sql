﻿CREATE TABLE [Insurance].[MedicalClinic]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
    [ContactEmail] NVARCHAR(256) NULL, 
    [MobilePhone_1] NVARCHAR(20) NOT NULL, 
    [MobilePhone_2] NVARCHAR(20) NULL, 
    [Telephone_1] NVARCHAR(20) NOT NULL, 
    [Telephone_2] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(256) NULL, 
    [ID_PostalCode] BIGINT NULL, 
    [LogoPhoto] NCHAR(10) NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_MedicalClinic] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_MedicalClinic_PostalCode] FOREIGN KEY([ID_PostalCode]) REFERENCES [Insurance].[PostalCode] ([ID])
)

