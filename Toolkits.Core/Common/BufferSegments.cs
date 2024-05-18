using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
/// a class of <see cref="BufferSegments{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay("{Count}")]
public class BufferSegments<T> : IDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int segmentCapacity;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Segment readSegment = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Segment writeSegment = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Awaiter awaiter = new Awaiter(0);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Queue<Segment> segments;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Queue<Segment> recycleSegments;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int count;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isDisposed;

    /// <summary>
    /// buffer count.
    /// </summary>
    public int Count => count;

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSegments{T}"/> class.
    /// </summary>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <param name="segmentCapacity">The segment capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// bufferSize
    /// or
    /// segmentCapacity
    /// </exception>
    public BufferSegments(int bufferSize = 1024, int segmentCapacity = 1024)
    {
        _ = bufferSize <= 0 ? throw new ArgumentOutOfRangeException(nameof(bufferSize)) : 0;
        _ = segmentCapacity <= 0 ? throw new ArgumentOutOfRangeException(nameof(segmentCapacity)) : 0;
        segments = new Queue<Segment>(bufferSize);
        recycleSegments = new Queue<Segment>();
        this.segmentCapacity = segmentCapacity;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    void IDisposable.Dispose()
    {
        isDisposed = true;

        awaiter?.Dispose();
        awaiter = null!;

        segments?.Clear();
        segments = null!;

        count = default!;

        segmentCapacity = default!;

        readSegment = default!;
        writeSegment = default!;
    }

    /// <summary>
    /// Reads the specified read length.
    /// </summary>
    /// <param name="readLength">Length of the read.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">readLength</exception>
    public T[] Read(int readLength)
    {
        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

        T[] buffer = new T[readLength];

        GetReadSegment();

        if (readSegment.ReadRemainLength > readLength)
        {
            readSegment.Read(buffer, 0, readLength);
        }
        else
        {
            int readLengthTemp = readLength;
            int readedLength = 0;
            while (readSegment.ReadRemainLength < readLengthTemp)
            {
                int readSegmentLength = readSegment.ReadRemainLength;

                readSegment.Read(buffer, readedLength, readSegmentLength);

                readedLength += readSegmentLength;
                readLengthTemp -= readSegmentLength;

                GetReadSegment();
            }

            if (readLengthTemp > 0)
            {
                readSegment.Read(buffer, readedLength, readLengthTemp);
            }
        }

        Interlocked.Add(ref count, -readLength);

        return buffer;
    }

    /// <summary>
    /// Reads the specified read length.
    /// </summary>
    /// <param name="readLength">Length of the read.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">readLength</exception>
    public async Task<T[]> ReadAsync(int readLength)
    {
        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

        T[] buffer = new T[readLength];

        await GetReadSegmentAsync();

        if (readSegment.ReadRemainLength > readLength)
        {
            readSegment.Read(buffer, 0, readLength);
        }
        else
        {
            int readLengthTemp = readLength;
            int readedLength = 0;
            while (readSegment.ReadRemainLength < readLengthTemp)
            {
                int readSegmentLength = readSegment.ReadRemainLength;

                readSegment.Read(buffer, readedLength, readSegmentLength);

                readedLength += readSegmentLength;
                readLengthTemp -= readSegmentLength;

                await GetReadSegmentAsync();
            }

            if (readLengthTemp > 0)
            {
                readSegment.Read(buffer, readedLength, readLengthTemp);
            }
        }

        Interlocked.Add(ref count, -readLength);

        return buffer;
    }

    /// <summary>
    /// Writes the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="length">The length.</param>
    /// <exception cref="ArgumentNullException">buffer</exception>
    public void Write(T[] buffer, int offset, int length)
    {
        _ = isDisposed ? throw new ObjectDisposedException(nameof(BufferSegments<T>)) : 0;

        _ = buffer ?? throw new ArgumentNullException(nameof(buffer));

        GetWriteSegment();

        if (writeSegment.WriteRemainLength > length)
        {
            writeSegment.Write(buffer, offset, length);
        }
        else
        {
            int writeLength = length;

            while (writeSegment.WriteRemainLength < writeLength)
            {
                int writeSegmentLength = writeSegment.WriteRemainLength;
                writeSegment.Write(buffer, offset, writeSegmentLength);
                offset += writeSegmentLength;

                writeLength -= writeSegmentLength;

                GetWriteSegment();
            }

            if (writeLength > 0)
            {
                writeSegment.Write(buffer, offset, writeLength);
            }
        }

        Interlocked.Add(ref count, length);

        awaiter.Release();
    }

    private void GetWriteSegment()
    {
        if (writeSegment is not null && writeSegment.WriteRemainLength > 0)
        {
            return;
        }

        Segment segment = recycleSegments.Count > 0 ? recycleSegments.Dequeue() : new Segment(segmentCapacity);

        segments.Enqueue(writeSegment = segment);
    }

    private void GetReadSegment()
    {
        if (readSegment is not null && readSegment.ReadRemainLength > 0)
        {
            return;
        }

        Segment? innerSegment = default!;

        if (segments.Count == 0)
        {
            awaiter.Wait();
        }

        if (readSegment is not null)
        {
            readSegment.Clear();
            recycleSegments.Enqueue(readSegment);
        }

        readSegment = innerSegment;
    }

    private async Task GetReadSegmentAsync()
    {
        if (readSegment is not null && readSegment.ReadRemainLength > 0)
        {
            return;
        }

        if (segments.Count == 0)
        {
            await awaiter.WaitAsync();
        }

        if (readSegment is not null)
        {
            readSegment.Clear();
            recycleSegments.Enqueue(readSegment);
        }

        readSegment = segments.Dequeue();
    }

    private class Segment
    {
        public readonly int Capacity;
        public T[] buffers;
        public int ReadOffset;
        public int WriteOffset;
        public int ReadRemainLength;
        public int WriteRemainLength;
        public int Count;

        public Segment(int capacity)
        {
            Capacity = WriteRemainLength = capacity;
            buffers = new T[capacity];
        }

        public void Clear()
        {
            Count = 0;
            WriteOffset = ReadOffset = 0;
            WriteRemainLength = ReadRemainLength = Capacity;
        }

        public void Write(T[] buffer, int offset, int length)
        {
            Array.ConstrainedCopy(buffer, offset, buffers, WriteOffset, length);
            WriteOffset += length;
            Count += length;
            WriteRemainLength = Capacity - Count;
            ReadRemainLength += length;
        }

        public void Read(T[] buffer, int offset, int length)
        {
            Array.ConstrainedCopy(buffers, ReadOffset, buffer, offset, length);
            ReadOffset += length;
            ReadRemainLength -= length;
        }
    }
}
