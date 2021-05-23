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
        private DateTime? createdDate;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        [NotMapped]
        object IEntity.Id
        {
            get { return Id; }
            set { Id = (TKey)value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime? DateModified { get; set; }

        //Methods
        public override string ToString()
        {
            return $"{Id}";
        }
    }
}
