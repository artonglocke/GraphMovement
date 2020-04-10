using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Element agnostic implementation with priority
// Priority Compares elements and returns -1, 0, 1
public interface IHeapElement<T> : IComparable<T>
{
	int index
	{
		get;
		set;
	}
}
