using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions
{
    public class TypeComparer : IEqualityComparer<IEnumerable<Type>>
    {
        static TypeComparer()
        {
            Default = new TypeComparer();
        }

        public static TypeComparer Default
        {
            get;
            private set;
        }

        public bool Equals(IEnumerable<Type> first, IEnumerable<Type> second)
        {
            var firstArray = first.ToArray();
            var secondArray = second.ToArray();

            bool equals = false;

            if (firstArray.Length == secondArray.Length)
            {
                @equals = !firstArray.Where((t, i) => t != secondArray[i]).Any();
            }

            return equals;
        }

        public bool Assignable(IEnumerable<Type> first, IEnumerable<Type> second)
        {
            var firstArray = first.ToArray();
            var secondArray = second.ToArray();

            bool assignable = false;

            if (firstArray.Length == secondArray.Length)
            {
                assignable = !firstArray.Where((t, i) => !t.IsAssignableFrom(secondArray[i])).Any();
            }

            return assignable;
        }

        public int GetHashCode(IEnumerable<Type> types)
        {
            return types.GetHashCode();
        }
    }
}
