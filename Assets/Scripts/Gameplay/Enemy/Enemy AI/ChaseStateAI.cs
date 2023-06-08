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

        #region Interface
        public void ToChaseState() => enemy.currentState = enemy.chaseState;
        public void ToIdleState() => enemy.currentState = enemy.idleState;
        public void ToAttackState() => enemy.currentState = enemy.attackState;
        #endregion

        private void ChaseEnemy()
        {
            if (enemy.target == null) ToIdleState();

            var distance = Vector3.Distance(enemy.target.transform.position, enemy.enemyTransform.position);

            enemy.navMeshAgent.destination = enemy.target.transform.position;

            if (distance <= enemy.enemyData.attackRange)
            {
                enemy.navMeshAgent.destination = enemy.enemyTransform.position;
                ToAttackState();
            }

            else if (distance > enemy.enemyData.surveillanceRange)
            {
                enemy.target = null;
                ToIdleState();
            }
        }
    }
}