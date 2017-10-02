CREATE TABLE [Insurance].[County]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [District_Code] INT NOT NULL, 
	[Code] INT NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
    CONSTRAINT [PK_County] PRIMARY KEY CLUSTERED ([ID] ASC)
    --CONSTRAINT [FK_County_District] FOREIGN KEY ([District_Code]) REFERENCES [Insurance].[District] ([Code])
)
