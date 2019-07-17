using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayMode : MonoBehaviour
{
    public enum Mode {
        FirstPerson = 0,
        Topdown = 1,
        Vr = 2,
    };

    [System.Serializable]
    public class ModeInitList
    {
        public Mode mode;
        public Component[] objects;
        public UnityEvent extra;
    }

    public Mode mode;

    public ModeInitList[] enableLists;

    // Start is called before the first frame update
    void Start()
    {
        setUpMode();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnValidate()
    {
        setUpMode();
    }

    public void setUpMode()
    {
        foreach (var m in enableLists) {
            setEnabledAll(m.objects, false);
        }
        foreach (var m in enableLists) {
            if (m.mode == mode) {
                setEnabledAll(m.objects, true);
                m.extra.Invoke();
            }
        }
    }

    private void setEnabledAll(Component[] components, bool enabled)
    {
        foreach (var c in components) {
            if (c is MonoBehaviour m) {
                m.enabled = enabled;
            } else if (c is Transform t) {
                t.gameObject.SetActive(enabled);
            } else {
                Debug.LogError("Object is neither behaviour or game object: " + c, this);
            }
        }
    }
}
