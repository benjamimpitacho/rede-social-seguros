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
