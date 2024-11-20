namespace PostgreSQLFunctionDemo.Services.FunctionDefinitions
{
    public static class GetFullNameFunction
    {
        public static string Script => @"
            CREATE OR REPLACE FUNCTION GetFullName(firstName VARCHAR, lastName VARCHAR)
            RETURNS VARCHAR AS $$
            BEGIN
                RETURN CONCAT(firstName, ' ', lastName);
            END;
            $$ LANGUAGE plpgsql;
        ";
    }
}
