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
        myTeam = s.team;
        teamDict[myTeam].Add(this);
        enabled = true;
    }
    Team myTeam;
    public int maxHealth;
    public int damage;
    public float speed;
    public float attackDistance;
    private int health;
    public int Health {
        get => health;
        set
        {
            health = Mathf.Min(maxHealth, value);
            print(health);
            if (health <= 0) Die();
        }
    }
    void Start()
    {
        
    }
    public void Die()
    {
        teamDict[myTeam].Remove(this);
        foreach(var t in targetors)
        {
            t.DefeatedEnemy();
        }
        Destroy(gameObject);
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

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (mode)
        {
            case Mode.searching:
                SearchEnemy();
                break;
            case Mode.attacking:
                Attack();
                break;
            case Mode.marching:
                if ((transform.position - target.transform.position).sqrMagnitude <= attackDistance * attackDistance) mode = Mode.attacking;
                else March();
                break;
        }
    }
    void Attack()
    {
        target.Health -= damage;
    }
    void March()
    {
        transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
    }
    void DefeatedEnemy()
    {
        target = null;
        mode = Mode.searching;
    }
}
