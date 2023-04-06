using System.Collections;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [Header("Gradients")]
    [SerializeField] private Gradient newGradient;
    private Gradient localNewGradient;
    [SerializeField] private Gradient oldGradient;
    private Gradient localOldGradient;
    [SerializeField] private TrailRenderer trail;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) StartCoroutine(SlowThenSpeedUpPlayerOvertime(other));
    }

    private IEnumerator SlowThenSpeedUpPlayerOvertime(Collider2D player)
    {
        var savedSpeed = player.GetComponent<PlayerController>().GetMovementSpeed();
        player.GetComponent<PlayerController>().UpdateMovementSpeed(-1);
        trail.colorGradient = newGradient;
        yield return new WaitForSeconds(2f);
        while (player.GetComponent<PlayerController>().GetMovementSpeed() < savedSpeed)
        {
            player.GetComponent<PlayerController>().UpdateMovementSpeed(0.25f);
            yield return new WaitForSeconds(1f);
        }
        trail.colorGradient = oldGradient;
    }
}
