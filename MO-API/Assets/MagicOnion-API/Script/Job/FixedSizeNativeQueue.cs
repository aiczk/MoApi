using Unity.Collections;
using UnityEngine;

namespace Script.Job
{
    public struct FixedSizeNativeQueue<T> where T : struct
    {
        private NativeQueue<T> queue;
        private int size;
        
        public int Count => queue.Count;
        public bool IsCreated => queue.IsCreated;

        public FixedSizeNativeQueue(Allocator allocator, int size)
        {
            queue = new NativeQueue<T>(allocator);
            this.size = size;
        }

        public void Clear() => queue.Clear();
        public T Dequeue() => queue.Dequeue();
        public void Dispose() => queue.Dispose();

        public void Enqueue(T entry)
        {
            if (Count >= size) 
                Dequeue();
            
            queue.Enqueue(entry);
        }

        public T Peek() => queue.Peek();
        public bool TryDequeue(out T item) => queue.TryDequeue(out item);
    }
}
