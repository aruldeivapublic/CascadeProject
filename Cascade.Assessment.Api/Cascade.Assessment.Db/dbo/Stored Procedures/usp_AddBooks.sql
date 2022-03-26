CREATE PROCEDURE [dbo].[usp_AddBooks]
	@jsonBooks varchar(max),
	@jsonAuthors varchar(max),
	@jsonPublishers varchar(max),
	@result int output
AS
BEGIN
	set @result=0
	BEGIN TRY
		BEGIN Transaction
			INSERT INTO Authors 
				SELECT AuthorJsonData.*
					FROM OPENJSON (@jsonAuthors, N'$')
					WITH (
						AuthorId uniqueidentifier N'$.AuthorId',
						AuthorLastName VARCHAR(100) N'$.AuthorLastName',
						AuthorFirstName VARCHAR(100) N'$.AuthorFirstName'
					) AS AuthorJsonData;

			INSERT INTO Publishers 
				SELECT PublisherJsonData.*
					FROM OPENJSON (@jsonPublishers, N'$')
						WITH (
						PublisherId uniqueidentifier N'$.PublisherId',
						Publisher VARCHAR(200) N'$.Publisher'
						) AS PublisherJsonData;

			INSERT INTO Books
				SELECT BookJsonData.*
					FROM OPENJSON (@jsonBooks, N'$')
						WITH (
						PublisherId uniqueidentifier N'$.PublisherId',
						AuthorId uniqueidentifier N'$.AuthorId',
						Title VARCHAR(200) N'$.Title',
						Price money N'$.Price',
						SubTitle VARCHAR(100) N'$.SubTitle',
						BookEdition VARCHAR(50) N'$.BookEdition',
						MinimumPages int N'$.MinimumPages',
						MaximumPages int N'$.MaximumPages',
						PublishedDate date N'$.PublishedDate',
						PublishedLocation varchar(100) N'$.PublishedLocation'
						) AS BookJsonData;
		Commit Transaction
		select @result=1
		Return 1
	END TRY
	BEGIN CATCH
		Rollback Transaction
		select @result=0
		return 0
	END CATCH
END

