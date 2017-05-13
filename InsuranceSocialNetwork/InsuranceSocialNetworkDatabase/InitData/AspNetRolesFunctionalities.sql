
/* ADMINISTRATOR */
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('1','USERS_MANAGEMENT',1);
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('1','ROLES_MANAGEMENT',1);
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('1','ALERTS_MANAGEMENT',1);

/* NORMAL_USER */
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('2','USERS_MANAGEMENT',0);

/* INSURANCE_PROFESSIONAL */
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('3','USERS_MANAGEMENT',0);

/* ASSOCIATED_PREMIUM */
INSERT INTO [dbo].[AspNetRolesFunctionalities] ([RoleId],[Token],[Active]) VALUES ('4','USERS_MANAGEMENT',0);
