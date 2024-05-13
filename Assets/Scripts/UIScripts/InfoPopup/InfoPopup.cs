using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopup : Generic.Singleton<InfoPopup>
{
    public enum MESSAGE { DEFAULT = 0
                        , DO_MOVE = 1
                        , IT_IS_MOVE_GAGE = 2
                        , IT_IS_TURN_END = 3
                        , DO_ACTION = 4};

    private Vector2[] prePosition = { new Vector2(0, 0) // default
                                    , new Vector2(-140, -35) // �÷��̾� ��ġ ��
                                    , new Vector2(133, -360) // �ൿ�� ��
                                    , new Vector2(447, -360) // ���� ��
    };

    [SerializeReference]
    private GameObject _infoMessagePrefab;

    [SerializeReference]
    private Transform _canvas;
    
    private readonly string INFO_POPUP_LOCALIZATION_PATH = $"InfoPopupLocalizationTable";
    private Dictionary<int, string> _infoMessage;
    private GameObject _curInfo;

    protected override void Awake()
    {
        base.Awake();
        FileRead.ParseLocalization(INFO_POPUP_LOCALIZATION_PATH, out _infoMessage);
    }

    public void Show(MESSAGE message)
    {
        if (_curInfo)
            Destroy(_curInfo);

        _curInfo = Instantiate(_infoMessagePrefab);
        _curInfo.transform.parent = _canvas;
        var anchorPos = prePosition[(int)message];
        _curInfo.GetComponent<RectTransform>().anchoredPosition = anchorPos;
        _curInfo.GetComponentInChildren<TMPro.TMP_Text>().text = _infoMessage[((int)message)];
        Invoke("CloseCurInfo", 5.0f);
    }

    private void CloseCurInfo()
    {
        if (_curInfo)
            _curInfo.SetActive(false);
    }
}
