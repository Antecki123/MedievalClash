using System.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerData playerData;

    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private CharacterAnimations animations;

    private InputSystem input;

    private float turnSmoothVelocity;
    private Vector2 movemementInput = Vector2.zero;

    private bool attackPossible = true;

    private void Awake()
    {
        playerTransform = transform;
        playerRigidbody = GetComponent<Rigidbody>();
        animations = new CharacterAnimations(GetComponentInChildren<Animator>(), GetComponentInChildren<ClientNetworkAnimator>());

        input = new InputSystem();

        input.Gameplay.Movement.performed += ctx => movemementInput = ctx.ReadValue<Vector2>();
        input.Gameplay.Movement.canceled += ctx => movemementInput = Vector2.zero;

        input.Gameplay.Action.started += ctx => Action();
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        if (!IsOwner) return;

        Movement();
        animations.Movement(playerRigidbody.velocity.magnitude);
    }

    private void Movement()
    {
        var direction = new Vector3(movemementInput.x, 0f, movemementInput.y).normalized;

        var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angleSmoothness = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, angle, ref turnSmoothVelocity, playerData.turnSmooth);

        if (movemementInput.magnitude > 0.1f)
        {
            playerRigidbody.velocity = playerData.velocity * direction;
            playerTransform.rotation = Quaternion.Euler(0f, angleSmoothness, 0f);
        }
        else
        {
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    private void Action()
    {
        Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        var distance = Vector3.Distance(playerTransform.transform.position, hit.point);
        if (distance > playerData.interactionRange) return;

        if (attackPossible && hit.collider.TryGetComponent(out IDamageable damageable)) Attack(hit, damageable);
        else if (hit.collider.TryGetComponent(out IInteractable interactable)) Interaction(hit, interactable);
    }

    private void Interaction(RaycastHit target, IInteractable interactable)
    {
        Debug.Log("Interact");
        if (!interactable.IsInteractable()) return;
        
        playerTransform.LookAt(target.transform);

        interactable.Interact(GetComponent<ClientCharacter>()); // cache player
        if (IsOwner) animations.Interact();
    }

    private void Attack(RaycastHit target, IDamageable damageable)
    {
        Debug.Log("Attack Melee");

        attackPossible = false;
        StartCoroutine(AttackCooldown(1f));

        playerTransform.LookAt(target.transform);

        damageable.DealDamage(playerData.meleeAttackStrength);
        if (IsOwner) animations.Attack();
    }

    private IEnumerator AttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        attackPossible = true;
    }
}