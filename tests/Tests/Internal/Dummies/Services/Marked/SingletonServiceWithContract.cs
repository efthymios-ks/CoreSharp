﻿using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

internal interface ISingletonServiceWithContract
{
}

internal class SingletonServiceWithContract :
    ISingletonServiceWithContract,
    ISingleton<ISingletonServiceWithContract>
{
}