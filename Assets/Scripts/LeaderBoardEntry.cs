using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderBoardEntry : MonoBehaviour {

    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _score;

    public Text Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Text Score
    {
        get { return _score; }
        set { _score = value; }
    }
}
