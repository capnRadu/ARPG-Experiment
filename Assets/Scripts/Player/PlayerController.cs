using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private InputAction setDestination;
    private PlayerCombat playerCombatScript;

    private NavMeshAgent agent;
    private Vector3 cachedDestination;
    private float speed = 5f;
    private Coroutine[] coroutines = new Coroutine[1];
    private bool isKeyHeld = false;

    private Animator animator;

    private const float ATTACK_RANGE = 2f;
    private const float DEFAULT_Y_POSITION = 0.493f;
    private const float MOVE_RANGE = 0.1f;

    private void Awake()
    {
        inputManager = new InputManager();
        playerCombatScript = GetComponent<PlayerCombat>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        setDestination = inputManager.Player.SetDestination;
        setDestination.Enable();
        setDestination.performed += SetDestinationPerformed;
        setDestination.canceled += SetDestinationCanceled;
    }

    private void OnDisable()
    {
        setDestination.Disable();
        setDestination.performed -= SetDestinationPerformed;
        setDestination.canceled -= SetDestinationCanceled;
    }

    // Button is first pressed, but not yet released
    private void SetDestinationPerformed(InputAction.CallbackContext context)
    {
        isKeyHeld = true;
        StartFollowingDestination();
    }

    // Button is released
    private void SetDestinationCanceled(InputAction.CallbackContext context)
    {
        isKeyHeld = false;
        HandleInteractionOrMove();
    }

    // Starts the coroutine to follow the destination
    private void StartFollowingDestination()
    {
        if (!playerCombatScript.IsCasting)
        {
            StopRunningCoroutine(0, false);
            coroutines[0] = StartCoroutine(FollowToDestination());
        }
    }

    // Follow the destination while the button is held
    private IEnumerator FollowToDestination()
    {
        while (isKeyHeld)
        {
            Vector3 destination;
            GameObject hitObject;
            bool isObjectDamageable;

            GetLocationUnderMouse(out destination, out hitObject, out isObjectDamageable);

            if (destination != Vector3.zero && !isObjectDamageable)
            {
                MoveAndRotate(destination);
                SetWalkingAnimation(true);
            }

            yield return null;
        }

        SetWalkingAnimation(false);
    }

    private void HandleInteractionOrMove()
    {
        Vector3 destination;
        GameObject hitObject;
        bool isObjectDamageable;

        GetLocationUnderMouse(out destination, out hitObject, out isObjectDamageable);

        if (isObjectDamageable && IsWithinAttackRange(hitObject.transform.position))
        {
            PerformAttack(hitObject);
        }
        else if (destination != Vector3.zero)
        {
            if (!playerCombatScript.IsCasting)
            {
                StartMoveTo(destination);
            }
        }
    }

    // Starts the coroutine to move the player to the target destination
    private void StartMoveTo(Vector3 destination)
    {
        StopRunningCoroutine(0, false);
        cachedDestination = destination;
        coroutines[0] = StartCoroutine(MoveToDestination(cachedDestination, MOVE_RANGE));
    }

    private bool IsWithinAttackRange(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= ATTACK_RANGE;
    }

    private void PerformAttack(GameObject target)
    {
        playerCombatScript.PrimaryAttack(target);
        StopRunningCoroutine(0, true);
        StartCoroutine(LookAtTarget(target.transform));
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator.GetBool("isWalking") != isWalking)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    public void StopRunningCoroutine(int index, bool stopMoving)
    {
        if (coroutines[index] != null)
        {
            StopCoroutine(coroutines[index]);
            coroutines[index] = null;

            if (stopMoving)
            {
                agent.ResetPath();
                SetWalkingAnimation(false);
            }
        }
    }

    // Finds the location under the mouse, and potentially the hit object
    private void GetLocationUnderMouse(out Vector3 destination, out GameObject hitObject, out bool isObjectDamageable)
    {
        destination = Vector3.zero;
        hitObject = null;
        isObjectDamageable = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            destination = hit.point;
            destination.y = DEFAULT_Y_POSITION; // Adjust y position to player's height

            hitObject = hit.collider.gameObject;
            isObjectDamageable = hitObject?.GetComponent<IDamageable>() != null;
        }
    }

    // Coroutine to move to a set destination
    private IEnumerator MoveToDestination(Vector3 destination, float range = MOVE_RANGE)
    {
        SetWalkingAnimation(true);

        while (Vector3.Distance(transform.position, destination) > range)
        {
            MoveAndRotate(destination);
            yield return null;
        }

        StopRunningCoroutine(0, true);
    }

    // Method to handle movement and rotation towards a destination
    private void MoveAndRotate(Vector3 destination)
    {
        if (agent.pathPending) return;

        agent.SetDestination(destination);
        agent.speed = speed;

        RotateTowards(destination);

        if (agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            StopRunningCoroutine(0, true);
        }
    }

    // Rotate smoothly towards the destination
    private void RotateTowards(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    // Coroutine to look at the target when starting an attack
    private IEnumerator LookAtTarget(Transform target)
    {
        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.position - transform.position)) > 10f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 10f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}