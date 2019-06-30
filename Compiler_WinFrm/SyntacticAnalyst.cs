using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Compiler_WinFrm
{
    class SyntacticAnalyst
    {
        private const string path = @"E:\SyntacticAnalyst.txt";
        private const string pathStk = @"E:\Stack.txt";
        private const char NULL = '$';
        private const char Landa = '~';
        private static int Len = 0;
        
        #region Help's Method
        private static int FindAddress(string lexem)
        {
            for (int i = 0; i < LexicalAnalyst.symbol.Count; i++)
                if (LexicalAnalyst.symbol[i].lexem == lexem)
                    return i;
            return -1;
        }

        private static string[,] SetCLRTable()
        {
            string EPlus = "The + or - operator must be applied to a pointer";
            string EId = "Identifier expected";
            string EMul = "The * or / operator must be applied to a pointer";
            string EVal = "value expected";
            string EOParantes = "'(' expected";
            string ECParantes = "')' expected";
            string EOAcolad = "Invalid expression term '{'";
            string ECAcolad = "Invalid expression term '}'";
            string EOPCon = "Cannot implicitly convert type 'int' to 'bool'";
            string EOPLogic= "Cannot implicitly convert type 'int' to 'bool'";
            string EIf = "Only 'if' expression can be used as a statement";
            string EElse = "Only 'else' expression can be used as a statement";
            string EWhile = "Only 'while' expression can be used as a statement";
            string EDo = "Only 'do' expression can be used as a statement";
            string EFor = "Only 'for' expression can be used as a statement";
            string ESwitch = "Only 'switch' expression can be used as a statement";
            string ECase = "Only 'case' expression can be used as a statement";
            string EBreak = "Only 'break' expression can be used as a statement";
            string EDT = "The name '" + LexicalAnalyst.Lexemes[Len].ToString() + "' does not exist in the current context";
            string ESC = "; expected";
            string EIC = "Only assignment, increment, decrement, expressions can be used as a statement";
            string EAssign = "Only assignment, increment, decrement, expressions can be used as a statement";
            string EColon = ": expected";
            string EDollar = "Invalid expression term '}' or 'break'";
            string EWow = "";

            string[,] clr = {
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"1","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//0
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"acc","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//1
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//2
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//3
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//4
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//5
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//6
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//7
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1S",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r1S",EDT,ESC,EIC,EWow,EAssign,"r1S","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//8
                {EId,EVal,EPlus,EMul,"S17",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//9
                {EId,EVal,EPlus,EMul,"S18",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//10
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S19",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//11
                {EId,EVal,EPlus,EMul,"S27",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//12
                {EId,EVal,EPlus,EMul,"S28",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//13
                {"S21",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//14
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,"S24",EWow,"S26",EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//15
                {"S20",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//16
                {"S42",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","41","","","","","","50","42","57","","","56",EColon,EElse},//17
                {"S42",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","69","","","","","","50","42","57","","","56",EColon,EElse},//18
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"90","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//19
                {EId,EVal,EPlus,EMul,EOParantes,"r2M",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,"r2M",ECase,"r2M",EDT,"r2M",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//20
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S22",EIC,EWow,"S25",EAssign,EDollar,"","","","","","","","","","","","","23","","","","","","",EColon,EElse},//21
                {EId,EVal,EPlus,EMul,"r1X",ECParantes,EOAcolad,"r1X",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r1X","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//22
                {EId,EVal,EPlus,EMul,"r3V",ECParantes,EOAcolad,"r3V",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r3V","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//23
                {EId,EVal,EPlus,EMul,EOParantes,"r2M",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,"r2M",ECase,"r2M",EDT,"r2M",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//24
                {"S48",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","48","57","","","56",EColon,EElse},//25
                {"S51",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","51","57","","","56",EColon,EElse},//26
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"","","","","","","","102","","98","","","97","","","","","","","",EColon,EElse},//27
                {"S29",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//28
                {EId,EVal,EPlus,EMul,EOParantes,"S30",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//29
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S31",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//30
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r1Z",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,"S32",EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","35","","","","","","","","","","","","","","","",EColon,EElse},//31
                {"S33","S33",EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//32
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","","S34",EElse},//33
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"37","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//34
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S36",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//35
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r7Y",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r7Z",EDT,ESC,EIC,EWow,EAssign,"r7Y","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//36
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"S38",EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//37
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S39",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//38
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,"S32",EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","40","","","","","","","","","","","","","","","",EColon,EElse},//39
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r7Z",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//40
                {EId,EVal,EPlus,EMul,EOParantes,"S46",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//41
                {EId,EVal,"r1B","S54",EOParantes,ECParantes,EOAcolad,ECAcolad,"S76",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//42
                {EId,EVal,EPlus,EMul,"S44",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//43
                {"S42",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,"S43",EWow,EAssign,EDollar,"","","","","","","","","","","","","","","50","42","57","","","56",EColon,EElse},//44
                {EId,EVal,"r1P","r1P",EOParantes,"r1P",EOAcolad,ECAcolad,"r1P","r1P",EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"r1P",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//45
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S47",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//46
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"60","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//47
                {EId,EVal,"r1B","S54",EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S49",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//48
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r3X",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r3X",EDT,"r3X",EIC,EWow,EAssign,"r3X","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//49
                {EId,EVal,EPlus,EMul,EOParantes,"r1C",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"r1C",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//50
                {EId,EVal,"r1B","S54",EOParantes,"r1B",EOAcolad,"r1B",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S52",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//51
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"r4M",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,"r4M",EDT,ESC,EIC,EWow,EAssign,"r4M","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//52
                {EId,EVal,"r2T",EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//53
                {"S45",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","55",EColon,EElse},//54
                {EId,EVal,"r1B","S54",EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","114","",EColon,EElse},//55
                {EId,EVal,"r1B","S54",EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//56
                {EId,EVal,"S59",EMul,EOParantes,"r1A",EOAcolad,ECAcolad,"r1A",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","58","","",EColon,EElse},//57
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,"r2E",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//58
                {"S116",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","116","117","","","56",EColon,EElse},//59
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S61",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//60
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","62","","","","","","","","","",EColon,"S63"},//61
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r8I","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//62
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S65",ECAcolad,EOPCon,EOPLogic,"S9",EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","115","","","","","","","","","","64","","","","","","","","",EColon,EElse},//63
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r2N","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//64
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"66","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//65
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S67",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//66
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r1N","","","","","","","","","","","68","","","","","","","","","",EColon,"S63"},//67
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r4G","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//68
                {EId,EVal,EPlus,EMul,EOParantes,"S86",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//69
                {"S42",EVal,EPlus,EMul,"S70",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","74","","","","","","50","73","57","","","56",EColon,EElse},//70
                {EId,EVal,EPlus,EMul,EOParantes,"S72",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//71
                {EId,EVal,EPlus,EMul,EOParantes,"r4C",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"r4C",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//72
                {EId,EVal,"r1B","S54",EOParantes,"S75",EOAcolad,ECAcolad,"S76",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//73
                {EId,EVal,EPlus,EMul,EOParantes,"S78",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//74
                {EId,EVal,"r3P","r3P",EOParantes,"r3P",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"r3P",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//75
                {"S77",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","77","57","","","56",EColon,EElse},//76
                {EId,EVal,"r1B","S54",EOParantes,"r3C",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//77
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,"S79",EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//78
                {EId,EVal,EPlus,EMul,"S83",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//79
                {"S42",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","81","57","","","56",EColon,EElse},//80
                {EId,EVal,"r1B","S54","S82",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//81
                {EId,EVal,EPlus,"r3P",EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//82
                {"S42",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","84","","","","","","50","42","57","","","56",EColon,EElse},//83
                {EId,EVal,EPlus,EMul,EOParantes,"S85",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//84
                {EId,EVal,EPlus,EMul,EOParantes,"r7R",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//85
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S87",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//86
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"88","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//87
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S89",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//88
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r7W","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//89
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S91",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//90
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,"S92",EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//91
                {EId,EVal,EPlus,EMul,"S93",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//92
                {"S42",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","94","","","","","","50","42","57","","","56",EColon,EElse},//93
                {EId,EVal,EPlus,EMul,EOParantes,"S95",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//94
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S96",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//95
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r9D","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//96
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r3F","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//97
                {"S42",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","99","","","","","","50","42","57","","","56",EColon,EElse},//98
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S100",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//99
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,"S16",EWow,EAssign,EDollar,"","","","","","","","","","101","","","","","","","","","","",EColon,EElse},//100
                {EId,EVal,EPlus,EMul,EOParantes,"S104",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//101
                {"S42",EVal,EPlus,EMul,"S80",ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,"S43",EAssign,EDollar,"","","","","","","","","103","","","","","","50","42","57","","","56",EColon,EElse},//102
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,"S108",EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//103
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S105",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//104
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"106","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//105
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S107",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//106
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r8U","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//107
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,"S16",EWow,EAssign,EDollar,"","","","","","","","","","109","","","","","","","","","","",EColon,EElse},//108
                {EId,EVal,EPlus,EMul,EOParantes,"S110",EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//109
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,"S111",ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//110
                {"S15",EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,"S9","S10","S11","S12","S13",ECase,EBreak,"S14",ESC,"S16",EWow,EAssign,EDollar,"112","2","5","6","","3","4","8","","7","","","","","","","","","","",EColon,EElse},//111
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,"S113",EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//112
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r8U","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//113
                {EId,EVal,"r3B",EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//114
                {EId,EVal,EPlus,EMul,EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,"r1G","","","","","","","","","","","","","","","","","","","","",EColon,EElse},//115
                {EId,EVal,"r1B","S54",EOParantes,ECParantes,EOAcolad,ECAcolad,EOPCon,EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","53","",EColon,EElse},//116
                {EId,EVal,"S59","r1A",EOParantes,ECParantes,EOAcolad,ECAcolad,"r1A",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","118","","",EColon,EElse},//117
                {EId,EVal,EPlus,"r2E",EOParantes,ECParantes,EOAcolad,ECAcolad,"r3A",EOPLogic,EIf,EWhile,EDo,EFor,ESwitch,ECase,EBreak,EDT,ESC,EIC,EWow,EAssign,EDollar,"","","","","","","","","","","","","","","","","","","","",EColon,EElse},//118
            };
            return clr;
        }
        #endregion

        public static string Compile()
        {
            Len = 0;
            string error = string.Empty;
            string[,] clr = SetCLRTable();
            Stack<string> stk = new Stack<string>();
            LexicalAnalyst.Lexemes.Add(NULL);
            stk.Push("0");

            #region Write Lexemes
            foreach (var item in LexicalAnalyst.Lexemes)
                File.AppendAllText(path, item.ToString() + "\t");
            File.AppendAllText(path, "\r\n");
            #endregion

            File.WriteAllText(pathStk,"");

            while (Len < LexicalAnalyst.Lexemes.Count) 
            {
                try
                {
                    int row = -1, col = -1;
                    int address = FindAddress(LexicalAnalyst.Lexemes[Len].ToString());
                    string topStk = stk.Peek();
                    int.TryParse(topStk, out row);

                    switch (LexicalAnalyst.Lexemes[Len].ToString())
                    {
                        case "+": case "-": col = 2; break;
                        case "*": case "/": case "%": case "^": col = 3; break;
                        case "(": col = 4; break;
                        case ")": col = 5; break;
                        case "{": col = 6; break;
                        case "}": col = 7; break;
                        case "if": col = 10; break;
                        case "while": col = 11; break;
                        case "do": col = 12; break;
                        case "for": col = 13; break;
                        case "switch": col = 14; break;
                        case "case": col = 15; break;
                        case "break": col = 16; break;
                        case ";": col = 18; break;
                        case "--": case "++": col = 19; break;
                        case "!": col = 20; break;
                        case "=": col = 21; break;
                        case "$": col = 22; break;
                        case "S": col = 23; break;
                        case "I": col = 24; break;
                        case "F": col = 25; break;
                        case "Y": col = 26; break;
                        case "Z": col = 27; break;
                        case "W": col = 28; break;
                        case "D": col = 29; break;
                        case "V": col = 30; break;
                        case "C": col = 31; break;
                        case "M": col = 32; break;
                        case "N": col = 33; break;
                        case "G": col = 34; break;
                        case "U": col = 35; break;
                        case "X": col = 36; break;
                        case "R": col = 37; break;
                        case "E": col = 38; break;
                        case "T": col = 39; break;
                        case "A": col = 40; break;
                        case "B": col = 41; break;
                        case "P": col = 42; break;
                    }
                    switch (LexicalAnalyst.symbol[address].token)
                    {
                        case "Inum": case "Fnum": case "Char": case "Str": case "id": col = 0; break;
                        case "Data Type": col = 17; break;
                    }
                    switch (LexicalAnalyst.symbol[address].type)
                    {
                        case "Condition relation": col = 8; break;
                        case "logic relation": col = 9; break;
                    }

                    if (clr[row, col].Length < 5 && clr[row, col].Length != 0)
                    {
                        if (clr[row, col] == "acc") break;
                        else if (clr[row, col][0] == 'S')
                        {
                            stk.Push(LexicalAnalyst.Lexemes[Len++].ToString());
                            stk.Push(clr[row, col].Substring(1, clr[row, col].Length - 1));
                        }
                        else if (clr[row, col][0] == 'r')
                        {
                            int popCount;
                            int.TryParse(clr[row, col][1].ToString(), out popCount);
                            for (int p = 1; p <= (popCount * 2); p++)
                                stk.Pop();
                            string temp = clr[row, col][2].ToString();
                            topStk = stk.Peek();
                            int.TryParse(topStk, out row);
                            stk.Push(temp);
                            switch (temp)
                            {
                                case "S": col = 23; break;
                                case "I": col = 24; break;
                                case "F": col = 25; break;
                                case "Y": col = 26; break;
                                case "Z": col = 27; break;
                                case "W": col = 28; break;
                                case "D": col = 29; break;
                                case "V": col = 30; break;
                                case "C": col = 31; break;
                                case "M": col = 32; break;
                                case "N": col = 33; break;
                                case "G": col = 34; break;
                                case "U": col = 35; break;
                                case "X": col = 36; break;
                                case "R": col = 37; break;
                                case "E": col = 38; break;
                                case "T": col = 39; break;
                                case "A": col = 40; break;
                                case "B": col = 41; break;
                                case "P": col = 42; break;
                            }
                            stk.Push(clr[row, col]);
                        }

                        #region Write stack
                        string[] array = stk.ToArray();
                        foreach (var item in array)
                            File.AppendAllText(pathStk, item + "\t");
                        File.AppendAllText(pathStk, "\r\n");
                        #endregion
                    }
                    else { throw new ErrorHandler(clr[row, col], "Syntax"); }
                }
                catch (ErrorHandler err) { error += (err.Message + "\r\n"); break; }
            }
            return error;
        }
    }
}