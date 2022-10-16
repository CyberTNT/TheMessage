using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiangLingSelect : MonoBehaviour
{
    private List<CardNameEnum> types = new List<CardNameEnum>();

    private void OnDisable()
    {
        types.Clear();
    }
    public void OnToggle(CardNameEnum cardName, bool enable)
    {
        if(enable)
        {
            types.Add(cardName);
        }
        else
        {
            if(types.Contains(cardName))
            {
                types.Remove(cardName);
            }
        }
    }

    public void OnClickCancel()
    {
        if (GameManager.Singleton.IsUsingSkill && GameManager.Singleton.selectSkill != null)
        {
            GameManager.Singleton.selectSkill.Cancel();
        }
        gameObject.SetActive(false);
    }

    public void OnClickSure()
    {
        if (types.Count > 2)
        {
            GameManager.Singleton.gameUI.ShowInfo("����ѡ��2�ֿ���");
            return;
        }
        else if (types.Count < 1)
        {
            GameManager.Singleton.gameUI.ShowInfo("ѡ��Ҫ���õĿ���");
            return;
        }

        ProtoHelper.SendSkill_QiangLing(true, types, GameManager.Singleton.seqId);

        if (GameManager.Singleton.IsUsingSkill && GameManager.Singleton.selectSkill != null)
        {
            GameManager.Singleton.selectSkill.OnUse();
        }

        gameObject.SetActive(false);
    }

}
