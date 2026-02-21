using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Alive))]
public class NPCShooter : MonoBehaviour
{
    [Header("Detekcja")]
    [Tooltip("Obiekt gracza do ataku. Zostaw puste, by auto-szukać tagu 'Player'.")]
    public Transform player;
    [Tooltip("Zasięg wykrywania gracza.")]
    public float detectionRange = 15f;

    [Header("Atak")]
    [Tooltip("Odstęp między strzałami w sekundach.")]
    public float fireRate = 1.5f;
    [Tooltip("Punkt, z którego wylatują pociski (np. lufa broni). Jeśli puste, użyje środka NPC.")]
    public Transform firePoint;
    
    [Header("Efekty")]
    [Tooltip("Prefab smugi pocisku (np. Line Renderer).")]
    public GameObject bulletTrailPrefab;

    private bool isPlayerInRange = false;
    private float nextFireTime = 0f;
    private Alive aliveScript;

    void Start()
    {
        aliveScript = GetComponent<Alive>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        if (player == null || aliveScript == null || !aliveScript.isAlive)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isPlayerInRange = true;
        }

        if (isPlayerInRange)
        {
            // Opcjonalnie: obróć w stronę gracza
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0; // zablokuj pochył
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
            }

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        // Tutaj logiki strzału - odtworzenie dźwięku, błysku, itp.
        
        if (bulletTrailPrefab != null)
        {
            GameObject trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
            LineRenderer lr = trail.GetComponent<LineRenderer>();
            
            if (lr != null)
            {
                // Ustaw punkt początkowy na lufie broni
                lr.SetPosition(0, firePoint.position);
                
                // Kierunek strzału to gracz + np. lekki offset na wysokość brzucha/klatki piersiowej
                Vector3 targetPosition = player.position + Vector3.up * 1f;

                // Ustaw punkt końcowy (na graczu lub tam, gdzie poleciał pocisk)
                lr.SetPosition(1, targetPosition);
            }

            // Zniszcz smugę po krótkim czasie, np. po 0.1s
            Destroy(trail, 0.1f);
        }
        else
        {
            // Fallback: Jeśli nie przypiszesz prefaba, domyślnie rysuje "promień" w trybie testowym okna Scene
            Debug.DrawLine(firePoint.position, player.position, Color.yellow, 0.1f);
        }

        // TODO: Nałóż obrażenia graczowi (np. przez odpytanie gracza o skrypt HP i zabranie mu punktów życia).
        // if(player.TryGetComponent(out PlayerHealth hp)) hp.TakeDamage(10);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
