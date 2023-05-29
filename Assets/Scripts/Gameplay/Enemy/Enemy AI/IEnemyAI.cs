public interface IEnemyAI
{
    public void UpdateAction();
    public void IdleState();
    public void ChaseState();
    public void AttackState();
}