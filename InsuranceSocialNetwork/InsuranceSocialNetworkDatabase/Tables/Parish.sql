CREATE TABLE [Insurance].[Parish]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [County_Code] INT NOT NULL, 
	[Code] NVARCHAR(2) NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL, 
	[Designation] NVARCHAR(256) NOT NULL, 
    CONSTRAINT [PK_Parish] PRIMARY KEY CLUSTERED ([ID] ASC)
    --CONSTRAINT [FK_Parish_County] FOREIGN KEY ([County_Code]) REFERENCES [Insurance].[County] ([Code])
)
