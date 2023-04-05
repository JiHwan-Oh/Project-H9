using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;

/// <summary>
/// 타일을 넘겨받으면 원하는 이펙트를 타일에 적용시켜주는 클래스
/// </summary>
public class TileEffector : Singleton<TileEffector>
{
    /// <summary>
    /// 타일에 우호적인 이펙트를 적용합니다.
    /// </summary>
    public void FriendlyEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 타일들에 적대적인 이펙트를 적용합니다. 
    /// </summary>
    public void HostileEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 타일들에 일반적인 하이라이팅 이펙트를 적용합니다.
    /// </summary>
    public void NormalEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 타일들에 뭔가 불가능해보이는 이펙트를 적용합니다.
    /// </summary>
    public void ImpossibleEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 타일들에 Invisible 이펙트를 적용합니다.
    /// </summary>
    public void InvisibleEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 타일들에 전장의 안개 이펙트를 적용합니다.
    /// </summary>
    /// <param name="tiles"></param>
    public void FogOfWarEffect(IEnumerator<Tile> tiles)
    {
        throw new NotImplementedException();
    }
}
