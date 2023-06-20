using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnedSkillUI : UISystem
{
    //Learned Skill UI
    [Header("Learned Skill UI")]
    public GameObject skillIconPrefab;
    private List<GameObject> _skillIconUIs = new List<GameObject>();
    [SerializeField] private GameObject _iconScrollContents;
    private readonly Vector3 ICON_INIT_POSITION = new Vector3(235, 280, 0);
    private const float ICON_INTERVAL = 100;

    private SkillManager _skillManager;

    // Start is called before the first frame update
    void Start()
    {
        _skillManager = UIManager.instance._skillManager;

        //Skill icon object pooling
        List<Skill> _skills = _skillManager.GetAllSkills();
        for (int i = 0; i < _skills.Count; i++)
        {
            Vector3 pos = ICON_INIT_POSITION;
            pos.x += i * ICON_INTERVAL;
            GameObject skillIcon = Instantiate(skillIconPrefab, pos, Quaternion.identity);
            skillIcon.transform.SetParent(_iconScrollContents.transform);

            skillIcon.SetActive(false);
            _skillIconUIs.Add(skillIcon);
        }
    }
    public override void OpenUI()
    {
        SetLearnedSkiilInfoUI();
    }
    public override void CloseUI()
    {
    }
    public void SetLearnedSkiilInfoUI()
    {
        for (int i = 0; i < _skillIconUIs.Count; i++)
        {
            _skillIconUIs[i].SetActive(false);
        }

        List<Skill> _skills = _skillManager.GetAllSkills();
        int cnt = 0;
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].isLearned)
            {
                Vector3 pos = ICON_INIT_POSITION;
                pos.x += cnt * ICON_INTERVAL;
                _skillIconUIs[i].transform.position = pos;
                _skillIconUIs[i].SetActive(true);

                cnt++;
            }
        }
        _iconScrollContents.GetComponent<RectTransform>().sizeDelta = new Vector2(cnt * ICON_INTERVAL + 25, 100);
    }

}
