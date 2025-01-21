using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CoinMissionManager : MonoBehaviour
{
    public PlayerInventory playerInventory; // Referensi ke PlayerInventory
    public GameObject missionUI; // UI yang akan diaktifkan saat misi selesai
    public GameObject pendingMissionUI; // UI yang akan muncul saat misi belum selesai
    public Text countText; // UI Text untuk menampilkan jumlah item

    public List<Missions> missions; // Daftar misi (harus terdiri dari dua misi)
    private int currentMissionIndex = 0; // Indeks misi saat ini
    private bool missionCompleted = false; // Flag untuk memastikan misi hanya dijalankan sekali
    private int previousItemCount = 0; // Untuk melacak perubahan jumlah item

    public EnemyStats enemyStats; // Referensi ke EnemyStats untuk misi kedua
    public GameObject enemyPrefab; // Prefab musuh yang akan diaktifkan

    void Start()
    {
        // Pastikan pendingMissionUI aktif saat start jika misi belum selesai
        pendingMissionUI.SetActive(!missionCompleted);

        // Update UI pada awal
        UpdateCountText();
    }

    void Update()
    {
        if (currentMissionIndex >= missions.Count) return; // Jika tidak ada misi lagi, keluar

        if (currentMissionIndex == 0)
        {
            // Misi pertama: Pengumpulan item
            int currentItemCount = playerInventory.weaponInventory.Count;

            // Cek jika jumlah item berubah
            if (currentItemCount != previousItemCount)
            {
                UpdateCountText();
                previousItemCount = currentItemCount;
            }

            // Cek jika jumlah item di weaponInventory mencapai jumlah yang dibutuhkan dan misi belum selesai
            if (currentItemCount >= missions[currentMissionIndex].requiredItemCount && !missionCompleted)
            {
                StartCoroutine(ShowMissionUI());
            }
        }
        else if (currentMissionIndex == 1)
        {
            // Misi kedua: Mengalahkan musuh
            if (enemyStats.currentHealth <= 0 && !missionCompleted)
            {
                StartCoroutine(ShowMissionUI());
            }
        }
    }

    private IEnumerator ShowMissionUI()
    {
        // Tandai misi sebagai selesai
        missionCompleted = true;

        // Aktifkan UI misi
        missionUI.SetActive(true);

        // Nonaktifkan pendingMissionUI
        pendingMissionUI.SetActive(false);

        // Tunggu selama 3 detik
        yield return new WaitForSeconds(3f);

        // Nonaktifkan UI misi
        missionUI.SetActive(false);

        // Beralih ke misi berikutnya
        currentMissionIndex++;
        missionCompleted = false;

        // Jika misi berikutnya adalah misi kedua, aktifkan musuh
        if (currentMissionIndex == 1)
        {
            ActivateEnemy();
        }

        // Perbarui UI untuk misi berikutnya jika ada
        if (currentMissionIndex < missions.Count)
        {
            UpdateCountText();
            pendingMissionUI.SetActive(true);
        }
        else
        {
            
            pendingMissionUI.SetActive(false);
            /*StartCoroutine(RestartGame());*/
        }
    }

    private void ActivateEnemy()
    {
        if (enemyPrefab != null)
        {
            enemyPrefab.SetActive(true);
        }
    }

    // Update UI Text untuk menampilkan jumlah item
    private void UpdateCountText()
    {
        if (currentMissionIndex < missions.Count)
        {
            Missions currentMission = missions[currentMissionIndex];
            if (currentMissionIndex == 0)
            {
                countText.text = $"{currentMission.description}\nItems collected: {playerInventory.weaponInventory.Count}/{currentMission.requiredItemCount}";
            }
            else if (currentMissionIndex == 1)
            {
                countText.text = $"{currentMission.description}\nDefeat the enemy!";
            }
        }
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
