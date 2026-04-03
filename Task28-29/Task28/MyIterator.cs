public interface MyIterator<T>
{
    bool hasNext();

    T next();

    void remove();
}