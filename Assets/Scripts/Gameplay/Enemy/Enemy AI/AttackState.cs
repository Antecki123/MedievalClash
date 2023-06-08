using System.Threading.Tasks;
using UnityEngine;

namespace RPG.Enemy
{
    public class AttackStateAI : IEnemyAI
    {
        private Enemy enemy;

        private int waitForAtackAnimationInMillis = 1000;

        public AttackStateAI(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateAction()
        {
            PerformAttack();
        }

        #region Interface
        public void ToChaseState() => enemy.currentState = enemy.chaseState;
        public void ToIdleState() => enemy.currentState = enemy.idleState;
        public void ToAttackState() => enemy.currentState = enemy.attackState;
        #endregion

        private async void PerformAttack()
        {
            if (enemy.attackCooldown >= enemy.enemyData.attackCooldown)
            {
                enemy.navMeshAgent.speed = 0;
                enemy.animations.Attack();

                enemy.attackCooldown = 0;
                ResetTimer();

                await Task.Delay(waitForAtackAnimationInMillis);

                enemy.navMeshAgent.speed = enemy.enemyData.velocity;
            }

            ToChaseState();
        }

        private async void ResetTimer()
        {
            while (enemy.attackCooldown < enemy.enemyData.attackCooldown)
            {
                enemy.attackCooldown += Time.deltaTime;
                await Task.Yield();
            }
        }
    }
}