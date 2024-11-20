namespace PostgreSQLFunctionDemo.Services.FunctionDefinitions
{
    public static class GetUserOrderSummaryFunction
    {
        public static string Script => @"
            CREATE OR REPLACE FUNCTION GetUserOrderSummary(userId INT, includeDetails BOOLEAN)
            RETURNS TEXT AS $$
            DECLARE
                result JSON;
            BEGIN
                IF includeDetails THEN
                    result := json_build_object(
                        'totalAmount', (
                            SELECT COALESCE(SUM(o.""Amount""), 0)
                            FROM ""Orders"" o
                            WHERE o.""UserId"" = userId
                        ),
                        'orderCount', (
                            SELECT COUNT(*)
                            FROM ""Orders"" o
                            WHERE o.""UserId"" = userId
                        ),
                        'orders', (
                            SELECT json_agg(o)
                            FROM ""Orders"" o
                            WHERE o.""UserId"" = userId
                        )
                    );
                ELSE
                    result := json_build_object(
                        'totalAmount', (
                            SELECT COALESCE(SUM(o.""Amount""), 0)
                            FROM ""Orders"" o
                            WHERE o.""UserId"" = userId
                        ),
                        'orderCount', (
                            SELECT COUNT(*)
                            FROM ""Orders"" o
                            WHERE o.""UserId"" = userId
                        )
                    );
                END IF;
                RETURN result::TEXT;
            END;
            $$ LANGUAGE plpgsql;
        ";
    }
}
