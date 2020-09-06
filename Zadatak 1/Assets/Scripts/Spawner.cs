using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public int StartAmount;

    public float SpawnTime;

    private int xPos;

    private bool spawn = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < StartAmount; i++)
        {
            SpawnObjects();
        }
        yield return new WaitForSeconds(SpawnTime);
        StartCoroutine(Spawn());
    }


    IEnumerator Spawn()
    {
        StopCoroutine(Init());
        while(spawn)
        {
            SpawnObjects();
            yield return new WaitForSeconds(SpawnTime);
        }
    }


    void SpawnObjects()
    {
        xPos = Random.Range(-5,5);
        GameObject obj = ObjectPoller.Instance.GetPulledObject();
        if(obj == null) return;
        obj.transform.position = new Vector3(xPos,1,1);
        obj.SetActive(true);
    }
   
}
