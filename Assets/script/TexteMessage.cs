using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TexteMessage : MonoBehaviour
{


    [SerializeField] private GameObject TextMessage;


    [SerializeField] private GameObject ObjectAbOUGER;

    private float TimerDespawn = 0f;
    [SerializeField] private float TimeToDespawn = 5f;

    public void Start()
    {

        //Afficher à un endroit aléatoire entre les bornes x et y

        float Randx = Random.RandomRange(500, 1400);
        float Randy = Random.RandomRange(300, 850);
       Bouger(Randx, Randy, 0);
    }

    void Update()
    {
        if (this.isActiveAndEnabled)
        {
            TimerDespawn += Time.deltaTime;
            if (TimerDespawn > TimeToDespawn)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void EcrireMessage(string message)
    {

        //Le message à écrire

        TextMessage.GetComponent<TMP_Text>().text = message;
    }

    public void Bouger(float x,float y,float z)
    {

        //Placer le message

        ObjectAbOUGER.transform.position = new Vector3(x,y,z);
    }
}
