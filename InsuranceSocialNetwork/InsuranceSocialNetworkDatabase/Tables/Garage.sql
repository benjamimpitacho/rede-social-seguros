﻿CREATE TABLE [Insurance].[Garage]
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
	[Fax] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(512) NULL,
	[PostalCode] NVARCHAR(10) NULL,  
    [ID_Parish] BIGINT NULL,  
    [ID_County] BIGINT NULL, 
    [ID_District] BIGINT NULL,
	
	[SameInformationForInvoice] BIT NOT NULL DEFAULT 1,
    [Invoice_Address] NVARCHAR(512) NULL,
	[Invoice_PostalCode] NVARCHAR(10) NULL,  
    [Invoice_ID_Parish] BIGINT NULL,  
    [Invoice_ID_County] BIGINT NULL, 
    [Invoice_ID_District] BIGINT NULL,

	[ID_Service] BIGINT NULL, 
    [LogoPhoto] IMAGE NULL, 
    [Website] NVARCHAR(256) NULL,
    [OfficialPartner] NVARCHAR(256) NULL, 
    [OfficialAgent] NVARCHAR(256) NULL,  
	[BusinessName] NVARCHAR(256) NULL,  
	[IBAN] NVARCHAR(256) NULL,  
	[LibaxEntityID] INT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Garage] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Garage_District] FOREIGN KEY([ID_District]) REFERENCES [Insurance].[District] ([ID]),
	CONSTRAINT [FK_Garage_County] FOREIGN KEY([ID_County]) REFERENCES [Insurance].[County] ([ID]),
	CONSTRAINT [FK_Garage_Parish] FOREIGN KEY([ID_Parish]) REFERENCES [Insurance].[Parish] ([ID]),
	CONSTRAINT [FK_Garage_Service] FOREIGN KEY([ID_Service]) REFERENCES [Insurance].[CompanyService] ([ID]),
	CONSTRAINT [FK_Garage_InvoiceDistrict] FOREIGN KEY([Invoice_ID_District]) REFERENCES [Insurance].[District] ([ID]),
	CONSTRAINT [FK_Garage_InvoiceCounty] FOREIGN KEY([Invoice_ID_County]) REFERENCES [Insurance].[County] ([ID]),
	CONSTRAINT [FK_Garage_InvoiceParish] FOREIGN KEY([Invoice_ID_Parish]) REFERENCES [Insurance].[Parish] ([ID])
)

