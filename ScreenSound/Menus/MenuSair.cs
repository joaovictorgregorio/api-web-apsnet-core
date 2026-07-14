using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

public class MenuSair : Menu
{
    public override void Executar(DAL<Artista> artistaDAL)
    {
        Console.WriteLine("Tchau tchau :)");
    }
}
