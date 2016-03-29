using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour {

    [SerializeField]
    private GameObject _menu;

    private LevelManager _lvlMan;
    private Button _me;
    private Animator _anim;
    // Use this for initialization

    void Awake()
    {
        _lvlMan = FindObjectOfType<LevelManager>();
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
        float time = 0;

        _anim.SetTrigger("BottomMenuSlideOut");

        while (time <= _anim.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        } 

        EventManager.TriggerEvent(GameSettings.UPDATE_ACTIVE_SCENE);
        EventManager.TriggerEvent(GameSettings.START_GAME);
        _menu.SetActive(false);
    }
	

}
