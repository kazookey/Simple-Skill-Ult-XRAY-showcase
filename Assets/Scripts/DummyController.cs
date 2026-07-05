using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DummyController : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    
    
    public float baseSpeed = 2f;
    public float moveDistance = 4f;
    private float currentSpeed;
    private Vector3 startPos;
    private float timeTrack;

    [Header("Visuals")]
    public Renderer rend;
    public Material xrayMaterial; 
    private Material originalMaterial;

    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
        startPos = transform.position;
        
        if (rend == null) rend = GetComponent<Renderer>();
        
        
        originalMaterial = rend.material; 
    }

    void Update()
    {
        timeTrack += Time.deltaTime * currentSpeed;
        float offset = Mathf.Sin(timeTrack) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void ApplySlow(float speedMultiplier, float duration)
    {
        StartCoroutine(SlowRoutine(speedMultiplier, duration));
    }

    private IEnumerator SlowRoutine(float speedMultiplier, float duration)
    {
        currentSpeed = baseSpeed * speedMultiplier;
        yield return new WaitForSeconds(duration);
        currentSpeed = baseSpeed;
    }

    public void ApplyReveal(float duration)
    {
        StartCoroutine(RevealRoutine(duration));
    }

    private IEnumerator RevealRoutine(float duration)
    {
       
        if (xrayMaterial != null) rend.material = xrayMaterial;
        
        yield return new WaitForSeconds(duration);
        
       
        rend.material = originalMaterial;
    }
}