﻿using CoreSharp.DependencyInjection.Attributes;

namespace Tests.Dummies.Services.Unmarked
{
    [IgnoreService]
    public interface IServiceWithIgnoreServiceAttributeOnContract
    {
    }

    /// <summary>
    /// Matches, but has <see cref="IgnoreServiceAttribute"/>.
    /// Should ignore.
    /// </summary>
    public class ServiceWithIgnoreServiceAttributeOnContract : IServiceWithIgnoreServiceAttributeOnContract
    {
    }
}
