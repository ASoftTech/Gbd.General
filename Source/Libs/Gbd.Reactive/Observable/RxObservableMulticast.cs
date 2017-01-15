using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;

// TODO Common Logging / NLog not yet available for "dotnet5.4" target
#if NET451
using Common.Logging;
#endif

namespace Gbd.Reactive.Observable
{
    /// <summary>
    /// Base class for Observables uses Rx ObservableBase which is the same as the Observable.Create
    /// method factory call, inheriting from this class is just a convenience, this is a variant that
    /// supports multiple subscribers.
    /// </summary>
    public class RxObservableMulticast<T> : ObservableBase<T>, IDisposable {

#if NET451
        /// <summary> Log Output. </summary>
        /// <value> NLog Instance. </value>
        private ILog Logger { get; } = LogManager.GetLogger(typeof(RxObservableMulticast<T>));
#endif

        /// <summary> List of Outbound Observers / Clients we throw messages out to. </summary>
        /// <value> The list of observers. </value>
        public IReadOnlyList<IObserver<T>> ObserverClients => _observerClients.AsReadOnly();

        protected List<IObserver<T>> _observerClients { get; set; }

        /// <summary> If disposed. </summary>
        /// <value> true if disposed, false if not. </value>
        protected bool Disposed { get; set; }

        /// <summary> If this class has been disposed. </summary>
        /// <value> If this class has been disposed. </value>
        public object IsDisposed => Disposed;

        /// <summary> Default Constructor. </summary>
        public RxObservableMulticast() {
            _observerClients = new List<IObserver<T>>();
        }

        /// <summary> Default Constructor with subscription. </summary>
        /// <param name="observ"> An observer to subscribe during creation. </param>
        public RxObservableMulticast(IObserver<T> observ) {
            _observerClients = new List<IObserver<T>>();
            Subscribe(observ);
        }

        /// <summary> Disposal. </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary> Disposal. </summary>
        /// <param name="disposing">    true to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing) {
            if (Disposed) return;
            if (disposing) {
                // Free other state (managed objects).
                if (_observerClients != null) {
                    OnCompletedCore();
                }
                _observerClients = null;
            }
            // Free your own state (unmanaged objects), Set large fields to null.
            Disposed = true;
        }

        /// <summary> Destructor. </summary>
        ~RxObservableMulticast() {
            Dispose(false);
        }

        /// <summary> Subscribe. </summary>
        /// <param name="observ"> The observer to subscribe. </param>
        /// <returns> An IDisposable to unsubscribe. </returns>
        protected override IDisposable SubscribeCore(IObserver<T> observ) {
            lock (_observerClients) {
                _observerClients.Add(observ);
            }
            var disp = Disposable.Create(() => {
                lock (_observerClients) {
                    _observerClients.Remove(observ);
                }
            });
#if NET451
            Logger.Debug("Observer Subscribed");
#endif
            return disp;
        }

        /// <summary> On Next Callback. </summary>
        /// <param name="value"> The value to pass on. </param>
        protected virtual void OnNextCore(T value) {
            lock (_observerClients) {
                foreach (var obitem in _observerClients) {
                    obitem.OnNext(value);
                }
            }
        }

        /// <summary> On Error Callback. </summary>
        /// <param name="error"> an error / exception to pass on. </param>
        protected virtual void OnErrorCore(Exception error) {
            lock (_observerClients) {
                foreach (var obitem in _observerClients) {
                    obitem.OnError(error);
                }
                _observerClients.Clear();
            }
        }

        /// <summary> On Completed Callback. </summary>
        protected virtual void OnCompletedCore() {
            lock (_observerClients) {
                foreach (var obitem in _observerClients) {
                    obitem.OnCompleted();
                }
                _observerClients.Clear();
            }
        }

    }

}
