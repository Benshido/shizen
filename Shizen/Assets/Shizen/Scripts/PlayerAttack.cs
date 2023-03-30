using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] bool stickToGround;
    [SerializeField] float maxGroundRange = 5f;
    [SerializeField] float groundOffset = 1f;
    [SerializeField] LayerMask IsGround;
    [SerializeField] Transform raycastStart;
    [SerializeField] Transform GroundObject;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
            Destroy(gameObject);
        else
        {
            GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
        }

        StartCoroutine(Destroying());
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
        {
            if (hit.distance < groundOffset + 0.5f)
            {
                GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
            }
        }
    }

    private IEnumerator Destroying()
    {
        yield return new WaitForSecondsRealtime(8);
        // Destroy(gameObject);
    }
}
