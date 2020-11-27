namespace Compiler
{
    public struct Symbol
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
}
