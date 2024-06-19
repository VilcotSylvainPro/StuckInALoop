using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        BoutonASupprimer.SetActive(false);
    }

    public void OnClickApplicationEnd()
    {

        //Relance le jeu. En acord avec le thème, il n'est pas possible (in game) de quitter le jeu

        SceneManager.LoadScene("SampleScene");
    }


}
