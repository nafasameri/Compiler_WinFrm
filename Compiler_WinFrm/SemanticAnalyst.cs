using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_WinFrm
{
    class SemanticAnalyst
    {
        private static int Len = 0;
        private const string error = "Semantic: ";
        public static Dictionary<int, Symbol> symbol = new Dictionary<int, Symbol>();

        private static int FindAddress(string lexem)
        {
            for (int i = 0; i < LexicalAnalyst.symbol.Count; i++)
                if (LexicalAnalyst.symbol[i].lexem == lexem)
                    return i;
            return 0;
        }

        public static string Compile()
        {
            string errSC = "; expected";
            string errId = "Identifier expected";
            string errDT = "The data type of this variable is unknown";
            string errAss = "= expected";
            string errVal = "value expected";
            string err = string.Empty;
            int val = 0, id = 0;

            Stack<string> stk = new Stack<string>();
            string[,] clr = { { "S2", errId, errAss, errVal, errSC, "", "1" },
                              { errDT, errId, errAss, errVal, errSC, "acc", ""},
                              { errDT, "S3", errAss, errVal, errSC, "", ""},
                              { errDT, errId, "S4", errVal, errSC, "", ""},
                              { errDT, errId, errAss, "S5", errSC, "", ""},
                              { errDT, errId, errAss, errVal, "S6", "", ""},
                              { errDT, errId, errAss, errVal, errSC, "R5", ""}};
            stk.Push("0");

            while (Len < LexicalAnalyst.Lexemes.Count)
            {
                try
                {
                    int row = -1, col = -1;
                    int address = FindAddress(LexicalAnalyst.Lexemes[Len].ToString());
                    string topStk = stk.Peek();
                    int.TryParse(topStk, out row);

                    if (LexicalAnalyst.symbol[address].token == "Data Type") col = 0;
                    if (LexicalAnalyst.symbol[address].token == "id") { id = address; col = 1; }
                    if (LexicalAnalyst.symbol[address].token == "UnKnown" || LexicalAnalyst.symbol[address].token == "Inum"
                        || LexicalAnalyst.symbol[address].token == "Fnum" || LexicalAnalyst.symbol[address].token == "Char"
                        || LexicalAnalyst.symbol[address].token == "Str") { val = address; col = 3; }
                    switch (LexicalAnalyst.Lexemes[Len].ToString())
                    {
                        case "$": col = 5; break;
                        case ";": col = 4; break;
                        case "=": col = 2; break;
                        case "S": col = 6; break;
                    }

                    if (clr[row, col].Length < 5 && clr[row, col].Length != 0)
                    {
                        if (clr[row, col] == "acc") break;
                        else if (clr[row, col][0] == 'S')
                        {
                            stk.Push(LexicalAnalyst.Lexemes[Len++].ToString());
                            stk.Push(clr[row, col].Substring(1, clr[row, col].Length - 1));
                        }
                        else if (clr[row, col][0] == 'R')
                        {
                            int popCount;
                            int.TryParse(clr[row, col][1].ToString(), out popCount);
                            for (int p = 1; p <= (popCount * 2); p++)
                                stk.Pop();
                            string temp = clr[row, col][2].ToString();
                            topStk = stk.Peek();
                            int.TryParse(topStk, out row);
                            stk.Push(temp);
                            if (temp == "S") col = 1;
                            stk.Push(clr[row, col]);
                        }

                    }
                    else { throw new ErrorHandler(clr[row, col], error); }
                }
                catch (ErrorHandler Err) { err += (Err.Message + "\r\n"); break; }
            }

            for (int i = 0; i < LexicalAnalyst.symbol.Count; i++)
                if (i == id)
                    symbol.Add(i, new Symbol("id", LexicalAnalyst.symbol[i].lexem, LexicalAnalyst.symbol[i].type, LexicalAnalyst.symbol[val].value, LexicalAnalyst.symbol[i].length));
                else
                    symbol.Add(i, LexicalAnalyst.symbol[i]);

            File.WriteAllText(@"E:\Sem.txt", "Address\t\tToken\t\tLexem\t\tType\t\tValue\t\tLength\r\n");
            for (int i = 0; i < symbol.Count; i++)
                File.AppendAllText(@"E:\Sem.txt", i.ToString() + "\t\t" + symbol[i].token + "\t\t" + symbol[i].lexem + "\t\t" + symbol[i].type + "\t\t" + symbol[i].value + "\t\t" + symbol[i].length + "\r\n");

            return err;
        }
    }
}