using TMPro;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Enemy
{
    [SelectionBase, DisallowMultipleComponent, RequireComponent(typeof(Outline)), RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] private TextMeshProUGUI enemyIDLabel; //DEBUG

        [SerializeField] private EnemyData enemyData;

        internal NavMeshAgent navMeshAgent { get; private set; }
        internal CharacterAnimations animations { get; private set; }

        [Header("Enemy AI")]
        internal IdleStateAI idleState;
        internal ChaseStateAI chaseState;
        internal AttackStateAI attackState;
        internal IEnemyAI currentState;

        internal Transform target;
        internal Transform startPosition;

        private Outline outline;
        private NetworkVariable<float> netHealth = new NetworkVariable<float>(0);

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animations = new CharacterAnimations(GetComponentInChildren<Animator>(), GetComponentInChildren<ClientNetworkAnimator>());
            outline = GetComponent<Outline>();

            idleState = new IdleStateAI(this);
            chaseState = new ChaseStateAI(this);
            attackState = new AttackStateAI(this);
            startPosition = transform;
        }

        public override void OnNetworkSpawn()
        {
            name = enemyData.enemyName;
            navMeshAgent.speed = enemyData.velocity;
            netHealth.Value = enemyData.baseHealth;
            outline.enabled = false;

            currentState = chaseState;
        }

        private void Update() //TODO: usunac po stworzeniu zarzadzania enemies
        {
            EnemyUpdate();
        }

        public void EnemyUpdate()
        {
            //currentState.UpdateAction();
            animations.Movement(navMeshAgent.velocity.magnitude);
        }

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
            animations.Death();
        }



        private void LateUpdate()
        {
            enemyIDLabel.text = netHealth.Value.ToString();
            enemyIDLabel.transform.LookAt(Camera.main.transform.position);
            enemyIDLabel.transform.Rotate(new Vector3(0f, 180f, 0f));
        }
    }
}