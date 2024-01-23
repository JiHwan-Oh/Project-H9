using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagazineUI : UIElement
{
    private int MAGAZINE_SIZE = 6;
    private int MAGAZINE_COUNT = 7;
    public void SetMagazineUI()
    {
        if (transform.childCount != MAGAZINE_COUNT) 
        {
            Debug.LogError("��ġ�� UI ��� ������ �ڵ� �� ����� �ٸ��ϴ�.");
            return;
        }
        int maxBullet = FieldSystem.unitSystem.GetPlayer().weapon.maxAmmo;
        int curBullet = FieldSystem.unitSystem.GetPlayer().weapon.currentAmmo;

        for (int i = 0; i < transform.childCount; i++) 
        {
            int existCnt = MAGAZINE_SIZE;
            int filledCnt = MAGAZINE_SIZE;
            if (curBullet <= MAGAZINE_SIZE)
            {
                filledCnt = curBullet;
                existCnt = maxBullet;
            }
            transform.GetChild(i).GetComponent<PlayerMagazineUIElement>().SetPlayerMagUIElement(existCnt, filledCnt);

            maxBullet -= MAGAZINE_SIZE;
            curBullet -= MAGAZINE_SIZE;
        }
    }
}
