#region CLR
private static void CLR(Stack<string> stk, string[,] clr, int row, int col)
{
    if (clr[row, col] == "acc") return;
    else if (clr[row, col][0] == 'S')
    {
        stk.Push(LexicalAnalyst.Lexemes[Len++].ToString());
        stk.Push(clr[row, col][1].ToString());
    }
    else if (clr[row, col][0] == 'R')
    {
        int popCount;
        int.TryParse(clr[row, col][1].ToString(), out popCount);
        for (int p = 1; p <= (popCount * 2); p++)
            stk.Pop();
        stk.Push("S");
        stk.Push("1");
    }
}

private static void DT()
{
    Stack<string> stk = new Stack<string>();
    string[,] clr = new string[5, 5];
    clr[3, 0] = "S4";
    clr[2, 1] = "S3";
    clr[0, 2] = "S2";
    clr[1, 3] = "acc";
    clr[4, 3] = "R3";
    clr[0, 4] = "1";
    clr[0, 0] = clr[1, 0] = clr[2, 0] = clr[4, 0] = "syntax: ; expected";
    clr[0, 3] = clr[2, 3] = clr[3, 3] = "syntax: ; expected";
    clr[0, 1] = clr[1, 1] = clr[3, 1] = clr[4, 1] = "syntax: Identifier expected";
    clr[1, 2] = clr[2, 2] = clr[3, 2] = clr[4, 2] = "syntax: The data type of this variable is unknown";// "The name '' does not exist in the current context";

    stk.Push("0");
    int row = -1, col = -1;

    while (true)
    {
        int address = FindAddress(LexicalAnalyst.Lexemes[Len].ToString());
        string temp = stk.Peek();

        int.TryParse(temp, out row);
        if (LexicalAnalyst.symbol[address].token == "Data Type") col = 2;
        if (LexicalAnalyst.symbol[address].token == "id") col = 1;
        if (LexicalAnalyst.symbol[address].token == "UnKnown" || LexicalAnalyst.symbol[address].token == "Inum") { col = 1; break; }
        switch (LexicalAnalyst.Lexemes[Len].ToString())
        {
            case "$": col = 3; break;
            case ";": col = 0; break;
            case "S": col = 4; break;
        }

        if (clr[row, col].Length < 4) CLR(stk, clr, row, col);
        else throw new ErrorHandler(clr[row, col]);
    }
    //if (col == 0 || col == 3)
    //  throw new ErrorHandler("; expected");
    //else if (col == 1)
    //  throw new ErrorHandler("Identifier expected");
    Len++;
}

private static void Assignment()
{
    Stack<string> stk = new Stack<string>();
    string[,] clr = new string[6, 6];
    clr[5, 2] = "S4";
    clr[2, 1] = "S3";
    clr[0, 0] = "S2";
    //clr[3, 3] = "S5";
    clr[1, 4] = "acc";
    clr[4, 4] = "R4";
    clr[0, 5] = "1";

    stk.Push("0");
    int row = -1, col = -1;

    while (true)
    {
        int address = FindAddress(LexicalAnalyst.Lexemes[Len].ToString());
        string temp = stk.Peek();

        int.TryParse(temp, out row);
        if (LexicalAnalyst.symbol[address].token == "id") col = 0;
        if (LexicalAnalyst.symbol[address].token == "UnKnown")
            throw new ErrorHandler("The left-hand side of an assignment must be a variable, property or indexer");
        switch (LexicalAnalyst.Lexemes[Len].ToString())
        {
            case "$": col = 4; break;
            case ";": col = 2; break;
            case "=": col = 1; break;
            case "S": col = 5; break;
        }

        if (clr[row, col] != null)
            CLR(stk, clr, row, col);
        else if (row == 3)
        {
            bool op = Operator();
            if (op)
            {
                stk.Push("E");
                stk.Push("5");
            }
            else
                throw new ErrorHandler("Math operation or value was not known");
        }
        else
        {
            if (col == 0 || row == 3)
                throw new ErrorHandler("Use of unassigned local variable '" + LexicalAnalyst.Lexemes[Len].ToString() + "'");
            else if (col == 2)
                throw new ErrorHandler("; expected");
        }
    }
    //throw new ErrorHandler("Invalid expression term '='");        
}

private static bool Operator()
{
    return true;
}
//if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[Len].ToString())].token == "Data Type") DT();
//if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[Len].ToString())].token == "id") Assignment();
#endregion