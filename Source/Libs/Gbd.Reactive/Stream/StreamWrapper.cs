using System;
using System.IO;
using System.Threading.Tasks;

//TODO Replace this with a couple of extension functions that generate an observer or observable from a stream
//without wrapping it
//https://github.com/dotnet/corefx/blob/master/src/System.IO/src/System/IO/Stream.cs
// is there a way of converting async class's such as stream direct to observables or observers instead?


//TODO it looks like BeginRead has been removed from the .NetCore version

//namespace Gbd.Reactive.Stream {

//    /// <summary>
//    /// A stream wrapper class, desgined to wrap read and writes to a underlying stream class.
//    /// </summary>
//    public class StreamWrapperBase : System.IO.Stream {

//        /// <summary> Delegate Function for reading / writing the Stream. </summary>
//        /// <value> Delegate Function for reading / writing the Stream. </value>
//        protected virtual Func<System.IO.Stream> StreamFunc { get; set; }

//        /// <summary> Gets the base stream that this stream references. </summary>
//        /// <value> Gets the base stream that this stream references. </value>
//        public System.IO.Stream BaseStream {
//            get {
//                if (StreamFunc != null) {
//                    return StreamFunc.Invoke();
//                }
//                return _BaseStream;
//            }
//        }

//        protected System.IO.Stream _BaseStream { get; set; }

//        /// <summary> Gets a value indicating whether to flush on write. </summary>
//        /// <value> true if automatic flush, false if not. </value>
//        public bool AutoFlush { get; set; }

//        /// <summary> Gets a value indicating whether the current stream supports reading. </summary>
//        /// <value> true if the stream supports reading; otherwise, false. </value>
//        public override bool CanRead {
//            get {
//                return BaseStream.CanRead;
//            }
//        }

//        /// <summary> Gets a value indicating whether the current stream supports seeking. </summary>
//        /// <value> true if the stream supports seeking; otherwise, false. </value>
//        public override bool CanSeek {
//            get {
//                return BaseStream.CanSeek;
//            }
//        }

//        /// <summary> Gets a value indicating whether the current stream supports writing. </summary>
//        /// <value> true if the stream supports writing; otherwise, false. </value>
//        public override bool CanWrite {
//            get {
//                return BaseStream.CanWrite;
//            }
//        }

//        /// <summary> Gets the length in bytes of the stream. </summary>
//        /// <value> A long value representing the length of the stream in bytes. </value>
//        public override long Length {
//            get {
//                return BaseStream.Length;
//            }
//        }

//        /// <summary> Gets or sets the position within the current stream. </summary>
//        /// <value> The current position within the stream. </value>
//        public override long Position {
//            get {
//                return BaseStream.Position;
//            }
//            set {
//                BaseStream.Position = value;
//            }
//        }

//        /// <summary> Default Constructor. </summary>
//        /// <param name="stream"> The stream to reference / intercept. </param>
//        public StreamWrapperBase(System.IO.Stream stream) {
//            _BaseStream = stream;
//        }

//        /// <summary> Default Constructor. </summary>
//        /// <param name="streamfunc"> Delegate Function for reading the Stream. </param>
//        public StreamWrapperBase(Func<System.IO.Stream> streamfunc) {
//            this.StreamFunc = streamfunc;
//        }

//        /// <summary>
//        /// Clears all buffers for this stream and causes any buffered data to be written to the
//        /// underlying device.
//        /// </summary>
//        public override void Flush() {
//            BaseStream.Flush();
//        }

//        /// <summary> Sets the position within the current stream. </summary>
//        /// <param name="offset">   A byte offset relative to the <paramref name="origin" /> parameter. </param>
//        /// <param name="origin">   A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the
//        ///                         reference point used to obtain the new position. </param>
//        /// <returns> The new position within the current stream. </returns>
//        public override long Seek(long offset, SeekOrigin origin) {
//            return BaseStream.Seek(offset, origin);
//        }

//        /// <summary> Sets the length of the current stream. </summary>
//        /// <param name="value"> The desired length of the current stream in bytes. </param>
//        public override void SetLength(long value) {
//            BaseStream.SetLength(value);
//        }

//        /// <summary>
//        /// Reads a sequence of bytes from the current stream and advances the position within the stream
//        /// by the number of bytes read.
//        /// </summary>
//        /// <param name="buffer">   An array of bytes. When this method returns, the buffer contains the
//        ///                         specified byte array with the values between
//        ///                         <paramref name="offset" /> and (<paramref name="offset" /> +
//        ///                         <paramref name="count" /> - 1) replaced by the bytes read from the
//        ///                         current source. </param>
//        /// <param name="offset">   The zero-based byte offset in <paramref name="buffer" /> at which to
//        ///                         begin storing the data read from the current stream. </param>
//        /// <param name="count">  The maximum number of bytes to be read from the current stream. </param>
//        /// <returns>
//        /// The total number of bytes read into the buffer. This can be less than the number of bytes
//        /// requested if that many bytes are not currently available, or zero (0) if the end of the
//        /// stream has been reached.
//        /// </returns>
//        public override int Read(byte[] buffer, int offset, int count) {
//            return BaseStream.Read(buffer, offset, count);
//        }

//        /// <summary> Begins a async read. </summary>
//        /// <param name="buffer">   The buffer to read the data into. </param>
//        /// <param name="offset">   The byte offset in <paramref name="buffer" /> at which to begin
//        ///                         writing data read from the stream. </param>
//        /// <param name="count">    The maximum number of bytes to read. </param>
//        /// <param name="callback"> An optional asynchronous callback, to be called when the read is
//        ///                         complete. </param>
//        /// <param name="state">    A user-provided object that distinguishes this particular
//        ///                         asynchronous read request from other requests. </param>
//        /// <returns> An IAsyncResult. </returns>
//        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
//            return BaseStream.BeginRead(buffer, offset, count, callback, state);
//        }

//        /// <summary>
//        /// Writes a sequence of bytes to the current stream and
//        /// advances the current position within this stream by the number of bytes written.
//        /// </summary>
//        /// <param name="buffer">   An array of bytes. This method copies <paramref name="count" /> bytes
//        ///                         from <paramref name="buffer" /> to the current stream. </param>
//        /// <param name="offset">   The zero-based byte offset in <paramref name="buffer" /> at which to
//        ///                         begin copying bytes to the current stream. </param>
//        /// <param name="count">  The number of bytes to be written to the current stream. </param>
//        public override void Write(byte[] buffer, int offset, int count) {
//            BaseStream.Write(buffer, offset, count);
//            if (AutoFlush) {
//                Flush();
//            }
//        }

//        /// <summary> Begins a async write. </summary>
//        /// <param name="buffer">   The buffer to write data from. </param>
//        /// <param name="offset">   The byte offset in <paramref name="buffer" /> from which to begin
//        ///                         writing. </param>
//        /// <param name="count">    The maximum number of bytes to write. </param>
//        /// <param name="callback"> An optional asynchronous callback, to be called when the write is
//        ///                         complete. </param>
//        /// <param name="state">    A user-provided object that distinguishes this particular
//        ///                         asynchronous write request from other requests. </param>
//        /// <returns> An IAsyncResult. </returns>
//        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
//            return BaseStream.BeginWrite(buffer, offset, count, callback, state);
//        }

//        /// <summary>
//        /// Asynchronously writes a sequence of bytes to the current stream, advances the current
//        /// position within this stream by the number of bytes written, and monitors cancellation
//        /// requests.
//        /// </summary>
//        /// <param name="buffer">            The buffer to write data from. </param>
//        /// <param name="offset">               The zero-based byte offset in <paramref name="buffer" />
//        ///                                     from which to begin copying bytes to the stream. </param>
//        /// <param name="count">             The maximum number of bytes to write. </param>
//        /// <param name="cancellationToken">    The token to monitor for cancellation requests. The
//        ///                                     default value is
//        ///                                     <see cref="P:System.Threading.CancellationToken.None" />. </param>
//        /// <returns> A task that represents the asynchronous write operation. </returns>
//        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken) {
//            return BaseStream.WriteAsync(buffer, offset, count, cancellationToken).ContinueWith(() => {
//                if (AutoFlush) Flush();
//            });
//            //INSTANT C# NOTE: Inserted the following 'return' since all code paths must return a value in C#:
//            return null;
//        }

//    }

//}
