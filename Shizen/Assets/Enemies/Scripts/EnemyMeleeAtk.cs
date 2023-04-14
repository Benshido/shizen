using UnityEngine;

public class EnemyMeleeAtk : MonoBehaviour
{
    public float damage;

    //private List<PlayerHP> hit = new();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test EnemyMeleeAtk");
        //var hp = other.gameObject.GetComponent<PlayerHP>();
        //if (hp != null && !hit.Contains(hp))
        //{
        //    hit.Add(hp);
        //    hp.TakeDamage(damage);
        //}
    }
    public void ClearHPObjectsHit()
    {
        //hit.Clear();
    }
}
