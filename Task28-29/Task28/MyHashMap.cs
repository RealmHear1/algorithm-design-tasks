using System;
using System.Collections.Generic;

public class MyHashMap<K, V> : MyMap<K, V>
{
    private class Entry
    {
        public K Key;
        public V Value;
        public Entry Next;

        public Entry(K key, V value, Entry next)
        {
            Key = key;
            Value = value;
            Next = next;
        }
    }

    private const int DefaultCapacity = 16;
    private const float DefaultLoadFactor = 0.75f;

    private Entry[] table;
    private int count;
    private readonly float loadFactor;
    private int threshold;

    public MyHashMap()
        : this(DefaultCapacity, DefaultLoadFactor)
    {
    }

    public MyHashMap(int initialCapacity)
        : this(initialCapacity, DefaultLoadFactor)
    {
    }

    public MyHashMap(int initialCapacity, float loadFactor)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity must be positive");
        if (loadFactor <= 0)
            throw new ArgumentException("Load factor must be positive");

        int capacity = 1;
        while (capacity < initialCapacity)
            capacity <<= 1;

        table = new Entry[capacity];
        this.loadFactor = loadFactor;
        threshold = Math.Max(1, (int)(capacity * loadFactor));
    }

    private int GetIndex(object key, int length)
    {
        if (key == null)
            return 0;

        return (key.GetHashCode() & 0x7fffffff) % length;
    }

    private bool KeysEqual(object a, object b)
    {
        return Equals(a, b);
    }

    private Entry FindEntry(object key)
    {
        int index = GetIndex(key, table.Length);
        Entry current = table[index];

        while (current != null)
        {
            if (KeysEqual(current.Key, key))
                return current;

            current = current.Next;
        }

        return null;
    }

    private void ResizeIfNeeded()
    {
        if (count < threshold)
            return;

        Entry[] oldTable = table;
        table = new Entry[oldTable.Length * 2];
        threshold = Math.Max(1, (int)(table.Length * loadFactor));

        for (int i = 0; i < oldTable.Length; i++)
        {
            Entry current = oldTable[i];
            while (current != null)
            {
                Entry next = current.Next;
                int newIndex = GetIndex(current.Key, table.Length);
                current.Next = table[newIndex];
                table[newIndex] = current;
                current = next;
            }
        }
    }

    public void Clear()
    {
        clear();
    }

    public void clear()
    {
        table = new Entry[DefaultCapacity];
        count = 0;
        threshold = Math.Max(1, (int)(table.Length * loadFactor));
    }

    public bool ContainsKey(object key)
    {
        return containsKey(key);
    }

    public bool containsKey(object key)
    {
        return FindEntry(key) != null;
    }

    public bool ContainsValue(object value)
    {
        return containsValue(value);
    }

    public bool containsValue(object value)
    {
        for (int i = 0; i < table.Length; i++)
        {
            Entry current = table[i];
            while (current != null)
            {
                if (Equals(current.Value, value))
                    return true;
                current = current.Next;
            }
        }

        return false;
    }

    public V Get(object key)
    {
        return get(key);
    }

    public V get(object key)
    {
        Entry entry = FindEntry(key);
        return entry != null ? entry.Value : default(V);
    }

    public bool IsEmpty()
    {
        return isEmpty();
    }

    public bool isEmpty()
    {
        return count == 0;
    }

    public V Put(K key, V value)
    {
        return put(key, value);
    }

    public V put(K key, V value)
    {
        ResizeIfNeeded();

        int index = GetIndex(key, table.Length);
        Entry current = table[index];

        while (current != null)
        {
            if (KeysEqual(current.Key, key))
            {
                V oldValue = current.Value;
                current.Value = value;
                return oldValue;
            }

            current = current.Next;
        }

        table[index] = new Entry(key, value, table[index]);
        count++;
        return default(V);
    }

    public void PutAll(MyMap<K, V> m)
    {
        putAll(m);
    }

    public void putAll(MyMap<K, V> m)
    {
        if (m == null)
            throw new ArgumentNullException("m");

        object[] entries = m.entrySet().toArray();
        for (int i = 0; i < entries.Length; i++)
        {
            KeyValuePair<K, V> pair = (KeyValuePair<K, V>)entries[i];
            put(pair.Key, pair.Value);
        }
    }

    public V Remove(object key)
    {
        return remove(key);
    }

    public V remove(object key)
    {
        int index = GetIndex(key, table.Length);
        Entry current = table[index];
        Entry previous = null;

        while (current != null)
        {
            if (KeysEqual(current.Key, key))
            {
                if (previous == null)
                    table[index] = current.Next;
                else
                    previous.Next = current.Next;

                count--;
                return current.Value;
            }

            previous = current;
            current = current.Next;
        }

        return default(V);
    }

    public int Size()
    {
        return size();
    }

    public int size()
    {
        return count;
    }

    public List<K> KeySet()
    {
        List<K> keys = new List<K>();

        for (int i = 0; i < table.Length; i++)
        {
            Entry current = table[i];
            while (current != null)
            {
                keys.Add(current.Key);
                current = current.Next;
            }
        }

        return keys;
    }

    public MySet<K> keySet()
    {
        MyHashSet<K> set = new MyHashSet<K>();
        List<K> keys = KeySet();

        for (int i = 0; i < keys.Count; i++)
            set.add(keys[i]);

        return set;
    }

    public MySet<KeyValuePair<K, V>> entrySet()
    {
        MyHashSet<KeyValuePair<K, V>> set = new MyHashSet<KeyValuePair<K, V>>();

        for (int i = 0; i < table.Length; i++)
        {
            Entry current = table[i];
            while (current != null)
            {
                set.add(new KeyValuePair<K, V>(current.Key, current.Value));
                current = current.Next;
            }
        }

        return set;
    }

    public MyCollection<V> values()
    {
        MyArrayList<V> list = new MyArrayList<V>();

        for (int i = 0; i < table.Length; i++)
        {
            Entry current = table[i];
            while (current != null)
            {
                list.add(current.Value);
                current = current.Next;
            }
        }

        return list;
    }
}
