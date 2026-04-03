public interface MyListIterator<T> : MyIterator<T>
{
    bool hasPrevious();

    T previous();

    int nextIndex();

    int previousIndex();

    void set(T element);

    void add(T element);
}