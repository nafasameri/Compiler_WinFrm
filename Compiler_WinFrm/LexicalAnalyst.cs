using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System;

namespace Compiler_WinFrm
{
    struct Symbol
    {
        public string token;
        public string lexem;
        public string type;
        public string value;
        public int length;

        public Symbol(string token, string lexem, string type, string value, int length = 0)
        {
            this.token = token;
            this.lexem = lexem;
            this.type = type;
            this.value = value;
            this.length = length;
        }
    }

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
            if (c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'h' || c == 'i' || c == 'j' || c == 'k' || c == 'l' || c == 'm' || c == 'n' ||
                c == 'o' || c == 'p' || c == 'q' || c == 'r' || c == 's' || c == 't' || c == 'u' || c == 'y' || c == 'w' || c == 'x' || c == 'v' || c == 'z' || c == 'g' ||
                c == 'A' || c == 'B' || c == 'C' || c == 'D' || c == 'E' || c == 'F' || c == 'H' || c == 'I' || c == 'J' || c == 'K' || c == 'L' || c == 'M' || c == 'N' ||
                c == 'O' || c == 'P' || c == 'Q' || c == 'R' || c == 'S' || c == 'T' || c == 'U' || c == 'Y' || c == 'W' || c == 'X' || c == 'V' || c == 'Z' || c == 'G')
                return true;
            return false;
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

        private static bool FindKeys(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == "key") 
                    return true;
            return false;
        }

        private static bool FindOperations(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == "operation")
                    return true;
            return false;
        }

        private static bool FindRelation(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == "Condition relation")
                    return true;
            return false;
        }

        private static bool FindBitRelation(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == "Bit relation")
                    return true;
            return false;
        }

        private static bool FindLogicRelation(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].type == "logic relation")
                    return true;
            return false;
        }

        private static bool FindDataType(string lexem)
        {
            for (int i = 0; i < symbol.Count; i++)
                if (symbol[i].lexem == lexem && symbol[i].token == "Data Type")
                    return true;
            return false;
        }
        #endregion

        #region Detect's Method
        private static void DetectCharacter(string str)
        {
            index = len;
            c = getNextChar(str);
            if (c == NULL) throw new ErrorHandler("Unexpected character '" + NULL + "'");
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
            else if (c == '!') { Lexemes.Add("!"); }
            else if (c == '&') state = 17;
            else if (c == '|') state = 18;
            else if (c == '@') throw new ErrorHandler("Keyword, identifier, or string expected after verbatim specifier: @");
            else if (c == ';') Lexemes.Add(";");

            else if ((Keys)c == Keys.Back) { }
            else if ((Keys)c == Keys.Enter) { }

            else if (c == char.MaxValue) { }
            else if (c == '\n') { }
            else if (c == '\r') { }

            else throw new ErrorHandler("Unreachable code detected!");
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
                throw new ErrorHandler("Identifier expected");
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
                index = len;
            }
            else
            {
                state = 0;
                unGetChar();
                if (lexem == string.Empty)
                    lexem = str.Substring(index, len - index);
                if (!FindSymbol(lexem) && !FindKeys(lexem) && !FindRelation(lexem) && !FindOperations(lexem) && !FindBitRelation(lexem) && !FindLogicRelation(lexem))
                {
                    symbol.Add(address, new Symbol("id", lexem, type, string.Empty, Length));
                    File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                    //Lexemes.Add(lexem);
                    lexem = string.Empty;
                    Length = 0;
                    type = string.Empty;
                }
                else if (FindDataType(lexem) && type == string.Empty)
                {
                    type = lexem;
                    lexem = string.Empty;
                }
                else if (FindDataType(lexem) && type != string.Empty)
                    throw new ErrorHandler("Identifier expected");
                if (FindSymbol(str.Substring(index, len - index))) Lexemes.Add(str.Substring(index, len - index));
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
            symbol.Add(address++, new Symbol("inc", "++", "operation", "++"));
            symbol.Add(address++, new Symbol("dec", "--", "operation", "--"));

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

            //symbol.Add(address++, new Symbol("id", "a", "int", ""));

            //File.WriteAllText(path, "Address\t\tToken\t\tLexem\t\tType\t\tValue\t\tLength\r\n");
            //for (int i = 0; i < symbol.Count; i++)
            //    File.AppendAllText(path, i.ToString() + "\t\t" + symbol[i].token + "\t\t" + symbol[i].lexem + "\t\t" + symbol[i].type + "\t\t" + symbol[i].value + "\t\t" + symbol[i].length + "\r\n");
        }

        public static string Compile(string str)
        {
            len = 0;
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
                        case 6: // character
                            c = getNextChar(str);
                            if (c.ToString() == "'") { state = 0; throw new ErrorHandler("Empty character literal"); }
                            else if (c.ToString() != "'") state = 60;
                            break;
                        case 60: // character
                            c = getNextChar(str);
                            if (c.ToString() == "'")
                            {
                                state = 0;
                                symbol.Add(address, new Symbol("char", str.Substring(index, len - index), "char", str.Substring(index, len - index)));
                                File.AppendAllText(path, address.ToString() + "\t\t" + symbol[address].token + "\t\t" + symbol[address].lexem + "\t\t" + symbol[address].type + "\t\t" + symbol[address].value + "\t\t" + symbol[address++].length.ToString() + "\r\n");
                                Lexemes.Add(str.Substring(index, len - index));
                            }
                            else { state = 0; throw new ErrorHandler("Too many characters in character literal"); }
                            break;
                        case 9: // == =
                            c = getNextChar(str);
                            if (c == '=') Lexemes.Add("==");
                            else { unGetChar(); Lexemes.Add("="); }
                            state = 0;
                            break;
                        case 16: // >= >
                            c = getNextChar(str);
                            if (c == '=') Lexemes.Add(">=");
                            else { unGetChar(); Lexemes.Add(">"); }
                            state = 0;
                            break;
                        case 8: // <= < <>
                            c = getNextChar(str);
                            if (c == '=') Lexemes.Add("<=");
                            else if (c == '>') Lexemes.Add("<>");
                            else { Lexemes.Add("<"); unGetChar(); }
                            state = 0;
                            break;
                        case 7: // *= *
                            c = getNextChar(str);
                            if (c == '=') Lexemes.Add("*=");
                            else { unGetChar(); Lexemes.Add("*"); }
                            state = 0;
                            break;
                        case 12: // %= %
                            c = getNextChar(str);
                            if (c == '=') Lexemes.Add("%=");
                            else { unGetChar(); Lexemes.Add("%"); }
                            state = 0;
                            break;
                        case 10: //++ + +=
                            c = getNextChar(str);
                            if (c == '+') Lexemes.Add("++");
                            else if (c == '=') Lexemes.Add("+=");
                            else { unGetChar(); Lexemes.Add("+"); }
                            state = 0;
                            break;
                        case 11: //-- - -=
                            c = getNextChar(str);
                            if (c == '-') Lexemes.Add("--");
                            else if (c == '=') Lexemes.Add("-=");
                            else { unGetChar(); Lexemes.Add("-"); }
                            state = 0;
                            break;
                        case 13: // comment
                            c = getNextChar(str);
                            if (c == '/')
                            {
                                while ((Keys)c != Keys.Enter)
                                    c = getNextChar(str);
                                File.AppendAllText(path, address.ToString() + "\t\tComment\t\t" + str.Substring(index, len - index));
                                //Lexemes.Add("comment");
                            }
                            else if (c == '=') Lexemes.Add("/=");
                            else Lexemes.Add("/");
                            //else throw new ErrorHandler("Invalid expression term '/'");
                            state = 0;
                            break;
                        case 14: // array [
                            c = getNextChar(str);
                            if (char.IsDigit(c)) state = 14;
                            else if (c == ']') state = 15;
                            else if (isDot(c)) throw new ErrorHandler("cannot convert from 'double' to 'int'");
                            else throw new ErrorHandler("Only assignment, call, increment, decrement can be used as statement.");
                            break;
                        case 15: // ]
                            int.TryParse(str.Substring(index, len - index - 1), out Length);
                            state = 3;
                            getNextChar(str);
                            break;
                        case 17: // && &
                            c = getNextChar(str);
                            if (c == '&') Lexemes.Add("&&");
                            else { unGetChar(); Lexemes.Add("&"); }
                            state = 0;
                            break;
                        case 18: // || |
                            c = getNextChar(str);
                            if (c == '|') Lexemes.Add("||");
                            else { unGetChar(); Lexemes.Add("|"); }
                            state = 0;
                            break;
                    }
                }
                catch (ErrorHandler err) { error += (err.Message + "\r\n"); }
            return error;
        }
        #endregion
    }
}
