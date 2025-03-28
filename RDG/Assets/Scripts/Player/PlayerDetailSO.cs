using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerDetail_", menuName ="Scriptable Objects/Player/Player Detail")]
public class PlayerDetailSO : ScriptableObject
{
    [Space(10)]
    [Header("Player base details")]
    public string playerCharacterName;

    public GameObject playerPrefab;

    public RuntimeAnimatorController runtimeAnimatorController;

    [Space(10)]
    [Header("Health")]
    public int playerHealthAmout;
    [Space(10)]
    [Header("Other")]
    public Sprite playerMiniMapIcon;
    public Sprite playerHandSprite;


#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmout), playerHealthAmout, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon), playerMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof (playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
    }
#endif
}
