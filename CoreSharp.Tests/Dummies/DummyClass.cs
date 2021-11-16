using System.Diagnostics;

namespace CoreSharp.Tests.Dummies
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal class DummyClass
    {
        //Constructors 
        public DummyClass() : this(default)
        {
        }

        public DummyClass(int id, string name = default)
        {
            Id = id;
            Name = name;
        }

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => ToString();

        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
            => $"Id={Id}, Name={Name}";
    }
}
