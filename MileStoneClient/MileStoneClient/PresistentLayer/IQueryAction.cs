namespace MileStoneClient.PresistentLayer
{
    public abstract class IQueryAction
    {
        public SQLCommunication Instance { get { return SQLCommunication.Instance; } }
        public abstract void ExecuteQuery(string query);
    }
}
