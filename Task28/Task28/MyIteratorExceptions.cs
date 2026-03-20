using System;

public class MyIteratorException : Exception
{
    public MyIteratorException(string message) : base(message)
    {
    }
}

public class NoSuchElementException : MyIteratorException
{
    public NoSuchElementException() : base("No such element")
    {
    }
}

public class IllegalStateException : MyIteratorException
{
    public IllegalStateException() : base("Illegal state")
    {
    }
}