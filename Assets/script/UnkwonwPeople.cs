using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnkwonwPeople : MonoBehaviour
{
    [SerializeField] private bool Depressed = false;
    [SerializeField] private bool Dead = false;
    [SerializeField] private bool Explosed = false;

    [SerializeField] private GameObject ExplosionDeadVFX;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionDeadVFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            for (int i = 0; i < ExplosionDeadVFX.transform.childCount; i++)
            {
                if (ExplosionDeadVFX.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>().isStopped)
                {
                    Explosed = true;
                }
            }
            if (Explosed)
            {
                Destroy(this.transform.parent.gameObject);
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
                    if (ExplosionDeadVFX.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>())
                    {
                        ExplosionDeadVFX.SetActive(true);
                        ExplosionDeadVFX.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>().Play();
                        Dead = true;
                    }
                }
            }
        }
    }

    public void SetDepressed(bool state)
    {
        Depressed = state;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if (other.GetComponent<DepressionGrenade>() != null)
            {
                Depressed = true;
            }
        }
    }
}
