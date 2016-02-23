using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    [SerializeField]
    private float _scrollSpeed;

    private Renderer _rend;
    private Vector2 _savedOffset;

	// Use this for initialization
	void Start () {
        _rend = GetComponent<Renderer>();
        _savedOffset = _rend.sharedMaterial.GetTextureOffset("_MainTex");
	}
	
	// Update is called once per frame
	void Update () {
        float y = Mathf.Repeat(Time.time * _scrollSpeed, 1);
        Vector2 offset = new Vector2(0, y);
        _rend.sharedMaterial.SetTextureOffset("_MainTex", offset);
	
	}

    void OnDisable()
    {
        _rend.sharedMaterial.SetTextureOffset("_MainTex", _savedOffset);
    }
}
