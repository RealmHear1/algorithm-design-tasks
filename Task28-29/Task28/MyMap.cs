using System.Collections.Generic;

public interface MyMap<K, V>
{
    void clear();
    bool containsKey(object key);
    V get(object key);
    bool containsValue(object value);
    bool isEmpty();
    MySet<K> keySet();
    MySet<KeyValuePair<K, V>> entrySet();
    V put(K key, V value);
    void putAll(MyMap<K, V> m);
    V remove(object key);
    int size();
    MyCollection<V> values();
}
