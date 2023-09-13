using edilex.ConfigToken;
using edilex.Manipuladores;

namespace edilex
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            EdiLexico edi = new EdiLexico("/projetos/edilex/data/teste.txt");
            // EdiLexico edi = new EdiLexico(args[0]);
            Token t = null;
            Console.WriteLine("");
            while ((t = edi.ProximoToken()).Nome != TipoToken.Fim)
            {
                Console.Write(t.ToString());
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
// "/projetos/edilex/data/teste.txt"