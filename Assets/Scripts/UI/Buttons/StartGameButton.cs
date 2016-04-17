using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour {

    [SerializeField]
    private GameObject _menu;

    private Button _me;
    private Animator _anim;
    // Use this for initialization

    void Awake()
    {
        _me = GetComponent<Button>();
        _anim = GetComponentInParent<Animator>();
    }

    void Start () 
    {
        _me.onClick.AddListener(delegate {
            StartCoroutine(StartGameSequence());
        });
	}

    private IEnumerator StartGameSequence()
    { 
        print("Press Play On Tape");
        float time = 0;

        _anim.SetTrigger(AnimatorStrings.SLIDE_SIDE_MENU_OUT);
        _anim.SetTrigger(AnimatorStrings.SLIDE_LINES_OUT);


        while (time <= 1.5f)
        {
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        EventManager.TriggerEvent(GameSettings.START_GAME);
    }


	

}
