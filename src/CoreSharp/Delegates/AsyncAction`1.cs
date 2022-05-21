﻿using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Delegates
{
    public delegate Task AsyncAction<TEventArgs>(TEventArgs args, CancellationToken cancellationToken = default);
}
