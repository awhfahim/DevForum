using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixedStoredProcedure : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"ALTER PROCEDURE [dbo].[GetQuestionsWithPaginationAndSorting]
						    @pageNumber INT,
						    @pageSize INT,
						    @sortOption VARCHAR(50) = 'Newest',
							@total INT OUTPUT
						AS
						BEGIN
						    SET NOCOUNT ON;

							SELECT @Total = COUNT(*) FROM Questions;
						    -- Step 1: Retrieve the data with only one ORDER BY parameter and store it in a temporary table
						    SELECT Q.Id, Q.Title, Q.Body, Q.CreatedAt,     
										(SELECT COUNT(*) FROM QuestionVote V WHERE Q.Id = V.QuestionId) AS TotalVotes,
										(SELECT COUNT(*) FROM Answers A WHERE Q.Id = A.QuestionId) AS TotalAnswers
						    INTO #TempTable
						    FROM Questions Q
						    LEFT JOIN QuestionVote V ON Q.Id = V.QuestionId
						    LEFT JOIN Answers A ON Q.Id = A.QuestionId
						    GROUP BY Q.Id, Q.CreatedAt, Q.Title, Q.Body, Q.AnswersCount
						    ORDER BY Q.CreatedAt DESC

						    -- Step 2: Add sorting to the data in the temporary table
						    DECLARE @sql NVARCHAR(MAX);

							SET @sql = N'SELECT * FROM #TempTable ORDER BY ';

							SET @sql = @sql + CASE
								WHEN @sortOption = 'Newest' THEN 'CreatedAt'
								WHEN @sortOption = 'Oldest' THEN 'CreatedAt'
								WHEN @sortOption = 'HighestScore' THEN 'TotalVotes'
								WHEN @sortOption = 'LowestScore' THEN 'TotalVotes'
								WHEN @sortOption = 'HighestAnswer' THEN 'TotalAnswers'
								WHEN @sortOption = 'NoAnswer' THEN 'TotalAnswers'
								ELSE 'CreatedAt'
							END + CASE 
								WHEN @sortOption IN ('Oldest', 'NoAnswer', 'LowestScore') THEN ' ASC'
								ELSE ' DESC'
							END;

							-- Add pagination
							SET @sql = @sql + ' OFFSET ' + CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@pageSize AS NVARCHAR(10)) + ' ROWS ONLY';

							EXEC sp_executesql @sql;

						    -- Drop the temporary table
						    DROP TABLE #TempTable;
						END";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        var sql = @"
							ALTER PROCEDURE [dbo].[GetQuestionsWithPaginationAndSorting]
						    @pageNumber INT,
						    @pageSize INT,
						    @sortOption VARCHAR(50) = 'Newest',
							@total INT OUTPUT
						AS
						BEGIN
						    SET NOCOUNT ON;

							SELECT @Total = COUNT(*) FROM Questions;
						    -- Step 1: Retrieve the data with only one ORDER BY parameter and store it in a temporary table
						    SELECT Q.Id, Q.Title, Q.Body, Q.CreatedAt, COUNT(V.Id) AS TotalVotes, COUNT(A.Id) AS TotalAnswers
						    INTO #TempTable
						    FROM Questions Q
						    LEFT JOIN QuestionVote V ON Q.Id = V.QuestionId
						    LEFT JOIN Answers A ON Q.Id = A.QuestionId
						    GROUP BY Q.Id, Q.CreatedAt, Q.Title, Q.Body, Q.AnswersCount
						    ORDER BY Q.CreatedAt DESC

						    -- Step 2: Add sorting to the data in the temporary table
						    DECLARE @sql NVARCHAR(MAX);

							SET @sql = N'SELECT * FROM #TempTable ORDER BY ';

							SET @sql = @sql + CASE
								WHEN @sortOption = 'Newest' THEN 'CreatedAt'
								WHEN @sortOption = 'Oldest' THEN 'CreatedAt'
								WHEN @sortOption = 'HighestScore' THEN 'TotalVotes'
								WHEN @sortOption = 'LowestScore' THEN 'TotalVotes'
								WHEN @sortOption = 'HighestAnswer' THEN 'TotalAnswers'
								WHEN @sortOption = 'NoAnswer' THEN 'TotalAnswers'
								ELSE 'CreatedAt'
							END + CASE 
								WHEN @sortOption IN ('Oldest', 'NoAnswer', 'LowestScore') THEN ' ASC'
								ELSE ' DESC'
							END;

							-- Add pagination
							SET @sql = @sql + ' OFFSET ' + CAST((@pageNumber - 1) * @pageSize AS NVARCHAR(10)) + ' ROWS FETCH NEXT ' + CAST(@pageSize AS NVARCHAR(10)) + ' ROWS ONLY';

							EXEC sp_executesql @sql;

						    -- Drop the temporary table
						    DROP TABLE #TempTable;
						END
						GO";
	        migrationBuilder.Sql(sql);
        }
    }
}
