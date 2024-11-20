namespace PostgreSQLFunctionDemo.Services.FunctionDefinitions
{
    public static class GetLastOrderDateFunction
    {
        public static string Script => @"
            CREATE OR REPLACE FUNCTION GetLastOrderDate(userId INT)
            RETURNS TIMESTAMP AS $$
            BEGIN
                RETURN (
                    SELECT MAX(o.""OrderDate"")
                    FROM ""Orders"" o
                    WHERE o.""UserId"" = userId
                );
            END;
            $$ LANGUAGE plpgsql;
        ";
    }
}
