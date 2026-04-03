using System.Collections.Generic;

public interface MyNavigableMap<K, V> : MySortedMap<K, V>
{
    KeyValuePair<K, V>? lowerEntry(K key);
    KeyValuePair<K, V>? floorEntry(K key);
    KeyValuePair<K, V>? higherEntry(K key);
    KeyValuePair<K, V>? ceilingEntry(K key);

    K lowerKey(K key);
    K floorKey(K key);
    K higherKey(K key);
    K ceilingKey(K key);

    KeyValuePair<K, V>? pollFirstEntry();
    KeyValuePair<K, V>? pollLastEntry();
    KeyValuePair<K, V>? firstEntry();
    KeyValuePair<K, V>? lastEntry();
}
