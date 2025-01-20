using PilotoStudio;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_Shoot : MonoBehaviour
{
    // Combat
    public float laserRange;
    public float laserRechargeTime = 2f;
    private float laserUsageTime = 0f;
    private float maxUsageTime = 3f;
    private bool isOverheated = false;

    // Modes 
    private float currentModeDamage = 0.1f;
    public float plasmaDamage = 0.1f;
    public float stasisDamage = 0.002f;
    public float stasisSlowMultiplier = 0.999f;

    // Interaction with puzzles
    private float holdTimer;
    public float requiredHoldTime = 2f;

    private PlayerControls playerControls;
    private InputAction fireAction;
    private InputAction switchModeAction;

    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private Camera mainCamera;

    // Particle stuff
    private BeamEmitter beamEmitter;
    private GameObject currentModeParticle;
    public GameObject plasmaParticle;
    public GameObject stasisParticle;

    RaycastHit hitInfo;

    [HideInInspector] public LaserMode currentMode = LaserMode.PLASMA;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        fireAction = playerControls.Player.Fire;
        switchModeAction = playerControls.Player.SwitchMode;
        switchModeAction.Enable();
        fireAction.Enable();

        switchModeAction.performed += ToggleLaserMode;

        InitializeLaserMode(); // Initialize the correct mode on enable               
    }

    private void OnDisable()
    {
        fireAction.Disable();
        switchModeAction.Disable();

        switchModeAction.performed -= ToggleLaserMode;
    }

    void Update()
    {
        if (!isOverheated)
        {
            Shoot();
        }
        else
        {
            StartCoroutine(RechargeLaser());
        }
    }

    public void Shoot()
    {
        if (fireAction.IsPressed() && laserUsageTime < maxUsageTime)
        {
            laserUsageTime += Time.deltaTime;

            laserLine.enabled = true;
            laserLine.SetPosition(0, laserOrigin.position);

            Ray ray = new Ray(laserOrigin.position, mainCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo, laserRange))
            {
                beamEmitter.SetBeamTarget(hitInfo.point);
                laserLine.SetPosition(1, hitInfo.point);

                if (!currentModeParticle.activeSelf)
                    currentModeParticle.SetActive(true);

                if (hitInfo.collider.gameObject.TryGetComponent(out SCR_Health enemyHealth))
                {
                    enemyHealth.TakeDamage(currentModeDamage);

                    if (hitInfo.collider.gameObject.TryGetComponent(out SCR_EnemyAI enemyAI)) { 
                        
                        if (currentMode == LaserMode.STASIS)
                        {
                            enemyAI.ApplyStasis(stasisSlowMultiplier);
                        }
                    }
                   
                }

                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable_Shoot interactObject))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= requiredHoldTime)
                    {
                        interactObject.Interact();
                        holdTimer = 0f;
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, laserOrigin.position + mainCamera.transform.forward * laserRange);
            }
        }
        else
        {
            laserLine.enabled = false;

            if (currentModeParticle.activeSelf)
                currentModeParticle.SetActive(false);

            if (laserUsageTime >= maxUsageTime)
            {
                isOverheated = true;
            }
        }
    }

    private IEnumerator RechargeLaser()
    {
        yield return new WaitForSeconds(laserRechargeTime);
        laserUsageTime = 0f;
        isOverheated = false;
    }

    private void ToggleLaserMode(InputAction.CallbackContext context)
    {
        if (currentMode == LaserMode.PLASMA)
        {
            SetStasisMode();
        }
        else
        {
            SetPlasmaMode();
        }
    }

    private void SetPlasmaMode()
    {
        currentMode = LaserMode.PLASMA;
        currentModeDamage = plasmaDamage;
        laserLine.startColor = Color.red;
        laserLine.endColor = Color.yellow;

        if (currentModeParticle != null)
            currentModeParticle.SetActive(false);

        currentModeParticle = plasmaParticle;

        if (!isOverheated)
            currentModeParticle.SetActive(true);

        beamEmitter = plasmaParticle.GetComponent<BeamEmitter>();
    }

    private void SetStasisMode()
    {
        currentMode = LaserMode.STASIS;
        currentModeDamage = stasisDamage;
        laserLine.startColor = Color.blue;
        laserLine.endColor = Color.grey;

        if (currentModeParticle != null)
            currentModeParticle.SetActive(false);

        currentModeParticle = stasisParticle;

        if (!isOverheated)
            currentModeParticle.SetActive(true);

        beamEmitter = stasisParticle.GetComponent<BeamEmitter>();
    }

    private void InitializeLaserMode()
    {
        currentMode = LaserMode.PLASMA;
        SetPlasmaMode();
    }
}

public enum LaserMode
{
    PLASMA, STASIS
}