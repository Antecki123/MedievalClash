using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Enemy
{
    [SelectionBase, DisallowMultipleComponent, RequireComponent(typeof(Outline)), RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] internal EnemyData enemyData;

        internal CharacterAnimations animations;

        [Header("Enemy AI")]
        internal IdleStateAI idleState;
        internal ChaseStateAI chaseState;
        internal AttackStateAI attackState;
        internal IEnemyAI currentState;

        [Header("Attack")]
        [SerializeField] internal ClientCharacter target;
        [SerializeField] internal float attackCooldown;

        [Header("Navigation")]
        internal NavMeshAgent navMeshAgent;
        internal Transform enemyTransform;
        internal Vector3 startPosition;

        private Outline outline;
        private Collider enemyColider;

        private NetworkVariable<float> netHealth = new NetworkVariable<float>(0);

        private void Awake()
        {
            enemyTransform = transform;
            startPosition = transform.position;

            navMeshAgent = GetComponent<NavMeshAgent>();
            animations = new CharacterAnimations(GetComponentInChildren<Animator>(), GetComponentInChildren<ClientNetworkAnimator>());
            enemyColider = GetComponent<Collider>();
            outline = GetComponent<Outline>();

            idleState = new IdleStateAI(this);
            chaseState = new ChaseStateAI(this);
            attackState = new AttackStateAI(this);
        }

        public override void OnNetworkSpawn()
        {
            name = enemyData.enemyName;
            navMeshAgent.speed = enemyData.velocity;
            netHealth.Value = enemyData.baseHealth;
            outline.enabled = false;

            currentState = idleState;
            attackCooldown = enemyData.attackCooldown;
        }

        private void Update() => currentState.UpdateAction();
        private void LateUpdate() => animations.Movement(navMeshAgent.velocity.magnitude);

        private void OnMouseEnter() => outline.enabled = true;
        private void OnMouseExit() => outline.enabled = false;

        public void DealDamage(float damage)
        {
            DealDamageServerRPC(damage);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DealDamageServerRPC(float damage)
        {
            netHealth.Value -= damage;

            if (netHealth.Value > 0)
                DealDamageClientRpc();
            else
                KillEnemyClientRpc();
        }

        [ClientRpc]
        private void DealDamageClientRpc()
        {
            animations.GetHit();
        }

        [ClientRpc]
        private void KillEnemyClientRpc()
        {
            StopAllCoroutines();
            animations.Death();

            enemyColider.enabled = false;
            navMeshAgent.enabled = false;
            enabled = false;
        }
    }
}