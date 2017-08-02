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

DECLARE @userId nvarchar(128);

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
	N'admin@falarseguros.pt', 
	1, 
	1, 
	NULL, 
	N'AAGUbgyLIPaeECvgdADYyv1OQFRxCUcKWGIizG4OpgaQe3BXuKHDOEHqB+joV8lEDA==', 
	NULL, 
	0, 
	N'c10e3574-b1ca-434a-a984-8d592a6bd4d1', 
	0, 
	N'admin@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (RoleId,UserId) VALUES ('1',@userId)

INSERT [Insurance].[Profile]([ID_User],[FirstName]) VALUES (@userId,N'Administrator')

