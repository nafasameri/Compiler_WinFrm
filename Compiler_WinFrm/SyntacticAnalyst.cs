using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Compiler_WinFrm
{
    class SyntacticAnalyst
    {
        private static Stack<string> stk = new Stack<string>();
        private static string[,] Table = new string[10, 10];
        private static int len = 0;
        private const string path = @"E:\SyntacticAnalyst.txt";
        private const char NULL = '$';
        private const char Landa = '~';

        public static int Len
        {
            set { len = value; }
            get { return len; }
        }

        #region Help's Method
        private static bool isOperator(string op)
        {
            return op == "==" || op == "<>" || op == ">" || op == ">=" || op == "<" || op == "<=";
        }

        private static bool isLogic(string op)
        {
            return op == "and" || op == "or";
        }

        private static int FindAddress(string lexem)
        {
            for (int i = 0; i < LexicalAnalyst.symbol.Count; i++)
                if (LexicalAnalyst.symbol[i].lexem == lexem)
                    return i;
            return -1;
        }
        #endregion

        #region Keyword's Method
        private static void For()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                if (Assign())
                {
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ";") { }
                    else throw new ErrorHandler("; expected");
                    if (Condition())
                    {
                        if (LexicalAnalyst.Lexemes[++len].ToString() == ";") { }
                        else throw new ErrorHandler("; expected");
                        if (Assign())
                            if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                                if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
                                {
                                    Compile();
                                    if (LexicalAnalyst.Lexemes[++len].ToString() == "}") { }
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
            if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                if (Condition())
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                        if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
                        {
                            Compile();
                            if (LexicalAnalyst.Lexemes[++len].ToString() == "}") { }
                            else throw new ErrorHandler("Invalid expression term '}'");
                        }
                        else throw new ErrorHandler("';' or '{' expected");
                    else throw new ErrorHandler("')' expected");
                else throw new ErrorHandler("Invalid expression term ')'");
            else throw new ErrorHandler("Syntax error, '(' expected");
        }

        private static void DoWhile()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
            {
                Compile();
                if (LexicalAnalyst.Lexemes[++len].ToString() == "}")
                    if (LexicalAnalyst.Lexemes[++len].ToString() == "while")
                        if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                            if (Condition())
                                if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                                    if (LexicalAnalyst.Lexemes[++len].ToString() == ";") { }
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
            if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                if (Condition())
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                        if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
                        {
                            Compile();
                            if (LexicalAnalyst.Lexemes[++len].ToString() == "}") Else();
                            else throw new ErrorHandler("Invalid expression term '}'");
                        }
                        else throw new ErrorHandler("';' or '{' expected");
                    else throw new ErrorHandler("')' expected");
                else throw new ErrorHandler("Invalid expression term ')'");
            else throw new ErrorHandler("Syntax error, '(' expected");
        }

        private static void Else()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "else")
                if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
                {
                    Compile();
                    if (LexicalAnalyst.Lexemes[++len].ToString() == "}") { }
                    else throw new ErrorHandler("Invalid expression term '}'");
                }
                else throw new ErrorHandler("';' or '{' expected");
        }

        private static void Switch()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++len].ToString())].token == "id")
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                        if (LexicalAnalyst.Lexemes[++len].ToString() == "{")
                        {
                            Case(LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[len - 2].ToString())].type);
                            if (LexicalAnalyst.Lexemes[++len].ToString() == "}") { }
                            else throw new ErrorHandler("Invalid expression term '}'");
                        }
                        else throw new ErrorHandler("';' or '{' expected");
                    else throw new ErrorHandler("')' expected");
                else throw new ErrorHandler("Identifier does not exist");
            else throw new ErrorHandler("Syntax error, '(' expected");
        }

        private static void Case(string type)
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "case")
                if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++len].ToString())].type == type)
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ":")
                    {
                        Compile();
                        if (LexicalAnalyst.Lexemes[++len].ToString() == "break")
                            if (LexicalAnalyst.Lexemes[++len].ToString() == "case") { len--; Case(type); }
                            else throw new ErrorHandler("Control cannot fall out of switch from final case label ('case " + LexicalAnalyst.Lexemes[len - 2].ToString() + ":')");
                    }
                    else throw new ErrorHandler("Syntax error, ':' expected");
                else throw new ErrorHandler("Cannot implicitly convert type '" + type + "' to '" + LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[len].ToString())].type + "'");
            //else throw new ErrorHandler("Syntax error, '(' expected");
        }

        private static void DataType()
        {
            if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[++len].ToString())].token == "id")
                if (LexicalAnalyst.Lexemes[++len].ToString() == ";") { }
                else throw new ErrorHandler("; expected");
            else throw new ErrorHandler("Identifier expected");
        }
        #endregion

        #region LL(1)
        private static bool Operator()
        {
            Table[0, 3] = Table[0, 5] = "AT";
            Table[1, 0] = Table[1, 4] = Table[3, 0] = Table[3, 1] = Table[3, 4] = Landa.ToString();
            Table[1, 1] = "AT+";
            Table[2, 3] = Table[2, 5] = "BF";
            Table[3, 2] = "BF*";
            Table[4, 3] = ")E(";
            Table[4, 5] = "id";

            stk.Push("E");
            string s = string.Empty;

            while (LexicalAnalyst.Lexemes[len].ToString() != ";" || !isOperator(LexicalAnalyst.Lexemes[len].ToString())) // datatype | condition
            {
                int row = -1, col = -1;
                int address = FindAddress(LexicalAnalyst.Lexemes[len].ToString());
                //if (address == -1) { File.AppendAllText(path, s + "\r\n"); return; }

                while (true)
                {
                    string temp = stk.Pop();

                    if (temp == "id" || temp == "(" || temp == ")" || temp == "*" || temp == "+" || temp == Landa.ToString())
                        if (LexicalAnalyst.Lexemes[len].ToString() == temp || LexicalAnalyst.symbol[address].token == temp
                            || LexicalAnalyst.symbol[address].token == "Inum" || LexicalAnalyst.symbol[address].token == "Fnum") { s += temp; len++; break; }

                    switch (temp)
                    {
                        case "E": row = 0; break;
                        case "A": row = 1; break;
                        case "T": row = 2; break;
                        case "B": row = 3; break;
                        case "F": row = 4; break;
                    }

                    switch (LexicalAnalyst.Lexemes[len].ToString())
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
                    else throw new ErrorHandler("The " + LexicalAnalyst.Lexemes[len].ToString() + " or -> operator must be applied to a pointer");

                    //if (LexicalAnalyst.Lexemes[len].ToString() == ";" && temp == NULL.ToString()) return true;                   
                }
                File.AppendAllText(path, s);
            }
            if (stk.Peek() == NULL.ToString())
                return true;
            return false;
        }

        private static bool Assign()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "=")
                if (Operator())
                {
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ";") return true;
                    else throw new ErrorHandler("; expected");
                }
                else return false;
            else throw new ErrorHandler("Use of unassigned local variable '" + LexicalAnalyst.Lexemes[len].ToString() + "'");
        }

        private static bool C()
        {
            if (Operator())
                if (isOperator(LexicalAnalyst.Lexemes[++len].ToString()))
                {
                    if (Operator())
                        return true;
                }
                else throw new ErrorHandler("There is no condition");
            return false;
        }

        private static bool Condition()
        {
            if (LexicalAnalyst.Lexemes[++len].ToString() == "(")
                if (C())
                    if (LexicalAnalyst.Lexemes[++len].ToString() == ")")
                        if (isLogic(LexicalAnalyst.Lexemes[++len].ToString()))
                            if (Condition())
                                return true;
                            else return false;
                        else return true;
                    else throw new ErrorHandler("')' expected");
                else return false;
            else return C();
        }
        #endregion

        public static string Compile()
        {
            string error = string.Empty;
            //stk.Push(NULL.ToString());
            //LexicalAnalyst.Lexemes.Add(NULL);

            foreach (var item in LexicalAnalyst.Lexemes)
                File.AppendAllText(path, item.ToString() + "\r\n");

            //while (len < LexicalAnalyst.Lexemes.Count - 1) 
            //{
            //    try
            //    {
            //        if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[len].ToString())].token == "Data Type") { DataType(); len++; }
            //        if (LexicalAnalyst.symbol[FindAddress(LexicalAnalyst.Lexemes[len].ToString())].token == "id") Assign();
            //        switch (LexicalAnalyst.Lexemes[len].ToString())
            //        {
            //            case "if": If(); break;
            //            case "for": For(); break;
            //            case "while": While(); break;
            //            case "switch": Switch(); break;
            //            case "do": DoWhile(); break;
            //        }
            //    }
            //    catch (ErrorHandler err) { error += (err.Message + "\r\n"); }
            //}
            return error;
        }
    }
}
                    

//private static void E()
//{
//    if (lookahead == "id" || lookahead == "Inum" || lookahead == "Fnum")
//        match(lookahead);
//    E();
//    if (lookahead == "op")
//        match(lookahead);
//    E();
//}

//private static void Assign()
//{
//if (lookahead == "id")
//{
//    match("id");
//    if (lookahead == "assign")
//        match("assign");
//    else
//        throw new ErrorHandler("Invalid expression term '='");
//    E();
//}
//else
//    throw new ErrorHandler("Invalid expression term '='");
//}