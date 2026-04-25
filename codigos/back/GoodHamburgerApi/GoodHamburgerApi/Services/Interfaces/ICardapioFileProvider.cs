namespace GoodHamburgerApi.Services.Interfaces
{
    public interface ICardapioFileProvider
    {
        bool Exists();
        string ReadAllText();
    }
}
