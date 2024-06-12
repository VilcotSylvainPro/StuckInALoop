using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TexteMessage : MonoBehaviour
{


    [SerializeField] private GameObject TextMessage;


    [SerializeField] private GameObject ObjectAbOUGER;

    public void Start()
    {

        float Randx = Random.RandomRange(500, 1400);
        float Randy = Random.RandomRange(300, 850);
       Bouger(Randx, Randy, 0);
    }



    public void EcrireMessage(string message)
    {
        TextMessage.GetComponent<TMP_Text>().text = message;
    }

    public void Bouger(float x,float y,float z)
    {
        ObjectAbOUGER.transform.position = new Vector3(x,y,z);
    }
}
