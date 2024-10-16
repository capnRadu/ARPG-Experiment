using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector3 randomPoint = GetRandomPoint(transform.position, 10f);

            GameObject enemy = Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            enemy.GetComponent<Enemy>().player = player;

            yield return new WaitForSeconds(8f);
        }
    }

    public Vector3 GetRandomPoint(Vector3 center, float maxDistance)
    {
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
