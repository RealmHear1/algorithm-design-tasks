public interface MySet<T> : MyCollection<T>
{
    T first();
    T last();
    MySet<T> subSet(T fromElement, T toElement);
    MySet<T> headSet(T toElement);
    MySet<T> tailSet(T fromElement);
}
