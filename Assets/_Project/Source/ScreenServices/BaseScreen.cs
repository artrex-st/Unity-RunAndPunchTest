using DataService;
using UnityEngine;
using Utility;

public class BaseScreen : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] protected ScreenReference _thisScreenRef;
    protected ISaveDataService SaveDataService;
    protected IScreenService ScreenService;
    protected ISoundService SoundService;

    protected void Initialize()
    {
        SaveDataService = ServiceLocator.Instance.GetService<ISaveDataService>();
        ScreenService = ServiceLocator.Instance.GetService<IScreenService>();
        SoundService = ServiceLocator.Instance.GetService<ISoundService>();
        this.LogEditorOnly($"Initialize <color=white>{_thisScreenRef.SceneName}</color>");
    }

    protected void Dispose()
    {
        this.LogEditorOnly($"Disposed <color=Green>{_thisScreenRef.SceneName}</color>");
    }
}
