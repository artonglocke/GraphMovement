﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zero based heap implementation, source https://en.wikipedia.org/wiki/Heap_(data_structure)
// Formulas needed:
// Parent node; (n - 1) / 2
// Left child: 2n + 1
// Right child: 2n + 2

namespace CustomGenerics
{
	public class Heap<T> where T : IHeapElement<T>
	{
		public int Count { get; private set; }
		private T[] m_elements;

		public Heap(int maxSize)
		{
			m_elements = new T[maxSize];
		}

		public void Add(T element)
		{
			element.index = Count;
			m_elements[Count] = element;
			SortUp(element);
			++Count;
		}

		public bool Contains(T element)
		{
			return Equals(m_elements[element.index], element);
		}

		public void Update(T element)
		{
			SortUp(element);
		}

		public T RemoveFirst()
		{
			// Remove element from heap, update count, set last element as first then re-sort the heap
			T firstElement = m_elements[0];
			--Count;
			T newFirstElement = m_elements[Count];
			m_elements[0] = newFirstElement;
			newFirstElement.index = 0;
			SortDown(newFirstElement);
			return firstElement;
		}

		private void SortUp(T element)
		{
			int parentIndex = (element.index - 1) / 2;
			while (true)
			{
				T parentElement = m_elements[parentIndex];
				if (element.CompareTo(parentElement) > 0)
				{
					Swap(element, parentElement);
					continue;
				}
				break;
			}
		}

		private void SortDown(T element)
		{
			// Sortin down requires children formula to sort against their weight
			// Left child: 2n + 1
			// Right child: 2n + 2
			while (true)
			{
				int indexLeft = element.index * 2 + 1;
				int indexRight = element.index * 2 + 2;
				int swapIndex = 0;

				if (indexLeft < Count)
				{
					swapIndex = indexLeft;

					if (indexRight < Count)
					{
						// Check children priority
						if (m_elements[indexLeft].CompareTo(m_elements[indexRight]) < 0)
						{
							swapIndex = indexRight;
						}
					}

					// Check parent priority against children priority
					if (element.CompareTo(m_elements[swapIndex]) < 0)
					{
						Swap(element, m_elements[swapIndex]);
						continue;
					}
					return;
				}
				else
				{
					return;
				}
			}
		}

		private void Swap(T first, T second)
		{
			// Swap elements
			m_elements[first.index] = second;
			m_elements[second.index] = first;

			// Swap indices
			int tmp = first.index;
			first.index = second.index;
			second.index = tmp;
		}
	}
}