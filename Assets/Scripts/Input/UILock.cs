using Utilities;
using UnityEngine;

public class UILock : Singleton<UILock> {
    [SerializeField] private int uiCount = 0;

    public bool allowGameplay => uiCount == 0;

    public void OpenUI() {
        uiCount++;
    }

    public void CloseUI() {
        uiCount = Mathf.Max(0, uiCount - 1);
    }
}