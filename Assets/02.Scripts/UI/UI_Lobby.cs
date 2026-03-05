using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;
    
    private ECharacterType _characterType;
    
    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);
    
    private void OnClickCharacterButton(ECharacterType gender)
    {
        _characterType = gender;
        
        _maleCharacter.SetActive(_characterType == ECharacterType.Male);
        _femaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }

}
