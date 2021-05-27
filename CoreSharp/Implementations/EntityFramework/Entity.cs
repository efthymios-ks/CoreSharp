using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using CoreSharp.Interfaces.EntityFramework;

namespace CoreSharp.Implementations.EntityFramework
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        //Fields 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        private DateTime? dateCreated;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        object IKeyedEntity.Id
        {
            get { return Id; }
            set { Id = (TKey)value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated
        {
            get { return dateCreated ?? DateTime.UtcNow; }
            set { dateCreated = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? DateModified { get; set; }

        //Methods
        public override string ToString() => $"{Id}";
    }
}
