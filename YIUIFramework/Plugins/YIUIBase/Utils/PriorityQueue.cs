using System;
using System.Collections;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// The priority queue is a max-heap used to find the maximum value.
    /// </summary>
    public sealed class PriorityQueue<T> : IEnumerable<T>
    {
        private IComparer<T> comparer;
        private T[] heap;
        private HashSet<T> fastFinder = new HashSet<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> 
        /// class.
        /// </summary>
        public PriorityQueue()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> 
        /// class, with specify capacity.
        /// </summary>
        public PriorityQueue(int capacity)
            : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> 
        /// class, with specify comparer.
        /// </summary>
        public PriorityQueue(IComparer<T> comparer)
            : this(16, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{T}"/> 
        /// class, with specify capacity and comparer.
        /// </summary>
        public PriorityQueue(int capacity, IComparer<T> _comparer)
        {
            comparer = _comparer == null ? Comparer<T>.Default : _comparer;
            heap = new T[capacity];
        }

        /// <summary>
        /// Gets the count in this queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the object at specify index.
        /// </summary>
        public T this[int index]
        {
            get { return heap[index]; }
        }

        public bool Contains(T v)
        {
            return fastFinder.Contains(v);
        }

        /// <summary>
        /// Get the enumerator for this item.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(heap, Count);
        }

        /// <summary>
        /// Get the enumerator for this item.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Clear this container.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            fastFinder.Clear();
        }

        /// <summary>
        /// Push a new value into this queue.
        /// </summary>
        public void Push(T v)
        {
            if (Count >= heap.Length)
            {
                Array.Resize(ref heap, Count * 2);
            }

            heap[Count] = v;
            SiftUp(Count++);
            fastFinder.Add(v);
        }

        /// <summary>
        /// Pop the max value out of this queue.
        /// </summary>
        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0)
            {
                SiftDown(0);
            }

            fastFinder.Remove(v);
            return v;
        }

        /// <summary>
        /// Access the max value in this queue.
        /// </summary>
        public T Top()
        {
            if (Count > 0)
            {
                return heap[0];
            }

            throw new InvalidOperationException("The PriorityQueue is empty.");
        }

        private void SiftUp(int n)
        {
            var v = heap[n];
            for (var n2 = n / 2;
                 n > 0 && comparer.Compare(v, heap[n2]) > 0;
                 n = n2, n2 /= 2)
            {
                heap[n] = heap[n2];
            }

            heap[n] = v;
        }

        private void SiftDown(int n)
        {
            var v = heap[n];
            for (var n2 = n * 2; n2 < Count; n = n2, n2 *= 2)
            {
                if (n2 + 1 < Count &&
                    comparer.Compare(heap[n2 + 1], heap[n2]) > 0)
                {
                    ++n2;
                }

                if (comparer.Compare(v, heap[n2]) >= 0)
                {
                    break;
                }

                heap[n] = heap[n2];
            }

            heap[n] = v;
        }

        /// <summary>
        /// The enumerator for this <see cref="PriorityQueue{T}"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] heap;
            private readonly int count;
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> 
            /// struct.
            /// </summary>
            internal Enumerator(T[] _heap, int _count)
            {
                heap = _heap;
                count = _count;
                index = -1;
            }

            public T Current
            {
                get { return heap[index]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                index = -1;
            }

            public bool MoveNext()
            {
                return (index <= count) && (++index < count);
            }
        }
    }
}