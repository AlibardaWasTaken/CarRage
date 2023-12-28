using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineHazard : AbstractHazard
{

    

    protected override void DoAction()
    {
        Instantiate(ParticleHolder.ExpParticle, this.transform.position, Quaternion.identity, null);
        Instantiate(ParticleHolder.ExpParticle, CarInputHandler.Instance.topDownCarController.gameObject.transform.position, Quaternion.identity, null);
        CommonSoundManager.Instance.ExpContainer.PlayRandom();
        BloodManager.Instance.RemoveBlood(Mathf.Clamp(7 - GameManager.ValueHolder.EnumsValuesDictionary[UpgradeEnums.Armor], 1, 99)/2);
        trapobject.SetActive(false);
    }

    protected override void DoEnemyAction(TopDownEnemyCar Enemy)
    {
        Enemy.DoDamage(2);
    }
}
