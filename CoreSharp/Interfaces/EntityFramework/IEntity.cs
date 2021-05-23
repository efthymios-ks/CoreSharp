using System;

namespace CoreSharp.Interfaces.EntityFramework
{
    public interface IEntity
    {
        //Properties 
        object Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
