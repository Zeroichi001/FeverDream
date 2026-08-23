using System.Transactions;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [Header("Filter Detection")]
    public LayerMask targetLayer; // Pilih layer di Inspector

    public GameObject DetectedObject { get; private set; } = null;

    private void OnTriggerEnter(Collider other)
    {
        //tectedObject = other.gameObject;
        if (other.gameObject.CompareTag(this.gameObject.tag))
        {
            DetectedObject = other.gameObject;
        }
    }
}