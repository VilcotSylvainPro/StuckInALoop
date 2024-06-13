using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementPNJ : MonoBehaviour
{

    [SerializeField] private GameObject[] Point;
    private int current = 0;
    private float RotationSpeed;
    [SerializeField] private float Speed;
    private float WPradius = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Point[current].transform.position, transform.position) < WPradius) 
        {
            current = Random.Range(0, Point.Length);
            if(current >= Point.Length) 
            {
                current = 0;
            }
        
        }
        transform.position = Vector3.MoveTowards(transform.position, Point[current].transform.position, Time.deltaTime * Speed);
    }
}
