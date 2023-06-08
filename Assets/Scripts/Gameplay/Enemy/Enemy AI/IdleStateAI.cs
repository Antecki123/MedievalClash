using UnityEngine;

namespace RPG.Enemy
{
    public class IdleStateAI : IEnemyAI
    {
        private Enemy enemy;

        public IdleStateAI(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateAction()
        {
            Surveillance();
        }

        #region Interface
        public void ToChaseState() => enemy.currentState = enemy.chaseState;
        public void ToIdleState() => enemy.currentState = enemy.idleState;
        public void ToAttackState() => enemy.currentState = enemy.attackState;
        #endregion

        private void Surveillance()
        {
            if (ClientCharacter.Players.Count == 0) return;

            foreach (var player in ClientCharacter.Players)
            {
                if (Vector3.Distance(player.transform.position, enemy.enemyTransform.position) <= enemy.enemyData.surveillanceRange)
                {
                    enemy.target = player;
                    ToChaseState();
                    return;
                }
            }

            if (enemy.target == null)
            {
                ReturnToOrigin();
            }
        }

        private void ReturnToOrigin()
        {
            enemy.navMeshAgent.destination = enemy.startPosition;
        }
    }
}