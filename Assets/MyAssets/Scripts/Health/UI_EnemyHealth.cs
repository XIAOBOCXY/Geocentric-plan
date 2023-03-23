using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyHealth : UI_Health
{

    protected override Transform GetTarget()
    {
        return GetComponentInParent<Enemy>().transform;
    }

}
