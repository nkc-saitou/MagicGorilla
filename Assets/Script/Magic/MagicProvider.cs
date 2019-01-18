using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum MagicType
{
    _ice,
    _fire,
    _rock,
    _lightning,
    //_sword,
    _none
}

public class MagicProvider : MonoBehaviour {

    static List<IAction> _actionLis = new List<IAction>();

    public static List<IAction> ActionLis
    {
        get; private set;
    }

    void Awake()
    {
        ActionLis = new List<IAction>();

        _actionLis.AddRange(FindObjectInterface<IAction>());
        ActionLis.AddRange(_actionLis.OrderBy(x => x.ToString().Length));
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
