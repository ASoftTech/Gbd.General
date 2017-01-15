using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gbd.Reactive.Stream {
    /// <summary> Acts as a observable memory stream for buffering and storing data. </summary>
    public class RxMemoryStream :
        System.IO.MemoryStream,
        IObservable<ArraySegment<byte>>,
        IObserver<byte[]> {

        public string _ObWrite { get; set; }

        /// <summary> Gets the byte buffer. </summary>
        /// <value> A Buffer for byte data. </value>
        public byte[] ByteBuffer {
            get { throw new NotImplementedException(); }
        }

        public bool Autoflush { get; set; }

        /// <summary> Default constructor. </summary>
        private RxMemoryStream() : base() {}

        /// <summary> Notifies the provider that an observer is to receive notifications. </summary>
        /// <param name="observer"> The object that is to receive notifications. </param>
        /// <returns>
        ///     A reference to an interface that allows observers to stop receiving notifications before
        ///     the provider has finished sending them.
        /// </returns>
        public IDisposable Subscribe(IObserver<ArraySegment<byte>> observer) {
            throw new NotImplementedException();
        }

        public void OnNext(byte[] value) {
            throw new NotImplementedException();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }
    }
}