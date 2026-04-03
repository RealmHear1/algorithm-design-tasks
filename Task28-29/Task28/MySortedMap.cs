public interface MySortedMap<K, V> : MyMap<K, V>
{
    K firstKey();
    K lastKey();
    MySortedMap<K, V> headMap(K end);
    MySortedMap<K, V> subMap(K start, K end);
    MySortedMap<K, V> tailMap(K start);
}
