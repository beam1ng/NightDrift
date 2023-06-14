using UnityEngine;

public class DieOut : MonoBehaviour
{
    public float delay = 3f;

    private void Start()
    {
        StartCoroutine(DelayedDestruction());
    }

    private System.Collections.IEnumerator DelayedDestruction()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
