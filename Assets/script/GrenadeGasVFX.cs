using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeGasVFX : MonoBehaviour
{
    private Player PlayerScript;

    [SerializeField] private GameObject GrenadeGas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < GrenadeGas.transform.GetChild(1).transform.childCount; i++)
        {
            if (GrenadeGas.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                if (GrenadeGas.transform.GetChild(1).transform.GetChild(i).GetComponent<ParticleSystem>().isStopped)
                {
                    Destroy(GrenadeGas);
                }
            }
        }

        if (this.transform.localScale.x < 10)
        {
            this.gameObject.transform.localScale = new Vector3(this.transform.localScale.x * 1.1f, this.transform.localScale.y * 1.1f, this.transform.localScale.z * 1.1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<UnkwonwPeople>().SetDepressed(true);
            PlayerScript.SetDepression(1);
            PlayerScript.SetNbVictim(1);
        }
    }

    public void SetPlayerScript(Player player)
    {
        PlayerScript = player;
    }
}
