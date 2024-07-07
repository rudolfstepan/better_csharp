using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FastHashtableDemo
{
    public class FastHashtable<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private unsafe struct HashNode
        {
            public TKey Key;
            public TValue Value;
            public HashNode* Next;
        }

        private unsafe HashNode** buckets;
        private int size;
        private int count;
        private const double LoadFactor = 0.75;
        private IntPtr nodePool;
        private int nodePoolSize;
        private int nodePoolIndex;

        public unsafe FastHashtable(int initialSize)
        {
            size = initialSize;
            buckets = (HashNode**)System.Runtime.InteropServices.Marshal.AllocHGlobal(size * sizeof(HashNode*));
            for (int i = 0; i < size; i++)
            {
                buckets[i] = null;
            }
            nodePoolSize = initialSize * 2;
            nodePool = System.Runtime.InteropServices.Marshal.AllocHGlobal(nodePoolSize * sizeof(HashNode));
            nodePoolIndex = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int GetBucketIndex(TKey key)
        {
            return (key.GetHashCode() & 0x7FFFFFFF) % size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe HashNode* AllocateNode()
        {
            if (nodePoolIndex >= nodePoolSize)
            {
                nodePoolSize *= 2;
                nodePool = System.Runtime.InteropServices.Marshal.ReAllocHGlobal(nodePool, (IntPtr)(nodePoolSize * sizeof(HashNode)));
            }
            return (HashNode*)((byte*)nodePool + (nodePoolIndex++) * sizeof(HashNode));
        }

        public unsafe void Add(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);
            HashNode* newNode = AllocateNode();
            newNode->Key = key;
            newNode->Value = value;
            newNode->Next = null;

            HashNode* current = buckets[index];
            if (current == null)
            {
                buckets[index] = newNode;
            }
            else
            {
                while (true)
                {
                    if (current->Key.Equals(key))
                    {
                        current->Value = value;
                        nodePoolIndex--;
                        return;
                    }
                    if (current->Next == null)
                    {
                        break;
                    }
                    current = current->Next;
                }
                current->Next = newNode;
            }
            count++;
            if ((double)count / size > LoadFactor)
            {
                Resize();
            }
        }

        public unsafe bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetBucketIndex(key);
            HashNode* current = buckets[index];

            while (current != null)
            {
                if (current->Key.Equals(key))
                {
                    value = current->Value;
                    return true;
                }
                current = current->Next;
            }

            value = default;
            return false;
        }

        public unsafe void Remove(TKey key)
        {
            int index = GetBucketIndex(key);
            HashNode* current = buckets[index];
            HashNode* previous = null;

            while (current != null)
            {
                if (current->Key.Equals(key))
                {
                    if (previous == null)
                    {
                        buckets[index] = current->Next;
                    }
                    else
                    {
                        previous->Next = current->Next;
                    }
                    nodePoolIndex--;
                    return;
                }
                previous = current;
                current = current->Next;
            }
        }

        private unsafe void Resize()
        {
            int newSize = size * 2;
            HashNode** newBuckets = (HashNode**)System.Runtime.InteropServices.Marshal.AllocHGlobal(newSize * sizeof(HashNode*));
            for (int i = 0; i < newSize; i++)
            {
                newBuckets[i] = null;
            }

            for (int i = 0; i < size; i++)
            {
                HashNode* current = buckets[i];
                while (current != null)
                {
                    HashNode* next = current->Next;
                    int newIndex = (current->Key.GetHashCode() & 0x7FFFFFFF) % newSize;

                    current->Next = newBuckets[newIndex];
                    newBuckets[newIndex] = current;

                    current = next;
                }
            }

            System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)buckets);
            buckets = newBuckets;
            size = newSize;
        }

        public unsafe void Dispose()
        {
            System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)buckets);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(nodePool);
        }
    }
}
