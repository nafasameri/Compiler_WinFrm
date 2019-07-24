using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System;

namespace Compiler_WinFrm
{
    class LexicalAnalyst
    {
        private const char NULL = '$';
        private const string path = @"E:\SymbolTable.txt";
        private static string type = string.Empty;
        private static string lexem = string.Empty;
        private static char c;
        private static int address = 0;
        private static int len = 0;
        private static int Length = 0;
        private static int state = 0;
        private static int index = 0;
        public static Dictionary<int, Symbol> symbol = new Dictionary<int, Symbol>();
        public static ArrayList Lexemes = new ArrayList();


        #region Help's Method
        private static char getNextChar(string str)
        {
            if (len >= str.Length)
            {
                len++;
                return char.MaxValue;
            }
            return str[len++];
        }

        private static void unGetChar()
        {
            len--;
        }

        private static bool isLetter(char c)
        {
            return c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'h' || c == 'i' || c == 'j' || c == 'k' || c == 'l' || c == 'm' || c == 'n' ||
                   c == 'o' || c == 'p' || c == 'q' || c == 'r' || c == 's' || c == 't' || c == 'u' || c == 'y' || c == 'w' || c == 'x' || c == 'v' || c == 'z' || c == 'g' ||
                   c == 'A' || c == 'B' || c == 'C' || c == 'D' || c == 'E' || c == 'F' || c == 'H' || c == 'I' || c == 'J' || c == 'K' || c == 'L' || c == 'M' || c == 'N' ||
                   c == 'O' || c == 'P' || c == 'Q' || c == 'R' || c == 'S' || c == 'T' || c == 'U' || c == 'Y' || c == 'W' || c == 'X' || c == 'V' || c == 'Z' || c == 'G';
        }

        private static bool isSpace(char c)
        {
            return c == ' ';
        }

        private static bool isDot(char c)
        {
            return c == '.';
        }
        #endregion

        #region Find's Method
        private static bool FindSymbol(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem)
                    return true;
            return false;
        }

        private static bool FindType(string lexem, string type)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == type)
                    return true;
            return false;
        }

        private static bool FindToken(string lexem, string token)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].token == token)
                    return true;
            return false;
        }
        #endregion

        #region Detect's Method
        private static void DetectCharacter(string str)
        {
            index = len;
            c = getNextChar(str);
            if (c == NULL) throw new ErrorHandler("Unexpected character '" + NULL + "'", "Lexical");
            else if (char.IsDigit(c)) state = 1;
            else if (isLetter(c) || c == '_') state = 3;
            else if (isSpace(c)) state = 4;
            else if (c == '"') state = 5;
            else if (c.ToString() == "'") state = 6;
            else if (c == '*') state = 7;
            else if (c == '<') state = 8;
            else if (c == '=') state = 9;
            else if (c == '+') state = 10;
            else if (c == '-') state = 11;
            else if (c == '%') state = 12;
            else if (c == '[') { state = 14; Lexemes.Add("["); }
            else if (c == ']') { state = 15; Lexemes.Add("]"); }
            else if (c == '>') state = 16;
            else if (c == '#') Lexemes.Add("#");
            else if (c == '/') state = 13;
            else if (c == '`') Lexemes.Add("`");
            else if (c == '\\') Lexemes.Add("\\");
            else if (c == '}') Lexemes.Add("}");
            else if (c == '{') Lexemes.Add("{");
            else if (c == '(') Lexemes.Add("(");
            else if (c == ')') Lexemes.Add(")");
            else if (c == '!') state = 20;
            else if (c == '&') state = 17;
            else if (c == '|') state = 18;
            else if (c == '@') throw new ErrorHandler("Keyword, identifier, or string expected after verbatim specifier: @", "Lexical");
            else if (c == ';') Lexemes.Add(";");
            else if (c == ':') Lexemes.Add(":");
            else if (c == '.') Lexemes.Add(".");

            else if ((Keys)c == Keys.Back) { }
            else if ((Keys)c == Keys.Enter) { }//Lexemes.Add(NULL.ToString());

            else if (c == char.MaxValue) { }
            else if (c == '\n') { }
            else if (c == '\r') { }

            else throw new ErrorHandler("Unreachable code detected!", "Lexical");
        }

        private static void DetectIntNumber(string str)
        {
            c = getNextChar(str);
            if (char.IsDigit(c))
                state = 1;
            else if (isDot(c))
                state = 2;
            else if (isLetter(c) && type != string.Empty)
            {
                do
                    c = getNextChar(str);
                while (isLetter(c));
                unGetChar();
                state = 0;
                type = string.Empty;
                symbol.Add(address, new Symbol("UnKnown", str.Substring(index, len - index), "UnKnown", str.Substring(index, len - index)));
                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                Lexemes.Add(str.Substring(index, len - index));
                throw new ErrorHandler("Identifier expected", "Lexical");
            }
            else
            {
                state = 0;
                unGetChar();
                symbol.Add(address, new Symbol("Inum", str.Substring(index, len - index), "Inum", str.Substring(index, len - index)));
                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                Lexemes.Add(str.Substring(index, len - index));
            }
        }

        private static void DetectFloatNumber(string str)
        {
            c = getNextChar(str);
            if (char.IsDigit(c))
                state = 2;
            else
            {
                state = 0;
                unGetChar();
                symbol.Add(address, new Symbol("Fnum", str.Substring(index, len - index), "Fnum", str.Substring(index, len - index)));
                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                Lexemes.Add(str.Substring(index, len - index));
            }
        }

        private static void DetectIdentify(string str)
        {
            c = getNextChar(str);
            if (isLetter(c) || char.IsDigit(c))
                state = 3;
            else if (c == '[')
            {
                state = 14;
                lexem = str.Substring(index, len - index - 1);
                Lexemes.Add(lexem);
                //Lexemes.Add("[");
                index = len;
            }
            else
            {
                state = 0;
                unGetChar();
                if (lexem == string.Empty)
                    lexem = str.Substring(index, len - index);
                if (!FindType(lexem, "key") && !FindType(lexem, "Condition relation") && !FindType(lexem, "operation") && !FindType(lexem, "Bit relation") && !FindType(lexem, "logic relation"))//!FindSymbol(lexem) && 
                {
                    if (!FindToken(lexem, "id"))
                    {
                        symbol.Add(address, new Symbol("id", lexem, type, string.Empty, Length));
                        File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                        //Lexemes.Add(lexem);
                        lexem = string.Empty;
                        Length = 0;
                        type = string.Empty;
                    }
                    else if (type != string.Empty) { type = string.Empty; throw new ErrorHandler("Embedded statement cannot be a declaration or labeled statement", "Semantic"); }
                }
                else if (FindToken(lexem, "Data Type") && type == string.Empty)
                {
                    type = lexem;
                    lexem = string.Empty;
                }
                else if (FindToken(lexem, "Data Type") && type != string.Empty)
                {
                    lexem = string.Empty;
                    throw new ErrorHandler("Identifier expected", "Semantic");
                }
                //if (!FindToken(lexem, "id")) lexem = string.Empty;
                if (FindSymbol(str.Substring(index, len - index))) Lexemes.Add(str.Substring(index, len - index));
                lexem = string.Empty;
            }
        }

        private static void DetectSpace(string str)
        {
            c = getNextChar(str);
            if (isSpace(c))
                state = 4;
            else
            {
                unGetChar();
                state = 0;
            }
        }

        private static void DetectString(string str)
        {
            c = getNextChar(str);
            if (c == '"')
            {
                state = 0;
                symbol.Add(address, new Symbol("Str", str.Substring(index, len - index), "Str", str.Substring(index, len - index), str.Substring(index, len - index).Length - 2));
                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                Lexemes.Add(str.Substring(index, len - index));
            }
            else
                state = 5;
        }

        private static void DetectChar(string str)
        {
            c = getNextChar(str);
            if (c.ToString() == "'") { state = 0; throw new ErrorHandler("Empty character literal", "Semantic"); }
            else if (c.ToString() != "'") state = 60;
        }

        private static void DetectCharNext(string str)
        {
            c = getNextChar(str);
            if (c.ToString() == "'")
            {
                state = 0;
                symbol.Add(address, new Symbol("Char", str.Substring(index, len - index), "Char", str.Substring(index, len - index)));
                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                Lexemes.Add(str.Substring(index, len - index));
            }
            else { state = 0; throw new ErrorHandler("Too many characters in character literal", "Semantic"); }
        }

        private static void DetectAssign(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add("==");
            else { unGetChar(); Lexemes.Add("="); }
            state = 0;
        }

        private static void DetectGrather(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add(">=");
            else { unGetChar(); Lexemes.Add(">"); }
            state = 0;
        }

        private static void DetectLesser(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add("<=");
            else if (c == '>') Lexemes.Add("<>");
            else { Lexemes.Add("<"); unGetChar(); }
            state = 0;
        }

        private static void DetectMult(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add("*=");
            else { unGetChar(); Lexemes.Add("*"); }
            state = 0;
        }

        private static void DetectDiv(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add("%=");
            else { unGetChar(); Lexemes.Add("%"); }
            state = 0;
        }

        private static void DetectADD(string str)
        {
            c = getNextChar(str);
            if (c == '+') Lexemes.Add("++");
            else if (c == '=') Lexemes.Add("+=");
            else { unGetChar(); Lexemes.Add("+"); }
            state = 0;
        }

        private static void DetectMines(string str)
        {
            c = getNextChar(str);
            if (c == '-') Lexemes.Add("--");
            else if (c == '=') Lexemes.Add("-=");
            else { unGetChar(); Lexemes.Add("-"); }
            state = 0;
        }

        private static void DetectComment(string str)
        {
            c = getNextChar(str);
            if (c == '/')
            {
                while ((Keys)c != Keys.Enter)
                    c = getNextChar(str);
                File.AppendAllText(path, address.ToString() + "\t\tComment\t\t" + str.Substring(index, len - index));
            }
            else if (c == '=') Lexemes.Add("/=");
            else Lexemes.Add("/");
            //else throw new ErrorHandler("Invalid expression term '/'", "Syntax");
            state = 0;
        }

        private static void DetectOpenBraket(string str)
        {
            c = getNextChar(str);
            if (char.IsDigit(c)) state = 14;
            else if (c == ']') state = 15; 
            else if (isDot(c)) throw new ErrorHandler("cannot convert from 'double' to 'int'", "Semantic");
            else throw new ErrorHandler("Only assignment, call, increment, decrement can be used as statement.", "Semantic");
        }

        private static void DetectBraket(string str)
        {           
            c = getNextChar(str);
        }

        private static void DetectCloseBraket(string str)
        {
            int.TryParse(str.Substring(index, len - index - 1), out Length);
            state = 3;
            if (Length == 0) throw new ErrorHandler("value expected", "Semantic");
            //Lexemes.Add(Length.ToString());
            //Lexemes.Add("]");
            //getNextChar(str);
        }

        private static void DetectAND(string str)
        {
            c = getNextChar(str);
            if (c == '&') Lexemes.Add("&&");
            else { unGetChar(); Lexemes.Add("&"); }
            state = 0;
        }

        private static void DetectOR(string str)
        {
            c = getNextChar(str);
            if (c == '|') Lexemes.Add("||");
            else { unGetChar(); Lexemes.Add("|"); }
            state = 0;
        }
        
        private static void DetectWow(string str)
        {
            c = getNextChar(str);
            if (c == '=') Lexemes.Add("!=");
            else { Lexemes.Add("!"); unGetChar(); }
            state = 0;
        }
        #endregion

        #region Public's Method
        public static void setKeysSymbolTable()
        {
            symbol.Add(address++, new Symbol("NULL", "$", "NULL", "$"));
            symbol.Add(address++, new Symbol("semicolon", ";", "semicolon", ";"));

            symbol.Add(address++, new Symbol("If", "if", "key", "if"));
            symbol.Add(address++, new Symbol("Else", "else", "key", "else"));
            symbol.Add(address++, new Symbol("While", "while", "key", "while"));
            symbol.Add(address++, new Symbol("For", "for", "key", "for"));
            symbol.Add(address++, new Symbol("Do", "do", "key", "do"));
            symbol.Add(address++, new Symbol("Switch", "switch", "key", "switch"));
            symbol.Add(address++, new Symbol("Case", "case", "key", "case"));
            symbol.Add(address++, new Symbol("Enum", "enum", "key", "enum"));
            symbol.Add(address++, new Symbol("Class", "class", "key", "class"));
            symbol.Add(address++, new Symbol("Begin", "{", "key", "{"));
            symbol.Add(address++, new Symbol("End", "}", "key", "}"));
            symbol.Add(address++, new Symbol("Open Parentheses", "(", "key", "("));
            symbol.Add(address++, new Symbol("Close Parentheses", ")", "key", ")"));
            symbol.Add(address++, new Symbol("Open bracket", "[", "key", "["));
            symbol.Add(address++, new Symbol("Close bracket", "]", "key", "]"));
            symbol.Add(address++, new Symbol("Include", "include", "key", "include"));
            symbol.Add(address++, new Symbol("NameSpace", "namespace", "key", "namspace"));
            symbol.Add(address++, new Symbol("Using", "using", "key", "using"));
            symbol.Add(address++, new Symbol("New", "new", "key", "new"));
            symbol.Add(address++, new Symbol("Delete", "delete", "key", "delete"));
            symbol.Add(address++, new Symbol("Struct", "struct", "key", "struct"));
            symbol.Add(address++, new Symbol("Static", "static", "key", "static"));
            symbol.Add(address++, new Symbol("Public", "public", "key", "public"));
            symbol.Add(address++, new Symbol("Private", "private", "key", "private"));
            symbol.Add(address++, new Symbol("Protected", "protected", "key", "protected"));;
            symbol.Add(address++, new Symbol("Break", "break", "key", "break"));;

            symbol.Add(address++, new Symbol("Data Type", "int", "key", "int"));
            symbol.Add(address++, new Symbol("Data Type", "float", "key", "float"));
            symbol.Add(address++, new Symbol("Data Type", "double", "key", "double"));
            symbol.Add(address++, new Symbol("Data Type", "bool", "key", "bool"));
            symbol.Add(address++, new Symbol("Data Type", "char", "key", "char"));
            symbol.Add(address++, new Symbol("Data Type", "string", "key", "string"));
            symbol.Add(address++, new Symbol("Data Type", "void", "key", "void"));

            symbol.Add(address++, new Symbol("ADD", "+", "operation", "+"));
            symbol.Add(address++, new Symbol("DIFE", "-", "operation", "-"));
            symbol.Add(address++, new Symbol("MUL", "*", "operation", "*"));
            symbol.Add(address++, new Symbol("DIV", "/", "operation", "/"));
            symbol.Add(address++, new Symbol("INC", "++", "operation", "++"));
            symbol.Add(address++, new Symbol("DEC", "--", "operation", "--"));
            symbol.Add(address++, new Symbol("Assign", "=", "operation", "="));

            symbol.Add(address++, new Symbol("and", "&&", "logic relation", "&&"));
            symbol.Add(address++, new Symbol("or", "||", "logic relation", "||"));
            symbol.Add(address++, new Symbol("not", "!", "logic relation", "!"));

            symbol.Add(address++, new Symbol("and Bit", "&", "Bit relation", "&"));
            symbol.Add(address++, new Symbol("or Bit", "|", "Bit relation", "|"));

            symbol.Add(address++, new Symbol("eqall", "==", "Condition relation", "=="));
            symbol.Add(address++, new Symbol("grather Than", ">=", "Condition relation", ">="));
            symbol.Add(address++, new Symbol("grather", ">", "Condition relation", ">"));
            symbol.Add(address++, new Symbol("leter", "<", "Condition relation", "<"));
            symbol.Add(address++, new Symbol("leter Than", "<=", "Condition relation", "<="));
            symbol.Add(address++, new Symbol("not Eqall", "<>", "Condition relation", "<>"));

            File.WriteAllText(path, "Address\t\tToken\t\tLexem\t\tType\t\tValue\t\tLength\r\n");
            for (int i = 0; i < symbol.Count; i++)
                File.AppendAllText(path, i.ToString() + "\t\t" + symbol[i].token + "\t\t" + symbol[i].lexem + "\t\t" + symbol[i].type + "\t\t" + symbol[i].value + "\t\t" + symbol[i].length + "\r\n");
        }

        public static string Compile(string str)
        {
            len = 0;
            Lexemes.Clear();
            string error = string.Empty;
            while (len <= str.Length)
                try
                {
                    switch (state)
                    {
                        case 0: DetectCharacter(str); break;
                        case 1: DetectIntNumber(str); break;
                        case 2: DetectFloatNumber(str); break;
                        case 3: DetectIdentify(str); break;
                        case 4: DetectSpace(str); break;
                        case 5: DetectString(str); break;
                        case 6: DetectChar(str); break;
                        case 60: DetectCharNext(str); break;
                        case 7: DetectMult(str); break;
                        case 8: DetectLesser(str); break;
                        case 9: DetectAssign(str); break;
                        case 10: DetectADD(str); break;
                        case 11: DetectMines(str); break;
                        case 12: DetectDiv(str); break;
                        case 13: DetectComment(str); break;
                        case 14: DetectOpenBraket(str); break;
                        case 15: DetectCloseBraket(str); break;
                        case 16: DetectGrather(str); break;
                        case 17: DetectAND(str); break;
                        case 18: DetectOR(str); break;
                        case 19: DetectBraket(str); break;
                        case 20: DetectWow(str); break;
                    }
                }
                catch (ErrorHandler err) { error += (err.Message + "\r\n"); }
            return error;
        }
        #endregion
    }
}
