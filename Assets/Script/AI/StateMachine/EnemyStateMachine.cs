public class EnemyStateMachine
{
    public EnemyState currentEnemyState;

    public void Inizialiaze(EnemyState state)
    {
        currentEnemyState = state;
        currentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState state)
    {
        currentEnemyState.ExitState();
        currentEnemyState = state;
        currentEnemyState.EnterState();
    }
}
