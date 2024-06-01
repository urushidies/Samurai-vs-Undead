using UnityEngine;

public class StunnedBounceBack : MonoBehaviour
{
    // Set the initial position
    private float initialPosition;
    
    // Set the number of times to bounce back
    private int numBounces = 3;

    // Set the distance and duration for each bounce
    private float distance = 1f;
    private float duration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the initial position
        initialPosition = transform.position.x;
        
        // Start the bouncing animation
        BounceBack();
    }

    // Function to animate the object bouncing back
    public void BounceBack()
    {
        // Check if all bounces are done
        if (numBounces == 0)
            return;

        // Animate the object to bounce back
        LeanTween.moveX(gameObject, initialPosition + distance, duration).setEaseInOutQuad().setOnComplete(() =>
        {
            // Reverse the direction
            distance = -distance;

            // Decrement the number of bounces left
            numBounces--;

            // Call the function recursively
            BounceBack();
        });
    }
}