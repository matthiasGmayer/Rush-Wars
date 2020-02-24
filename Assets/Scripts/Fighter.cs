using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Fighter: MonoBehaviour
{
    // Start is called before the first frame update
    private static readonly Dictionary<Team, List<Fighter>> teamDict = new Dictionary<Team,List<Fighter>>();
    static Fighter()
    {
        foreach(Team t in Teams)
        {
            teamDict.Add(t, new List<Fighter>());
        }
    }
    private static Team[] Teams
    {
        get => (Team[]) Enum.GetValues(typeof(Team));
    }
    private enum Mode
    {
        attacking,
        searching,
        marching
    }
    public void Init(Spawner s)
    {
        iF = GetComponentInParent<IFighter>();
        myTeam = s.team;
        teamDict[myTeam].Add(this);
        health = iF.maxHealth;
        float x = Time.realtimeSinceStartup, y = Time.realtimeSinceStartup * 2;
        iF.transform.position = s.transform.position + new Vector3(Mathf.PerlinNoise(x,y)-0.5f, Mathf.PerlinNoise(y,x)-0.5f);
    }
    private IFighter iF;
    Team myTeam;
    public int health;
    public int Health {
        get => health;
        set
        {
            health = Mathf.Min(iF.maxHealth, value);
            if (health <= 0) Die();
            healthbar.SetState((float)health / iF.maxHealth);
            SearchEnemy();
        }
    }
    private Rigidbody2D myRigidbody;
    public Healthbar healthbar;
    void Start()
    {
        iF = GetComponentInParent<IFighter>();
        myRigidbody = GetComponentInParent<Rigidbody2D>();
    }
    public void Die()
    {
        teamDict[myTeam].Remove(this);
        foreach(var t in targetors)
        {
            t.DefeatedEnemy();
        }
        Destroy(transform.parent.gameObject);
    }

    private Fighter target;
    private List<Fighter> targetors = new List<Fighter>();
    private Mode mode = Mode.searching;
    void SearchEnemy()
    {
        foreach(Team t in Teams)
        {
            if (t == myTeam || !teamDict[t].Any()) continue;
            Fighter targ = teamDict[t].OrderBy(f => (f.transform.position - transform.position).sqrMagnitude).First();
            target?.targetors.Remove(this);
            target = targ;
            target?.targetors.Add(this);
            mode = Mode.marching;
        }
    }

    float elapsed= 0;
    // Update is called once per frame
    void FixedUpdate()
    {
            switch (mode)
            {
                case Mode.searching:
                    SearchEnemy();
                    break;
                case Mode.attacking:
                if ((transform.position - target.transform.position).sqrMagnitude > iF.attackDistance* iF.attackDistance) mode = Mode.searching;
                elapsed += Time.deltaTime;
                if (elapsed >= iF.attackTime)
                {
                    Attack(target);
                    elapsed -= iF.attackTime;
                }
                    break;
                case Mode.marching:
                    if ((transform.position - target.transform.position).sqrMagnitude <= iF.attackDistance * iF.attackDistance) mode = Mode.attacking;
                    else March();
                    break;
        }
    }
    public Action<Fighter> Attack;
    void March()
    {
        myRigidbody.MovePosition(iF.transform.position + (target.iF.transform.position - iF.transform.position).normalized * iF.speed * Time.deltaTime);
    }
    void DefeatedEnemy()
    {
        target = null;
        mode = Mode.searching;
    }
}
