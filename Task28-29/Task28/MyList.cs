public interface MyList<T> : MyCollection<T>
{
    void add(int index, T e);
    bool addAll(int index, MyCollection<T> c);
    T get(int index);
    int indexOf(object o);
    int lastIndexOf(object o);
    MyListIterator<T> listIterator();
    MyListIterator<T> listIterator(int index);
    T remove(int index);
    T set(int index, T e);
    MyList<T> subList(int fromIndex, int toIndex);
}
