using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MetaGameLoader))]
public class MetaGameLoaderDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var component = (MetaGameLoader) target;

        if (GUILayout.Button("Sync"))
        {
            component.Sync();
        }
    }
}
