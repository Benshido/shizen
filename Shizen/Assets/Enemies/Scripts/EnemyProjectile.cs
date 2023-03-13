using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public float range;
    public Vector3 target;
    public LayerMask mask;

    private Vector3 startPoint;
    private Vector3 oldPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        target.y = transform.position.y;
        oldPosition = transform.position;
        range += 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Move projectile forward to maximum range
        Ray ray = new(startPoint, target - startPoint);
        var newPos = Vector3.MoveTowards(transform.position, ray.GetPoint(range), Time.deltaTime * speed);

        Debug.DrawRay(startPoint, newPos - startPoint, Color.red, 0.5f);
        transform.position = newPos;

        //If the object reaches the max range, destroy
        if (transform.position == ray.GetPoint(range))
        {
            //could replace this with a fade effect and destroy etc.
            Destroy(gameObject);
        }

        //Check if there is a collider between previous frame position and current position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.position - oldPosition, out hit, Vector3.Distance(transform.position, oldPosition), mask))
        {
            //var otherHP = hit.collider.gameObject.GetComponent<PlayerHP>();
            //if (otherHP != null) otherHP.TakeDamage(damage);

            //Could replace this with an explode and destroy effect etc.
            Destroy(gameObject);
        }
        oldPosition = transform.position;
    }
}
