using System;

namespace Zeitgeist.Core.Patterns.Observer
{
    public interface ISubject<T>
    {
        void Subcribe(IObserver<T> observer);
        void Unsubscribe(IObserver<T> observer);
        void Unsubscribe(string Id);
        void NotifyAll(Func<T> func);
        void NotifyObserverId(string Id, Func<T> func);
    }
}