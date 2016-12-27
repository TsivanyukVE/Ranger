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
    public class Int64VirtualRangeTests
    {
        [Fact]
        public void UnionWith()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(101, 200, 1);
            var ranges = range1.UnionWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 200);
                Assert.Equal(range.Count, (UInt64)200);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(100, 200, 1);
            ranges = range1.UnionWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 200);
                Assert.Equal(range.Count, (UInt64)200);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(100, 200, 1);
            ranges = range2.UnionWith(range1);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 200);
                Assert.Equal(range.Count, (UInt64)200);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(30, 40, 1);
            ranges = range2.UnionWith(range1);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)100);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(102, 200, 1);
            ranges = range1.UnionWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 100);
                        Assert.Equal(range.Count, (UInt64)100);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 102);
                        Assert.Equal(range.To, 200);
                        Assert.Equal(range.Count, (UInt64)99);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(102, 200, 1);
            ranges = range2.UnionWith(range1);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 100);
                        Assert.Equal(range.Count, (UInt64)100);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 102);
                        Assert.Equal(range.To, 200);
                        Assert.Equal(range.Count, (UInt64)99);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void ExceptWith()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(1, 100, 1);
            var ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 0);

            range1 = new Int64VirtualRange(50, 100, 1);
            range2 = new Int64VirtualRange(1, 150, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 0);

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 50, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 51);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)50);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(10, 150, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 9);
                Assert.Equal(range.Count, (UInt64)9);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(25, 75, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 24);
                        Assert.Equal(range.Count, (UInt64)24);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 76);
                        Assert.Equal(range.To, 100);
                        Assert.Equal(range.Count, (UInt64)25);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 1, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 2);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)99);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(101, 110, 1);
            ranges = range1.ExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)100);
                Assert.Equal(range.Offset, 1);
            }
        }

        [Fact]
        public void SymmetricExceptWith()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(1, 100, 1);
            var ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 0);

            range1 = new Int64VirtualRange(50, 100, 1);
            range2 = new Int64VirtualRange(1, 150, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 49);
                        Assert.Equal(range.Count, (UInt64)49);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 101);
                        Assert.Equal(range.To, 150);
                        Assert.Equal(range.Count, (UInt64)50);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 50, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 51);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)50);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(10, 150, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 9);
                        Assert.Equal(range.Count, (UInt64)9);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 101);
                        Assert.Equal(range.To, 150);
                        Assert.Equal(range.Count, (UInt64)50);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(25, 75, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 24);
                        Assert.Equal(range.Count, (UInt64)24);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 76);
                        Assert.Equal(range.To, 100);
                        Assert.Equal(range.Count, (UInt64)25);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 1, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 2);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)99);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(101, 110, 1);
            ranges = range1.SymmetricExceptWith(range2);
            Assert.Equal(ranges.Count, 2);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                switch (i)
                {
                    case 0:
                        Assert.Equal(range.From, 1);
                        Assert.Equal(range.To, 100);
                        Assert.Equal(range.Count, (UInt64)100);
                        Assert.Equal(range.Offset, 1);
                        break;
                    default:
                        Assert.Equal(range.From, 101);
                        Assert.Equal(range.To, 110);
                        Assert.Equal(range.Count, (UInt64)10);
                        Assert.Equal(range.Offset, 1);
                        break;
                }
            }
        }

        [Fact]
        public void IntersectWith()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(101, 150, 1);
            var ranges = range1.IntersectWith(range2);
            Assert.Equal(ranges.Count, 0);

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 50, 1);
            ranges = range1.IntersectWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 1);
                Assert.Equal(range.To, 50);
                Assert.Equal(range.Count, (UInt64)50);
                Assert.Equal(range.Offset, 1);
            }

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(100, 150, 1);
            ranges = range1.IntersectWith(range2);
            Assert.Equal(ranges.Count, 1);
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];
                Assert.Equal(range.From, 100);
                Assert.Equal(range.To, 100);
                Assert.Equal(range.Count, (UInt64)1);
                Assert.Equal(range.Offset, 1);
            }
        }

        [Fact]
        public void Overlaps()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(101, 200, 1);
            Assert.False(range1.Overlaps(range2));

            range1 = new Int64VirtualRange(5, 100, 1);
            range2 = new Int64VirtualRange(1, 200, 1);
            Assert.True(range1.Overlaps(range2));

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(100, 200, 1);
            Assert.True(range1.Overlaps(range2));

            range1 = new Int64VirtualRange(50, 100, 1);
            range2 = new Int64VirtualRange(1, 50, 1);
            Assert.True(range1.Overlaps(range2));

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(25, 50, 1);
            Assert.True(range1.Overlaps(range2));
        }

        [Fact]
        public void Contains()
        {
            var range1 = new Int64VirtualRange(1, 100, 1);
            var range2 = new Int64VirtualRange(101, 200, 1);
            Assert.False(range1.Contains(range2));

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(1, 100, 1);
            Assert.True(range1.Contains(range2));

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(50, 200, 1);
            Assert.False(range1.Contains(range2));

            range1 = new Int64VirtualRange(1, 100, 1);
            range2 = new Int64VirtualRange(25, 50, 1);
            Assert.True(range1.Contains(range2));
        }

        [Fact]
        public void Ctor()
        {
            var range = new Int64VirtualRange(1, 120, 1);
            Assert.Equal(range.From, 1);
            Assert.Equal(range.To, 120);
            Assert.Equal(range.Count, (UInt64)120);
            Assert.Equal(range.Offset, 1);

            range = new Int64VirtualRange(1, 121, 2);
            Assert.Equal(range.From, 1);
            Assert.Equal(range.To, 121);
            Assert.Equal(range.Count, (UInt64)61);
            Assert.Equal(range.Offset, 2);

            range = new Int64VirtualRange(1, (UInt64)100, 2);
            Assert.Equal(range.From, 1);
            Assert.Equal(range.To, 199);
            Assert.Equal(range.Count, (UInt64)100);
            Assert.Equal(range.Offset, 2);

            range = new Int64VirtualRange(1, 301, 151, 2);
            Assert.Equal(range.From, 1);
            Assert.Equal(range.To, 301);
            Assert.Equal(range.Count, (UInt64)151);
            Assert.Equal(range.Offset, 2);
        }
    }
}