using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISummoner
{
    void ReceiveMinion(Actor actor);

    void RemoveMinion(Actor actor);
}

public interface IMinion
{
    void ReceiveMaster(Actor actor);

    void OnDeath();
}
