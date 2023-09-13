using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace edilex.Manipuladores
{
    public class LeitorDeArquivosTexto
    {
        private const int TAMANHO_BUFFER = 20;
        private int[] _bufferDeLeitura;
        private int _ponteiro;
        private int _bufferAtual;
        private int _inicioLexema;
        private string _lexema;
        private readonly ILogger<LeitorDeArquivosTexto> _logger;
        private StreamReader _fileReader;
        public LeitorDeArquivosTexto(string file)
        {
            this._logger = Log.getLoggerFactory().CreateLogger<LeitorDeArquivosTexto>(); 

            try {
                this._fileReader = new StreamReader(file, Encoding.UTF8);
                InicializarBuffer();
            } catch (Exception e) {
                _logger.LogError(e.Message);
            }
        }

        private void InicializarBuffer()
        {
            this._bufferAtual = 2;
            this._inicioLexema = 0;
            this._lexema = "";
            _bufferDeLeitura = new int[TAMANHO_BUFFER * 2];
            this._ponteiro = 0;
            RecarregarBuffer1();
        }

        private void IncrementarPonteiro()
        {
            this._ponteiro++;
            if (this._ponteiro == TAMANHO_BUFFER)
                RecarregarBuffer2();
            else if (this._ponteiro == TAMANHO_BUFFER * 2) {
                RecarregarBuffer1();
                this._ponteiro = 0;
            }
        }
        
        private void RecarregarBuffer1()
        {
            if (this._bufferAtual == 2)
            {
                this._bufferAtual = 1;

                for (int i = 0; i < TAMANHO_BUFFER; i++)
                {
                    try {
                        _bufferDeLeitura[i] = this._fileReader.Read();
                        if (_bufferDeLeitura[i] == -1) break;
                    } catch (Exception e) {
                        _logger.LogError(e.Message);
                    }
                }
            }
        }
        private void RecarregarBuffer2()
        {
            if (this._bufferAtual == 1)
            {
                this._bufferAtual = 2;
                
                for (int i = TAMANHO_BUFFER; i < TAMANHO_BUFFER * 2; i++)
                {
                    try {
                        _bufferDeLeitura[i] = this._fileReader.Read();
                        if (_bufferDeLeitura[i] == -1) break;
                    } catch (Exception e) {
                        _logger.LogError(e.Message);
                    }
                }
            }
        }

        private int LerCaractereDoBuffer()
        {
            int ret = _bufferDeLeitura[this._ponteiro];
            this.IncrementarPonteiro();
            return ret;
        }


        public int LerProximoCaractere()
        {
            int ret = LerCaractereDoBuffer();
            this._lexema += (char)ret;
            return ret;
        }

        public void Retroceder()
        {
            this._ponteiro--;
            this._lexema = this._lexema.Substring(0, this._lexema.Length - 1);
            if(this._ponteiro < 0)
                this._ponteiro = TAMANHO_BUFFER * 2 - 1;
        }

        public void Zerar()
        {
            this._ponteiro = this._inicioLexema;
            this._lexema = "";
        }

        public void Confirmar()
        {
            this._inicioLexema = this._ponteiro;
            this._lexema = "";
        }

        public string Lexema()
        {
            return this._lexema;
        }

        public override string ToString()
        {
            string ret  = "Buffer:[";
            foreach (int b in _bufferDeLeitura)
            {
                char c = Convert.ToChar(b);
                ret += Char.IsWhiteSpace(c) ? ' ' : c;
            }
            ret += Environment.NewLine;
            ret += "        ";

            for (int i = 0; i < TAMANHO_BUFFER * 2; i++)
            {
                if (i == this._inicioLexema && i == this._ponteiro) ret += "%";
                else  if (i == this._inicioLexema)  ret += "^";
                else  if (i == this._ponteiro)  ret += "*";
                else ret += " ";
            }

            return ret;
        }
    }
}