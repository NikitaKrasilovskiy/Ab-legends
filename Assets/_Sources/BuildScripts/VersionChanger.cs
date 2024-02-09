#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

/*
public class VersionChanger : IPreprocessBuildWithReport
{

    public int callbackOrder { get; }
    public void OnPreprocessBuild(BuildReport report)
    {
        var lastCode = PlayerSettings.Android.bundleVersionCode;
        lastCode++;
        string version = string.Format("{0}.{1}.{2}",(lastCode / 10000),(lastCode % 10000 / 100),(lastCode % 10000%100));
        PlayerSettings.bundleVersion = version;
        PlayerSettings.Android.bundleVersionCode = lastCode;
        PlayerSettings.iOS.buildNumber = lastCode.ToString();
    }

}
*/
#endif