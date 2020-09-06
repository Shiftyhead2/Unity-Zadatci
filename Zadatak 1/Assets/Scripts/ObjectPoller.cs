using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoller : MonoBehaviour
{
    public static ObjectPoller Instance;
    public GameObject PolledObject;

    public int PolledAmount;

    public bool WillGrow;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        Instance = this;
    }
     private void Start() 
     {
         pooledObjects = new List<GameObject>();
        for(int i = 0; i < PolledAmount; i++)
        {
            GameObject obj = Instantiate(PolledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
     }

     public GameObject GetPulledObject()
     {
         for(int i = 0; i < pooledObjects.Count; i++)
         {
             if(!pooledObjects[i].activeInHierarchy)
             {
                 return pooledObjects[i];
             }
         }

        if(WillGrow)
        {
            GameObject obj = Instantiate(PolledObject);
            pooledObjects.Add(obj);
            return obj;
        }

         return null;
     }
}
