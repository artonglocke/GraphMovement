using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GraphSystem;

[CustomEditor(typeof(GridSystem))]
public class EditorGridSystem : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GridSystem grid = target as GridSystem;

		if (GUILayout.Button("Rebuild Grid"))
		{
			grid.InitializeGrid();
		}
	}
}
