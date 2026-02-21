using System.Collections;
using UnityEngine;

public class NPCFlee : MonoBehaviour
{
    [Header("Ustawienia ucieczki")]
    [Tooltip("Obiekt gracza. Jeśli puste, skrypt poszuka obiektu z tagiem 'Player'.")]
    public Transform player;
    
    [Tooltip("Odległość, przy której NPC zaczyna uciekać.")]
    public float detectionRange = 5f;
    
    [Tooltip("Prędkość ucieczki NPC.")]
    public float runSpeed = 5f;

    private bool isFleeing = false;
    private Alive aliveScript;

    public Animator animator;

    void Start()
    {
        aliveScript = GetComponent<Alive>();
        // Jeśli nie przypisano gracza w inspektorze, spróbuj znaleźć go po tagu "Player"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono gracza w skrypcie NPCFlee. Upewnij się, że gracz ma tag 'Player' lub przypisz go ręcznie w inspektorze.");
            }
        }
    }

    IEnumerator run()
    {
        yield return new WaitForSeconds(1f);
        isFleeing = true;
    }

    void Update()
    {
        if (player == null) return;

        // Obliczamy odległość między NPC a graczem
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Jeśli gracz znajduje się w zasięgu wykrywania
        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(run());
            animator.SetBool("run", true);
        }

        if(!aliveScript.isAlive)
        {
            animator.speed = 0;
        }
        // Jeśli NPC zaczął już uciekać a skrypt Alive pozwala na ruch (żyje)
        if (isFleeing && (aliveScript == null || aliveScript.isAlive))
        {
            // Obliczamy kierunek ucieczki (przeciwny do pozycji gracza)
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            
            // Zerujemy oś Y, aby NPC nie unosił się w powietrze ani nie wbijał w ziemię
            fleeDirection.y = 0;

            // Przesuwamy NPC w wyznaczonym kierunku
            transform.position += fleeDirection * runSpeed * Time.deltaTime;

            // Obracamy NPC w stronę, w którą ucieka, by ruch wyglądał naturalnie
            if (fleeDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    // Ten kod rysuje pomocniczą sferę w edytorze Unity (gdy zaznaczysz NPC), pokazującą zasięg wykrywania
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
