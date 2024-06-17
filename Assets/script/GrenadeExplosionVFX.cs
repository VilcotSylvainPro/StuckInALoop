using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosionVFX : MonoBehaviour
{
    [SerializeField] private GameObject GrenadeExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < GrenadeExplosion.transform.GetChild(1).transform.childCount; i++) 
        {
            if (GrenadeExplosion.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                if (GrenadeExplosion.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>().isStopped)
                {
                    Destroy(GrenadeExplosion);
                }
            }
        }    
    }
}
