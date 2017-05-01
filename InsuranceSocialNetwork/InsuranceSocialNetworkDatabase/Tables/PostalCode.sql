CREATE TABLE [Insurance].[PostalCode]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [Codigo_Postal] INT NOT NULL, 
    [Extensao] INT NOT NULL, 
    [Designacao_Postal] NVARCHAR(256) NOT NULL, 
    [Localidade] NVARCHAR(256) NOT NULL, 
    [Codigo_Arteria] INT NULL, 
    [Tipo_Arteria] NVARCHAR(256) NULL, 
    [Prep1] NVARCHAR(10) NULL, 
    [Titulo_Arteria] NVARCHAR(256) NULL, 
    [Prep2] NVARCHAR(10) NULL, 
    [Nome_Arteria] NVARCHAR(256) NULL, 
    [Local_Arteria] NVARCHAR(256) NULL, 
    [Troco] NVARCHAR(256) NULL, 
    [Porta] INT NULL,
    CONSTRAINT [PK_PostalCode] PRIMARY KEY CLUSTERED ([ID] ASC)
)
