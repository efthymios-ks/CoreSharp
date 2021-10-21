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
        [Display(
            Name = "Option 1 Name",
            ShortName = "Option 1 ShortName",
            Description = "Option 1 Description")]
        Option1 = 1,

        /// <summary>
        /// Option 2.
        /// </summary>
        [Display(
            Name = "Option 2 Name",
            ShortName = "Option 2 ShortName",
            Description = "Option 2 Description")]
        Option2 = 2,

        /// <summary>
        /// Option 3.
        /// </summary>
        [Display(
            Name = "Option 3 Name",
            ShortName = "Option 3 ShortName",
            Description = "Option 3 Description")]
        Option3 = 3
    }
}
