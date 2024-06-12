using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


    PlayerInput playerInput;
    [SerializeField] private GameObject Objectif;
    [SerializeField] private GameObject InterfaceInteraction;
    [SerializeField] private GameObject InteractionEcrire;
    [SerializeField] private GameObject SortirMaison;
    [SerializeField] private GameObject MaisonExterieur;
    [SerializeField] private GameObject TravailExterieur;
    [SerializeField] private GameObject BureauPlayer;
    [SerializeField] private GameObject TravailSortie;
    [SerializeField] private GameObject LitPlayer;
    [SerializeField] private float speed;

    private bool repos = false;
    private bool travail = true;
    private string LastObjectCollideName;

    private bool InterractionGameplay;
    InputAction moving;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moving = playerInput.actions.FindAction("Move");
        InterfaceInteraction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movingPlayer();
        ObjectifFonction();

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
            this.gameObject.transform.position = new Vector3(-295.24f, 1.6f, 14.67f);
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

}
