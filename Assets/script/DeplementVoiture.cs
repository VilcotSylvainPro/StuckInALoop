using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplementVoiture : MonoBehaviour
{

    Rigidbody body;
    [SerializeField] private float VitesseVoiture;

    [SerializeField] private GameObject Voiture;
    [SerializeField] private GameObject FinRoute;
    [SerializeField] private GameObject FinRouteBis;

    // Start is called before the first frame update

    private void Awake()
    {
        FinRoute = GameObject.Find("VoitureSpawn/Despawn");
        FinRouteBis = GameObject.Find("VoitureSpawn/DespawnBis");
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.AddForce(VitesseVoiture, 0, 0, ForceMode.Impulse);
        this.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 80f, 0f));
    }



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "VoitureFin" && VitesseVoiture < 0)
        {
            Instantiate(Voiture, Voiture.transform.position = new Vector3(54.2f, 3.14f, 54.5f), Quaternion.Euler(new Vector3(0f,90f,0f)));
            Voiture.GetComponent<DeplementVoiture>().SetVitesseVOiture(-0.5f);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "VoitureFinBis" && VitesseVoiture > 0)
        {
            Instantiate(Voiture, Voiture.transform.position = new Vector3(-126.48f, 3.14f, 65.38f), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
            Voiture.GetComponent<DeplementVoiture>().SetVitesseVOiture(0.5f);
            Destroy(this.gameObject);
        }

    }



    public void SetVitesseVOiture(float vitesse)
    {
        VitesseVoiture = vitesse;
    }


}
