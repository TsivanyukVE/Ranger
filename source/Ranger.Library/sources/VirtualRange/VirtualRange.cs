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
    public class VirtualRange<TValue, TOffset> : IVirtualRange<TValue, TOffset> where TValue : IComparable where TOffset : IComparable
    {
        #region Ctors
        public VirtualRange(TValue from, UInt64 count, TOffset offset)
        {
            if (count == 0)
            {
                throw new ArgumentOutOfRangeException("count equals 0");
            }
            this.from = from;
            this.count = count;
            this.offset = offset;
            this.to = this.from;
            for (UInt64 i = 1; i < this.count; i++)
            {
                this.to += this.offset;
            }
        }

        public VirtualRange(TValue from, UInt64 count, TOffset offset, Func<IVirtualRange<TValue, TOffset>, IVirtualRange<TValue, TOffset>, Int32> comparer) : this(from, count, offset)
        {
            this.comparer = comparer;
        }

        public VirtualRange(TValue from, TValue to, TOffset offset)
        {
            if (from.CompareTo(to) == 1)
            {
                throw new ArgumentOutOfRangeException("from > to");
            }
            this.from = from;
            this.to = to;
            this.offset = offset;
            this.count = 1;
            var tempTo = this.from;
            while (tempTo.CompareTo(this.to) == -1)
            {
                tempTo += this.offset;
                this.count += 1;
            }
            if (this.to.CompareTo(tempTo) == 1)
            {
                throw new ArgumentOutOfRangeException("params incorrect");
            }
        }

        public VirtualRange(TValue from, TValue to, TOffset offset, Func<IVirtualRange<TValue, TOffset>, IVirtualRange<TValue, TOffset>, Int32> comparer) : this(from, to, offset)
        {
            this.comparer = comparer;
        }

        public VirtualRange(TValue from, TValue to, UInt64 count, TOffset offset)
        {
            if (count == 0)
            {
                throw new ArgumentOutOfRangeException("count equals 0");
            }
            if (from.CompareTo(to) == 1)
            {
                throw new ArgumentOutOfRangeException("from > to");
            }
            this.from = from;
            this.to = to;
            this.count = count;
            this.offset = offset;
            UInt64 tempCount = 1;
            var tempTo = this.from;
            while (tempTo.CompareTo(this.to) == -1)
            {
                tempTo += this.offset;
                tempCount += 1;
            }
            if (this.to.CompareTo(tempTo) == 1 || tempCount.CompareTo(this.count) != 0)
            {
                throw new ArgumentOutOfRangeException("params incorrect");
            }
        }

        public VirtualRange(TValue from, TValue to, UInt64 count, TOffset offset, Func<IVirtualRange<TValue, TOffset>, IVirtualRange<TValue, TOffset>, Int32> comparer) : this(from, to, count, offset)
        {
            this.comparer = comparer;
        }
        #endregion

        #region Memders
        protected Func<IVirtualRange<TValue, TOffset>, IVirtualRange<TValue, TOffset>, Int32> comparer = (l, r) =>
            {
                return l.From.CompareTo(r.From) == 0 ? l.To.CompareTo(r.To) == 0 ? 0 : l.To.CompareTo(r.To) : l.From.CompareTo(r.From);
            };

        protected dynamic from;

        protected dynamic to;

        protected dynamic offset;

        protected UInt64 count;
        #endregion

        #region Properties
        public TValue From { get { return this.from; } }

        public TValue To { get { return this.to; } }

        public TOffset Offset { get { return this.offset; } }

        public UInt64 Count { get { return this.count; } }

        public TValue this[UInt64 index]
        {
            get
            {
                var result = this.from;
                for (UInt64 i = 1; i < index; i++)
                {
                    result += this.offset;
                }
                return result;
            }
        }
        #endregion

        #region Functions
        private Boolean Sync(dynamic thisValue, dynamic otherValue, dynamic thisOffset, dynamic otherOffset)
        {
            if (thisOffset.CompareTo(otherOffset) != 0)
            {
                throw new ArgumentOutOfRangeException("Offsets is not equal.");
            }
            var minValue = thisValue.CompareTo(otherValue) == -1 ? thisValue : otherValue;
            var maxValue = thisValue.CompareTo(otherValue) == -1 ? otherValue : thisValue;
            var compareResult = -1;
            while ((compareResult = minValue.CompareTo(maxValue)) == -1)
            {
                minValue += thisOffset;
            }
            return compareResult == 0;
        }
        #endregion

        #region Methods
        public Boolean Contains(TValue value)
        {
            if (!this.Sync(this.From, value, this.Offset, this.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            return (this.From.CompareTo(value) < 1 && this.To.CompareTo(value) > -1);
        }

        public Boolean Contains(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            return (this.From.CompareTo(other.From) < 1 && this.To.CompareTo(other.To) > -1);
        }

        public Boolean Overlaps(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            return !((this.From.CompareTo(other.From) == 1 && this.From.CompareTo(other.To) == 1) || (this.To.CompareTo(other.From) == -1 && this.To.CompareTo(other.To) == -1));
        }

        public Int32 CompareTo(IVirtualRange<TValue, TOffset> other)
        {
            return this.comparer(this, other);
        }

        public List<IVirtualRange<TValue, TOffset>> UnionWith(TValue value)
        {
            if (!this.Sync(this.From, value, this.Offset, this.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (this.From.CompareTo(value) == -1)
            {
                if (value.CompareTo(this.To) == 1)
                {
                    if (this.to + this.offset == (dynamic)value)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, value, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { this, new VirtualRange<TValue, TOffset>(value, value, this.Offset)  };
                    }
                }
                else
                {
                    if (this.To.CompareTo(value) == -1)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, value, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { this };
                    }
                }
            }
            else
            {
                if (this.From.CompareTo(value) == 1)
                {
                    if ((dynamic)value + this.offset == this.from)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, this.To, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, value, this.Offset), this };
                    }
                }
                else
                {
                    if (this.To.CompareTo(value) == -1)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, value, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, this.To, this.Offset) };
                    }
                }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> UnionWith(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (this.From.CompareTo(other.From) == -1)
            {
                if (other.From.CompareTo(this.To) == 1)
                {
                    if (this.to + this.offset == (dynamic)other.From)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, other.To, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { this, other };
                    }
                }
                else
                {
                    if (this.To.CompareTo(other.To) == -1)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, other.To, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { this };
                    }
                }
            }
            else
            {
                if (this.From.CompareTo(other.To) == 1)
                {
                    if ((dynamic)other.To + this.offset == this.from)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(other.From, this.To, this.Offset) };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { other, this };
                    }
                }
                else
                {
                    if (this.To.CompareTo(other.To) == -1)
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { other };
                    }
                    else
                    {
                        return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(other.From, this.To, this.Offset) };
                    }
                }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> ExceptWith(TValue value)
        {
            if (!this.Sync(this.From, value, this.Offset, this.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (((this.From.CompareTo(value) == 1 && this.From.CompareTo(value) == 1) || (this.To.CompareTo(value) == -1 && this.To.CompareTo(value) == -1)))
            {
                return new List<IVirtualRange<TValue, TOffset>>() { this };
            }
            if (this.From.CompareTo(value) == -1)
            {
                if (value.CompareTo(this.To) == -1)
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)value - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)value + this.offset, this.To, this.Offset) };
                }
                else
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)value - this.offset, this.Offset) };
                }
            }
            else
            {
                if (value.CompareTo(this.To) == -1)
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>((dynamic)value + this.offset, this.To, this.Offset) };
                }
                else
                {
                    return new List<IVirtualRange<TValue, TOffset>>();
                }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> ExceptWith(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (((this.From.CompareTo(other.From) == 1 && this.From.CompareTo(other.To) == 1) || (this.To.CompareTo(other.From) == -1 && this.To.CompareTo(other.To) == -1)))
            {
                return new List<IVirtualRange<TValue, TOffset>>() { this };
            }
            if (this.From.CompareTo(other.From) == -1)
            {
                if (other.To.CompareTo(this.To) == -1)
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)other.From - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)other.To + this.offset, this.To, this.Offset) };
                }
                else
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)other.From - this.offset, this.Offset) };
                }
            }
            else
            {
                if (other.To.CompareTo(this.To) == -1)
                {
                    return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>((dynamic)other.To + this.offset, this.To, this.Offset) };
                }
                else
                {
                    return new List<IVirtualRange<TValue, TOffset>>();
                }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> SymmetricExceptWith(TValue value)
        {
            if (!this.Sync(this.From, value, this.Offset, this.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (this.From.CompareTo(value) == 1 && this.From.CompareTo(value) == 1)
            {
                return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, value, this.Offset), this };
            }
            if (this.To.CompareTo(value) == -1 && this.To.CompareTo(value) == -1)
            {
                return new List<IVirtualRange<TValue, TOffset>>() { this, new VirtualRange<TValue, TOffset>(value, value, this.Offset) };
            }
            switch (this.From.CompareTo(value))
            {
                case -1:
                    switch (value.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)value - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)value + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)value - this.offset, this.Offset) };
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)value - this.offset, this.Offset), new VirtualRange<TValue, TOffset>(this.to + this.offset, value, this.Offset) };
                    }
                case 0:
                    switch (value.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>((dynamic)value + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>();
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.to + this.offset, value, this.Offset) };
                    }
                default:
                    switch (value.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, this.from - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)value + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, this.from - this.offset, this.Offset) };
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(value, this.from - this.offset, this.Offset), new VirtualRange<TValue, TOffset>(this.to + this.offset, value, this.Offset) };
                    }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> SymmetricExceptWith(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (this.From.CompareTo(other.From) == 1 && this.From.CompareTo(other.To) == 1)
            {
                return new List<IVirtualRange<TValue, TOffset>>() { other, this };
            }
            if (this.To.CompareTo(other.From) == -1 && this.To.CompareTo(other.To) == -1)
            {
                return new List<IVirtualRange<TValue, TOffset>>() { this, other };
            }
            switch (this.From.CompareTo(other.From))
            {
                case -1:
                    switch (other.To.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)other.From - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)other.To + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)other.From - this.offset, this.Offset) };
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.From, (dynamic)other.From - this.offset, this.Offset), new VirtualRange<TValue, TOffset>(this.to + this.offset, other.To, this.Offset) };
                    }
                case 0:
                    switch (other.To.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>((dynamic)other.To + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>();
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(this.to + this.offset, other.To, this.Offset) };
                    }
                default:
                    switch (other.To.CompareTo(this.To))
                    {
                        case -1:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(other.From, this.from - this.offset, this.Offset), new VirtualRange<TValue, TOffset>((dynamic)other.To + this.offset, this.To, this.Offset) };
                        case 0:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(other.From, this.from - this.offset, this.Offset) };
                        default:
                            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(other.From, this.from - this.offset, this.Offset), new VirtualRange<TValue, TOffset>(this.to + this.offset, other.To, this.Offset) };
                    }
            }
        }

        public List<IVirtualRange<TValue, TOffset>> IntersectWith(TValue value)
        {
            if (!this.Sync(this.From, value, this.Offset, this.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (((this.From.CompareTo(value) == 1 && this.From.CompareTo(value) == 1) || (this.To.CompareTo(value) == -1 && this.To.CompareTo(value) == -1)))
            {
                return new List<IVirtualRange<TValue, TOffset>>();
            }
            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(
                value.CompareTo(this.From) == 1 ?value : this.From, 
                value.CompareTo(this.To) == 1 ? this.To : value, 
                this.Offset) };
        }

        public List<IVirtualRange<TValue, TOffset>> IntersectWith(IVirtualRange<TValue, TOffset> other)
        {
            if (!this.Sync(this.From, other.From, this.Offset, other.Offset))
            {
                throw new ArgumentOutOfRangeException("Ranges is not sync.");
            }
            if (((this.From.CompareTo(other.From) == 1 && this.From.CompareTo(other.To) == 1) || (this.To.CompareTo(other.From) == -1 && this.To.CompareTo(other.To) == -1)))
            {
                return new List<IVirtualRange<TValue, TOffset>>();
            }
            return new List<IVirtualRange<TValue, TOffset>>() { new VirtualRange<TValue, TOffset>(
                other.From.CompareTo(this.From) == 1 ? other.From : this.From, 
                other.To.CompareTo(this.To) == 1 ? this.To : other.To, 
                this.Offset) };
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (TValue i = this.from; i <= this.to; i += this.offset)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}