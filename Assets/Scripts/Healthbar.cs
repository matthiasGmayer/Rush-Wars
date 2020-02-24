using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public List<Sprite> states;

    private SpriteRenderer srenderer;
    // Start is called before the first frame update
    void Start()
    {
        length = states.Count;
        srenderer = GetComponent<SpriteRenderer>();
    }
    private int length;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState(float s)
    {
        srenderer.sprite = states[Mathf.Min((int)((1-s)*length), length-1)];
    }
}
