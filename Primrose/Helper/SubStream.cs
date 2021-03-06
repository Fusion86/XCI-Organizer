﻿// Taken from https://github.com/AArnott/IronPigeon/blob/920eeb5dc4d8c6b89bb08bf76fcbee616abca69b/src/IronPigeon/Substream.cs

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Primrose.Helper
{
    /// <summary>
    /// Wraps an underlying stream with another that has a shorter length so that folks reading cannot read to the end.
    /// </summary>
    /// <remarks>
    /// The parent stream should not be repositioned while the substream is in use.
    /// </remarks>
    class SubStream : Stream
    {
        /// <summary>
        /// The underlying (possibly longer) stream.
        /// </summary>
        private readonly Stream underlyingStream;

        /// <summary>
        /// The initial position of the underlying stream when this stream was constructed.
        /// </summary>
        private readonly long initialPosition;

        /// <summary>
        /// The length of this slice of the underlying stream.
        /// </summary>
        private readonly long length;

        /// <summary>
        /// The position this stream is currently in, assuming its starting position were 0.
        /// </summary>
        private long positionRelativeToStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="Substream"/> class.
        /// </summary>
        /// <param name="underlyingStream">The stream to wrap.</param>
        /// <param name="length">The length of the stream to expose in this wrapper.</param>
        public SubStream(Stream underlyingStream, long length)
        {
            if (underlyingStream == null) throw new Exception();
            if (length <= 0) throw new Exception();

            this.underlyingStream = underlyingStream;
            this.length = length;
            this.initialPosition = underlyingStream.CanSeek ? underlyingStream.Position : -1;
        }

        /// <inheritdoc/>
        public override long Position
        {
            get
            {
                return this.underlyingStream.Position - this.initialPosition;
            }

            set
            {
                if (value <= 0) throw new Exception();

                this.underlyingStream.Position = this.initialPosition + value;
                this.positionRelativeToStart = value;
            }
        }

        /// <inheritdoc/>
        public override long Length
        {
            get { return this.length; }
        }

        /// <inheritdoc/>
        public override bool CanRead
        {
            get { return this.underlyingStream.CanRead; }
        }

        /// <inheritdoc/>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public override bool CanSeek
        {
            get { return this.underlyingStream.CanSeek; }
        }

        /// <inheritdoc/>
        public override bool CanTimeout
        {
            get { return this.underlyingStream.CanTimeout; }
        }

        /// <inheritdoc/>
        public override int ReadTimeout
        {
            get { return this.underlyingStream.ReadTimeout; }
            set { this.underlyingStream.ReadTimeout = value; }
        }

        /// <inheritdoc/>
        public override int WriteTimeout
        {
            get { return this.underlyingStream.WriteTimeout; }
            set { this.underlyingStream.WriteTimeout = value; }
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            long bytesRemaining = this.length - this.positionRelativeToStart;
            int bytesRead = this.underlyingStream.Read(buffer, offset, Math.Min(count, (int)bytesRemaining));
            this.positionRelativeToStart += bytesRead;
            return bytesRead;
        }

        /// <inheritdoc/>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            long bytesRemaining = this.length - this.positionRelativeToStart;
            int bytesRead = await this.underlyingStream.ReadAsync(buffer, offset, Math.Min(count, (int)bytesRemaining), cancellationToken).ConfigureAwait(false);
            this.positionRelativeToStart += bytesRead;
            return bytesRead;
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            long bytesRemaining = this.length - this.positionRelativeToStart;
            if (bytesRemaining > 0)
            {
                int result = this.underlyingStream.ReadByte();
                this.positionRelativeToStart++;
                return result;
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void WriteByte(byte value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            // We could implement this if needed.
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            underlyingStream.Dispose();
            base.Dispose(disposing);
        }
    }
}
