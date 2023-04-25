using System;
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

    private PlayerController player;

    private void Awake() => player = FindObjectOfType<PlayerController>();

    // when player goes over trap start IEnumerator passing down the value of the collider
    private void OnTriggerEnter2D(Collider2D other) => StartCoroutine(SlowThenSpeedUpPlayerOvertime());

    private IEnumerator SlowThenSpeedUpPlayerOvertime()
    {
        // save players current movement speed
        var savedSpeed = player.GetMovementSpeed();
        // take 1 from players movement speed to slow player down
        player.UpdateMovementSpeed(-1);
        // change the color of the trail renderer
        trail.colorGradient = newGradient;
        yield return new WaitForSeconds(2f);
        // slowly increase players speed after a 2s delay
        while (player.GetMovementSpeed() < savedSpeed)
        {
            player.UpdateMovementSpeed(0.25f);
            yield return new WaitForSeconds(1f);
        }
        // change trail renderer color back to original color
        trail.colorGradient = oldGradient;
    }
}
