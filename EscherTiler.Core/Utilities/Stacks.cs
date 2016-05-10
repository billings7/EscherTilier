#region © Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
// Copyright (c) 2015, Web Applications UK Ltd
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Web Applications UK Ltd nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL WEB APPLICATIONS UK LTD BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EscherTiler.Utilities
{
    #region Stack with 2 items.
    /// <summary>
    ///		A stack of <c>Tuple&lt;T1, T2&gt;</c>.
    /// </summary>
    /// <typeparam name="T1">The type of item 1.</typeparam>
    /// <typeparam name="T2">The type of item 2.</typeparam>
    [PublicAPI]
    public class Stack<T1, T2> : Stack<Tuple<T1, T2>>
    {
        /// <summary>
        ///		Initializes a new instance of the <see cref="Stack{T1, T2}"/> class that is empty
        ///		and has the specified initial capacity or the default initial capacity, whichever is greater.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public Stack(int capacity = 4) : base(capacity)
        {
        }

        /// <summary>
        ///		Initializes a new instance of the <see cref="Stack{T1, T2}"/> class that contains elements copied
        ///		from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public Stack([NotNull] IEnumerable<Tuple<T1, T2>> collection) : base(collection)
        {
        }

        /// <summary>
        ///		Inserts an object at the top of the <see cref="Stack{T1, T2}" />.
        /// </summary>
        /// <param name="item1">Item 1 of the tuple.</param>
        /// <param name="item2">Item 2 of the tuple.</param>
        public void Push(
            T1 item1,
            T2 item2)
        {
            Push(new Tuple<T1, T2>(item1, item2));
        }

        /// <summary>
        ///		Removes and returns the object at the top of the <see cref="Stack{T1, T2}"/>.
        /// </summary>
        /// <param name="item1">Item 1 of the tuple.</param>
        /// <param name="item2">Item 2 of the tuple.</param>
        public void Pop(
            out T1 item1,
            out T2 item2)
        {
            Tuple<T1, T2> tuple = Pop();
            if (tuple == null) throw new NullReferenceException();
            item1 = tuple.Item1;
            item2 = tuple.Item2;
        }

        /// <summary>
        ///		Returns the object at the top of the <see cref="Stack{T1, T2}"/> without removing it.
        /// </summary>
        /// <param name="item1">Item 1 of the tuple.</param>
        /// <param name="item2">Item 2 of the tuple.</param>
        public void Peek(
            out T1 item1,
            out T2 item2)
        {
            Tuple<T1, T2> tuple = Peek();
            if (tuple == null) throw new NullReferenceException();
            item1 = tuple.Item1;
            item2 = tuple.Item2;
        }

        /// <summary>
        ///		Removes and returns the object at the top of the <see cref="Stack{T1, T2}"/>.
        /// </summary>
        /// <param name="item1">Item 1 of the tuple.</param>
        /// <param name="item2">Item 2 of the tuple.</param>
        /// <returns><see langword="true"/> if the stack was not empty; otherwise <see langword="false"/>.</returns>
        public bool TryPop(
            out T1 item1,
            out T2 item2)
        {
            if (Count < 1)
            {
                item1 = default(T1);
                item2 = default(T2);
                return false;
            }
            Tuple<T1, T2> tuple = Pop();
            if (tuple == null) throw new NullReferenceException();
            item1 = tuple.Item1;
            item2 = tuple.Item2;
            return true;
        }

        /// <summary>
        ///		Returns the object at the top of the <see cref="Stack{T1, T2}"/> without removing it.
        /// </summary>
        /// <param name="item1">Item 1 of the tuple.</param>
        /// <param name="item2">Item 2 of the tuple.</param>
        /// <returns><see langword="true"/> if the stack was not empty; otherwise <see langword="false"/>.</returns>
        public bool TryPeek(
            out T1 item1,
            out T2 item2)
        {
            if (Count < 1)
            {
                item1 = default(T1);
                item2 = default(T2);
                return false;
            }
            Tuple<T1, T2> tuple = Peek();
            if (tuple == null) throw new NullReferenceException();
            item1 = tuple.Item1;
            item2 = tuple.Item2;
            return true;
        }
    }
    #endregion

}