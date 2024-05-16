using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopup : Generic.Singleton<InfoPopup>
{
    public enum MESSAGE { DEFAULT = 0
                        , DO_MOVE = 1
                        , IT_IS_MOVE_GAGE = 2
                        , IT_IS_TURN_END = 3
                        , COMBAT_TURN = 4
                        , COMBAT_ACTION = 5
                        , COMBAT_HP = 6};

    private Vector2[] prePosition = { new Vector2(0, 0) // default 0
                                    , new Vector2(-140, -35) // �÷��̾� ��ġ �� 1
                                    , new Vector2(79, -360) // �ൿ�� �� 2
                                    , new Vector2(447, -360) // ���� �� 3
                                    , new Vector2(-494, 376) // ���� �� ui(���) ���ʾƷ� 4
                                    , new Vector2(-140, -35) // �÷��̾� ��ġ �� 5
                                    , new Vector2(79, -360) // ü�� ��
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
        _curInfo = Instantiate(_infoMessagePrefab);
        _curInfo.transform.parent = _canvas;
        var anchorPos = prePosition[(int)message];
        _curInfo.GetComponent<RectTransform>().anchoredPosition = anchorPos;
        _curInfo.GetComponentInChildren<TMPro.TMP_Text>().text = _infoMessage[((int)message)];

        // delete�� ��ư���� �Ҵ�
    }
}
