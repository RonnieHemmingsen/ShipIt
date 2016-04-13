using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositionTest : MonoBehaviour {

    private RectTransform _thisRect;

	void Awake () {
        _thisRect = GetComponent<RectTransform>();
	}
	
    public void ChangePosition()
    {
        print("Ive changed, I swear!");
        Vector2 v2 = new Vector2(-273, -65);
        _thisRect.anchoredPosition = v2;
    }




}
