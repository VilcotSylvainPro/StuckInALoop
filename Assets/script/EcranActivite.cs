using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EcranActivite : MonoBehaviour
{
    private Player PlayerScript;
    [SerializeField] private string[] MessageAffichage;
    [SerializeField] private GameObject MessageEcran;
    [SerializeField] private GameObject Music;
    private float attendre = 4;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.Find("Player").GetComponent<Player>();
    }


    private void OnEnable()
    {

        //Choisir aléatoirement le message à afficher et activé le bruitage


        int Rand = UnityEngine.Random.Range(0, MessageAffichage.Length);

        EcrireMessage(MessageAffichage[Rand]);
        Music.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        attendre = attendre - Time.deltaTime;

        if (attendre < 0)
        {
            //Enlever l'écran remette la music en inactif et attendre à sa valeur d'origine pour la prochaine itération
            attendre = 4;
            Debug.Log("Attendre = " + attendre);
            Music.SetActive (false);
            this.gameObject.SetActive(false);
            PlayerScript.SetCharacterController(true);   
        }
    }



    public void EcrireMessage(string message)
    {

        //Ecrire le message à afficher


        MessageEcran.GetComponent<TMP_Text>().text = message;
    }
}
