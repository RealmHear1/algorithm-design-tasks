using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task30
{
    using System;
    using System.Collections.Generic;

    public class MyString
    {
        private char[] value;
        private int length;

        public MyString()
        {
            value = new char[0];
            length = 0;
        }

        public MyString(char[] value)
        {
            if (value == null)
                throw new ArgumentNullException();

            this.value = new char[value.Length];
            Array.Copy(value, this.value, value.Length);
            length = value.Length;
        }

        public MyString(MyString original)
        {
            if (original == null)
                throw new ArgumentNullException();

            value = new char[original.length];
            Array.Copy(original.value, value, original.length);
            length = original.length;
        }

        public int Length() => length;

        public char CharAt(int index)
        {
            if (index < 0 || index >= length)
                throw new IndexOutOfRangeException();
            return value[index];
        }

        public MyString Substring(int beginIndex, int endIndex)
        {
            if (beginIndex < 0 || endIndex > length || beginIndex > endIndex)
                throw new IndexOutOfRangeException();

            int newLen = endIndex - beginIndex;
            char[] result = new char[newLen];

            for (int i = 0; i < newLen; i++)
                result[i] = value[beginIndex + i];

            return new MyString(result);
        }

        public MyString Concat(MyString str)
        {
            if (str == null)
                throw new ArgumentNullException();

            char[] result = new char[length + str.length];

            for (int i = 0; i < length; i++)
                result[i] = value[i];

            for (int i = 0; i < str.length; i++)
                result[length + i] = str.value[i];

            return new MyString(result);
        }

        public bool Equals(MyString str)
        {
            if (str == null || length != str.length)
                return false;

            for (int i = 0; i < length; i++)
                if (value[i] != str.value[i])
                    return false;

            return true;
        }

        public bool EqualsIgnoreCase(MyString str)
        {
            if (str == null || length != str.length)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (char.ToLower(value[i]) != char.ToLower(str.value[i]))
                    return false;
            }

            return true;
        }

        public MyString ToLowerCase()
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = char.ToLower(value[i]);

            return new MyString(result);
        }

        public MyString ToUpperCase()
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = char.ToUpper(value[i]);

            return new MyString(result);
        }

        public MyString Trim()
        {
            int start = 0;
            int end = length - 1;

            while (start <= end && char.IsWhiteSpace(value[start]))
                start++;

            while (end >= start && char.IsWhiteSpace(value[end]))
                end--;

            if (start > end)
                return new MyString();

            return Substring(start, end + 1);
        }

        public MyString Replace(char oldChar, char newChar)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = value[i] == oldChar ? newChar : value[i];

            return new MyString(result);
        }

        public bool Contains(MyString substr)
        {
            return IndexOf(substr) != -1;
        }

        public int IndexOf(MyString substr)
        {
            if (substr == null)
                throw new ArgumentNullException();

            if (substr.length == 0)
                return 0;

            for (int i = 0; i <= length - substr.length; i++)
            {
                bool found = true;

                for (int j = 0; j < substr.length; j++)
                {
                    if (value[i + j] != substr.value[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    return i;
            }

            return -1;
        }

        public MyString[] Split(char delimiter)
        {
            List<MyString> parts = new List<MyString>();
            int start = 0;

            for (int i = 0; i < length; i++)
            {
                if (value[i] == delimiter)
                {
                    parts.Add(Substring(start, i));
                    start = i + 1;
                }
            }

            parts.Add(Substring(start, length));
            return parts.ToArray();
        }

        public bool StartsWith(MyString prefix)
        {
            if (prefix == null || prefix.length > length)
                return false;

            for (int i = 0; i < prefix.length; i++)
                if (value[i] != prefix.value[i])
                    return false;

            return true;
        }

        public bool EndsWith(MyString suffix)
        {
            if (suffix == null || suffix.length > length)
                return false;

            int start = length - suffix.length;

            for (int i = 0; i < suffix.length; i++)
                if (value[start + i] != suffix.value[i])
                    return false;

            return true;
        }

        public MyString Reverse()
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = value[length - 1 - i];

            return new MyString(result);
        }

        public static MyString ValueOf(int i)
        {
            return new MyString(i.ToString().ToCharArray());
        }

        public static MyString ValueOf(double d)
        {
            return new MyString(d.ToString().ToCharArray());
        }

        public static MyString ValueOf(bool b)
        {
            return new MyString(b.ToString().ToCharArray());
        }

        public override string ToString()
        {
            return new string(value);
        }
    }
    internal class Program
    {
        static void Main()
        {
            MyString s1 = new MyString("Hello".ToCharArray());
            MyString s2 = new MyString("World".ToCharArray());

            Console.WriteLine(s1.Concat(s2)); // HelloWorld
            Console.WriteLine(s1.Substring(1, 4)); // ell
            Console.WriteLine(s1.IndexOf(new MyString("ll".ToCharArray()))); // 2
            Console.WriteLine(s1.ToUpperCase()); // HELLO
            Console.WriteLine(s1.Reverse()); // olleH
        }
    }
}
