using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;


public class HighScoreLoader : MonoBehaviour {

    [SerializeField]
    private int _numberOfEntries;
    [SerializeField]
    private GameObject _entry;
    [SerializeField]
    private bool _isAroundMeBoard;
    [SerializeField]
    private bool _isSocialLeaderBoard;

    private List<GameObject> _entries;
    private LeaderBoardEntry _thisEntry;

	// Use this for initialization
	void Start () {
        _entries = new List<GameObject>();
	}

    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.UPDATE_LEADERBOARDS, UpdateLeaderboards);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.UPDATE_LEADERBOARDS, UpdateLeaderboards);
    }
        

    private void UpdateLeaderboards()
    {
        print("Updating Leaderboards");
        DestroyEntries();
        if(!_isSocialLeaderBoard)
        {
            if(_isAroundMeBoard)
            {
                GetAroundMeLeaderBoard();
            }
            else
            {
                GetLeaderBoard();    
            }
        }

        if(_isSocialLeaderBoard)
        {
            GetSocialLeaderBoard();
        }

            
    }

    private void GetSocialLeaderBoard()
    {
        new SocialLeaderboardDataRequest().SetLeaderboardShortCode(BackendVariables.HIGHSCORE_BOARD).
        SetEntryCount(_numberOfEntries).
        SetDontErrorOnNotSocial(true).
        Send((response) => {
            foreach (var entry in response.Data) {
                GameObject GO = Instantiate(_entry) as GameObject;
                _entries.Add(GO);
                _thisEntry = GO.GetComponent<LeaderBoardEntry>();

                _thisEntry.Rank.text = entry.Rank.ToString();
                _thisEntry.Name.text = entry.UserName.ToString();
                _thisEntry.Score.text = entry.GetNumberValue(BackendVariables.TRAVELSCORE_ATTRIBUTE).ToString();
                GO.transform.SetParent(this.transform, false);
            }
        });

    }
	
    private void GetAroundMeLeaderBoard()
    {
        new AroundMeLeaderboardRequest_HIGH_SCORE_LB().
        SetEntryCount(_numberOfEntries).
        Send((response) => {
            foreach (var entry in response.Data) {
                GameObject GO = Instantiate(_entry) as GameObject;
                _entries.Add(GO);
                _thisEntry = GO.GetComponent<LeaderBoardEntry>();

                _thisEntry.Rank.text = entry.Rank.ToString();
                _thisEntry.Name.text = entry.UserName.ToString();
                _thisEntry.Score.text = entry.GetNumberValue(BackendVariables.TRAVELSCORE_ATTRIBUTE).ToString();
                GO.transform.SetParent(this.transform, false);
            }
        });
    }

    private void GetLeaderBoard()
    {
        new LeaderboardDataRequest().SetLeaderboardShortCode(BackendVariables.HIGHSCORE_BOARD).
        SetEntryCount(_numberOfEntries).
        Send((response) => {
            

            foreach (var entry in response.Data)
            {
                GameObject GO = Instantiate(_entry) as GameObject;
                _entries.Add(GO);
                _thisEntry = GO.GetComponent<LeaderBoardEntry>();

                _thisEntry.Rank.text = entry.Rank.ToString();
                _thisEntry.Name.text = entry.UserName.ToString();
                _thisEntry.Score.text = entry.GetNumberValue(BackendVariables.TRAVELSCORE_ATTRIBUTE).ToString();
                GO.transform.SetParent(this.transform, false);
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
