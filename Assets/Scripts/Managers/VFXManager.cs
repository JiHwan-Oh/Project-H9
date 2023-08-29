using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VFXManager : Generic.Singleton<VFXManager>
{
    private Dictionary<string, GameObject> _caches;

    private class VFXWrapper
    {
        public bool Enable;
        private bool LimitedTime;
        public string Key;
        public GameObject Instance;
        public float LimitTime;
        public float LifeTime;

        public VFXWrapper(string key, GameObject instance, float limitTime, Vector3 position, Space space = Space.World)
        {
            this.Key = key;
            this.Instance = instance;
            if (space == Space.Self)
                instance.transform.localPosition = position;
            if (space == Space.World)
                instance.transform.position = position;
            this.LimitTime = limitTime;
            this.LifeTime = limitTime;
            this.Enable = true;
            if (limitTime < 0)
                LimitedTime = false;
            else
                LimitedTime = true;
        }

        public void Update()
        {
            if (LimitedTime)
            {
                LifeTime -= Time.deltaTime;
                if (LifeTime < 0)
                {
                    Enable = false;
                    if (Instance != null)
                    {
                        Instance.SetActive(false);
                    }
                }
            }

        }
    };
    private List<VFXWrapper> _instances;

    public new void Awake()
    {
        base.Awake();

        // �̱����̱⿡ �ߺ��� ������Ʈ�� Destroy�Ǿ� �ߺ��� Awake�� ���� ���� �̷������ �ʴ´ٰ� �����մϴ�.
        _caches = new Dictionary<string, GameObject>();
        _instances = new List<VFXWrapper>();
        SceneManager.sceneUnloaded += OnSceneChanged;
    }

    public void Update()
    {
        for (int i = 0; i < _instances.Count; i++)
        {
            if (_instances[i].Enable)
                _instances[i].Update();
            else
            {
                if (_instances[i].Instance)
                    Destroy(_instances[i].Instance);
                _instances.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// ��ο� �����ϴ� VFX�� �����մϴ�.
    /// </summary>
    /// <param name="key"> VFX�� ���Ե� Prefab�� Resources���� ã�� �� �ִ� path�Դϴ�. </param>
    /// <param name="limitTime"> -1�̶��, �ð��� ������ �������� �ʽ��ϴ�. </param>
    /// <param name="position"></param>
    /// <param name="space"></param>
    /// <returns></returns>
    public bool TryInstantiate(string key, float limitTime, Vector3 position, Space space = Space.World)
    {
        GameObject result = default;
        if (!_caches.ContainsKey(key))
        {
            GameObject value = Resources.Load(key) as GameObject;
            if (value == null)
            {
                return false;
            }
            _caches[key] = value;
        }

        result = Instantiate(_caches[key]);
        _instances.Add(new VFXWrapper(key, result, limitTime, position, space));
        return true;
    }

    public bool TryLineRender(string key, float limitTime, Vector3 start, Vector3 dest, Space space = Space.World)
    {
        GameObject result = default;
        LineRenderer line = default;
        if (!_caches.ContainsKey(key))
        {
            GameObject value = Resources.Load(key) as GameObject;
            if (value == null)
            {
                return false;
            }
            if (value.GetComponent<LineRenderer>() == null)
            {
                return false;
            }
            _caches[key] = value;
        }

        result = Instantiate(_caches[key]);
        line = result.GetComponent<LineRenderer>();
        _instances.Add(new VFXWrapper(key, result, limitTime, Vector3.zero, space));
        line.SetPosition(0, start);
        line.SetPosition(1, dest);
        return true;
    }

    #region private
    private void OnSceneChanged(Scene scene)
    {
        ClearCaches();
    }
    private void ClearCaches()
    {
        _caches.Clear();
    }
    #endregion
}