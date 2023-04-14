using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosCoreAnimator : MonoBehaviour
{
    [SerializeField] EnemyHP Hp;
    [SerializeField] Animator anim;
    int currentSceneBuildIndex;

    private void Start()
    {
       if(anim==null) anim = GetComponent<Animator>();
       if(Hp==null) Hp = GetComponent<EnemyHP>();
    }
    private void Update()
    {
        if (!Hp.IsAlive)
        {
            anim.SetBool("IsAlive", Hp.IsAlive);
        }
    }

    IEnumerator LoadNextScene()
    {
        Debug.Log("Testing!!!");
        yield return new WaitForSeconds(5);
        currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex + 1);
        StopAllCoroutines();
    }
}
