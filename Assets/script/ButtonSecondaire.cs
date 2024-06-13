using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSecondaire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject BoutonASupprimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnClickButton()
    {

        //Supprimer le bouton

        Destroy(BoutonASupprimer);
    }

    public void OnClickApplicationEnd()
    {

        //Quitter le jeu

        Application.Quit();
    }


}
