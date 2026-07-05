using UnityEngine;
using System.Collections;


public class SkillController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform; 
    public Transform firePoint;
    private AudioSource audioSource; 

    [Header("Audio")]
    public AudioClip scanSound;     
    public AudioClip ultimateSound;  

    [Header("Tactical: Hybrid Scan")]
    public float scanRadius = 20f;
    public float scanAngle = 45f; 
    public float slowMultiplier = 0.3f; 
    public float scanDuration = 3f;
    public GameObject scanVisualPrefab; 

    [Header("Ultimate: Piercing Beam")]
    public float beamRange = 30f;
    public float beamRadius = 2.0f;
    public float beamDamage = 80f;
    public float beamRevealDuration = 1.5f;
    public GameObject beamVisualPrefab; 

    void Start()
    {
       
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ExecuteScan();
        if (Input.GetKeyDown(KeyCode.X)) ExecuteUltimateBeam();
    }

    void ExecuteScan()
    {
        
        if (scanSound != null) audioSource.PlayOneShot(scanSound);

        StartCoroutine(ScanVisualRoutine());

        Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius);
        foreach (Collider hit in hits)
        {
            DummyController dummy = hit.GetComponent<DummyController>();
            if (dummy != null)
            {
                Vector3 directionToTarget = (dummy.transform.position - transform.position).normalized;
                if (Vector3.Angle(cameraTransform.forward, directionToTarget) < scanAngle)
                {
                    dummy.ApplyReveal(scanDuration);
                    dummy.ApplySlow(slowMultiplier, scanDuration);
                }
            }
        }
    }

    void ExecuteUltimateBeam()
    {
       
        if (ultimateSound != null) audioSource.PlayOneShot(ultimateSound);

        StartCoroutine(BeamVisualRoutine());

        RaycastHit[] hits = Physics.SphereCastAll(cameraTransform.position, beamRadius, cameraTransform.forward, beamRange);
        foreach (RaycastHit hit in hits)
        {
            DummyController dummy = hit.collider.GetComponent<DummyController>();
            if (dummy != null)
            {
                dummy.TakeDamage(beamDamage);
                dummy.ApplyReveal(beamRevealDuration);
            }
        }
    }

    private IEnumerator ScanVisualRoutine()
    {
        if (scanVisualPrefab == null) yield break;

        GameObject visual = Instantiate(scanVisualPrefab, transform.position, Quaternion.identity);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(0, scanRadius * 2, elapsed / duration);
            visual.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Destroy(visual);
    }

    private IEnumerator BeamVisualRoutine()
    {
        if (beamVisualPrefab == null || firePoint == null) yield break;

        Vector3 spawnPosition = firePoint.position + (firePoint.forward * (beamRange / 2f));
        GameObject beam = Instantiate(beamVisualPrefab, spawnPosition, firePoint.rotation);
        
      
        beam.transform.rotation = firePoint.rotation * Quaternion.Euler(90f, 0f, 0f);

        float diameter = beamRadius * 2f;
        float halfLength = beamRange / 2f;
        beam.transform.localScale = new Vector3(diameter, halfLength, diameter);

        yield return new WaitForSeconds(0.4f);
        Destroy(beam);
    }
}