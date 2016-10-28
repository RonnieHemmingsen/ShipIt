using UnityEngine;
using System.Collections;

public class BackgroundSwitcher : MonoBehaviour {

    [SerializeField]
    private float _switchTime;
    [SerializeField]
    private float _alphaIncrement;
    [SerializeField]
    private GameObject[] _tiles;

    private bool _isCycling;
    private bool _hasSwitched;
    private int _switchToIndex;
    private int _switchFromIndex;
    private int _curIndex;
    private int _lastDisplayed;

	// Use this for initialization
	void Start () {
        ResetRenderers();
	}

    private void ResetRenderers()
    {
        _curIndex = 0;
        _switchFromIndex = 0;
        _switchToIndex = 2;
        _lastDisplayed = _switchToIndex;

        Renderer r0 = _tiles[0].GetComponent<Renderer>();
        Renderer r1 = _tiles[1].GetComponent<Renderer>();
        Color c = r0.material.color;

        c.a = 1;
        r0.material.color = c;
        r1.material.color = c;

        for (int i = 2; i < _tiles.Length; i++)
        {
            Renderer rend = _tiles[i].GetComponent<Renderer>();
            Color col = rend.material.color;
            col.a = 0;
            rend.material.color = col;
        }
    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.START_GAME, StartCycle);
        EventManager.StartListening(GameSettings.GAME_OVER, StopCycle);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.START_GAME, StartCycle);
        EventManager.StopListening(GameSettings.GAME_OVER, StopCycle);
    }

    private void StartCycle()
    {
        ResetRenderers();
        _isCycling = true;
        StartCoroutine(ChangeOpacity());
    }

    private void StopCycle()
    {
        ResetRenderers();
        _isCycling = false;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(_hasSwitched && _isCycling)
        {
            _hasSwitched = false;
            if(_curIndex <= _tiles.Length - 3)
            {
                _switchFromIndex = _curIndex;
                _switchToIndex = _curIndex + 2;    
                StartCoroutine(ChangeOpacity());
            }

            else
            {
                _curIndex = 0;
                _switchFromIndex = _lastDisplayed;
                _switchToIndex = _curIndex;
                if(_lastDisplayed == 0)
                {
                    _switchToIndex = _curIndex + 2;    
                }

                StartCoroutine(ChangeOpacity());
            }

        }
	}

    private Renderer GetRenderer(int index) 
    {
        return _tiles[index].GetComponent<Renderer>();

    }

    private Color GetColor(Renderer rend)
    {
        return rend.material.color;
    }

    private IEnumerator ChangeOpacity()
    {
        Renderer rendOne = GetRenderer(_switchFromIndex);
        Renderer rendTwo = GetRenderer(_switchFromIndex +1);
        Renderer rendThree = GetRenderer(_switchToIndex);
        Renderer rendFour = GetRenderer(_switchToIndex +1);

        Color colOne = GetColor(rendOne);
        Color colTwo = GetColor(rendTwo);
        Color colThree = GetColor(rendThree);
        Color colFour = GetColor(rendFour);

        colOne.a = 1;
        colTwo.a = 1;
        colThree.a = 0;
        colFour.a = 0;

        while (rendOne.material.color.a >= 0 && rendTwo.material.color.a >= 0 && _isCycling) {
            //print(string.Format("ColOne"))
            colOne.a -= _alphaIncrement;
            colTwo.a -= _alphaIncrement;
            colThree.a += _alphaIncrement;
            colFour.a += _alphaIncrement;

            rendOne.material.color = colOne;
            rendTwo.material.color = colTwo;
            rendThree.material.color = colThree;
            rendFour.material.color = colFour;
            yield return new WaitForSeconds(_switchTime);
        }
        _curIndex += 2;
        _lastDisplayed = _switchToIndex;
        _hasSwitched = true;

    }
}
