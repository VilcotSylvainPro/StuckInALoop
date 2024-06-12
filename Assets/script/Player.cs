using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{


    PlayerInput playerInput;
    [SerializeField] private GameObject Objectif;
    [SerializeField] private GameObject InterfaceInteraction;
    [SerializeField] private GameObject InteractionEcrire;
    [SerializeField] private GameObject SortirMaison;
    [SerializeField] private GameObject MaisonExterieur;
    [SerializeField] private GameObject TravailExterieur;
    [SerializeField] private GameObject TravailInterieur;
    [SerializeField] private GameObject BureauPlayer;
    [SerializeField] private GameObject TravailSortie;
    [SerializeField] private GameObject LitPlayer;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource Gresillement;


    [SerializeField] private string[] MessageAEcrireBonMoral;
    [SerializeField] private string[] MessageAEcrireMoyenMoral;
    [SerializeField] private string[] MessageAEcrireBadMoral;

    private int Moral = 0;

    [SerializeField] private GameObject MessageSecondaire;

    private float TempsAleatoire;
    private bool repos = false;
    private bool travail = true;
    private string LastObjectCollideName;

    private bool InterractionGameplay;
    InputAction moving;

    // Start is called before the first frame update
    void Start()
    {
        TempsAleatoire = Random.RandomRange(5, 6);
        Debug.Log(TempsAleatoire);
        playerInput = GetComponent<PlayerInput>();
        moving = playerInput.actions.FindAction("Move");
        InterfaceInteraction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movingPlayer();
        ObjectifFonction();
        AffichageMessageRandom();

    }


    public void movingPlayer()
    {
        Vector2 direction = moving.ReadValue<Vector2>();
        transform.position = transform.position + new Vector3(direction.x,0,direction.y) * speed * Time.deltaTime;
    }




    public void Navigation()
    {
        if(LastObjectCollideName == "Sortir Maison")
        {
            this.gameObject.transform.position = new Vector3(MaisonExterieur.transform.position.x, 1.6f, MaisonExterieur.transform.position.z);
        }

        if (LastObjectCollideName == "Entrer Maison")
        {
            this.gameObject.transform.position = new Vector3(-295.24f, 1.6f, 14.67f);
        }

        if (LastObjectCollideName == "Entrer Travail")
        {
            this.gameObject.transform.position = new Vector3(TravailInterieur.transform.position.x, 1.6f, TravailInterieur.transform.position.z);
        }



        if (LastObjectCollideName == "Bureau 6")
        {
            travail = false;
            repos = true;
            Gresillement.volume += 1;
            Moral = Moral - 2;
        }

        if (LastObjectCollideName == "Sortir Travail")
        {
            this.gameObject.transform.position = new Vector3(TravailExterieur.transform.position.x, 1.6f, TravailExterieur.transform.position.z);
        }

        if (LastObjectCollideName == "Dormir")
        {
            travail = true;
            repos = false;
            Gresillement.volume += 1;
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {
            Debug.Log("Interraction");
            InterfaceInteraction.SetActive(true);
            SetLastCollideObjectName(other.name);
            InteractionEcrire.GetComponent<TMP_Text>().text = other.name;
            InterractionGameplay = true;
            


            //InteractionEcrire.GetComponent<TMP_Text>().text = "test";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {

            Debug.Log("Interraction Continu");
            InterractionGameplay = true;
           /*if (other.gameObject == SortirMaison && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E appuyé");
                this.gameObject.transform.position = MaisonExterieur.transform.position;
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {

            Debug.Log("Plus Interraction");
            InterfaceInteraction.SetActive(false);
            InterractionGameplay = false;
        }
    }

    public void SetLastCollideObjectName(string name)
    {
        LastObjectCollideName = name;
        Debug.Log(LastObjectCollideName);
    }


    public string ReturnNomInteraction(string value)
    {
        return value;
    }


    public void ObjectifFonction()
    {
        if(travail == true)
        {
            Objectif.GetComponent<TMP_Text>().text = "Travail";
        }
        else if (repos == true)
        {
            Objectif.GetComponent<TMP_Text>().text = "Repos";
        }
    }



    public void SpawnCommentaireSecondaire()
    {
        Instantiate(MessageSecondaire);

    }


    public void AffichageMessageRandom()
    {
        TempsAleatoire = TempsAleatoire - Time.deltaTime;

        if( TempsAleatoire < 0 )
        {
            SpawnCommentaireSecondaire();

            if (Moral >= 0)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBonMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBonMoral[Rand]);

            }
            if (Moral >= -5 && Moral < -1)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireMoyenMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireMoyenMoral[Rand]);

            }
            if (Moral >= -10 && Moral < -6)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);

            }

            TempsAleatoire = Random.RandomRange(5, 15);
            Debug.Log(TempsAleatoire);

        }
    }


}
