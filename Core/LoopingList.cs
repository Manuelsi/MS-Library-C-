using System;
using System.Collections;
using System.Collections.Generic;

namespace NullrefLib.Collections {

	/// <summary>
	/// A list with it's own index with a Next method and a looping behaviour, preventing Out Of Index errors unless the count is 0.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class LoopingList<T> : IEnumerable<T> {
		private int index = 0;
		private List<T> list;
		public List<T> List => list;

		/// <summary>
		/// Points to LAST USED value.
		/// Setting this index to point a value and using 'Next' will result in returning the value indexed after the set one.
		/// </summary>
		public int Index {
			get {
				return index;
			}
			set {
				value %= list.Count;
				index = value;
			}
		}

		/// <summary>
		/// Count of items in the list.
		/// </summary>
		public int Count => List.Count;

		public T this[int i] {
			get => list[i % list.Count];
			set => list[i % list.Count] = value;
		}

		public LoopingList() => list = new List<T>();

		public LoopingList(int size) => list = new List<T>(size);

		public LoopingList(List<T> l) => list = new List<T>(l);

		public T Current {
			get => list[Index];
			set => list[Index] = value;
		}

		/// <summary>
		/// Returns next value from the loop. This updates its internal index, then returns value.
		/// </summary>
		public T Next() {
			Index += 1;
			return list[Index];
		}

		public void Add(T value) => list.Add(value);

		public void Remove(T value) => list.Remove(value);

		/// <summary>
		/// CAREFUL: Removing at a defined index still uses the looping behaviour.
		/// Be sure to know which element you're trying to delete after the looping behaviour!
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) {
			index = index % Count;
			list.RemoveAt(index);
			if (Index >= index && Index > 0)
				Index--;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newList"></param>
		/// <param name="resetIndex"></param>
		public void Replace(List<T> newList, bool resetIndex = true) {
			list = new List<T>(newList);
			if (resetIndex)
				index = 0;
		}

		public static explicit operator LoopingList<T>(List<T> l) => new LoopingList<T>(l);

		public static implicit operator List<T>(LoopingList<T> l) => l.List;

		public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

}