namespace CoreSharp.Interfaces.EntityFramework
{
    public interface IEntity<TKey> : IEntity
    {
        //Properties 
        new TKey Id { get; set; }
    }
}
