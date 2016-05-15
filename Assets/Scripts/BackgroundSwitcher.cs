using UnityEngine;
using System.Collections;

public class BackgroundSwitcher : MonoBehaviour {

    [SerializeField]
    private float _switchTime;
    [SerializeField]
    private GameObject[] _tiles;


    private bool _hasSwitched;
    private int _switchToIndex;
    private int _switchFromIndex;
    private int _curIndex;
    private int _lastDisplayed;

	// Use this for initialization
	void Start () {
        _curIndex = 0;
        _switchFromIndex = 0;
        _switchToIndex = 2;
        _lastDisplayed = _switchToIndex;

        for (int i = 2; i < _tiles.Length; i++)
        {
            Renderer rend = _tiles[i].GetComponent<Renderer>();
            Color col = rend.material.color;
            col.a = 0;
            rend.material.color = col;

        }

        StartCoroutine(ChangeOpacity());
	}
	
	// Update is called once per frame
	void Update () {
	
        if(_hasSwitched)
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

        while (rendOne.material.color.a >= 0 && rendTwo.material.color.a >= 0) {
            //print(string.Format("ColOne"))
            colOne.a -= _switchTime;
            colTwo.a -= _switchTime;
            colThree.a += _switchTime;
            colFour.a += _switchTime;

            rendOne.material.color = colOne;
            rendTwo.material.color = colTwo;
            rendThree.material.color = colThree;
            rendFour.material.color = colFour;
            yield return new WaitForSeconds(1f);
        }
        _curIndex += 2;
        _lastDisplayed = _switchToIndex;
        _hasSwitched = true;

    }
}
