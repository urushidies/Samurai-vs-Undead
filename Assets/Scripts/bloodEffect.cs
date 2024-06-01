using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void PlayBloodEffect(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        animator.Play("Blood");
    }

    // This method should be called at the end of the animation using an Animation Event
    public void OnAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
