using UnityEngine;
using System.Collections;

public class DummySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject dummyPrefab;
    public int numberOfDummies = 7;
    public float spawnRadius = 8f;
    public float respawnDelay = 3f;
    
   
    private GameObject[] currentDummies;
    private bool[] isRespawning;

    void Start()
    {
        
        currentDummies = new GameObject[numberOfDummies];
        isRespawning = new bool[numberOfDummies];

        for (int i = 0; i < numberOfDummies; i++)
        {
            SpawnDummy(i);
        }
    }

    void Update()
    {
        
        for (int i = 0; i < numberOfDummies; i++)
        {
           
            if (currentDummies[i] == null && !isRespawning[i])
            {
                StartCoroutine(RespawnRoutine(i));
            }
        }
    }

    private void SpawnDummy(int index)
    {
        
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        
       
        Vector3 randomPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        
        currentDummies[index] = Instantiate(dummyPrefab, randomPos, transform.rotation);
    }

    private IEnumerator RespawnRoutine(int index)
    {
        isRespawning[index] = true;
        
        yield return new WaitForSeconds(respawnDelay);
        
        SpawnDummy(index);
        isRespawning[index] = false;
    }

    
    // unity editor gizmo to visualize the spawn radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}