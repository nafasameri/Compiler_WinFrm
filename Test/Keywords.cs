#region Keyword's Method
private static void For()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
        if (Assign())
        {
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ";") { }
            else throw new ErrorHandler("; expected");
            if (Condition())
            {
                if (LexicalAnalyst.Lexemes[++Len].ToString() == ";") { }
                else throw new ErrorHandler("; expected");
                if (Assign())
                    if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                        if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
                        {
                            Compile();
                            if (LexicalAnalyst.Lexemes[++Len].ToString() == "}") { }
                            else throw new ErrorHandler("Invalid expression term '}'");
                        }
                        else throw new ErrorHandler("';' or '{' expected");
                    else throw new ErrorHandler("')' expected");
                else throw new ErrorHandler("Assignment operation not performed");
            }
            else throw new ErrorHandler("There is no condition");
        }
        else throw new ErrorHandler("Assignment operation not performed");
    else throw new ErrorHandler("Syntax error, '(' expected");
}

private static void While()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
        if (Condition())
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
                {
                    Compile();
                    if (LexicalAnalyst.Lexemes[++Len].ToString() == "}") { }
                    else throw new ErrorHandler("Invalid expression term '}'");
                }
                else throw new ErrorHandler("';' or '{' expected");
            else throw new ErrorHandler("')' expected");
        else throw new ErrorHandler("Invalid expression term ')'");
    else throw new ErrorHandler("Syntax error, '(' expected");
}

private static void DoWhile()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
    {
        Compile();
        if (LexicalAnalyst.Lexemes[++Len].ToString() == "}")
            if (LexicalAnalyst.Lexemes[++Len].ToString() == "while")
                if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
                    if (Condition())
                        if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                            if (LexicalAnalyst.Lexemes[++Len].ToString() == ";") { }
                            else throw new ErrorHandler("; expected");
                        else throw new ErrorHandler("')' expected");
                    else throw new ErrorHandler("Invalid expression term ')'");
                else throw new ErrorHandler("Syntax error, '(' expected");
            else throw new ErrorHandler("Syntax error, 'while' expected");
        else throw new ErrorHandler("Invalid expression term '}'");
    }
    else throw new ErrorHandler("';' or '{' expected");
}

private static void If()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
        if (Condition())
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
                {
                    Compile();
                    if (LexicalAnalyst.Lexemes[++Len].ToString() == "}") Else();
                    else throw new ErrorHandler("Invalid expression term '}'");
                }
                else throw new ErrorHandler("';' or '{' expected");
            else throw new ErrorHandler("')' expected");
        else throw new ErrorHandler("Invalid expression term ')'");
    else throw new ErrorHandler("Syntax error, '(' expected");
}

private static void Else()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "else")
        if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
        {
            Compile();
            if (LexicalAnalyst.Lexemes[++Len].ToString() == "}") { }
            else throw new ErrorHandler("Invalid expression term '}'");
        }
        else throw new ErrorHandler("';' or '{' expected");
}

private static void Switch()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
        if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++Len].ToString())].token == "id")
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                if (LexicalAnalyst.Lexemes[++Len].ToString() == "{")
                {
                    Case(LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[Len - 2].ToString())].type);
                    if (LexicalAnalyst.Lexemes[++Len].ToString() == "}") { }
                    else throw new ErrorHandler("Invalid expression term '}'");
                }
                else throw new ErrorHandler("';' or '{' expected");
            else throw new ErrorHandler("')' expected");
        else throw new ErrorHandler("Identifier does not exist");
    else throw new ErrorHandler("Syntax error, '(' expected");
}

private static void Case(string type)
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "case")
        if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++Len].ToString())].type == type)
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ":")
            {
                Compile();
                if (LexicalAnalyst.Lexemes[++Len].ToString() == "break")
                    if (LexicalAnalyst.Lexemes[++Len].ToString() == "case") { Len--; Case(type); }
                    else throw new ErrorHandler("Control cannot fall out of switch from final case label ('case " + LexicalAnalyst.Lexemes[Len - 2].ToString() + ":')");
            }
            else throw new ErrorHandler("Syntax error, ':' expected");
        else throw new ErrorHandler("Cannot implicitly convert type '" + type + "' to '" + LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[Len].ToString())].type + "'");
    //else throw new ErrorHandler("Syntax error, '(' expected");
}

private static void DataType()
{
    string error = string.Empty;
    if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++Len].ToString())].token == "id") { }
    else error += "Identifier expected";
    if (LexicalAnalyst.Lexemes[++Len].ToString() == ";") { }
    else error += "; expected";
    //else throw new ErrorHandler("; expected");
    //else throw new ErrorHandler("Identifier expected");
    throw new ErrorHandler(error);
}
#endregion