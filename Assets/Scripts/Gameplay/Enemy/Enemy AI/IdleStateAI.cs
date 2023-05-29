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

        }

        public void ChaseState() => enemy.currentState = enemy.chaseState;
        public void IdleState() => enemy.currentState = enemy.idleState;
        public void AttackState() => enemy.currentState = enemy.attackState;
    }
}