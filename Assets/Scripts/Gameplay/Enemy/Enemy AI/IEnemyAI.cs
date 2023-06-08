public interface IEnemyAI
{
    public void UpdateAction();

    public void ToIdleState();
    public void ToChaseState();
    public void ToAttackState();
}