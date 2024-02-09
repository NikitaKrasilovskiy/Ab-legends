using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public bool isTutorialComplete = false;
    private const string TUTORIAL = "Tutorial";
    private const string TUTORIAL_STEP = "TutorialStep";
    public int TutorialStep { get; private set; }
    public static TutorialController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("TutorialController").AddComponent<TutorialController>();
            }

            return _instance;
        }
    }

    private static TutorialController _instance;
    private TutorialEngine _tutorialEngine;

    public TutorialEngine TutorialEngine
    {
        get
        {
            if (_tutorialEngine == null)
                _tutorialEngine = FindObjectOfType<TutorialEngine>();
            return _tutorialEngine;
        }
    }

    void Start()
    {
        _instance = this;
        isTutorialComplete = PlayerPrefs.GetInt(TUTORIAL, 0) > 0;
        TutorialStep = PlayerPrefs.GetInt(TUTORIAL_STEP, 0);
        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowTutorial()
    {
        SoundEngine.PlayEffect("click");
        TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
        BalanceAnalytics.TutorialStep(TutorialStep);
    }

    public void ShowTutorial(int i)
    {
        SoundEngine.PlayEffect("click");
        TutorialEngine.SetCurrentStep(i, NextStep);
    }

    async void NextStep()
    {
        SoundEngine.PlayEffect("click");
        //var eventsParams = new CustomEventParams();
        //eventsParams.AddParam("step_name", string.Format("{0:2}",TutorialStep));
        //DevToDev.Analytics.CustomEvent("tutorial", eventsParams);
        TutorialStep++;
        switch (TutorialStep)
        {
            case 1:
                {
                    DevToDev.Analytics.Tutorial(DevToDev.TutorialState.Start);
                    break;
                }
            case 3:
                {
                    FindObjectOfType<PanelManager>().OpenPanel("DECKBUILDER_PANEL");
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 7:
                {
                    var cardDataActivator = FindObjectsOfType<TutorialObj>().ToList().Find(x => x.id == 0);
                    cardDataActivator.onNext.Invoke();
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 8:
                {
                    var cardUpgrade = FindObjectsOfType<TutorialObj>().ToList().Find(x => x.id == 1);
                    cardUpgrade.onNext.Invoke();
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 10:
                {
                    var close = FindObjectsOfType<TutorialObj>().ToList().Find(x => x.id == 2);
                    close.onNext.Invoke();
                    await UniTask.Delay(500);
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 11:
                {
                    var cardDataActivator = FindObjectsOfType<TutorialObj>().ToList().Find(x => x.id == 3);
                    cardDataActivator.onNext.Invoke();
                    TutorialEngine.Hide();
                    break;
                }
            case 17:
                {
                    FindObjectOfType<PanelManager>().CloseCurentPanel();
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 18:
                {
                    TutorialEngine.Hide();
                    FindObjectOfType<PanelManager>().OpenPanel("AcornCamp");
                    await UniTask.WaitForEndOfFrame();
                    FindObjectOfType<CampsController>().ShowAcornCompany(true);
                    await UniTask.Delay(500);
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 19:
                {
                    await SoundEngine.PlayEffect("click");
                    BattleDataContainer.CompanyBattleData(Fraction.Acorn, 0);
                    SceneManager.LoadScene("Battle_Preloader");
                    break;
                }
            case 23:
                {
                    FindObjectOfType<BattleEngine>().Battle();
                    TutorialEngine.Hide();
                    break;
                }
            case 24:
                {
                    TutorialEngine.Hide();
                    break;
                }
            case 25:
                {
                    TutorialEngine.Hide();
                    break;
                }
            case 26:
                {
                    FindObjectOfType<PanelManager>().OpenPanel("Arena");
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 27:
                {
                    FindObjectOfType<PanelManager>().CloseCurentPanel();
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
            case 31:
                {
                    var prize = FindObjectsOfType<TutorialObj>().ToList().Find(x => x.id == 4);
                    prize.onNext.Invoke();
                    TutorialEngine.Hide();
                    break;
                }
            case 32:
                {
                    CompleteTutorial();
                    TutorialEngine.Hide();
                    //FB.LogAppEvent("Tutorial Complete");
                    DevToDev.Analytics.Tutorial(DevToDev.TutorialState.Finish);
                    break;
                }
            default:
                {
                    TutorialEngine.SetCurrentStep(TutorialStep, NextStep);
                    break;
                }
        }

    }

    public void CompleteTutorial()
    {
        isTutorialComplete = true;

        PlayerPrefs.SetInt(TUTORIAL, 1);
    }

}
