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

:r .\InitData\AspNetRoles.sql
:r .\InitData\NotificationType.sql
:r .\InitData\PostSubject.sql
:r .\InitData\PostType.sql
:r .\InitData\AspNetRolesFunctionalities.sql
:r .\InitData\AspNetUsers.sql
:r .\InitData\FriendStatus.sql
:r .\InitData\BannerType.sql
:r .\InitData\District.sql
:r .\InitData\County.sql
:r .\InitData\Parish.sql
:r .\InitData\CompanyType.sql
:r .\InitData\CompanyService.sql
:r .\InitData\PaymentType.sql
:r .\InitData\PaymentStatus.sql
:r .\InitData\SystemSettings.sql

:r .\InitData\dummy_data.sql
