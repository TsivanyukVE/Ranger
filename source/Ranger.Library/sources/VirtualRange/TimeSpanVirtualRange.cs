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

namespace Ranger
{
    public class TimeSpanVirtualRange : VirtualRange<TimeSpan, TimeSpan>
    {
        #region Ctors
        public TimeSpanVirtualRange(TimeSpan from, UInt64 count, TimeSpan offset) : base(from, count, offset)
        {
        }

        public TimeSpanVirtualRange(TimeSpan from, UInt64 count, TimeSpan offset, Func<IVirtualRange<TimeSpan, TimeSpan>, IVirtualRange<TimeSpan, TimeSpan>, Int32> comparer) : base(from, count, offset, comparer)
        {
        }
        public TimeSpanVirtualRange(TimeSpan from, TimeSpan to, TimeSpan offset) : base(from, to, offset)
        {
        }

        public TimeSpanVirtualRange(TimeSpan from, TimeSpan to, TimeSpan offset, Func<IVirtualRange<TimeSpan, TimeSpan>, IVirtualRange<TimeSpan, TimeSpan>, Int32> comparer) : base(from, to, offset, comparer)
        {
        }

        public TimeSpanVirtualRange(TimeSpan from, TimeSpan to, UInt64 count, TimeSpan offset) : base(from, to, count, offset)
        {
        }

        public TimeSpanVirtualRange(TimeSpan from, TimeSpan to, UInt64 count, TimeSpan offset, Func<IVirtualRange<TimeSpan, TimeSpan>, IVirtualRange<TimeSpan, TimeSpan>, Int32> comparer) : base(from, to, count, offset, comparer)
        {
        }
        #endregion
    }
}