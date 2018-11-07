using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicType
{
    _fire,
    _ice,
    _lightning,
    _rock,
    _sword,
    _none
}

public class MagicProvider : MonoBehaviour {

    static List<IAction> _actionLis = new List<IAction>();

    public static List<IAction> ActionLis
    {
        get { return _actionLis; }
    }

    void Awake()
    {
        _actionLis.AddRange(FindObjectInterface<IAction>());
    }

    void Start ()
    {

    }
	
	void Update ()
    {
		
	}

    /// <summary>
    /// 投げることが出来る(IActionインターフェイスが実装されている)ものを取得する
    /// </summary>
    /// <typeparam name="T">InterFaceの型</typeparam>
    /// <returns>InterFaceが実装されているコンポーネントのList</returns>
    List<T> FindObjectInterface<T>() where T : class
    {
        List<T> tempLis = new List<T>();
        foreach (var n in FindObjectsOfType<Component>())
        {
            var component = n as T;
            if (component != null)
            {
                tempLis.Add(component);
            }
        }
        return tempLis;
    }
}
