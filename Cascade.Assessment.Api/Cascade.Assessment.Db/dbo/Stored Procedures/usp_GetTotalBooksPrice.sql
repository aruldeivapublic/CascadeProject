CREATE PROCEDURE [dbo].[usp_GetTotalBooksPrice]
AS
begin
	SELECT sum(Price) from Books with(nolock)
end
RETURN 0
