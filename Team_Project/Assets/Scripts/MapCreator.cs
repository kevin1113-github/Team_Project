using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public float width = 20f;
    public float height = 10f;
    public int room_count = 5;
    public GameObject room;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < room_count; i++) {
            Instantiate(room, new Vector3(Random.Range(-width/2, width/2), 0, Random.Range(-height/2, height/2)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
