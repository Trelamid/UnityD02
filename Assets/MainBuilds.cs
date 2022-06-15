using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilds : Unit
{
    [SerializeField]private List<GameObject> secondsBuilds;
    [SerializeField] private GameObject _warrior;

    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _time)
        {
            _time += 10;
            for (int i = 0; i < 4; i++)
            {
                if(!secondsBuilds[i])
                    _time += 2.5f;
            }
            // foreach (var VARIABLE in secondsBuilds)
            // {
            //     if (!VARIABLE.activeSelf)
            //         _time += 2.5f;
            // }
            // Debug.Log(_time);
            SpawnWarrior();
        }
    }

    void SpawnWarrior()
    {
        Instantiate(_warrior, transform.position, Quaternion.identity);
    }
}
