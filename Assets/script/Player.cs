using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Security.Cryptography;

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
    [SerializeField] private AudioSource Horloge;
    [SerializeField] private GameObject Maison;
    [SerializeField] private GameObject Rue;
    [SerializeField] private GameObject Office;


    [SerializeField] private string[] DialogueBonMoral;
    [SerializeField] private string[] DialogueMoyenMoral;


    [SerializeField] private string[] MessageAEcrireBonMoral;
    [SerializeField] private string[] MessageAEcrireMoyenMoral;
    [SerializeField] private string[] MessageAEcrireBadMoral;

    [SerializeField] private int Depression = 4;

    [SerializeField] private GameObject MessageSecondaire;
    [SerializeField] private GameObject InterractionMobilier;
    [SerializeField] private GameObject InteractionEcrireMobilier;
    [SerializeField] private GameObject MessageFinal;
    [SerializeField] private GameObject BoutonFinal;

    [SerializeField] private GameObject EcranBoulot;
    [SerializeField] private GameObject EcranDodo;

    [SerializeField] private AudioSource Bruitage;
    [SerializeField] private AudioClip Asenceur;
    [SerializeField] private AudioClip SortieBureau;
    [SerializeField] private AudioClip SortieMaison;
    [SerializeField] private AudioClip EntreeMaison;

    [SerializeField] private GameObject[] GroupAPersonnage;
    [SerializeField] private GameObject[] GroupBPersonnage;
    private Transform finalBoutonTransform;

    private GameObject canvas;
    private float TempsAleatoire;
    private bool repos = false;
    private bool travail = true;
    private string LastObjectCollideName;

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
        LookPlayerObjectA();
       

    }


    public void movingPlayer()
    {
        Vector2 direction = moving.ReadValue<Vector2>();
        transform.position = transform.position + new Vector3(direction.x,0,direction.y) * speed * Time.deltaTime;
    }




    public void Navigation()
    {

        //Pour voyager d'une pièce à l'autre du jeu


        if(LastObjectCollideName == "Sortir Maison")
        {
            this.gameObject.transform.position = new Vector3(MaisonExterieur.transform.position.x, 1.6f, MaisonExterieur.transform.position.z);
            Maison.SetActive(false);
            Rue.SetActive(true);
            Bruitage.PlayOneShot(EntreeMaison);

        }

        if (LastObjectCollideName == "Entrer Maison")
        {
            this.gameObject.transform.position = new Vector3(-295.24f, 1.6f, 14.67f);
            Maison.SetActive(true);
            Rue.SetActive(false);
            Bruitage.PlayOneShot(SortieMaison);
        }

        if (LastObjectCollideName == "Entrer Travail")
        {
            this.gameObject.transform.position = new Vector3(TravailInterieur.transform.position.x, 1.6f, TravailInterieur.transform.position.z);
            Office.SetActive(true);
            Rue.SetActive(false);
            Bruitage.PlayOneShot(Asenceur);
        }

        if (LastObjectCollideName == "Bureau 6")
        {
            travail = false;
            repos = true;
            EcranBoulot.SetActive(true);

            //Ici moral baisse et l'ambiance sonore augmente en volume graduellement

            Gresillement.volume += 0.2f;
            CameraShake.Instance.ShakeCamera(0.1f);
            Horloge.volume += 0.2f;
            Depression = Depression + 2;
        }

        if (LastObjectCollideName == "Sortir Travail")
        {
            if( travail == false)
            {
                this.gameObject.transform.position = new Vector3(TravailExterieur.transform.position.x, 1.6f, TravailExterieur.transform.position.z);
                Office.SetActive(false);
                Rue.SetActive(true);
                Bruitage.PlayOneShot(SortieBureau);
            }
            else
            {
                InterractionMobilier.SetActive(true);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai du travail...";
            }

        }

        if (LastObjectCollideName == "Dormir")
        {
            if(repos == true)
            {
                travail = true;
                repos = false;
                EcranDodo.SetActive(true);
            }
            else
            {
                InterractionMobilier.SetActive(true);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai pas vraiment sommeil.";
            }

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

         
        if (LastObjectCollideName == "Inconnu")
        {
            if(Depression < 4)
            {
                InterractionMobilier.SetActive(true);

                int Rand = UnityEngine.Random.Range(0, DialogueBonMoral.Length);

                string MessageTempo = EcrireMessageIci(DialogueBonMoral[Rand]);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = MessageTempo;

                //InteractionEcrireMobilier.GetComponent<TMP_Text>().EcrireMessageIci(MessageAEcrireMoyenMoral[Rand]);
            }
            else if (Depression >= 4 && Depression < 8)
            {
                InterractionMobilier.SetActive(true);

                int Rand = UnityEngine.Random.Range(0, DialogueMoyenMoral.Length);

                string MessageTempo = EcrireMessageIci(DialogueMoyenMoral[Rand]);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = MessageTempo;
            }
            else if (Depression >= 8)
            {
                InterractionMobilier.SetActive(true);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "...";
            }
        }

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {

            //Détecter si on touche une interraction

            Debug.Log("Interraction");
            InterfaceInteraction.SetActive(true);
            SetLastCollideObjectName(other.name);
            InteractionEcrire.GetComponent<TMP_Text>().text = other.name;
            



        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interaction")
        {
            Debug.Log("Interraction");
            InterfaceInteraction.SetActive(true);
            SetLastCollideObjectName(collision.gameObject.name);
            InteractionEcrire.GetComponent<TMP_Text>().text = collision.gameObject.name;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {
            //Détecter si on reste sur une interraction

            Debug.Log("Interraction Continu");

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interaction")
        {

            //Détecter si on quitte une interraction



            Debug.Log("Plus Interraction");
            InterfaceInteraction.SetActive(false);


        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Interaction")
        {

            //Détecter si on quitte une interraction



            Debug.Log("Plus Interraction");
            InterfaceInteraction.SetActive(false);


        }
    }

    public void SetLastCollideObjectName(string name)
    {
        //Stocker le nom de la dernière collision pour l'afficher dans l'interface


        LastObjectCollideName = name;
        Debug.Log(LastObjectCollideName);
    }


    public string ReturnNomInteraction(string value)
    {
        return value;
    }


    public void ObjectifFonction()
    {

        //Affichage objectif du joueur en cours



        if (travail == true)
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

        //Faire apparaitre les commentaires du personnage


        GameObject BoutonASpawn = Instantiate(MessageSecondaire);
        BoutonASpawn.transform.parent = (canvas.transform);
        BoutonASpawn.transform.SetSiblingIndex(finalBoutonTransform.GetSiblingIndex()-1);

    }


    public void AffichageMessageRandom()
    {

        //Instantier les message sur un lapse de temps en dépendant du niveau de moral du joueur


        TempsAleatoire = TempsAleatoire - Time.deltaTime;

        if( TempsAleatoire < 0 )
        {
            SpawnCommentaireSecondaire();

            if (Depression >= 0 && Depression < 4)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBonMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBonMoral[Rand]);
                TempsAleatoire = Random.RandomRange(5, 10);

            }
            if (Depression >= 4 && Depression < 8)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireMoyenMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireMoyenMoral[Rand]);
                TempsAleatoire = Random.RandomRange(4, 8);
                Debug.Log(TempsAleatoire);

            }
            if (Depression >= 8 && Depression < 12)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);
                TempsAleatoire = Random.RandomRange(4, 6);
                Debug.Log(TempsAleatoire);

            }
            if ( Depression >= 12)
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


    public void LookPlayerObjectA()
    {

        //Dépendant de son moral les gens vont observer le joueur ça se apsse ici #Paranoïa

        if (Depression > 4)
        {
            foreach(GameObject Objet in GroupAPersonnage)
            {
                Objet.transform.LookAt(this.gameObject.transform);
            }
        }
        if (Depression > 8)
        {
            foreach (GameObject Objet in GroupAPersonnage)
            {
                Objet.transform.LookAt(this.gameObject.transform);
            }

            foreach (GameObject ObjetBis in GroupBPersonnage)
            {
                ObjetBis.transform.LookAt(this.gameObject.transform);
            }
        }



       

    }



    public string EcrireMessageIci(string message)
    {

        //Ecrire le message à afficher
        return message;

        
    }


}
