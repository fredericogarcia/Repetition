using System.Collections;
using UnityEngine;

public class Traps : MonoBehaviour
{
    /*
     * variables used to store and select colors for the trail renderer
     */
    [Header("Gradients")]
    [SerializeField] private Gradient newGradient;
    private Gradient localNewGradient;
    [SerializeField] private Gradient oldGradient;
    private Gradient localOldGradient;
    [SerializeField] private TrailRenderer trail;

    // when player goes over trap start IEnumerator passing down the value of the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) StartCoroutine(SlowThenSpeedUpPlayerOvertime(other));
    }

    private IEnumerator SlowThenSpeedUpPlayerOvertime(Collider2D player)
    {
        // save players current movement speed
        var savedSpeed = player.GetComponent<PlayerController>().GetMovementSpeed();
        // take 1 from players movement speed to slow player down
        player.GetComponent<PlayerController>().UpdateMovementSpeed(-1);
        // change the color of the trail renderer
        trail.colorGradient = newGradient;
        yield return new WaitForSeconds(2f);
        // slowly increase players speed after a 2s delay
        while (player.GetComponent<PlayerController>().GetMovementSpeed() < savedSpeed)
        {
            player.GetComponent<PlayerController>().UpdateMovementSpeed(0.25f);
            yield return new WaitForSeconds(1f);
        }
        // change trail renderer color back to original color
        trail.colorGradient = oldGradient;
    }
}
