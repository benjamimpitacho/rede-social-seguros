﻿CREATE TABLE [Insurance].[Profile]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_User] NVARCHAR(128) UNIQUE NOT NULL, 
    [FirstName] NVARCHAR(32) NULL, 
    [LastName] NVARCHAR(32) NULL, 
    [ContactEmail] NVARCHAR(256) NULL, 
    [Birthdate] DATETIME2 NULL, 
	[CompanyName] NVARCHAR(256) NULL, 
    [MobilePhone_1] NVARCHAR(20) NULL, 
    [MobilePhone_2] NVARCHAR(20) NULL, 
    [Telephone_1] NVARCHAR(20) NULL, 
    [Telephone_2] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(512) NULL, 
	[PostalCode] NVARCHAR(10) NULL, 
	[Locality] NVARCHAR(64) NULL, 
	[County] NVARCHAR(64) NULL, 
	[District] NVARCHAR(64) NULL, 
	[AboutMe] NVARCHAR(max) NULL,
	[Facebook] NVARCHAR(512) NULL, 
	[Twitter] NVARCHAR(512) NULL, 
	[GooglePlus] NVARCHAR(512) NULL, 
	[Skype] NVARCHAR(32) NULL, 
	[Whatsapp] NVARCHAR(32) NULL, 
	[Website] NVARCHAR(max) NULL,
	[ProfessionalNumber] NVARCHAR(64) NULL,
	[CompaniesWorkingWith] NVARCHAR(512) NULL,
    [ID_PostalCode] BIGINT NULL, 
    [ProfilePhoto] IMAGE NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Profile_AspNetUsers] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id]),
	CONSTRAINT [FK_Profile_PostalCode] FOREIGN KEY([ID_PostalCode]) REFERENCES [Insurance].[PostalCode] ([ID])
)

