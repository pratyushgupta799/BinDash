using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject[] _spawners;
    [SerializeField] private GameObject trash1;
    [SerializeField] private GameObject trash2;
    
    private float _timeBetweenSpawns;
    
    [SerializeField] private float startTimeBetweenSpawns;
    [SerializeField] private float minTimeBetweenSpawns;
    [SerializeField] private float decreaseTimeBetweenSpawns;
    void Awake()
    {
        int childCount = transform.childCount;
        _spawners = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            _spawners[i] = transform.GetChild(i).gameObject;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int rand = Random.Range(0, _spawners.Length);
        GameObject curSpawner = _spawners[rand];
        if (_timeBetweenSpawns <= 0)
        {
            int rand2 = Random.Range(0, 2);
            if (rand2 == 0)
            {
                Instantiate(trash1, curSpawner.transform.position, Quaternion.identity);
                _timeBetweenSpawns = startTimeBetweenSpawns;
            }
            else
            {
                Instantiate(trash2, curSpawner.transform.position, Quaternion.identity);
                _timeBetweenSpawns = startTimeBetweenSpawns;
            }
        }
        else
        {
            _timeBetweenSpawns -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (startTimeBetweenSpawns > minTimeBetweenSpawns)
        {
            startTimeBetweenSpawns -= decreaseTimeBetweenSpawns;
        }
    }
}
