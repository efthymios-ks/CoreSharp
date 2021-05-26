using System;

namespace CoreSharp.Interfaces.EntityFramework
{
    public interface IModifiableEntity
    {
        //Properties
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
