using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public int RowCount = 6;
    public int ColCount = 6;

    void Start()
    {
        //int x=0,y=0;
        ////Instantiate(prefab, new Vector3(Mathf.PerlinNoise(10,3), Mathf.PerlinNoise(1, 20), -1), Quaternion.identity);
        //for(int i = 0; i < RowCount; i++)
        //{
        //    for (int j = 0; j < ColCount; j++)
        //    {
        //        Instantiate(prefab, new Vector3(x,0,y), Quaternion.identity);
        //        x += 5;
        //    }
        //    x = 0;
        //    y += 5;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
