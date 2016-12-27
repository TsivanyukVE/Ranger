//  The MIT License (MIT)
//
//  Copyright(c) 2016 Vitaliy Tsivanyuk <tsivanyukve@gmail.com>
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ranger
{
    public abstract class VirtualRangeSet<TValue, TOffset> : IVirtualRangeSet<TValue, TOffset>, ICollection<IVirtualRange<TValue, TOffset>>, IEnumerable<IVirtualRange<TValue, TOffset>>, IEnumerable where TValue : IComparable where TOffset : IComparable
    {
        #region Ctors
        public VirtualRangeSet()
        {
        }
        #endregion

        #region Members
        protected readonly List<IVirtualRange<TValue, TOffset>> list = new List<IVirtualRange<TValue, TOffset>>();

        protected Boolean isReadOnly = false;
        #endregion

        #region Properties
        public virtual Int32 Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public virtual Boolean IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
        }

        public virtual UInt64 TotalCount
        {
            get
            {
                UInt64 result = 0;
                foreach (var item in this.list)
                {
                    result += item.Count;
                }
                return result;
            }
        }

        public virtual TValue this[UInt64 index]
        {
            get
            {
                foreach (var item in this.list)
                {
                    if (item.Count.CompareTo(index) == 1)
                    {
                        return item[index];
                    }
                    else
                    {
                        index -= item.Count;
                    }
                }
                throw new ArgumentOutOfRangeException("index");
            }
        }

        public virtual IVirtualRange<TValue, TOffset> this[Int32 index]
        {
            get
            {
                return this.list[index];
            }
        }
        #endregion

        #region Methods
        public virtual void Add(IVirtualRange<TValue, TOffset> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            if (this.list.Count == 0)
            {
                this.list.Add(range);
                return;
            }
            Int32 fromIndex = 0;
            Int32 toIndex = 0;
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].To.CompareTo(range.From) < 1)
                {
                    fromIndex = toIndex = i;
                }
                if (this.list[i].To.CompareTo(range.To) < 1)
                {
                    toIndex = i + 1 == this.list.Count ? i : i + 1;
                }
                else
                {
                    break;
                }
            }
            var ranges = new List<IVirtualRange<TValue, TOffset>>() { range };
            for (int i = fromIndex; i <= toIndex; i++)
            {
                var insertRanges = this.list[i].UnionWith(ranges[ranges.Count - 1]);
                ranges.RemoveAt(ranges.Count - 1);
                ranges.AddRange(insertRanges);
            }
            for (int i = fromIndex; i <= toIndex; i++)
            {
                this.list.RemoveAt(fromIndex);
            }
            this.list.InsertRange(fromIndex, ranges);
        }

        public virtual Boolean Remove(IVirtualRange<TValue, TOffset> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            if (this.list.Count == 0)
            {
                return true;
            }
            Int32 fromIndex = 0;
            Int32 toIndex = 0;
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].To.CompareTo(range.From) < 1)
                {
                    fromIndex = toIndex = i;
                }
                if (this.list[i].To.CompareTo(range.To) < 1)
                {
                    toIndex = i + 1 == this.list.Count ? i : i + 1;
                }
                else
                {
                    break;
                }
            }
            var ranges = new List<IVirtualRange<TValue, TOffset>>();
            for (int i = fromIndex; i <= toIndex; i++)
            {
                ranges.AddRange(this.list[i].ExceptWith(range));
            }
            for (int i = fromIndex; i <= toIndex; i++)
            {
                this.list.RemoveAt(fromIndex);
            }
            this.list.InsertRange(fromIndex, ranges);
            return true;
        }

        public virtual void Clear()
        {
            this.list.Clear();
        }

        public virtual Boolean Contains(TValue value)
        {
            Int32 index;
            for (index = 0; index < this.list.Count && this.list[index].To.CompareTo(value) == -1; index++)
            { }
            return this.list.Count == index ? false : this.list[index].Contains(value);
        }

        public virtual Boolean Contains(IVirtualRange<TValue, TOffset> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            Int32 index;
            for (index = 0; index < this.list.Count && this.list[index].To.CompareTo(range.To) == -1; index++)
            { }
            return this.list.Count == index ? false : this.list[index].Contains(range);
        }

        public virtual Boolean Overlaps(IVirtualRange<TValue, TOffset> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            Int32 index;
            for (index = 0; index < this.list.Count && this.list[index].To.CompareTo(range.From) == -1; index++)
            { }
            return this.list.Count == index ? false : this.list[index].Overlaps(range);
        }

        public virtual void CopyTo(IVirtualRange<TValue, TOffset>[] array, Int32 arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<IVirtualRange<TValue, TOffset>> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}