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
    public class Int32VirtualRange : VirtualRange<Int32, Int32>
    {
        #region Ctors
        public Int32VirtualRange(Int32 from, UInt64 count, Int32 offset) : base(from, count, offset)
        {
        }

        public Int32VirtualRange(Int32 from, UInt64 count, Int32 offset, Func<IVirtualRange<Int32, Int32>, IVirtualRange<Int32, Int32>, Int32> comparer) : base(from, count, offset, comparer)
        {
        }

        public Int32VirtualRange(Int32 from, Int32 to, Int32 offset) : base(from, to, offset)
        {
        }

        public Int32VirtualRange(Int32 from, Int32 to, Int32 offset, Func<IVirtualRange<Int32, Int32>, IVirtualRange<Int32, Int32>, Int32> comparer) : base(from, to, offset, comparer)
        {
        }

        public Int32VirtualRange(Int32 from, Int32 to, UInt64 count, Int32 offset) : base(from, to, count, offset)
        {
        }

        public Int32VirtualRange(Int32 from, Int32 to, UInt64 count, Int32 offset, Func<IVirtualRange<Int32, Int32>, IVirtualRange<Int32, Int32>, Int32> comparer) : base(from, to, count, offset, comparer)
        {
        }
        #endregion
    }
}