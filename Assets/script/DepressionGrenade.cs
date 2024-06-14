using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepressionGrenade : MonoBehaviour
{
    private Player Player;

    [SerializeField] private GameObject DepressionGrenadeExplosionVFX;

    [SerializeField] private Collider DepressionGrenadeCloud;
    [SerializeField] private GameObject DepressionGrenadeCloudVFX;

    private Vector3 Direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public void Explosion()
    {
        for (int i = 0; i < DepressionGrenadeExplosionVFX.transform.childCount; i++) 
        {
            DepressionGrenadeExplosionVFX.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
    }

    public void Cloud()
    {
        StartCoroutine(CloudExpansion());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<UnkwonwPeople>())
        {
            Player.SetDepression(1);
            Player.SetNbVictim(1);
        }

        if (collision.gameObject.layer == 7)
        {
            //Destroy(this.transform.parent.gameObject);
        }
    }

    IEnumerator CloudExpansion()
    {
        if (DepressionGrenadeCloud.bounds.size.x < 3)
        {
            for (int i = 0; i < DepressionGrenadeCloudVFX.transform.childCount; i++)
            {
                DepressionGrenadeCloudVFX.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
            }
        }

        yield return new WaitForSeconds(2f);

        Destroy(this.transform.parent.gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }
}
