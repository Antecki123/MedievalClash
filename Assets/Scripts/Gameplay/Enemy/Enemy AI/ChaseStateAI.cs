using UnityEngine;

namespace RPG.Enemy
{
    public class ChaseStateAI : IEnemyAI
    {
        private Enemy enemy;

        public ChaseStateAI(Enemy enemy)
        {
            this.enemy = enemy;
        }
        public void UpdateAction()
        {
            ChaseEnemy();
        }

        public void ChaseState() => enemy.currentState = enemy.chaseState;
        public void IdleState() => enemy.currentState = enemy.idleState;
        public void AttackState() => enemy.currentState = enemy.attackState;

        private void ChaseEnemy()
        {
            enemy.target = Object.FindFirstObjectByType<PlayerNetwork>().transform;
            if (enemy.target == null) return;

            enemy.navMeshAgent.destination = enemy.target.position;

            if (Vector3.Distance(enemy.transform.position, enemy.target.position) <= 2f)
            {
                AttackState();
            }
        }
    }
}