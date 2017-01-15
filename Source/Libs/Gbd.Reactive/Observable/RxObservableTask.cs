using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Gbd.Reactive.Observable
{
    /// <summary>
    /// Base class for Observables that require a Task to be run during subscription to read data
    /// from a source like a stream.
    /// </summary>
    public class RxObservableTask<T> : RxObservable<T> {

        /// <summary> The Task Initiated as part of the read process. </summary>
        /// <value> The read task. </value>
        public Task ReadTask => _ReadTask;

        private Task _ReadTask { get; set; }

        /// <summary> Token Source for Cancelation of the read task. </summary>
        /// <value> Token Source for Cancelation of the read task. </value>
        public CancellationTokenSource TokenSource => _TokenSource;

        private CancellationTokenSource _TokenSource { get; set; }

        /// <summary> Delegate Function for writing the Stream. </summary>
        /// <value> Delegate Function for writing the Stream. </value>
        private Action<IObserver<T>> TaskReadDataFunc { get; set; }

        /// <summary> Default constructor. </summary>
        public RxObservableTask() {
        }

        /// <summary> Default Constructor. </summary>
        /// <param name="readDataAction"> The action to perform when reading data. </param>
        public RxObservableTask(Action<IObserver<T>> readDataAction) {
            TaskReadDataFunc = readDataAction;
        }

        /// <summary> Disposal. </summary>
        /// <param name="disposing">    true to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        protected override void Dispose(bool disposing) {
            _TokenSource?.Cancel();
            base.Dispose(disposing);
        }

        /// <summary> Subscribe. </summary>
        /// <param name="observ"> The observer to subscribe. </param>
        /// <returns> An IDisposable to unsubscribe. </returns>
        protected override IDisposable SubscribeCore(IObserver<T> observ) {

            // Setup the subscription
            _observerClient = observ;

            lock (_observerClient) {

                // Get rid of the old subscription
                _TokenSource?.Cancel();

                // Start up the read of data
                _TokenSource = new CancellationTokenSource();
                _ReadTask = Task.Factory.StartNew(() => TaskReadDataFunc.Invoke(observ));

                // Return a Disposable
                return Disposable.Create(() => {
                    lock (_observerClient) {
                        _TokenSource.Cancel();
                        _observerClient = null;
                    }
                });

            }

        }

    }

}
