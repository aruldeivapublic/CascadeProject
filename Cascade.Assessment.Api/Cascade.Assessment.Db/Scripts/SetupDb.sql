USE [Cascade.Assessment.Db]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[AuthorId] [uniqueidentifier] NOT NULL,
	[AuthorFirstName] [varchar](100) NOT NULL,
	[AuthorLastName] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookId] [bigint] IDENTITY(1,1) NOT NULL,
	[PublisherId] [uniqueidentifier] NOT NULL,
	[AuthorId] [uniqueidentifier] NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[Price] [money] NOT NULL,
	[SubTitle] [varchar](100) NULL,
	[BookEdition] [varchar](50) NULL,
	[MinimumPages] [int] NOT NULL,
	[MaximumPages] [int] NOT NULL,
	[PublishedDate] [date] NOT NULL,
	[PublishedLocation] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publishers](
	[PublisherId] [uniqueidentifier] NOT NULL,
	[Publisher] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PublisherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestTable]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTable](
	[Number] [varchar](100) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Customer] [varchar](200) NOT NULL,
	[Quantity] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_AddBooks]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  StoredProcedure [dbo].[usp_GetBooks]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetBooks]
	@selectCriteria varchar(max),
	@sortCriteria varchar(max)
AS
Begin	
	declare @sql varchar(max)
	set @sql = ''
	select @sql='SELECT
		P.Publisher,B.Title, A.AuthorFirstName,A.AuthorLastName,B.Price,B.SubTitle,B.BookEdition,B.MinimumPages,B.MaximumPages,B.PublishedDate,B.PublishedLocation
	From Books B with(nolock)
		inner join Publishers P with(nolock) on B.PublisherId=P.PublisherId
		inner join Authors A with(nolock) on B.AuthorId=A.AuthorId'
	if LEN(LTRIM(RTRIM(@selectCriteria))) > 0
	begin
		select @sql = @sql + ' where ' + @selectCriteria
	end

	if LEN(LTRIM(RTRIM(@sortCriteria))) > 0
	begin
		select @sql = @sql + ' order by ' + @sortCriteria
	end

	exec (@sql)
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetTotalBooksPrice]    Script Date: 3/25/2022 8:23:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetTotalBooksPrice]
AS
begin
	SELECT sum(Price) from Books with(nolock)
end
RETURN 0
GO
