using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace JsArray
{

    public class FlexArray<T> : IList<T>
    {

        private object _lock = new object();


        private int MaxIndex { get; set; } = 0;


        private Hashtable Hashtable { get; set; } = new Hashtable();


        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (this.Hashtable.ContainsKey(index))
                {
                    return (T)this.Hashtable[index];
                }
                else
                {
                    return default(T);
                }
            }
            set
            {
                if (index < 0) throw new ArgumentOutOfRangeException("The value of the index must be greater than or equal to zero.");

                lock (_lock)
                {
                    if (index >= this.MaxIndex) this.MaxIndex = index + 1;

                    this.Hashtable[index] = value;
                }
            }
        }


        /// <summary>
        /// Gets the number of elements contained in the JsArray.
        /// </summary>
        public int Count => this.MaxIndex;


        /// <summary>
        ///  Gets a value indicating whether the JsArray is read-only.
        /// </summary>
        public bool IsReadOnly => false;


        /// <summary>
        ///  Adds an item to the JsArray.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (_lock)
            {
                this.Hashtable.Add(this.MaxIndex, item);
                this.MaxIndex++;
            }
        }


        /// <summary>
        ///  Removes all items from the JsArray.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                this.Hashtable.Clear();
                this.MaxIndex = 0;
            }
        }


        /// <summary>
        /// Determines whether the JsArray contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the JsArray.</param>
        /// <returns>true if item is found in the JsArray; otherwise, false.</returns>
        public bool Contains(T item) => this.Hashtable.ContainsValue(item);


        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection`1 to an System.Array,
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.Generic.ICollection`1. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException();
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException();
            if (array.Length - arrayIndex < this.Count) throw new ArgumentException();

            for (int dst_i = arrayIndex, src_i = 0; src_i < this.Count; dst_i++, src_i++)
            {
                array[dst_i] = this[src_i];
            }
        }


        /// <summary>
        /// Determines the index of a specific item in the JsArray.
        /// </summary>
        /// <param name="item">The object to locate in the JsArray.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (item.Equals(this[i])) return i;
            }

            return -1;
        }


        /// <summary>
        /// Inserts an item to the System.Collections.Generic.IList`1 at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the JsArray.</param>
        public void Insert(int index, T item)
        {
            for (int i = this.MaxIndex; i > index; i--)
            {
                this[i] = this[i - 1];
            }

            this[index] = item;
        }


        /// <summary>
        /// Removes the first occurrence of a specific object from the JsArray.
        /// </summary>
        /// <param name="item">The object to remove from the JsArray.</param>
        /// <returns>
        /// true if item was successfully removed from the JsArray;
        /// otherwise, false. This method also returns false if item is not found in the JsArray.
        /// </returns>
        public bool Remove(T item)
        {
            var i = this.IndexOf(item);
            if (i == -1) return false;

            lock (_lock)
            {
                this.Hashtable.Remove(i);

                this.Shrink();
                return true;
            }
        }


        /// <summary>
        /// Removes the JsArray item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                this.Hashtable.Remove(index);
                this.Shrink();
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        private void Shrink()
        {
            T defValue = default(T);
            while (this.MaxIndex > 0 && this[this.MaxIndex - 1].Equals(defValue)) this.MaxIndex--;
        }
    }
}
