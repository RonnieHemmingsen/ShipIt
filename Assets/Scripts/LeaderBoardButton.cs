using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderBoardButton : MonoBehaviour {

    private Button _me;
    private Animator _anim;

	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>();
        _anim = GetComponentInParent<Animator>();
	}

    void Start()
    {
        _me.onClick.AddListener(delegate {
            StartCoroutine(ChangePlaces());
        });
    }
	
    private IEnumerator ChangePlaces()
    {
        _anim.SetTrigger(AnimatorStrings.SLIDE_SIDE_MENU_OUT);

        yield return new WaitForSeconds(0.7f);

        _anim.SetTrigger(AnimatorStrings.SLIDE_TOP_MENU_IN);
    }

}
