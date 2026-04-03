using System.Collections.Generic;

public interface MyNavigableSet<T> : MySortedSet<T>
{
    KeyValuePair<T, object>? lowerEntry(T key);
    KeyValuePair<T, object>? floorEntry(T key);
    KeyValuePair<T, object>? higherEntry(T key);
    KeyValuePair<T, object>? ceilingEntry(T key);

    T lowerKey(T key);
    T floorKey(T key);
    T higherKey(T key);
    T ceilingKey(T key);

    KeyValuePair<T, object>? pollFirstEntry();
    KeyValuePair<T, object>? pollLastEntry();
    KeyValuePair<T, object>? firstEntry();
    KeyValuePair<T, object>? lastEntry();
}
