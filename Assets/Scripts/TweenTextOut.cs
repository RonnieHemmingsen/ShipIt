using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenTextOut : MonoBehaviour {

    [SerializeField]
    private GameObject _tweenObject;
    [SerializeField]
    private float _tweenTime;
    [SerializeField]
    private float _tweenScale;
    [SerializeField]
    private RectTransform _canvasRect;

    private ObjectPoolManager _objPool;
    private RectTransform _rectTransform;

    private Text _text;
    private Vector3 _objectSize;
    private Vector3 _spawnPosition;


	// Use this for initialization
	void Awake () 
    {
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _rectTransform = _tweenObject.GetComponent<RectTransform>();
	}
        

    void OnEnable()
    {
        _text = _tweenObject.GetComponent<Text>();
        _tweenObject.transform.localScale = Vector3.zero;
        EventManager.StartListeningForObjectEvent(EventStrings.TEXT_TWEEN, DiscoverSelf);
    }

    void OnDisable()
    {
        EventManager.StopListeningForObjectEvent(EventStrings.TEXT_TWEEN, DiscoverSelf);
    }

    private void DiscoverSelf(GameObject self)
    {
        SetDisplayPosition(self.transform.position);
        SetTextValue(self.tag);
        StartCoroutine(TweenOut());

    }

    private void SetDisplayPosition(Vector3 spawnPos)
    {
        //Convert from world 3d space, to UI 2d
        Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(spawnPos);
        Vector2 screenPos=new Vector2(
            ((ViewportPosition.x *_canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
            -((ViewportPosition.y *_canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));

        _rectTransform.anchoredPosition = screenPos;
    }

    private void SetTextValue(string text)
    {
        string whoAmI = "not found";
        switch (text)
        {
            case ObjectStrings.COIN:
                whoAmI = TextStrings.COIN_AQUIRED;
                break;
            case ObjectStrings.DESTROY_ALL:
                whoAmI = TextStrings.DESTROY_ALL_AQUIRED;
                break;
            case ObjectStrings.INVULNERABLE:
                whoAmI = TextStrings.INVULNERABILITY_AQUIRED;
                break;
            case ObjectStrings.LUDICROUS_SPEED:
                whoAmI = TextStrings.SPEED_AQUIRED;
                break;
            default:
                break;
        }
        _text.text = whoAmI;
    }

    private IEnumerator TweenOut()
    {
        float time = 0f;

        do {
            time += Time.deltaTime;
            _tweenObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * _tweenScale, time / _tweenTime);
            yield return new WaitForEndOfFrame();
                

        } while (time < _tweenTime);

        _objPool.ReturnObjectToPool(ObjectStrings.TWEEN_TEXT_OUT, gameObject);
    }


}
