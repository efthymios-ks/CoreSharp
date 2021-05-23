using System;

namespace CoreSharp.Interfaces.EntityFramework
{
    public interface IEntity
    {
        //Properties 
        object Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
