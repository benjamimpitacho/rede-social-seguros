CREATE TABLE [Insurance].[MedicalClinicFavorite]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_MedicalClinic] BIGINT NOT NULL, 
    [ID_User] NVARCHAR(128) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_MedicalClinicFavorite] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_MedicalClinicFavorite_MedicalClinic] FOREIGN KEY([ID_MedicalClinic]) REFERENCES [Insurance].[MedicalClinic] ([ID]),
	CONSTRAINT [FK_MedicalClinicFavorite_User] FOREIGN KEY([ID_User]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

