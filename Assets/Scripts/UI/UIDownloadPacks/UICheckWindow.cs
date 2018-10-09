using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UICheckWindow : MonoBehaviour
{

    public UIGrid m_Grid;
    public GameObject m_Item;

    public void SetCheckItems(List<ItemOfReward> itemOfRewards)
    {
        ClearItem(m_Grid.transform);
        int index = 0;
        foreach (ItemOfReward item in itemOfRewards)
        {
            index++;
            GameObject go = Instantiate(m_Item, m_Grid.transform);
            go.SetActive(true);
            go.name = go.name + index;
            RewardCell cell = go.GetComponent<RewardCell>();
            cell.InitUI(item);
        }
        m_Grid.repositionNow = true;
    }

    public GameObject GetItem()
    {
        return Resources.Load("China/Pack/ZQ/RewardCell") as GameObject;
    }


    public GameObject Instantiate(GameObject original, Transform parent)
    {
        GameObject newObj = Object.Instantiate(original) as GameObject;
        newObj.transform.parent = parent;
        newObj.transform.localPosition = original.transform.localPosition;
        newObj.transform.localScale = original.transform.localScale;
        return newObj;
    }

    public void ClearItem(Transform transform)
    {
        foreach (Transform trans in transform)
        {
            if (trans.gameObject.activeSelf)
                Destroy(trans.gameObject);
        }
    }
}
