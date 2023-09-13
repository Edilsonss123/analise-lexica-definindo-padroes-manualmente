
using edilex.ConfigToken;

namespace edilex.Manipuladores
{
    public class EdiLexico
    {
        private LeitorDeArquivosTexto _leitorDeArquivosTexto;

        public EdiLexico(string arquivo)
        {
            this._leitorDeArquivosTexto = new LeitorDeArquivosTexto(arquivo);
        }

        public Token ProximoToken()
        {
            try
            {
                Token proximoToken = null;

                this.MontarTokenEspacoEComentario();
                _leitorDeArquivosTexto.Confirmar();

                proximoToken = this.MontarTokenFim();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenPalavrasChave();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenVariavel();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenNumero();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenOperadorAritmetico();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenOperadorRelacional();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenDelimitador();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenParenteses();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                proximoToken = this.MontarTokenCadeia();
                if (proximoToken == null) _leitorDeArquivosTexto.Zerar();
                else {
                    _leitorDeArquivosTexto.Confirmar();
                    return proximoToken;
                }

                Console.WriteLine(_leitorDeArquivosTexto.ToString());
            }
            catch (System.Exception)
            {
                Console.WriteLine("Erro léxico");
            }
            return null;
        }

        private Token? MontarTokenParenteses()
        {
            int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
            char c =   (char)caracterLido;
            switch (c)
            {
                case '(':
                    return new Token(TipoToken.AbrePar, this._leitorDeArquivosTexto.Lexema());
                case ')':
                    return new Token(TipoToken.FechaPar, this._leitorDeArquivosTexto.Lexema());
                default:
                    return null;
            }
        }

        private Token? MontarTokenOperadorRelacional()
        {
            int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
            char c =   (char)caracterLido;
            switch (c)
            {
                case '<':
                    caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                    char c2 =   (char)caracterLido;
                    if (c2 == '>')
                        return new Token(TipoToken.OpRelDif, this._leitorDeArquivosTexto.Lexema());
                    else if (c2 == '=')
                        return new Token(TipoToken.OpRelMenorIgual, this._leitorDeArquivosTexto.Lexema());
                    else {
                        this._leitorDeArquivosTexto.Retroceder();
                        return new Token(TipoToken.OpRelMenor, this._leitorDeArquivosTexto.Lexema());
                    } 
                case '=':
                    return new Token(TipoToken.OpRelIgual, this._leitorDeArquivosTexto.Lexema());
                case '>':
                    caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                    c2 =   (char)caracterLido;
                    if (c2 == '=')
                        return new Token(TipoToken.OpRelMaiorIgual, this._leitorDeArquivosTexto.Lexema());
                    else {
                        this._leitorDeArquivosTexto.Retroceder();
                        return new Token(TipoToken.OpRelMaior, this._leitorDeArquivosTexto.Lexema());
                    } 
                default:
                    return null;
            }
        }

        private Token? MontarTokenOperadorAritmetico()
        {
            int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
            char c =   (char)caracterLido;
            switch (c)
            {
               case '*':
                    return new Token(TipoToken.OpAritMult, this._leitorDeArquivosTexto.Lexema());
                case '/':
                    return new Token(TipoToken.OpAritDiv, this._leitorDeArquivosTexto.Lexema());
                case '+':
                    return new Token(TipoToken.OpAritSoma, this._leitorDeArquivosTexto.Lexema());
                case '-':
                    return new Token(TipoToken.OpAritSub, this._leitorDeArquivosTexto.Lexema());
                default:
                    return null;
            }
        }

        private Token? MontarTokenDelimitador()
        {
            int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
            char c =   (char)caracterLido;

            return c == ':' ? new Token(TipoToken.Delim, this._leitorDeArquivosTexto.Lexema()): null;
        }

        /*numeros*/
        private Token? MontarTokenNumero()
        {
            int estado =1;
            while (true)
            {
                int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                char c =   (char)caracterLido;

                switch (estado)
                {
                    case 1:
                        if (Char.IsDigit(c)) estado = 2;
                        else return null;
                        break;
                    case 2:
                        if (c == '.') {
                            c =   (char)caracterLido;
                            if (Char.IsDigit(c)) estado = 3;
                            else return null;
                        } else if (!Char.IsDigit(c))
                        {
                            this._leitorDeArquivosTexto.Retroceder();
                            return new Token(TipoToken.NumInt, this._leitorDeArquivosTexto.Lexema());
                        }
                        break;
                    case 3:
                        if (!Char.IsDigit(c)) {
                            this._leitorDeArquivosTexto.Retroceder();
                            return new Token(TipoToken.NumReal, this._leitorDeArquivosTexto.Lexema());
                        }
                        break;
                    default:
                        return null;
                }
                 
            }

        }

        /*variavel*/
        private Token? MontarTokenVariavel()
        {
            int estado =1;
            while (true)
            {
                int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                char c =   (char)caracterLido;

                switch (estado)
                {
                    case 1:
                        if (Char.IsLetter(c)) estado = 2;
                        else return null;
                        break;
                    case 2:
                        if (!Char.IsLetterOrDigit(c)) {
                            this._leitorDeArquivosTexto.Retroceder();
                            return new Token(TipoToken.Var, this._leitorDeArquivosTexto.Lexema());
                        }
                        break;
                    default:
                        return null;
                }
                 
            }

        }

        /*cadeia*/
        private Token? MontarTokenCadeia()
        {
            int estado =1;
            while (true)
            {
                int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                char c =   (char)caracterLido;

                switch (estado)
                {
                    case 1:
                        if (c == '\'') estado = 2;
                        else return null;
                        break;
                    case 2:
                        if (c == '\n') return null;

                        if (c == '\'') return new Token(TipoToken.Cadeia, _leitorDeArquivosTexto.Lexema());
                        else if(c == '\\') estado = 3;

                        break;
                    case 3:
                        if (c == '\n') return null;
                        else estado = 2;
                        break;
                    default:
                        return null;
                }
                 
            }

        }

        /*espacoes e comentarios*/
        private Token? MontarTokenEspacoEComentario()
        {
            int estado =1;
            while (true)
            {
                int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                char c =   (char)caracterLido;

                switch (estado)
                {
                    case 1:
                        if (Char.IsWhiteSpace(c) || c == ' ') estado = 2;
                        else if(c == '%') estado = 3;
                        else {
                            _leitorDeArquivosTexto.Retroceder();
                            return null;
                        };
                        break;
                    case 2:
                        if (c == '%') estado = 3;
                        else if(!(Char.IsWhiteSpace(c) || c == ' ')) {
                            _leitorDeArquivosTexto.Retroceder();
                            return null;
                        }

                        break;
                    case 3:
                        if (c == '\n') return null;
                        break;
                    default:
                        return null;
                }
                 
            }

        }

        /*palavras e chaves*/
        private Token? MontarTokenPalavrasChave()
        {
            while (true)
            {
                int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();
                char c =   (char)caracterLido;
                if (!Char.IsLetter(c))
                {    
                    _leitorDeArquivosTexto.Retroceder();
                    string lexema = _leitorDeArquivosTexto.Lexema();               
                    switch (lexema)
                    {
                        case "DECLARACOES":
                            return new Token(TipoToken.PCDeclaracoes ,lexema);
                        case "ALGORITMO":
                            return new Token(TipoToken.PCAlgoritmo ,lexema);
                        case "INT":
                            return new Token(TipoToken.PCInteiro ,lexema);
                        case "REAL":
                            return new Token(TipoToken.PCReal ,lexema);
                        case "ATRIBUIR":
                            return new Token(TipoToken.PCAtribuir ,lexema);
                        case "A":
                            return new Token(TipoToken.PCA ,lexema);
                        case "LER":
                            return new Token(TipoToken.PCLer ,lexema);
                        case "IMPRIMIR":
                            return new Token(TipoToken.PCImprimir ,lexema);
                        case "SE":
                            return new Token(TipoToken.PCSe ,lexema);
                        case "ENTAO":
                            return new Token(TipoToken.PCEntao ,lexema);
                        case "ENQUANTO":
                            return new Token(TipoToken.PCEnquanto ,lexema);
                        case "INICIO":
                            return new Token(TipoToken.PCInicio ,lexema);
                        case "FIM":
                            return new Token(TipoToken.PCFim ,lexema);
                        case "E":
                            return new Token(TipoToken.OpBoolE ,lexema);
                        case "OU":
                            return new Token(TipoToken.OpBoolOu ,lexema);
                        default:
                            return null;
                    }
                }
                 
            }

        }

        /*fim*/ //1:0000

        private Token? MontarTokenFim()
        {
            int caracterLido = this._leitorDeArquivosTexto.LerProximoCaractere();

            if(caracterLido == -1) return new Token(TipoToken.Fim, "Fim");

            return null;
        }
    }
}