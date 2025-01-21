using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource region1Music;
    public AudioSource region2Music;
    public Collider region1Collider;
    public Collider region2Collider;

    private void Start()
    {
        // Pastikan musik tidak bermain di awal
        region1Music.Stop();
        region2Music.Stop();

        // Tambahkan trigger event ke Collider
        region1Collider.gameObject.AddComponent<RegionTrigger>().Initialize(region1Music, region2Music, "Region1");
        region2Collider.gameObject.AddComponent<RegionTrigger>().Initialize(region2Music, region1Music, "Region2");
    }
}

public class RegionTrigger : MonoBehaviour
{
    private AudioSource enterMusic;
    private AudioSource exitMusic;
    private string regionName;

    public void Initialize(AudioSource enter, AudioSource exit, string name)
    {
        enterMusic = enter;
        exitMusic = exit;
        regionName = name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterMusic.Play();
            exitMusic.Stop();
            Debug.Log($"Entering {regionName}");
        }
    }
}
