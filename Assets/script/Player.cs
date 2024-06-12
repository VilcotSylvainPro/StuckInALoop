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
    [SerializeField] private GameObject Maison;
    [SerializeField] private GameObject Rue;
    [SerializeField] private GameObject Office;


    [SerializeField] private string[] MessageAEcrireBonMoral;
    [SerializeField] private string[] MessageAEcrireMoyenMoral;
    [SerializeField] private string[] MessageAEcrireBadMoral;

    private int Moral = 4;

    [SerializeField] private GameObject MessageSecondaire;
    [SerializeField] private GameObject InterractionMobilier;
    [SerializeField] private GameObject InteractionEcrireMobilier;
    [SerializeField] private GameObject MessageFinal;
    private Transform finalBoutonTransform;

    private GameObject canvas;
    private float TempsAleatoire;
    private bool repos = false;
    private bool travail = true;
    private string LastObjectCollideName;

    private bool InterractionGameplay;
    InputAction moving;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        finalBoutonTransform = MessageFinal.transform;

        TempsAleatoire = Random.RandomRange(5, 15);
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
            Maison.SetActive(false);
            Rue.SetActive(true);

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
            Gresillement.volume += 0.1f;
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
        }


        if (LastObjectCollideName == "Regarder TV")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai pas vraiment envie de regarder la télé.";

        }

        if (LastObjectCollideName == "Manger")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai pas vraiment envie de manger actuellement.";

        }

        if (LastObjectCollideName == "Asseoir")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai pas vraiment envie de m'asseoir devant la TV, prochaine fois.";
        }

        if (LastObjectCollideName == "Bureau 1")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ce n'est pas mon bureau ici...";
        }

        if (LastObjectCollideName == "Bureau 2")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ce n'est pas mon bureau ici...";
        }

        if (LastObjectCollideName == "Bureau 3")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ce n'est pas mon bureau ici...";
        }

        if (LastObjectCollideName == "Bureau 4")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ce n'est pas mon bureau ici...";
        }

        if (LastObjectCollideName == "Bureau 5")
        {
            InterractionMobilier.SetActive(true);
            InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ce n'est pas mon bureau ici...";
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
       GameObject BoutonASpawn = Instantiate(MessageSecondaire);
        BoutonASpawn.transform.parent = (canvas.transform);
        BoutonASpawn.transform.SetSiblingIndex(finalBoutonTransform.GetSiblingIndex()-1);

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
                TempsAleatoire = Random.RandomRange(5, 10);

            }
            if (Moral >= -5 && Moral < -1)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireMoyenMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireMoyenMoral[Rand]);
                TempsAleatoire = Random.RandomRange(5, 10);
                Debug.Log(TempsAleatoire);

            }
            if (Moral >= -10 && Moral < -6)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);
                TempsAleatoire = Random.RandomRange(5, 5);
                Debug.Log(TempsAleatoire);

            }
            if ( Moral <= -12)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);
                //TempsAleatoire = Random.RandomRange(1, 1);
                MessageFinal.SetActive(true);
                Debug.Log(TempsAleatoire);

            }




        }
    }


}
