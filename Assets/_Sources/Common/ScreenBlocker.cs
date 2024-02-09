using System;
using UnityEngine;

namespace _Sources.Common
{
    public class ScreenBlocker : MonoBehaviour
    {
        private static GameObject _blocker;
        [SerializeField] private GameObject blockerGo;

        private void Awake()
        {
            _blocker = blockerGo;
        }

        public static void BlockScreen(bool enable)
        {
            if(_blocker)
                _blocker.SetActive(enable);
        }
    }
}
