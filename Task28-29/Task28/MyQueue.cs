public interface MyQueue<T> : MyCollection<T>
{
    T element();
    bool offer(T obj);
    T peek();
    T poll();
}
