#region LL(1)
private static bool Operator()
{
    Stack<string> stk = new Stack<string>();
    string[,] Table = new string[6, 6];
    Table[0, 3] = Table[0, 5] = "AT";
    Table[1, 0] = Table[1, 4] = Table[3, 0] = Table[3, 1] = Table[3, 4] = Landa.ToString();
    Table[1, 1] = "AT+";
    Table[2, 3] = Table[2, 5] = "BF";
    Table[3, 2] = "BF*";
    Table[4, 3] = ")E(";
    Table[4, 5] = "id";

    stk.Push("E");
    string s = string.Empty;

    while (LexicalAnalyst.Lexemes[Len].ToString() != ";" || !isOperator(LexicalAnalyst.Lexemes[Len].ToString())) // datatype | condition
    {
        int row = -1, col = -1;
        int address = FindAddress(LexicalAnalyst.Lexemes[Len].ToString());
        //if (address == -1) { File.AppendAllText(path, s + "\r\n"); return; }

        while (true)
        {
            string temp = stk.Pop();

            if (temp == "id" || temp == "(" || temp == ")" || temp == "*" || temp == "+" || temp == Landa.ToString())
                if (LexicalAnalyst.Lexemes[Len].ToString() == temp || LexicalAnalyst.symbol[address].token == temp
                    || LexicalAnalyst.symbol[address].token == "Inum" || LexicalAnalyst.symbol[address].token == "Fnum") { s += temp; Len++; break; }

            switch (temp)
            {
                case "E": row = 0; break;
                case "A": row = 1; break;
                case "T": row = 2; break;
                case "B": row = 3; break;
                case "F": row = 4; break;
            }

            switch (LexicalAnalyst.Lexemes[Len].ToString())
            {
                case "$": col = 0; break;
                case "+": col = 1; break;
                case "*": col = 2; break;
                case "(": col = 3; break;
                case ")": col = 4; break;
            }
            if (LexicalAnalyst.symbol[address].token == "id" || LexicalAnalyst.symbol[address].token == "Inum" || LexicalAnalyst.symbol[address].token == "Fnum")
                col = 5;

            if (Table[row, col] != null)
                if (Table[row, col] == "id") stk.Push(Table[row, col]);
                else if (Table[row, col] == Landa.ToString()) { }
                else foreach (var item in Table[row, col])
                        stk.Push(item.ToString());
            else throw new ErrorHandler("The " + LexicalAnalyst.Lexemes[Len].ToString() + " or -> operator must be applied to a pointer");

            //if (LexicalAnalyst.Lexemes[Len].ToString() == ";" && temp == NULL.ToString()) return true;                   
        }
        File.AppendAllText(path, s);
    }
    if (stk.Peek() == NULL.ToString())
        return true;
    return false;
}

private static bool Assign()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "=")
        if (Operator())
        {
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ";") return true;
            else throw new ErrorHandler("; expected");
        }
        else return false;
    else throw new ErrorHandler("Use of unassigned local variable '" + LexicalAnalyst.Lexemes[Len].ToString() + "'");
}

private static bool C()
{
    if (Operator())
        if (isOperator(LexicalAnalyst.Lexemes[++Len].ToString()))
        {
            if (Operator())
                return true;
        }
        else throw new ErrorHandler("There is no condition");
    return false;
}

private static bool Condition()
{
    if (LexicalAnalyst.Lexemes[++Len].ToString() == "(")
        if (C())
            if (LexicalAnalyst.Lexemes[++Len].ToString() == ")")
                if (isLogic(LexicalAnalyst.Lexemes[++Len].ToString()))
                    if (Condition())
                        return true;
                    else return false;
                else return true;
            else throw new ErrorHandler("')' expected");
        else return false;
    else return C();
}
#endregion