using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopoleExplosionVFX : MonoBehaviour
{
    [SerializeField] private GameObject PeopleExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PeopleExplosion.transform.GetChild(1).transform.childCount; i++)
        {
            if (PeopleExplosion.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                if (PeopleExplosion.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>().isStopped)
                {
                    Destroy(PeopleExplosion);
                }
            }
        }
    }
}
