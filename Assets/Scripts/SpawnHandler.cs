using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum Team
{
    Left,
    Right
}
public class SpawnHandler : MonoBehaviour
{
    public List<GameObject> spawnerObjects;
    public IEnumerable<Spawner> spawners;

    public GameObject toSpawn;

    public bool isSpawning;
    public bool spawnAction;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawners = spawnerObjects.Select(o => o.GetComponentInChildren<Spawner>());
        elapsed = spawnTime;
    }

    private float elapsed;
    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= spawnTime)
        {
            if (isSpawning || spawnAction)
            {
                spawnAction = false;
                elapsed -= spawnTime;
                foreach(var s in spawners)
                {
                    Spawn(s);
                }
            }
        }
        
    }
    void Spawn(Spawner s)
    {

        var o = Instantiate(s.toSpawn, null);
        o.GetComponentInChildren<Fighter>().Init(s);
    }
}
