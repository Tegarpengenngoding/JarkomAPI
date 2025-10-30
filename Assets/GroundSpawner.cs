using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab;   // prefab ground
    public Transform player;          // referensi ke player
    public float groundLength = 10f;  // panjang tiap potong ground
    public int numberOfGrounds = 3;   // berapa potong aktif di layar

    private GameObject[] grounds;
    private int nextGroundIndex = 0;

    void Start()
    {
        // buat pool ground
        grounds = new GameObject[numberOfGrounds];
        for (int i = 0; i < numberOfGrounds; i++)
        {
            Vector3 pos = new Vector3(i * groundLength, 0, 0);
            grounds[i] = Instantiate(groundPrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        // cek apakah player sudah melewati ground tertentu
        if (player.position.x > grounds[nextGroundIndex].transform.position.x + groundLength)
        {
            // pindahkan ground ke depan
            Vector3 newPos = grounds[nextGroundIndex].transform.position;
            newPos.x += groundLength * numberOfGrounds;
            grounds[nextGroundIndex].transform.position = newPos;

            // update index
            nextGroundIndex = (nextGroundIndex + 1) % numberOfGrounds;
        }
    }
}
