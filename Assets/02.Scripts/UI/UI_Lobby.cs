using TMPro;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;
    [SerializeField] private TMP_InputField _nickNameInputField;
    [SerializeField] private TMP_InputField _roomNameInputField;


    private ECharacterType _characterType;

    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);

    private void OnClickCharacterButton(ECharacterType gender)
    {
        _characterType = gender;

        _maleCharacter.SetActive(_characterType == ECharacterType.Male);
        _femaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }

    public void MakeRoom()
    {
        string nickname = _nickNameInputField.text;
        string roomName = _roomNameInputField.text;
        // Nickname 도메인 분리 후 유효성 검사를 해야 된다.
        // 유효성 검사는 명세 패턴으로 분리
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName)) return;

        var request = new RoomCreationInfo(roomName, nickname);
        PhotonRoomManager.Instance.CreateRoom(request);
    }
}
