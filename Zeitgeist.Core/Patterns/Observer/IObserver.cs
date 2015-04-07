using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeitgeist.Core.Patterns.Observer
{
    public interface IObserver<T>
    {
        string Id { get; set; }
        void Notify(T message);
    }
}
