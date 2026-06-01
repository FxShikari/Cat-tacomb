using UnityEngine;

public interface IAttackable
{
    public abstract void GetAttacked(int damage);
    public abstract void PlayhitFx(FX hitFx, Vector3 positionFx);
}
