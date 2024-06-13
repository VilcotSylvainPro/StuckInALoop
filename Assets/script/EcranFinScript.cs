using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcranFinScript : MonoBehaviour
{

    [SerializeField] GameObject BoutonAfficher;

    private float attendre = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        attendre = attendre - Time.deltaTime;

        if (attendre < 0)
        {
            //Dernier bouton fin du jeu
            BoutonAfficher.SetActive(true);
        }
    }
}
