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
go

