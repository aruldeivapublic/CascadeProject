CREATE TABLE [dbo].[Books]
(
	[BookId] BIGINT NOT NULL PRIMARY KEY Identity, 
    [PublisherId] UNIQUEIDENTIFIER NOT NULL, 
    [AuthorId] UNIQUEIDENTIFIER NOT NULL,
    [Title] VARCHAR(200) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [SubTitle] VARCHAR(100) NULL, 
    [BookEdition] VARCHAR(50) NULL, 
    [MinimumPages] INT NOT NULL, 
    [MaximumPages] INT NOT NULL, 
    [PublishedDate] DATE NOT NULL, 
    [PublishedLocation] VARCHAR(100) NULL
)
