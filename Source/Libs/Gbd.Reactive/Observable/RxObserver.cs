using System;
using System.Reactive;
using System.Threading;

// TODO Common Logging / NLog not yet available for "dotnet5.4" target
#if NET451
using Common.Logging;
#endif

namespace Gbd.Reactive.Observable
{
    /// <summary>
    /// Base Class for Observers. This is just a convenience class where inheritance is prefered.
    /// </summary>
    public class RxObserver<T> : ObserverBase<T> {

        /// <summary> Current State of the Observer. </summary>
        /// <value> Current State of the Observer. </value>
        public RunningState State => _State;

        protected RunningState _State;

#if NET451
        /// <summary> Log Output. </summary>
        /// <value> NLog Instance. </value>
        private ILog Logger { get; } = LogManager.GetLogger(typeof(RxObserver<T>));
#endif

        /// <summary> State of the Observer. </summary>
        public enum RunningState {
            Active = 1,
            Stopped = 2,
            Errored = 3
        }

        /// <summary> Default Constructor. </summary>
        public RxObserver() {
            object tmpobj = _State;
            Interlocked.Exchange(ref tmpobj, RunningState.Active);
        }

        /// <summary> Default Constructor. </summary>
        /// <param name="observ"> The Observable to subscribe to. </param>
        public RxObserver(IObservable<T> observ) {
            object tmpobj = _State;
            Interlocked.Exchange(ref tmpobj, RunningState.Active);
            observ.Subscribe(this);
        }

        /// <summary> Sequence Next Value. </summary>
        /// <param name="value"> Next element in the sequence. </param>
        protected override void OnNextCore(T value) {
#if NET451
            Logger.Debug("OnNext: " + value);
#endif
        }

        /// <summary> Sequence Errored. </summary>
        /// <param name="error"> The error that has occurred. </param>
        protected override void OnErrorCore(Exception error) {
#if NET451
            Logger.Debug("OnError: " + error);
#endif
            object tmpobj = _State;
            Interlocked.Exchange(ref tmpobj, RunningState.Errored);
        }

        /// <summary> Sequence Complted / Closed. </summary>
        protected override void OnCompletedCore() {
#if NET451
            Logger.Debug("OnCompleted");
#endif
            object tmpobj = _State;
            Interlocked.Exchange(ref tmpobj, RunningState.Stopped);
        }

    }

}
