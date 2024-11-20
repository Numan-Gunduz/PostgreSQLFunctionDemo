namespace PostgreSQLFunctionDemo.Services.FunctionDefinitions
{
    public static class CalculateTotalOrderFunction
    {
        public static string Script => @"
            CREATE OR REPLACE FUNCTION calculatetotalorder(userId INT)
            RETURNS DECIMAL AS $$
            BEGIN
                RETURN (
                    SELECT COALESCE(SUM(o.""Amount""), 0)
                    FROM ""Orders"" o
                    WHERE o.""UserId"" = userId
                );
            END;
            $$ LANGUAGE plpgsql;
        ";
    }
}
