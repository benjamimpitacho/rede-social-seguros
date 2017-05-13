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
	N'APyxnVe8q6Jg5S6N4QpDb8OYU5ommCD2rxv5NSXMRGvjdkSWZ9CZNT9Dr6/7ejADqQ==', 
	NULL, 
	0, 
	N'c85f3301-03a8-42f1-b190-9d8469f1b652', 
	0, 
	N'admin@falarseguros.pt'
)

INSERT [dbo].[AspNetUserRoles] (
	RoleId,
	UserId
)
VALUES
(
	'1',
	@userId
)

INSERT [Insurance].[Profile]
(
	[ID_User],
	[FirstName]
)
VALUES
(
	@userId,
	N'Administrator'
)
