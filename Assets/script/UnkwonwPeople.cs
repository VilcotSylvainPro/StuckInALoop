using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnkwonwPeople : MonoBehaviour
{
    private bool Depressed = false;
    private bool Dead = false;
    private bool Explosed = false;

    private GameObject ExplosionDeadVFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            if (Explosed)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (Depressed)
            {
                Suicide();
            }
        }
    }

    private void Suicide()
    {
        if (!Dead)
        {
            if (Depressed)
            {
                for (int i = 0; i < ExplosionDeadVFX.transform.childCount; i++)
                {
                    if (ExplosionDeadVFX.transform.GetChild(i).GetComponent<ParticleSystem>())
                    {
                        ExplosionDeadVFX.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DepressionGrenade>() != null)
        {
            if (!Dead && !Depressed)
            {
                Depressed = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DepressionGrenade>() != null)
        {
            if (!Dead && !Depressed)
            {
                Depressed = true;
            }
        }
    }
}
