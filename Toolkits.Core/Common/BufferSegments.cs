//using System.Diagnostics;

//namespace Toolkits.Core;

///// <summary>
///// a class of <see cref="BufferSegments{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//[DebuggerDisplay("{Count}")]
//public class BufferSegments<T>
//{
//    private readonly InnerQueue segments = new InnerQueue();
//    int bufferCapacity = 1024; int segmentCapacity = 1024;
//    /// <summary>
//    /// Gets the count.
//    /// </summary>
//    public int Count => segments.Sum(i => i.Count);

//    /// <summary>
//    /// segments reader.
//    /// </summary>
//    private ISegmentReader<T> Reader { get; }

//    /// <summary>
//    /// segments writer.
//    /// </summary>
//    private ISegmentWriter<T> Writer { get; }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="BufferSegments{T}"/> class.
//    /// </summary>
//    public BufferSegments(int bufferCapacity=64,int segmentCapacity = 1024)
//    {
//        this.bufferCapacity = bufferCapacity;
//        this.segmentCapacity = segmentCapacity;
//        Reader = new SegmentReader(this);
//        Writer = new SegmentWriter(this);
//    }

//    private class SegmentReader : ISegmentReader<T>
//    {
//        private readonly BufferSegments<T> bufferSegments;

//        public SegmentReader(BufferSegments<T> bufferSegments)
//        {
//            this.bufferSegments = bufferSegments;
//        }

//        public int Count => bufferSegments.Count;

//        public Task ReadAsync(T[] buffer, int offset, int length)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task  WaitToReadAsync()
//        {
//            if (bufferSegments.segments.Count == 0)
//            {
//                await
//            }
//        }
//    }

//    private class SegmentWriter : ISegmentWriter<T>
//    {
//        private readonly BufferSegments<T> bufferSegments;

//        public SegmentWriter(BufferSegments<T> bufferSegments)
//        {
//            this.bufferSegments = bufferSegments;
//        }

//        public int Count => bufferSegments.Count;

//        public async Task  WaitToWriteAsync()
//        {
//            if(bufferSegments.segments.Count == bufferSegments.bufferCapacity)
//            {
//                await bufferSegments.segments.DequeueWaitAsync();
//            }

//        }

//        public Task WriteAsync(T[] buffer, int offset, int length)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    private class Segment
//    {
//        private readonly T[] buffers;
//        private int readOffset;
//        private int writeOffset;
//        private int readRemain;
//        private int writeRemain;

//        public int Count => writeOffset - readOffset;

//        public bool ReadCompleted => readOffset == buffers.Length;
//        public bool WriteCompleted => writeOffset == buffers.Length;

//        public Segment(int capacity)
//        {
//            buffers = new T[capacity];
//            writeOffset = 0;
//            readOffset = 0;
//            readRemain = 0;
//            writeRemain = buffers.Length;
//        }

//        public void Clear()
//        {
//            writeOffset = 0;
//            readOffset = 0;
//            readRemain = 0;
//            writeRemain = buffers.Length;
//        }

//        public int Write(T[] buffer, int offset, int length)
//        {
//            int canWrite = length - writeRemain;

//            if (canWrite <= 0)
//            {
//                Array.ConstrainedCopy(buffer, offset, buffers, writeOffset, length);

//                _ = Interlocked.Add(ref writeOffset, length);
//                _ = Interlocked.Add(ref readRemain, length);
//                _ = Interlocked.Add(ref writeRemain, -length);

//                return 0;
//            }

//            Array.ConstrainedCopy(buffer, offset, buffers, writeOffset, canWrite);

//            _ = Interlocked.Add(ref writeOffset, writeRemain);
//            _ = Interlocked.Add(ref readRemain, writeRemain);
//            _ = Interlocked.Add(ref writeRemain, -writeRemain);

//            return canWrite;
//        }

//        public int Read(T[] buffer, int offset, int length)
//        {
//            int canRead = length - readRemain;

//            if (canRead <= 0)
//            {
//                Array.ConstrainedCopy(buffers, readOffset, buffer, offset, length);
//                _ = Interlocked.Add(ref readOffset, length);
//                _ = Interlocked.Add(ref readRemain, -length);
//                return 0;
//            }

//            Array.ConstrainedCopy(buffers, readOffset, buffer, offset, readRemain);
//            _ = Interlocked.Add(ref readOffset, readRemain);
//            _ = Interlocked.Add(ref readRemain, -readRemain);

//            return canRead;
//        }
//    }


//    private class InnerQueue : Queue<Segment>
//    {
//        private readonly Awaiter enqueueWaiter = new Awaiter(0, 1);
//        private readonly Awaiter dnqueueWaiter = new Awaiter(0, 1);

//        public new Segment Dequeue()
//        {
//            Segment item = base.Dequeue();
//            dnqueueWaiter.Release();
//            return item;
//        }
//        public new void Enqueue(Segment item)
//        {
//            base.Enqueue(item);

//            enqueueWaiter.Release();
//        }

//        public async Task DequeueWaitAsync()
//        {
//            await dnqueueWaiter.WaitAsync();
//        }

//        public async Task EnqueueWaitAsync()
//        {
//            await enqueueWaiter.WaitAsync();
//        }

//    }

//}

///// <summary>
///// a <see langword="interface"/> of <see cref="ISegmentReader{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//public interface ISegmentReader<T>
//{
//    /// <summary>
//    /// buffer count.
//    /// </summary>
//    int Count { get; }

//    /// <summary>
//    /// Waits to read asynchronous.
//    /// </summary>
//    /// <returns></returns>
//    Task  WaitToReadAsync();

//    /// <summary>
//    /// Reads the asynchronous.
//    /// </summary>
//    /// <param name="buffer">The buffer.</param>
//    /// <param name="offset">The offset.</param>
//    /// <param name="length">The length.</param>
//    /// <returns></returns>
//    Task ReadAsync(T[] buffer, int offset, int length);
//}

///// <summary>
///// a <see langword="interface"/> of <see cref="ISegmentWriter{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//public interface ISegmentWriter<T>
//{
//    /// <summary>
//    /// buffer count.
//    /// </summary>
//    int Count { get; }

//    /// <summary>
//    /// Waits to write asynchronous.
//    /// </summary>
//    /// <returns></returns>
//    Task  WaitToWriteAsync();

//    /// <summary>
//    /// Writes the asynchronous.
//    /// </summary>
//    /// <param name="buffer">The buffer.</param>
//    /// <param name="offset">The offset.</param>
//    /// <param name="length">The length.</param>
//    /// <returns></returns>
//    Task WriteAsync(T[] buffer, int offset, int length);
//}
