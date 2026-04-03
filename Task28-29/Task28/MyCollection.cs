public interface MyCollection<T>
{
    bool add(T e);
    bool addAll(MyCollection<T> c);
    void clear();
    bool contains(object o);
    bool containsAll(MyCollection<T> c);
    bool isEmpty();
    bool remove(object o);
    bool removeAll(MyCollection<T> c);
    bool retainAll(MyCollection<T> c);
    int size();
    object[] toArray();
    T[] toArray(T[] a);
}
