/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--DECLARE @userId nvarchar(128);
DECLARE @postTypeId bigint;
DECLARE @postSubjectId bigint;

SELECT @postTypeId = Id FROM [Insurance].[PostType] WHERE Token='TEXT_POST';
SELECT @postSubjectId = Id FROM [Insurance].[PostSubject] WHERE Token='PERSONAL_POST';

SET @userId = CONVERT(nvarchar(128), NEWID());

INSERT [dbo].[AspNetUsers] (
	[Id], 
	[AccessFailedCount],
	[Email], 
	[EmailConfirmed], 
	[LockoutEnabled], 
	[LockoutEndDateUtc],  
	[PasswordHash], 
	[PhoneNumber], 
	[PhoneNumberConfirmed], 
	[SecurityStamp], 
	[TwoFactorEnabled], 
	[UserName]
) 
VALUES 
(
	@userId, 
	0, 
	N'user1@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'user1@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('2',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'User1')

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #1!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #2!!',GETDATE(),GETDATE());

SET @userId = CONVERT(nvarchar(128), NEWID());

INSERT [dbo].[AspNetUsers] (
	[Id], 
	[AccessFailedCount],
	[Email], 
	[EmailConfirmed], 
	[LockoutEnabled], 
	[LockoutEndDateUtc],  
	[PasswordHash], 
	[PhoneNumber], 
	[PhoneNumberConfirmed], 
	[SecurityStamp], 
	[TwoFactorEnabled], 
	[UserName]
) 
VALUES 
(
	@userId, 
	0, 
	N'user2@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'user2@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('2',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'User2')

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #3!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #4!!',GETDATE(),GETDATE());

SET @userId = CONVERT(nvarchar(128), NEWID());

INSERT [dbo].[AspNetUsers] (
	[Id], 
	[AccessFailedCount],
	[Email], 
	[EmailConfirmed], 
	[LockoutEnabled], 
	[LockoutEndDateUtc],  
	[PasswordHash], 
	[PhoneNumber], 
	[PhoneNumberConfirmed], 
	[SecurityStamp], 
	[TwoFactorEnabled], 
	[UserName]
) 
VALUES 
(
	@userId, 
	0, 
	N'user3@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'user3@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('2',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'User3')

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #5!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #6!!',GETDATE(),GETDATE());

SET @userId = CONVERT(nvarchar(128), NEWID());

INSERT [dbo].[AspNetUsers] (
	[Id], 
	[AccessFailedCount],
	[Email], 
	[EmailConfirmed], 
	[LockoutEnabled], 
	[LockoutEndDateUtc],  
	[PasswordHash], 
	[PhoneNumber], 
	[PhoneNumberConfirmed], 
	[SecurityStamp], 
	[TwoFactorEnabled], 
	[UserName]
) 
VALUES 
(
	@userId, 
	0, 
	N'user4@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'user4@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('2',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'User4')

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #7!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #8!!',GETDATE(),GETDATE());

SET @userId = CONVERT(nvarchar(128), NEWID());

INSERT [dbo].[AspNetUsers] (
	[Id], 
	[AccessFailedCount],
	[Email], 
	[EmailConfirmed], 
	[LockoutEnabled], 
	[LockoutEndDateUtc],  
	[PasswordHash], 
	[PhoneNumber], 
	[PhoneNumberConfirmed], 
	[SecurityStamp], 
	[TwoFactorEnabled], 
	[UserName]
) 
VALUES 
(
	@userId, 
	0, 
	N'user5@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'user5@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('2',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'User5')

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #9!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES (@userId,@postTypeId,@postSubjectId,'Este post é um teste dummy - #10!!',GETDATE(),GETDATE());


-- Posts for Administrator

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES ((SELECT [Id] FROM [dbo].[AspNetUsers] WHERE [Email]='admin@falarseguros.pt'),@postTypeId,@postSubjectId,'Este post é um teste dummy - #admin1!!',GETDATE(),GETDATE());

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES ((SELECT [Id] FROM [dbo].[AspNetUsers] WHERE [Email]='admin@falarseguros.pt'),@postTypeId,@postSubjectId,'Este post é um teste dummy - #admin2!!',GETDATE(),GETDATE());

SELECT @postSubjectId = Id FROM [Insurance].[PostSubject] WHERE Token='SPONSORED_POST';

INSERT [Insurance].[Post] ([ID_User],[ID_PostType],[ID_PostSubject],[Text],[CreateDate],[LastChangeDate])
	VALUES ((SELECT [Id] FROM [dbo].[AspNetUsers] WHERE [Email]='admin@falarseguros.pt'),@postTypeId,@postSubjectId,'Este post é um teste sponsor dummy - #admin3!!',GETDATE(),GETDATE());


INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (1,'Banner Topo #1',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF50000399749444154,GETDATE(),GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (1,'Banner Topo #2',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF5000039EC49444154,GETDATE(),GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (3,'Banner Lateral #1',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF50000399749444154,GETDATE(),GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (3,'Banner Lateral #2',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF5000039EC49444154,GETDATE(),GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (3,'Banner Lateral #3',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF50000399749444154,GETDATE(),GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Banner] ([ID_Banner_Type],[Description],[Image],[StartDate],[CreateDate],[LastChangeDate],[Active]) VALUES (3,'Banner Lateral #4',0x89504E470D0A1A0A0000000D49484452000001F40000007808020000008BB197CA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001974455874536F667477617265007061696E742E6E657420342E302E31364469AFF5000039EC49444154,GETDATE(),GETDATE(),GETDATE(),1);


INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #1', 'Descrição da garagem numero #1.','123456789','garagem1@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº10 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #2', 'Descrição da garagem numero #2.','123456789','garagem2@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº11 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #3', 'Descrição da garagem numero #3.','123456789','garagem3@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº12 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #4', 'Descrição da garagem numero #4.','123456789','garagem4@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº13 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #5', 'Descrição da garagem numero #5.','123456789','garagem5@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº14 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
INSERT INTO [Insurance].[Garage] ([Name],[Description],[NIF],[ContactEmail],[MobilePhone_1],[MobilePhone_2],[Telephone_1],[Telephone_2],[Address],[Website],[OfficialPartner],[OfficialAgent],[CreateDate],[LastChangeDate],[Active]) VALUES ('Garagem #6', 'Descrição da garagem numero #6.','123456789','garagem6@falarseguros.com','919876543','931234567','219845678','229647893','Avenida da Liberdade, nº15 Lisboa','www.google.com','Parceiro oficial Falar Seguros','Agente oficial Falar Seguros',GETDATE(),GETDATE(),1);
