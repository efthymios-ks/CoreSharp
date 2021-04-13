using System.ComponentModel;

namespace CoreSharp.Tests.Dummies
{
    /// <summary>
    /// Dummy enum. 
    /// </summary>
    internal enum DummyEnum
    {
        /// <summary>
        /// Option 1. 
        /// </summary>
        [Description("Description 1")]
        Option1 = 1,

        /// <summary>
        /// Option 2. 
        /// </summary>
        [Description("Description 2")]
        Option2 = 2,

        /// <summary>
        /// Option 3. 
        /// </summary>
        [Description("Description 3")]
        Option3 = 3
    }
}
