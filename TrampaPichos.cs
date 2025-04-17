using UnityEngine;

public class TrampaPinchos : MonoBehaviour
{
    public Transform pinchos; // el objeto que se mueve (el mesh de pinchos)
    public float arriba = 1.5f;
    public float abajo = 0f;
    public float speed = 2f;
    public float interval = 4f;

    private bool isUp = false;
    private float timer = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = pinchos.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            isUp = !isUp; // alternar
            timer = 0f;
        }

        // Movimiento 
        Vector3 target = startPos + Vector3.up * (isUp ? arriba : abajo);
        pinchos.localPosition = Vector3.MoveTowards(pinchos.localPosition, target, speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador dañado!");
            Destroy(other.gameObject); 
        }
    }
}
