using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Delegates;

public delegate Task AsyncDelegate(CancellationToken cancellationToken = default);
