using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Option 1")]
        [Description("Description 1")]
        Option1 = 1,

        /// <summary>
        /// Option 2.
        /// </summary>
        [Display(Name = "Option 2")]
        [Description("Description 2")]
        Option2 = 2,

        /// <summary>
        /// Option 3.
        /// </summary>
        [Display(Name = "Option 3")]
        [Description("Description 3")]
        Option3 = 3
    }
}
