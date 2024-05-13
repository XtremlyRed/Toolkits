using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
/// a class of <see cref="BufferSegments{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class BufferSegments<T>
{
    private volatile int rwInterlockedIndex;
    private readonly int segmentCapacity;
    private Segment readSegment = default!;
    private Segment writeSegment = default!;
    private readonly Queue<Segment> segments = new Queue<Segment>();
    private static readonly Queue<Segment> recycleSegments = new Queue<Segment>();

    /// <summary>
    /// buffer count.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BufferSegments{T}"/> class.
    /// </summary>
    /// <param name="segmentCapacity">The segment capacity.</param>
    public BufferSegments(int segmentCapacity = 1024)
    {
        this.segmentCapacity = segmentCapacity;
    }

    /// <summary>
    /// Reads the specified read length.
    /// </summary>
    /// <param name="readLength">Length of the read.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">readLength</exception>
    public T[] Read(int readLength)
    {
        if (readLength > Count)
        {
            throw new ArgumentOutOfRangeException(nameof(readLength));
        }

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
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

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

        Count += length;
    }

    private void GetWriteSegment()
    {
        if (writeSegment is not null && writeSegment.WriteRemainLength > 0)
        {
            return;
        }

        try
        {
            SpinLock();

            Segment? segment = default!;

#if NET451 || NET48
            if (recycleSegments.Count > 0)
            {
                segment = recycleSegments.Dequeue();
            }
            else
            {
                segment = new Segment(segmentCapacity);
            }
#else
            if (recycleSegments.TryDequeue(out segment) == false)
            {
                segment = new Segment(segmentCapacity);
            }
#endif
            segments.Enqueue(writeSegment = segment);
        }
        finally
        {
            Interlocked.Exchange(ref rwInterlockedIndex, 0);
        }
    }

    private void GetReadSegment()
    {
        if (readSegment is not null && readSegment.ReadRemainLength > 0)
        {
            return;
        }

        try
        {
            SpinLock();

            Segment? innerSegment = default!;

#if NET451 || NET48
            if (segments.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            innerSegment = segments.Dequeue();
#else
            if (segments.TryDequeue(out innerSegment) == false)
            {
                throw new ArgumentOutOfRangeException();
            }
#endif

            if (readSegment is not null)
            {
                readSegment.Clear();
                recycleSegments.Enqueue(readSegment);
            }

            readSegment = innerSegment;
        }
        finally
        {
            Interlocked.Exchange(ref rwInterlockedIndex, 0);
        }
    }

    private void SpinLock()
    {
        int count = 0;
        while (Interlocked.CompareExchange(ref rwInterlockedIndex, 1, 0) == 1)
        {
            count++;
            if (count > 1000)
            {
                Thread.SpinWait(10);
                count = 0;
            }
        }
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
