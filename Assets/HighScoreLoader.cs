using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;


public class HighScoreLoader : MonoBehaviour {

    [SerializeField]
    private int _numberOfEntries;
    [SerializeField]
    private float _yPadding;
    [SerializeField]
    private GameObject _entry;

    private Vector3 _deltaPos;
    private Vector2 _deltaPanel;
    private Rect _panelRect;
    private Rect _entryRect;
    private float _leaderboardHeight;
    private List<GameObject> _entries;
    private LeaderBoardEntry _thisEntry;

	// Use this for initialization
	void Start () {
        _deltaPos = Vector3.zero;
        _panelRect = GetComponent<RectTransform>().rect;
        _deltaPanel = new Vector2(_panelRect.width, _panelRect.height);
        _entryRect = _entry.GetComponent<RectTransform>().rect;
        _entries = new List<GameObject>();


	}

    public void Login()
    {
        DestroyEntries();
        GetLeaderBoard();
            
    }
	
    private void GetLeaderBoard()
    {
        new LeaderboardDataRequest().SetLeaderboardShortCode("HIGH_SCORE_LB").
        SetEntryCount(_numberOfEntries).
        Send((response) => {
        
            foreach (var entry in response.Data)
            {
                
                GameObject GO = Instantiate(_entry) as GameObject;
                _entries.Add(GO);
                _thisEntry = GO.GetComponent<LeaderBoardEntry>();

                _thisEntry.Name.text = entry.UserName.ToString();
                _thisEntry.Score.text = entry.GetNumberValue("SCORE_ATTR").ToString();
                GO.transform.SetParent(this.transform,false);

                GO.transform.position = this.transform.position + _deltaPos;
                print(GO.GetComponent<RectTransform>().rect.height);
                _deltaPos = _deltaPos + new Vector3(0, -(_entryRect.height + _yPadding), 0);
                _leaderboardHeight += GO.GetComponent<RectTransform>().rect.height;

                _deltaPanel = new Vector2(_panelRect.width, _leaderboardHeight + 10);

                this.GetComponent<RectTransform>().sizeDelta = _deltaPanel;
            }
           
        });
    }

    private void DestroyEntries()
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            Destroy(_entries[i]);
        }
        _entries.Clear();
    }

}
