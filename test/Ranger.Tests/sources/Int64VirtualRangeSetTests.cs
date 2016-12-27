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
using Xunit;

namespace Ranger.Tests
{
    public class Int64VirtualRangeSetTests
    {
        [Fact]
        public void Add()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(11, 30, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 30);
                Assert.Equal(range.Count, (UInt64)30);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(21, 30, 1));
            Assert.Equal(set.Count, 2);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 21);
                        Assert.Equal(range.To, 30);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(21, 30, 1));
            set.Add(new Int64VirtualRange(11, 20, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 30);
                Assert.Equal(range.Count, (UInt64)30);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(3, 10, 1));
            set.Add(new Int64VirtualRange(21, 30, 1));
            set.Add(new Int64VirtualRange(1, 40, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 40);
                Assert.Equal(range.Count, (UInt64)40);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(5, 10, 1));
            set.Add(new Int64VirtualRange(1, 5, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 10);
                Assert.Equal(range.Count, (UInt64)10);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(21, 30, 1));
            set.Add(new Int64VirtualRange(15, 17, 1));
            Assert.Equal(set.Count, 3);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                    case 1:
                        Assert.Equal(range.From, 15);
                        Assert.Equal(range.To, 17);
                        Assert.Equal(range.Count, (UInt64)3);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 21);
                        Assert.Equal(range.To, 30);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(30, 30, 1));
            set.Add(new Int64VirtualRange(21, 30, 1));
            Assert.Equal(set.Count, 2);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 21);
                        Assert.Equal(range.To, 30);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(10, 10, 1));
            set.Add(new Int64VirtualRange(30, 37, 1));
            set.Add(new Int64VirtualRange(3, 7, 1));
            Assert.Equal(set.Count, 3);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 3);
                        Assert.Equal(range.To, 7);
                        Assert.Equal(range.Count, (UInt64)5);
                        Assert.Equal(range.Offset, 1);
                        break;
                    case 1:
                        Assert.Equal(range.From, 10);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)1);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 30);
                        Assert.Equal(range.To, 37);
                        Assert.Equal(range.Count, (UInt64)8);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void Remove()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Remove(new Int64VirtualRange(11, 30, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 10);
                Assert.Equal(range.Count, (UInt64)10);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Remove(new Int64VirtualRange(10, 30, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 9);
                Assert.Equal(range.Count, (UInt64)9);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Remove(new Int64VirtualRange(1, 10, 1));
            Assert.Equal(set.Count, 0);

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 40, 1));
            set.Remove(new Int64VirtualRange(21, 30, 1));
            Assert.Equal(set.Count, 2);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 20);
                        Assert.Equal(range.Count, (UInt64)20);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 31);
                        Assert.Equal(range.To, 40);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(5, 10, 1));
            set.Remove(new Int64VirtualRange(1, 7, 1));
            Assert.Equal(set.Count, 1);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                Assert.Equal(range.From, 8);
                Assert.Equal(range.To, 10);
                Assert.Equal(range.Count, (UInt64)3);
                Assert.Equal(range.Offset, 1);
            }

            set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 15, 1));
            set.Add(new Int64VirtualRange(20, 30, 1));
            set.Remove(new Int64VirtualRange(10, 25, 1));
            Assert.Equal(set.Count, 2);
            for (int i = 0; i < set.Count; i++)
            {
                var range = set[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 9);
                        Assert.Equal(range.Count, (UInt64)9);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 26);
                        Assert.Equal(range.To, 30);
                        Assert.Equal(range.Count, (UInt64)5);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void Clear()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            set.Clear();
            Assert.Equal(set.Count, 0);
        }

        [Fact]
        public void Contains()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            Assert.False(set.Contains(new Int64VirtualRange(5, 15, 1)));
            Assert.True(set.Contains(new Int64VirtualRange(20, 40, 1)));
            Assert.False(set.Contains(new Int64VirtualRange(5, 25, 1)));
            Assert.True(set.Contains(new Int64VirtualRange(25, 40, 1)));
            Assert.True(set.Contains(5));
            Assert.False(set.Contains(15));
            Assert.True(set.Contains(50));
            Assert.False(set.Contains(65));
        }

        [Fact]
        public void Overlaps()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            Assert.True(set.Overlaps(new Int64VirtualRange(5, 15, 1)));
            Assert.True(set.Overlaps(new Int64VirtualRange(20, 40, 1)));
            Assert.False(set.Overlaps(new Int64VirtualRange(15, 19, 1)));
            Assert.True(set.Overlaps(new Int64VirtualRange(25, 54, 1)));
        }

        [Fact]
        public void CopyTo()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            var ranges = new Int64VirtualRange[3];
            set.CopyTo(ranges, 0);
            Assert.Equal(ranges.Length, 3);
            for (int i = 0; i < ranges.Length; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                    case 1:
                        Assert.Equal(range.From, 20);
                        Assert.Equal(range.To, 40);
                        Assert.Equal(range.Count, (UInt64)21);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 50);
                        Assert.Equal(range.To, 60);
                        Assert.Equal(range.Count, (UInt64)11);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void GetEnumerator()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            var i = 0;
            foreach (var range in set)
            {
                switch (i++)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 10);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                    case 1:
                        Assert.Equal(range.From, 20);
                        Assert.Equal(range.To, 40);
                        Assert.Equal(range.Count, (UInt64)21);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 50);
                        Assert.Equal(range.To, 60);
                        Assert.Equal(range.Count, (UInt64)11);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void Count()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            Assert.Equal(set.Count, 3);
        }

        [Fact]
        public void TotalCount()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            Assert.Equal(set.TotalCount, (UInt64)42);
        }

        [Fact]
        public void This()
        {
            var set = new Int64VirtualRangeSet();
            set.Add(new Int64VirtualRange(1, 10, 1));
            set.Add(new Int64VirtualRange(20, 40, 1));
            set.Add(new Int64VirtualRange(50, 60, 1));
            Assert.Equal(set.Count, 3);
            var range = set[1];
            Assert.Equal(range.From, 20);
            Assert.Equal(range.To, 40);
            Assert.Equal(range.Count, (UInt64)21);
            Assert.Equal(range.Offset, 1);
            Assert.Equal(set[(UInt64)20], 29);
        }
    }
}