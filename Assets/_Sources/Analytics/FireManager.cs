using UnityEngine;

public class FireManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //{
        //    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        //});

        DontDestroyOnLoad(this);
    }
}
