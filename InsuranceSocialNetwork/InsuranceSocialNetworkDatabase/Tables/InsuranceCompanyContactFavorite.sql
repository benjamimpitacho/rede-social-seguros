CREATE TABLE [Insurance].[InsuranceCompanyContactFavorite]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_InsuranceCompanyContact] BIGINT NOT NULL, 
    [ID_User] NVARCHAR(128) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_InsuranceCompanyContactFavorite] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_InsuranceCompanyContactFavorite_InsuranceCompanyContact] FOREIGN KEY([ID_InsuranceCompanyContact]) REFERENCES [Insurance].[InsuranceCompanyContact] ([ID]),
	CONSTRAINT [FK_InsuranceCompanyContactFavorite_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

