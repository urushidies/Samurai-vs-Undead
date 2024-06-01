using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public int MaxEnemyHealth = 25;
    public int EnemyHealthAmount;

    private Animator anim;

    void Start()
    {
        EnemyHealthAmount = MaxEnemyHealth;
        anim = GetComponent<Animator>(); // Get the Animator component
    }

    public void TakeDamageEnemy(int damage)
    {
        EnemyHealthAmount -= damage;
        if (EnemyHealthAmount <= 0)
        {
            // Trigger the death animation
            anim.SetTrigger("Die");
            
            // Start the coroutine to wait for the animation to complete before destroying the game object
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the end of the current animation (assuming the death animation is the only one playing)
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Destroy the game object
        Destroy(gameObject);
    }
}
