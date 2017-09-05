﻿CREATE TABLE [Insurance].[Profile]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_User] NVARCHAR(128) UNIQUE NOT NULL, 
    [FirstName] NVARCHAR(32) NULL, 
    [LastName] NVARCHAR(32) NULL, 
    [ContactEmail] NVARCHAR(256) NULL, 
    [Birthdate] DATETIME2 NULL, 
    [MobilePhone_1] NVARCHAR(20) NULL, 
    [MobilePhone_2] NVARCHAR(20) NULL, 
    [Telephone_1] NVARCHAR(20) NULL, 
    [Telephone_2] NVARCHAR(20) NULL, 
    [Address] NVARCHAR(512) NULL, 
	[AboutMe] NVARCHAR(max) NULL,
	[Website] NVARCHAR(max) NULL,
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

