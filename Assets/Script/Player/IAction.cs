﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    /// <summary>
    /// チャージ
    /// </summary>
    void Charge();

    /// <summary>
    /// 打つ,投げる
    /// </summary>
    void Shot(GameObject target);
}
