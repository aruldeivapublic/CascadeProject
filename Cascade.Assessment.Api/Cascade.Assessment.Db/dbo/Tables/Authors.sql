CREATE TABLE [dbo].[Authors]
(
	[AuthorId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AuthorFirstName] VARCHAR(100) NOT NULL, 
    [AuthorLastName] VARCHAR(100) NOT NULL
)
