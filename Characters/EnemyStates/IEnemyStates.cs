using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStates
{
    void Execute();
    void Enter(Enemy enemy);
    void Exit();
}
