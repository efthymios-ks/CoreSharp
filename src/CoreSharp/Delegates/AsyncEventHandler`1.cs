using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Delegates
{
    public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken = default);
}
