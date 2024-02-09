using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!TutorialController.Instance.isTutorialComplete)
        {
            //TODO: снять предохранитель
            if(SceneManager.GetActiveScene().name.Equals("Main_menu") && TutorialController.Instance.TutorialStep>2)
                TutorialController.Instance.ShowTutorial(25);
            else
                TutorialController.Instance.ShowTutorial();
        }
    }
    
}
