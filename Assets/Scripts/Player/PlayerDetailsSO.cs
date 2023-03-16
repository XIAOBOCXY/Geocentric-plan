using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("PLAYER BASE DETAILS")]
    public string playerCharacterName;
    public GameObject playerPrefab;
    public RuntimeAnimatorController runtimeAnimatorController;

    [Space(10)]
    [Header("HEALTH")]
    public int playerHealthAmount;

    [Space(10)]
    [Header("OTHER")]
    public Sprite playerMinimapIcon;
    public Sprite playerHandSprite;

    #region VALIDATION
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtlities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtlities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtlities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);
        HelperUtlities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtlities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
    }
#endif
    #endregion
}
