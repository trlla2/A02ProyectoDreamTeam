using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingSquares))]
public class GenerateEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		MarchingSquares marching = (MarchingSquares)target;
		if (GUILayout.Button("GENERATE MAP")) 
		{
			marching.UpdateGrid();
		}
	}
}