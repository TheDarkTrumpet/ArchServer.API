namespace libToggl.api
{
    public interface IBase
    {
        string ApiKey { get; set; }
        string BaseURL { get; set; }
        void GenerateClient();
    }
}