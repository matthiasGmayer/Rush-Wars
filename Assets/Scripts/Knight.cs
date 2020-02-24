using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    Fighter fighter;
    IFighter iFighter;
    // Start is called before the first frame update
    void Start()
    {
        fighter = GetComponentInChildren<Fighter>();
        iFighter = GetComponent<IFighter>();
        fighter.Attack = target => target.Health -= iFighter.damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
