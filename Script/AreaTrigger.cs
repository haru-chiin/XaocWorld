using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    // Daftar objek yang akan diaktifkan atau dinonaktifkan
    public GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other)
    {
        // Jika pemain masuk ke collider
        if (other.CompareTag("Player"))
        {
            // Aktifkan semua objek dalam daftar
            SetObjectsActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Jika pemain keluar dari collider
        if (other.CompareTag("Player"))
        {
            // Nonaktifkan semua objek dalam daftar
            SetObjectsActive(false);
        }
    }

    private void SetObjectsActive(bool state)
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }
}
