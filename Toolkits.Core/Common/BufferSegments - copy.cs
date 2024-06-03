//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Toolkits.Core;

///// <summary>
///// a class of <see cref="BufferSegments{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//[DebuggerDisplay("{Count}")]
//public class BufferSegments<T> : IDisposable
//{
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private int segmentCapacity;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private Segment readSegment = default!;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private Segment writeSegment = default!;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private Awaiter awaiter = new Awaiter(0);

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private Queue<Segment> segments;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private Queue<Segment> recycleSegments;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private int count;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private bool isDisposed;

//    /// <summary>
//    /// buffer count.
//    /// </summary>
//    public int Count => count;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="BufferSegments{T}"/> class.
//    /// </summary>
//    /// <param name="bufferSize">Size of the buffer.</param>
//    /// <param name="segmentCapacity">The segment capacity.</param>
//    /// <exception cref="ArgumentOutOfRangeException">
//    /// bufferSize
//    /// or
//    /// segmentCapacity
//    /// </exception>
//    public BufferSegments(int bufferSize = 1024, int segmentCapacity = 1024)
//    {
//        _ = bufferSize <= 0 ? throw new ArgumentOutOfRangeException(nameof(bufferSize)) : 0;
//        _ = segmentCapacity <= 0 ? throw new ArgumentOutOfRangeException(nameof(segmentCapacity)) : 0;
//        segments = new Queue<Segment>(bufferSize);
//        recycleSegments = new Queue<Segment>();
//        this.segmentCapacity = segmentCapacity;
//    }

//    [EditorBrowsable(EditorBrowsableState.Never)]
//    void IDisposable.Dispose()
//    {
//        isDisposed = true;

//        awaiter?.Dispose();
//        awaiter = null!;

//        segments?.Clear();
//        segments = null!;

//        count = default!;

//        segmentCapacity = default!;

//        readSegment = default!;
//        writeSegment = default!;
//    }

//    /// <summary>
//    /// Reads the specified buffer.
//    /// </summary>
//    /// <param name="buffer">The buffer.</param>
//    /// <param name="offset">The offset.</param>
//    /// <param name="length">The length.</param>
//    /// <exception cref="ObjectDisposedException">T</exception>
//    public void Read(T[] buffer, int offset, int length)
//    {
//        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

//        GetReadSegment();

//        if (readSegment.ValidReadLength > length)
//        {
//            readSegment.Read(buffer, 0, length);
//        }
//        else
//        {
//            int readLengthTemp = length;
//            int readedLength = 0;
//            while (readSegment.ValidReadLength < readLengthTemp)
//            {
//                int readSegmentLength = readSegment.ValidReadLength;

//                readSegment.Read(buffer, readedLength, readSegmentLength);

//                readedLength += readSegmentLength;
//                readLengthTemp -= readSegmentLength;

//                GetReadSegment();
//            }

//            if (readLengthTemp > 0)
//            {
//                readSegment.Read(buffer, readedLength, readLengthTemp);
//            }
//        }

//        Interlocked.Add(ref count, -length);
//    }

//    /// <summary>
//    /// Reads the asynchronous.
//    /// </summary>
//    /// <param name="buffer">The buffer.</param>
//    /// <param name="offset">The offset.</param>
//    /// <param name="readLength">Length of the read.</param>
//    /// <exception cref="ObjectDisposedException">T</exception>
//    public async Task ReadAsync(T[] buffer, int offset, int readLength)
//    {
//        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

//        await GetReadSegmentAsync();

//        if (readSegment.ValidReadLength > readLength)
//        {
//            readSegment.Read(buffer, 0, readLength);
//        }
//        else
//        {
//            int readLengthTemp = readLength;
//            int readedLength = 0;
//            while (readSegment.ValidReadLength < readLengthTemp)
//            {
//                int readSegmentLength = readSegment.ValidReadLength;

//                readSegment.Read(buffer, readedLength, readSegmentLength);

//                readedLength += readSegmentLength;
//                readLengthTemp -= readSegmentLength;

//                await GetReadSegmentAsync();
//            }

//            if (readLengthTemp > 0)
//            {
//                readSegment.Read(buffer, readedLength, readLengthTemp);
//            }
//        }

//        Interlocked.Add(ref count, -readLength);
//    }

//    /// <summary>
//    /// Writes the specified buffer.
//    /// </summary>
//    /// <param name="buffer">The buffer.</param>
//    /// <param name="offset">The offset.</param>
//    /// <param name="length">The length.</param>
//    /// <exception cref="ArgumentNullException">buffer</exception>
//    public void Write(T[] buffer, int offset, int length)
//    {
//        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

//        _ = buffer ?? throw new ArgumentNullException(nameof(buffer));

//        GetWriteSegment();

//        if (writeSegment.ValidWriteLength > length)
//        {
//            writeSegment.Write(buffer, offset, length);
//        }
//        else
//        {
//            int writeLength = length;

//            while (writeSegment.ValidWriteLength < writeLength)
//            {
//                int writeSegmentLength = writeSegment.ValidWriteLength;
//                writeSegment.Write(buffer, offset, writeSegmentLength);
//                offset += writeSegmentLength;

//                writeLength -= writeSegmentLength;

//                GetWriteSegment();
//            }

//            if (writeLength > 0)
//            {
//                writeSegment.Write(buffer, offset, writeLength);
//            }
//        }

//        Interlocked.Add(ref count, length);

//        awaiter.Release();
//    }

//    private void GetWriteSegment()
//    {
//        if (writeSegment is not null && writeSegment.ValidWriteLength > 0)
//        {
//            return;
//        }

//        writeSegment = recycleSegments.Count > 0 ? recycleSegments.Dequeue() : new Segment(segmentCapacity);

//        segments.Enqueue(writeSegment);
//    }

//    private void GetReadSegment()
//    {
//        if (readSegment is not null && readSegment.ReadCompleted == false)
//        {
//            if (readSegment.ValidReadLength == 0)
//            {
//                awaiter.Wait();
//            }

//            return;
//        }

//        while (segments.Count == 0)
//        {
//            awaiter.Wait();
//        }

//        if (readSegment is not null)
//        {
//            readSegment.Clear();
//            recycleSegments.Enqueue(readSegment);
//        }

//        readSegment = segments.Dequeue();
//    }

//    private async Task GetReadSegmentAsync()
//    {
//        if (readSegment is not null && readSegment.ReadCompleted == false)
//        {
//            if (readSegment.ValidReadLength == 0)
//            {
//                await awaiter.WaitAsync();
//            }

//            return;
//        }

//        while (segments.Count == 0)
//        {
//            await awaiter.WaitAsync();
//        }

//        if (readSegment is not null)
//        {
//            readSegment.Clear();
//            recycleSegments.Enqueue(readSegment);
//        }

//        readSegment = segments.Dequeue();
//    }

//    private class Segment
//    {
//        T[] buffers;
//        int ReadOffset;
//        int WriteOffset;

//        public bool ReadCompleted;
//        public bool WriteCompleted;

//        public int ValidReadLength;
//        public int ValidWriteLength;

//        public Segment(int capacity)
//        {
//            ValidWriteLength = capacity;
//            buffers = new T[capacity];
//        }

//        public void Clear()
//        {
//            ValidReadLength = 0;
//            WriteOffset = 0;
//            ReadOffset = 0;
//            ValidWriteLength = buffers.Length;
//        }

//        public void Write(T[] buffer, int offset, int length)
//        {
//            Array.ConstrainedCopy(buffer, offset, buffers, WriteOffset, length);

//            WriteOffset += length;
//            ValidWriteLength -= length;
//            ValidReadLength += length;

//            WriteCompleted = buffers.Length == WriteOffset;
//        }

//        public void Read(T[] buffer, int offset, int length)
//        {
//            Array.ConstrainedCopy(buffers, ReadOffset, buffer, offset, length);

//            ReadOffset += length;
//            ValidReadLength -= length;

//            ReadCompleted = ReadOffset == buffers.Length;
//        }
//    }
//}
