using System;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace CoreSharp.Interfaces.CultureLocalizer
{
    public interface ICultureLocalizer : IStringLocalizer
    {
        //Properties
        Type ResourceType { get; }
        CultureInfo Culture { get; }
    }
}
