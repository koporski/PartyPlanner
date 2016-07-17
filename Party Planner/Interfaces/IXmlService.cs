namespace Party_Planner.Interfaces
{
    public interface IXmlService
    {
        void Serialize<T>(T obj, string path);
        T Deserialize<T>(string path);
    }
}
