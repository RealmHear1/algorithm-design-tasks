using System;
using Task6;

namespace Task28
{
    internal class Program
    {
        static void Main()
        {
            TestHashSetIterator();
            TestTreeSetIterator();
            TestArrayDequeIterator();
            TestPriorityQueueIterator();

            TestArrayListListIterator();
            TestVectorListIterator();
            TestLinkedListListIterator();

            TestIteratorExceptions();
            TestListIteratorExceptions();

            Console.WriteLine();
            Console.WriteLine("ВСЕ ТЕСТЫ ЗАВЕРШЕНЫ");
        }

        static void PrintLine()
        {
            Console.WriteLine("-----------------------------------");
        }

        static void PrintObjectArray(object[] arr)
        {
            int i;
            for (i = 0; i < arr.Length; i++)
                Console.Write(arr[i] + " ");
            Console.WriteLine();
        }

        static void PrintIntArray(int[] arr)
        {
            int i;
            for (i = 0; i < arr.Length; i++)
                Console.Write(arr[i] + " ");
            Console.WriteLine();
        }

        static void TestHashSetIterator()
        {
            PrintLine();
            Console.WriteLine("MyHashSet iterator()");

            MyHashSet<int> set = new MyHashSet<int>();
            set.add(10);
            set.add(20);
            set.add(30);

            MyIterator<int> it = set.iterator();

            Console.WriteLine("Проход и удаление 20:");
            while (it.hasNext())
            {
                int v = it.next();
                Console.WriteLine(v);

                if (v == 20)
                    it.remove();
            }

            Console.WriteLine("После удаления:");
            PrintObjectArray(set.toArray());
        }

        static void TestTreeSetIterator()
        {
            PrintLine();
            Console.WriteLine("MyTreeSet iterator()");

            MyTreeSet<int> set = new MyTreeSet<int>();
            set.Add(30);
            set.Add(10);
            set.Add(20);
            set.Add(40);

            MyIterator<int> it = set.iterator();

            Console.WriteLine("Проход и удаление 20:");
            while (it.hasNext())
            {
                int v = it.next();
                Console.WriteLine(v);

                if (v == 20)
                    it.remove();
            }

            Console.WriteLine("После удаления:");
            MyIterator<int> it2 = set.iterator();
            while (it2.hasNext())
                Console.WriteLine(it2.next());
        }

        static void TestArrayDequeIterator()
        {
            PrintLine();
            Console.WriteLine("MyArrayDeque iterator()");

            MyArrayDeque<int> deque = new MyArrayDeque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            deque.Add(4);

            MyIterator<int> it = deque.iterator();

            Console.WriteLine("Проход и удаление 3:");
            while (it.hasNext())
            {
                int v = it.next();
                Console.WriteLine(v);

                if (v == 3)
                    it.remove();
            }

            Console.WriteLine("После удаления:");
            PrintObjectArray(deque.ToArray());
        }

        static void TestPriorityQueueIterator()
        {
            PrintLine();
            Console.WriteLine("MyPriorityQueue iterator()");

            MyPriorityQueue<int> queue = new MyPriorityQueue<int>();
            queue.Add(30);
            queue.Add(10);
            queue.Add(20);
            queue.Add(5);

            MyIterator<int> it = queue.iterator();

            Console.WriteLine("Проход и удаление 20:");
            while (it.hasNext())
            {
                int v = it.next();
                Console.WriteLine(v);

                if (v == 20)
                    it.remove();
            }

            Console.WriteLine("После удаления:");
            int[] arr = queue.ToArray();
            PrintIntArray(arr);
        }

        static void TestArrayListListIterator()
        {
            PrintLine();
            Console.WriteLine("MyArrayList listIterator()");

            MyArrayList<int> list = new MyArrayList<int>();
            list.Add(10);
            list.Add(20);
            list.Add(30);

            MyListIterator<int> it = list.listIterator();

            Console.WriteLine("Вперед:");
            while (it.hasNext())
            {
                int x = it.next();
                Console.WriteLine(x);

                if (x == 20)
                    it.set(200);

                if (x == 30)
                    it.remove();
            }

            Console.WriteLine("После set/remove:");
            PrintObjectArray(list.ToArray());

            it = list.listIterator(1);
            it.add(500);

            Console.WriteLine("После add через iterator:");
            PrintObjectArray(list.ToArray());

            Console.WriteLine("Назад:");
            while (it.hasPrevious())
                Console.WriteLine(it.previous());
        }

        static void TestVectorListIterator()
        {
            PrintLine();
            Console.WriteLine("MyVector listIterator()");

            MyVector<int> list = new MyVector<int>();
            list.Add(10);
            list.Add(20);
            list.Add(30);

            MyListIterator<int> it = list.listIterator();

            Console.WriteLine("Вперед:");
            while (it.hasNext())
            {
                int x = it.next();
                Console.WriteLine(x);

                if (x == 20)
                    it.set(200);

                if (x == 30)
                    it.remove();
            }

            Console.WriteLine("После set/remove:");
            PrintObjectArray(list.ToArray());

            it = list.listIterator(1);
            it.add(500);

            Console.WriteLine("После add через iterator:");
            PrintObjectArray(list.ToArray());

            Console.WriteLine("Назад:");
            while (it.hasPrevious())
                Console.WriteLine(it.previous());
        }

        static void TestLinkedListListIterator()
        {
            PrintLine();
            Console.WriteLine("MyLinkedList listIterator()");

            MyLinkedList<int> list = new MyLinkedList<int>();
            list.Add(10);
            list.Add(20);
            list.Add(30);

            MyListIterator<int> it = list.listIterator();

            Console.WriteLine("Вперед:");
            while (it.hasNext())
            {
                int x = it.next();
                Console.WriteLine(x);

                if (x == 20)
                    it.set(200);

                if (x == 30)
                    it.remove();
            }

            Console.WriteLine("После set/remove:");
            PrintIntArray(list.ToArray());

            it = list.listIterator(1);
            it.add(500);

            Console.WriteLine("После add через iterator:");
            PrintIntArray(list.ToArray());

            Console.WriteLine("Назад:");
            while (it.hasPrevious())
                Console.WriteLine(it.previous());
        }

        static void TestIteratorExceptions()
        {
            PrintLine();
            Console.WriteLine("Проверка исключений обычного iterator()");

            MyHashSet<int> set = new MyHashSet<int>();
            MyIterator<int> it = set.iterator();

            try
            {
                it.next();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("Пустой iterator next(): " + ex.Message);
            }

            set.add(1);
            it = set.iterator();

            try
            {
                it.remove();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("remove() до next(): " + ex.Message);
            }

            try
            {
                it.next();
                it.remove();
                it.remove();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("двойной remove(): " + ex.Message);
            }
        }

        static void TestListIteratorExceptions()
        {
            PrintLine();
            Console.WriteLine("Проверка исключений listIterator()");

            MyArrayList<int> list = new MyArrayList<int>();
            list.Add(10);
            list.Add(20);

            MyListIterator<int> it = list.listIterator();

            try
            {
                it.previous();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("previous() в начале: " + ex.Message);
            }

            try
            {
                it.set(100);
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("set() до next()/previous(): " + ex.Message);
            }

            try
            {
                it.remove();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("remove() до next()/previous(): " + ex.Message);
            }

            it = list.listIterator(list.Size());

            try
            {
                it.next();
            }
            catch (MyIteratorException ex)
            {
                Console.WriteLine("next() в конце: " + ex.Message);
            }

            it = list.listIterator();
            Console.WriteLine("Проверка previous() после next():");
            Console.WriteLine(it.next());
            Console.WriteLine(it.previous());

            it = list.listIterator(1);
            Console.WriteLine("nextIndex = " + it.nextIndex());
            Console.WriteLine("previousIndex = " + it.previousIndex());
        }
    }
}