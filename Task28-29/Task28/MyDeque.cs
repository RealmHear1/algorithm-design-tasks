public interface MyDeque<T> : MyCollection<T>
{
    void addFirst(T obj);
    void addLast(T obj);
    T getFirst();
    T getLast();
    bool offerFirst(T obj);
    bool offerLast(T obj);
    T pop();
    void push(T obj);
    T peekFirst();
    T peekLast();
    T pollFirst();
    T pollLast();
    T removeLast();
    T removeFirst();
    bool removeLastOccurrence(object obj);
    bool removeFirstOccurrence(object obj);
}
