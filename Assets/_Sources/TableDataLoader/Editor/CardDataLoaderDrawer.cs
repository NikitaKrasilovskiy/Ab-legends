using System.Collections;
using System.Collections.Generic;
using _Sources.TableDataLoader;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardDataLoader))]
public class CardDataLoaderDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var component = (CardDataLoader) target;

        if (GUILayout.Button("Sync"))
        {
            component.Sync();
        }
    }
}
