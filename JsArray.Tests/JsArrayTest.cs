using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace JsArray.Tests
{

    [TestClass]
    public class JsArrayTest
    {

        [TestMethod]
        public void Indexer()
        {
            var ary = new FlexArray<int>() { 1, 2, 3 };
            ary[0].Is(1);
            ary[1].Is(2);
            ary[2].Is(3);
            ary[ary.Count].Is<int>(default(int));


            ary = new FlexArray<int>();
            ary[1] = 10;
            ary[0].Is(default(int));
            ary[1].Is(10);
        }


        [TestMethod]
        public void Indexer_ArgumentOutOfRangeException()
        {
            var ary = new FlexArray<int>() { 1, 2, 3 };
            var _ = ary[-1];
        }


        [TestMethod]
        public void Count()
        {
            var ary = new FlexArray<int>();
            ary.Count.Is(0);

            for (int i = 1; i < 3; i++)
            {
                ary.Add(i);
                ary.Count.Is(i);
            }

            ary = new FlexArray<int>();
            ary[2] = 10;
            ary.Count.Is(3);
        }


        [TestMethod]
        public void IsReadOnly()
        {
            new FlexArray<object>().IsReadOnly.Is(false);
        }


        [TestMethod]
        public void Add()
        {
            var ary = new FlexArray<int>();
            for (int i = 0; i < 10; i++)
            {
                ary.Count.Is(i);
                ary.Add(i);
                ary.Count.Is(i + 1);
            }

            ary = new FlexArray<int>();
            ary[2] = 10;
            ary.Count.Is(3);
            ary.Add(20);
            ary.Count.Is(4);
            ary[3].Is(20);
        }


        [TestMethod]
        public void Clear()
        {
            var ary = new FlexArray<int>();
            ary.Count.Is(0);
            ary.Clear();
            ary.Count.Is(0);

            ary.Add(10);
            ary[0] = 10;
            ary[0].Is(10);
            ary.Clear();
            ary.Count.Is(0);
            ary[0].Is(default(int));

            ary.Add(10);
            ary.Add(20);
            ary.Clear();
            ary.Count.Is(0);
            ary[0].Is(default(int));
            ary[1].Is(default(int));
        }


        [TestMethod]
        public void Contains()
        {
            var ary = new FlexArray<int>() { 1, 2, 3 };

            ary.Contains(2).Is(true);
            ary.Contains(4).Is(false);
        }


        [TestMethod]
        public void CopyTo()
        {
            int[] ary = new int[1];
            var flexAry = new FlexArray<int>() { 10 };
            flexAry.CopyTo(ary, 0);
            ary[0].Is(10);

            ary = new int[3];
            flexAry = new FlexArray<int>() { 10, 20 };
            flexAry.CopyTo(ary, 1);
            ary[1].Is(10);
            ary[2].Is(20);

            ary = new int[3];
            flexAry = new FlexArray<int>() { 10, 20 };
            flexAry.CopyTo(ary, 0);
            ary[0].Is(10);
            ary[1].Is(20);

            ary = new int[3];
            flexAry = new FlexArray<int>() { 10, 20, 30 };
            flexAry.CopyTo(ary, 0);
            ary.Is(flexAry);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyTo_ArgumentNullException()
        {
            new FlexArray<int>().CopyTo(null, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyTo_ArgumentOutOfRangeException()
        {
            int[] ary = new int[10];
            new FlexArray<int>().CopyTo(ary, -1);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyTo_ArgumentException()
        {
            int[] ary = new int[1];
            var flexAry = new FlexArray<int>() { 10, 20 };

            flexAry.CopyTo(ary, 0);
        }


        [TestMethod]
        public void IndexOf()
        {
            var ary = new FlexArray<int>() { 10, 20, 30 };

            ary.IndexOf(10).Is(0);
            ary.IndexOf(20).Is(1);
            ary.IndexOf(30).Is(2);
            ary.IndexOf(40).Is(-1);
        }


        [TestMethod]
        public void Insert()
        {
            var ary = new FlexArray<int>();

            ary.Insert(0, 10);
            ary[0].Is(10);

            ary.Insert(0, 20);
            ary.Is(new int[] { 20, 10 });

            ary.Insert(1, 30);
            ary.Is(new int[] { 20, 30, 10 });

            ary.Insert(ary.Count, 40);
            ary.Is(new int[] { 20, 30, 10, 40 });

            ary.Insert(2, 50);
            ary.Is(new int[] { 20, 30, 50, 10, 40 });

            ary.Insert(ary.Count + 1, 60);
            ary.Is(new int[] { 20, 30, 50, 10, 40, default(int), 60 });
        }


        [TestMethod]
        public void Remove()
        {
            var ary = new FlexArray<int>() { 1, 2, 3 };

            ary.Remove(1);
            ary.Is(new int[] { default(int), 2, 3 });

            ary.Remove(2);
            ary.Is(new int[] { default(int), default(int), 3 });

            ary.Remove(3);
            ary.Count.Is(0);

            ary = new FlexArray<int>() { 1, 2, 3 };
            ary.Remove(2);
            ary.Remove(3);
            ary.Count.Is(1);
            ary[0].Is(1);
        }


        [TestMethod]
        public void RemoveAt()
        {
            var ary = new FlexArray<int>() { 10, 20, 30 };

            ary.RemoveAt(1);
            ary.Is(new int[] { 10, default(int), 30 });

            ary.RemoveAt(2);
            ary.Is(new int[] { 10 });

            ary.RemoveAt(2);
            ary.Is(new int[] { 10 });

            ary.RemoveAt(-1);
            ary.Is(new int[] { 10 });

            ary.RemoveAt(0);
            ary.Count.Is(0);
        }


        [TestMethod]
        public void Enumeration()
        {
            var ary = new FlexArray<int>() { 10, 20, 30 };

            foreach (var _ in ary)
                ;
        }
    }


}
