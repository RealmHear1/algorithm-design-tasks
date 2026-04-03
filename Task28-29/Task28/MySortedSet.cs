public interface MySortedSet<T> : MySet<T>
{
    new T first();
    new T last();
    new MySet<T> subSet(T fromElement, T toElement);
    new MySet<T> headSet(T toElement);
    new MySet<T> tailSet(T fromElement);
}
