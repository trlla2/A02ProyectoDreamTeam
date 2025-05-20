using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MarchingSquares marchingSquares;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject hitmarkerPrefab;
    [SerializeField] private Vector2 spawnDelayRange = new Vector2(2f, 5f);

    [Header("Movement Settings")]
    [SerializeField] private float meteorSpeed = 5f;
    [SerializeField] private float spawnDistance = 15f;

    private Camera mainCamera;
    private List<Vector2> validWorldPositions = new List<Vector2>();

    private void Start()
    {
        mainCamera = Camera.main;
        PopulateWorldPositions();
    }

    private void PopulateWorldPositions()
    {
        if (marchingSquares == null || marchingSquares.validPositions.Count == 0)
        {
            Debug.LogError("Missing MarchingSquares reference or no valid positions!");
            return;
        }

        foreach (Vector2Int gridPos in marchingSquares.validPositions)
        {
            validWorldPositions.Add(new Vector3(gridPos.x * marchingSquares.gridResolution, gridPos.y * marchingSquares.gridResolution, 0));
        }
    }

    float TimeToStart = 2.0f;
    private void Update()
    {
        TimeToStart -= Time.deltaTime;
        if (TimeToStart < 0)
        {
            TimeToStart = Random.Range(spawnDelayRange.x, spawnDelayRange.y);
            StartCoroutine(SpawnMeteorSequence());
        }
    }

    private IEnumerator SpawnMeteorSequence()
    {
        if (validWorldPositions.Count == 0) yield break;

        //Random
        Vector2 targetPosition = validWorldPositions[Random.Range(0, validWorldPositions.Count)];

        GameObject hitmarker = Instantiate(hitmarkerPrefab, targetPosition, Quaternion.identity);

        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        Vector2 spawnDirection = new Vector2(randomX, randomY);
        spawnDirection.Normalize();

        Vector2 startPosition = targetPosition + spawnDirection * spawnDistance; //move asteroid outside following a direction

        //Spawn meteor
        GameObject meteor = Instantiate(meteorPrefab, startPosition, Quaternion.identity);
        Animator meteorAnimator = meteor.GetComponent<Animator>();

        float Dist = Vector2.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        //Move asteroid to target
        while (Vector2.Distance(meteor.transform.position, targetPosition) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * meteorSpeed;
            float t = distanceCovered / Dist;
            meteor.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        meteor.transform.position = targetPosition;

       
        meteorAnimator.SetBool("Exploded", true);
        Destroy(hitmarker);

        // Detect players in explosion radius
        Collider[] hits = Physics.OverlapSphere(targetPosition, 0.2f);

        foreach (Collider hit in hits)
        {

            Tank_Behaviour tank = hit.GetComponent<Tank_Behaviour>();
            Interactable Int = hit.GetComponent<Interactable>();

            if (tank != null)
            {
                // Check which player was killed
                if (tank.GetPlayer() == 1)
                {
                    GameManager.Instance.GetTank1IsDead();
                    tank.Dead();
                }
                else if (tank.GetPlayer() == 2)
                {
                    GameManager.Instance.GetTank2IsDead();
                    tank.Dead();
                }
            }
            if (Int != null)
            {
                Int.BulletHit();
            }
        }

        yield return new WaitForSeconds(0.5f); //duration of explode animation clip, imp no exit time is required
        Destroy(meteor);
    }
}