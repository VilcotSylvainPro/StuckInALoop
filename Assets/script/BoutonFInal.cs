using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonFInal : MonoBehaviour
{


    [SerializeField] private GameObject FinalBouton;
    private GameObject canvas;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        
    }

    private void OnEnable()
    {
        
        canvas.GetComponent<Canvas>().sortingOrder = 10000000;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
