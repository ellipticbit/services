//-----------------------------------------------------------------------------
// Copyright (c) 2025 EllipticBit, LLC All Rights Reserved.
//-----------------------------------------------------------------------------

#if NET6_0_OR_GREATER

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
			where TKey : notnull
		{
			ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
			if (exists) {
				return val;
			}

			val = value;
			return value;
		}

		public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
			where TKey : notnull
		{
			ref var dictionaryValue = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
			if (Unsafe.IsNullRef(ref dictionaryValue)) {
				return false;
			}

			dictionaryValue = value;
			return true;
		}
	}
}

#endif
