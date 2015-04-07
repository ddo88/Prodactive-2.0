using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeitgeist.Core.Patterns.Observer
{
    public class Subject<T> : ISubject<T>
    {

        public Subject()
        {
            Observers= new List<IObserver<T>>();
        }

        private List<IObserver<T>> Observers { get; set; }

        public void Subcribe(IObserver<T> observer)
        {
            Observers.Add(observer);
        }

        public void Unsubscribe(IObserver<T> observer)
        {
            Observers.Remove(observer);
        }

        public void Unsubscribe(string Id)
        {
            var result = Observers.Where(x => x.Id.Equals(Id)).ToList();
            if (result.Count > 0)
            {
                Observers.Remove(result.First());
            }
            
        }

        public void NotifyAll(Func<T> func)
        {
            foreach (var observer in Observers)
            {
                observer.Notify(func());
            }
        }

        public void NotifyObserverId(string Id,Func<T> func)
        {
            foreach (var observer in Observers)
            {
                if (observer.Id == Id)
                {
                    observer.Notify(func());
                    break;
                }
                
            }
        }
    }
}
