namespace TradingPlatform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса хэширования
    /// </summary>
    public interface IHashService
    {
        public string CreateHash(string value);
        public string CreateHash(params string[] value);
    }
}