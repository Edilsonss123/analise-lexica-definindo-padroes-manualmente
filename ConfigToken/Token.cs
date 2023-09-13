namespace edilex.ConfigToken
{
    public class Token
    {
        public TipoToken Nome;
        public string Lexema;

        public Token(TipoToken nome, string lexema)
        {
            this.Nome = nome;
            this.Lexema = lexema;
        }

        public override string ToString()
        {
            return $"<{this.Nome},{this.Lexema}>";
        }
    }
}