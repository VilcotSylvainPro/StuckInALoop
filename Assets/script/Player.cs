using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Security.Cryptography;
using UnityEngine.InputSystem.EnhancedTouch;

public class Player : MonoBehaviour
{
    private 

    PlayerInput playerInput;
    [SerializeField] private GameObject Objectif;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject InterfaceInteraction;
    [SerializeField] private GameObject InteractionEcrire;
    [SerializeField] private GameObject SortirMaison;
    [SerializeField] private GameObject MaisonExterieur;
    [SerializeField] private GameObject TravailExterieur;
    [SerializeField] private GameObject TravailInterieur;
    [SerializeField] private GameObject[] Inconnus;
    [SerializeField] private string[,] Dialogues;
    [SerializeField] private GameObject Telephone;
    [SerializeField] private GameObject Demenager;
    [SerializeField] private GameObject NouveauTravail;
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

    [SerializeField] private GameObject MessageParadis;

    [SerializeField] private int Depression = 4;

    [SerializeField] private GameObject MessageSecondaire;
    [SerializeField] private GameObject InterractionMobilier;
    [SerializeField] private GameObject InteractionEcrireMobilier;
    [SerializeField] private GameObject MessageFinal;
    [SerializeField] private GameObject BoutonFinal;
    [SerializeField] private GameObject EcranFin;
    [SerializeField] private GameObject Fin;
    [SerializeField] private Font NormalFont;
    [SerializeField] private Font HorrorFont;

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

    [SerializeField] private CharacterController CharacterController;
    [SerializeField] private InputManager Input_Manager;
    private Vector2 direction;
    [SerializeField] private bool TimerDeadActivation = false;
    [SerializeField] private float TimerDead = 0f;
    [SerializeField] private bool Dead = false;
    private Vector3 SpawnPosition = Vector3.zero;

    [SerializeField] private CharacterController GroundController;
    [SerializeField] private Vector3 GroundForce = Vector3.zero;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private float Gravity;
    private bool InJump = false;

    private bool CanDepressOther = false;
    private bool Shoot = false;
    [SerializeField] private Transform DepressionGrenadeSpawn;
    [SerializeField] private GameObject DepressionGrenade;
    [SerializeField] private GameObject Grenade;
    [SerializeField] private float Force;
    [SerializeField] private float UpForce;

    private Vector2 CursorPositionVector = Vector2.zero;

    [SerializeField] private GameObject[] ObjectsToRotate;
    private int NbVictim = 0;
    private int MaxVictime = 5;

    [SerializeField] private CameraShake CamShake;

    private bool END;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        finalBoutonTransform = MessageFinal.transform;

        TempsAleatoire = Random.Range(5, 15);
        Debug.Log(TempsAleatoire);
        playerInput = GetComponent<PlayerInput>();
        moving = playerInput.actions.FindAction("Move");
        InterfaceInteraction.SetActive(false);
        SpawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //movingPlayer();
        ObjectifFonction();
        AffichageMessageRandom();
        LookPlayerObjectA();
       
        if (Dead || NbVictim > MaxVictime)
        {
            Respawn(SpawnPosition);
        }

        if (CharacterController.enabled && !END)
        {
            CharacterController.Move(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime);
        }


        Debug.Log("Is grounded " + GroundController.isGrounded);

        if (!GroundController.isGrounded && GroundForce.y > -0.05f && !InJump && !END)
        {
            GroundForce.y -= Gravity * Time.deltaTime;
        }
        else if (InJump && GroundForce.y < 3 && !END)
        {
            GroundForce.y = 0 + Gravity * Time.deltaTime;
        }

        if (GroundController.enabled && !END)
        {
            GroundController.Move(GroundForce);
        }

        if (TimerDeadActivation)
        {
            CharacterController.enabled = false;
            GroundController.enabled = false;
            TimerDead += Time.deltaTime;
            if (TimerDead > 3f)
            {
                Dead = true;
            }
        }

        if (Depression >= 2)
        {
            CanDepressOther = true;
        }

        if (Shoot)
        {
            if (Grenade == null)
            {
                Grenade = Instantiate(DepressionGrenade, DepressionGrenadeSpawn.position, Quaternion.identity);
                Grenade.transform.GetChild(0).GetComponent<DepressionGrenade>().SetPlayer(this);
                Grenade.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).transform.forward.x * Force * Time.deltaTime, UpForce * Time.deltaTime, transform.GetChild(0).transform.forward.z * Force * Time.deltaTime, ForceMode.Impulse);
            }
        }

        if (!InterractionMobilier.activeSelf && !EcranBoulot.activeSelf && !EcranDodo.activeSelf && !TimerDeadActivation)
        {
            CharacterController.enabled = true;
            Input_Manager.enabled = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(CursorPositionVector);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            foreach (GameObject ObjectToRotate in ObjectsToRotate)
            {
                ObjectToRotate.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }

        if (this.transform.position.y > 100)
        {
            TimerDeadActivation = true;
            CamShake.ChangeCameraRotation(2);
            MessageParadis.SetActive(true);
        }
    }


    public Vector2 movingPlayer(Vector2 Direction)
    {
        //Vector2 direction = moving.ReadValue<Vector2>();
        //transform.position = transform.position + new Vector3(direction.x,0,direction.y) * speed * Time.deltaTime;
        //CharacterController.Move(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime);
        direction = Direction;
        return direction;
    }

    public void Jump(bool State)
    {
        InJump = State;
    }

    public void DepressionGunShot(bool State)
    {
        if (CanDepressOther)
        {
            Shoot = State;
        }
    }

    public void CursorPosition(Vector2 CursorPosition)
    {
        CursorPositionVector = CursorPosition;
    }

    public void SetDepression(int value)
    {
        if (Depression > 0)
        {
            Depression -= value;
        }
    }

    public void Navigation()
    {

        //Pour voyager d'une pièce à l'autre du jeu
        Input_Manager.enabled = false;
        if (LastObjectCollideName == "Sortir Maison")
        {
            CharacterController.enabled = false;
            PlayerObject.transform.position = new Vector3(MaisonExterieur.transform.position.x, 1.6f, MaisonExterieur.transform.position.z);
            Debug.Log(PlayerObject.transform.position);
            Maison.SetActive(false);
            Rue.SetActive(true);
            Bruitage.PlayOneShot(EntreeMaison);
            if (PlayerObject.transform.position == new Vector3(MaisonExterieur.transform.position.x, 1.6f, MaisonExterieur.transform.position.z))
            {
                CharacterController.enabled = true;
                Input_Manager.enabled = true;
            }
        }

        if (LastObjectCollideName == "Entrer Maison")
        {
            CharacterController.enabled = false;
            this.gameObject.transform.position = new Vector3(-295.24f, 1.6f, 14.67f);
            Maison.SetActive(true);
            Rue.SetActive(false);
            Bruitage.PlayOneShot(SortieMaison);
            if (this.gameObject.transform.position == new Vector3(-295.24f, 1.6f, 14.67f))
            {
                CharacterController.enabled = true;
                Input_Manager.enabled = true;
            }
        }

        if (LastObjectCollideName == "Entrer Travail")
        {
            CharacterController.enabled = false;
            this.gameObject.transform.position = new Vector3(TravailInterieur.transform.position.x, 1.6f, TravailInterieur.transform.position.z);
            Office.SetActive(true);
            Rue.SetActive(false);
            Bruitage.PlayOneShot(Asenceur);
            if (this.gameObject.transform.position == new Vector3(TravailInterieur.transform.position.x, 1.6f, TravailInterieur.transform.position.z))
            {
                CharacterController.enabled = true;
                Input_Manager.enabled = true;
            }
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
            if ( travail == false)
            {
                CharacterController.enabled = false;
                this.gameObject.transform.position = new Vector3(TravailExterieur.transform.position.x, 1.6f, TravailExterieur.transform.position.z);
                Office.SetActive(false);
                Rue.SetActive(true);
                Bruitage.PlayOneShot(SortieBureau);
                if (this.gameObject.transform.position == new Vector3(TravailExterieur.transform.position.x, 1.6f, TravailExterieur.transform.position.z))
                {
                    CharacterController.enabled = true;
                    Input_Manager.enabled = true;
                }
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

        if (LastObjectCollideName == "Appeler psy")
        {
            if (Depression < 4)
            {
                InterractionMobilier.SetActive(true);
                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "J'ai plus besoin de psy";
            }
            else
            {
                MessageFinal.SetActive(true);
                StopAllCoroutines();
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Finalement, j'aurais peut-être pas dû arrêter les rendez vous...";
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().font = NormalFont;
                MessageFinal.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
                END = true;
                StartCoroutine(MessageFinalDisparition(2,1));
            }
        }
        if (LastObjectCollideName == "Déménager")
        {
            if (Depression < 4)
            {
                InterractionMobilier.SetActive(true);

                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Un déménagement, ça se prépare quand même !";
            }
            else
            {
                MessageFinal.SetActive(true);
                StopAllCoroutines();
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Au diable les préparatifs! J'ai besoin de m'en aller.";
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().font = NormalFont;
                MessageFinal.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
                END = true;
                StartCoroutine(MessageFinalDisparition(2, 1));
            }
        }
        if (LastObjectCollideName == "Changer de travail")
        {
            if (Depression < 4)
            {
                InterractionMobilier.SetActive(true);

                InteractionEcrireMobilier.GetComponent<TMP_Text>().text = "Ah ben non, j'ai enfin trouvé un travail pas loin de chez moi !";
            }
            else
            {
                MessageFinal.SetActive(true);
                StopAllCoroutines();
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Changer de travail, c'est vraiment ce dont j'ai besoin";
                MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().font = NormalFont;
                MessageFinal.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
                END = true;
                StartCoroutine(MessageFinalDisparition(2, 1));
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

        if (collision.gameObject.GetComponent<DeplementVoiture>() && Depression<4)
        {
            CharacterController.enabled = false;
            GroundController.enabled = false;
            Rigidbody.AddForce((transform.position - collision.transform.position) * 500 * Time.deltaTime, ForceMode.Impulse);
            TimerDeadActivation = true;
        }
        else if (collision.gameObject.GetComponent<DeplementVoiture>())
        {
            OnSuicideDeath();
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
                TempsAleatoire = Random.Range(5, 10);

            }
            if (Depression >= 4 && Depression < 8)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireMoyenMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireMoyenMoral[Rand]);
                TempsAleatoire = Random.Range(4, 8);
                Debug.Log(TempsAleatoire);

            }
            if (Depression >= 8 && Depression < 12)
            {
                // MessageSecondaire;

                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);
                TempsAleatoire = Random.Range(4, 6);
                Debug.Log(TempsAleatoire);

            }
            if ( Depression >= 12)
            {
                // MessageSecondaire;
                TempsAleatoire = Random.Range(1, 2);
                int Rand = UnityEngine.Random.Range(0, MessageAEcrireBadMoral.Length);

                MessageSecondaire.GetComponent<TexteMessage>().EcrireMessage(MessageAEcrireBadMoral[Rand]);
                //TempsAleatoire = Random.RandomRange(1, 1);
                //MessageFinal.SetActive(true);
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

    public void Respawn(Vector3 Position)
    {
        CamShake.ResetCameraRotation();
        MessageParadis.SetActive(false);
        transform.position = Position;
        if (transform.position == Position)
        {
            CharacterController.enabled = true;
        }
        TimerDeadActivation = false;
        MaxVictime += 2;
        NbVictim = 0;
        TimerDead = 0f;
        Dead = false;
    }

    public void SetCharacterController(bool State)
    {
        CharacterController.enabled = State;
    }

    public void SetNbVictim(int value)
    {
        NbVictim += value;
    }

    public void OnSuicideDeath()
    {
        EcranFin.transform.GetChild(1).gameObject.GetComponent<Text>().text = "C'était la seule solution.";
        EcranFin.SetActive(true);
        Fin.SetActive(true);
        MessageFinal.SetActive(false);
    }

    public void Suicide()
    {
        MessageFinal.SetActive(true);
        if (Depression <= 4)
        {
            StopAllCoroutines();
            MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "En finir avec cette boucle ? Pourquoi tu me parles de ca ? Je vais bien !";
            MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().font = NormalFont;
            MessageFinal.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
            StartCoroutine(MessageFinalDisparition(2,0));
        }
        else
        {
            MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Tu as raison. Il est temps.";
            MessageFinal.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().font = HorrorFont;
            MessageFinal.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
            END = true;
            StartCoroutine(MessageFinalDisparition(2, 0));
        }
    }

    IEnumerator MessageFinalDisparition(int time, int endType)
    {
        yield return new WaitForSeconds(time);
        if (END)
        {
            StartCoroutine(End(endType));
        }
        else
        {
            MessageFinal.SetActive(false);
        }
    }
    IEnumerator End(int endType)
    {
        if (endType == 0)
        {
            Input_Manager.enabled = false;
            gameObject.transform.position = new Vector3(30, 1.5f, 49);
            Vector3 pos = gameObject.transform.position;
            yield return new WaitForSeconds(1f);
            while (pos.x < 55)
            {
                yield return new WaitForSeconds(0.01f);
                CharacterController.Move(new Vector3(0, 0, 1) * speed * Time.deltaTime);
                pos = gameObject.transform.position;
            }
        }
        else if (endType == 1)
        {
            Input_Manager.enabled = false;
            EcranFin.SetActive(true);
            EcranFin.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Ma psy pense que changer encore de boulot va m'aider.";
            Fin.SetActive(true);
            MessageFinal.SetActive(false);
        }
        else if (endType == 2)
        {
            Input_Manager.enabled = false;
            EcranFin.SetActive(true);
            EcranFin.transform.GetChild(1).gameObject.GetComponent<Text>().text = "J'espère que tu ne me suivra pas.";
            Fin.SetActive(true);
            MessageFinal.SetActive(false);
        }
        else if (endType == 3)
        {
            Input_Manager.enabled = false;
            EcranFin.SetActive(true);
            EcranFin.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Un nouveau boulot, pas de nouveau soucis, hein ?";
            Fin.SetActive(true);
            MessageFinal.SetActive(false);
        }
    }
}
