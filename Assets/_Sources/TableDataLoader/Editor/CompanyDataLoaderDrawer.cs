using System.Collections;
using System.Collections.Generic;
using _Sources.TableDataLoader;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CompanyLoader))]
public class CompanyDataLoaderDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var component = (CompanyLoader) target;

        if (GUILayout.Button("Sync"))
        {
            component.Sync();
        }
    }
}
