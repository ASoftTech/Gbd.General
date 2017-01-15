using System;
using System.Reactive;
using System.Reactive.Disposables;

// TODO Common Logging / NLog not yet available for "dotnet5.4" target
#if NET451
using Common.Logging;
#endif

namespace Gbd.Reactive.Observable {

    /// <summary>
    ///     Base class for Observables uses Rx ObservableBase which is the same as the
    ///     Observable.Create method factory call inheriting from this class is just a convenience,
    ///     note we only support a single subscription at a time here although you can distribute to
    ///     many via .Publish / .Multicast.
    /// </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public class RxObservable<T> :
        ObservableBase<T>,
        IDisposable {

#if NET451
        /// <summary> Log Output. </summary>
        /// <value> NLog Instance. </value>
        private ILog Logger { get; } = LogManager.GetLogger(typeof (RxObservable<T>));
#endif

        /// <summary> Outbound Observer / Client we throw messages out to. </summary>
        /// <value> The observer client. </value>
        public IObserver<T> ObserverClient => _observerClient;

        protected IObserver<T> _observerClient { get; set; }

        /// <summary> If disposed. </summary>
        /// <value> true if disposed, false if not. </value>
        protected bool Disposed { get; set; }

        /// <summary> If this class has been disposed. </summary>
        /// <value> If this class has been disposed. </value>
        public object IsDisposed => Disposed;

        /// <summary> Default constructor. </summary>
        public RxObservable() {}

        /// <summary> Default Constructor with subscription. </summary>
        /// <param name="observ"> An observer to subscribe during creation. </param>
        public RxObservable(IObserver<T> observ) {
            Subscribe(observ);
        }

        /// <summary> Destructor. </summary>
        ~RxObservable() {
            Dispose(false);
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
                _observerClient?.OnCompleted();
                _observerClient = null;
            }
            // Free your own state (unmanaged objects), Set large fields to null.
            Disposed = true;
        }

        /// <summary> Subscribe. </summary>
        /// <param name="observ"> The observer to subscribe. </param>
        /// <returns> An IDisposable to unsubscribe. </returns>
        protected override IDisposable SubscribeCore(IObserver<T> observ) {
            _observerClient = observ;
            var disp = Disposable.Create(() => { _observerClient = null; });
#if NET451
            Logger.Debug("Observer Subscribed");
#endif
            return disp;
        }
    }
}