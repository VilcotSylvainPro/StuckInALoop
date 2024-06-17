using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepressionGrenade : MonoBehaviour
{
    private Player Player;

    [SerializeField] private GameObject DepressionGrenadeExplosionVFX;

    [SerializeField] private Collider DepressionGrenadeCloud;
    [SerializeField] private GameObject DepressionGrenadeCloudVFX;

    [SerializeField] private Vector3 PositionExplosion = Vector3.zero;

    [SerializeField] private LayerMask GroundLayer;

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

    public void Explosion(Vector3 Position)
    {
        Instantiate(DepressionGrenadeExplosionVFX, Position, Quaternion.identity);
        StartCoroutine(CloudExpansion(Position));
    }

    public void Cloud(Vector3 Position)
    {
        StartCoroutine(CloudExpansion(Position));
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8)
        {
            other.GetComponent<UnkwonwPeople>().SetDepressed(true);
            Player.SetDepression(1);
            Player.SetNbVictim(1);

            Explosion(other.gameObject.transform.position);
        }

        if (other.gameObject.layer == 7)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1f, GroundLayer))
            {
                Debug.DrawRay(ray.origin, ray.direction);
                Explosion(hit.point);
            }
        }
    }

    IEnumerator CloudExpansion(Vector3 Position)
    {
        yield return new WaitForSeconds(3f);

        GameObject DepressionCloudVFX = Instantiate(DepressionGrenadeCloudVFX, Position, Quaternion.identity);
        DepressionCloudVFX.transform.GetChild(0).GetComponent<GrenadeGasVFX>().SetPlayerScript(Player);

        yield return new WaitForSeconds(0.5f);

        Destroy(this.transform.parent.gameObject);
    }
}
