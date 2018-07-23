using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image currentBar;
    [SerializeField] private Image nextTurnBar;
    [SerializeField] private Text resourceNameText;
    private ResourceStorage storage;

    public void Init(ResourceType resource, ResourceStorage storage)
    {
        this.storage = storage;
        this.resourceNameText.text = resource.ToString();
    }

    public void OnChanged(int maxValue)
    {
        if (storage.CurrentAmount >= storage.NextAmount)
        {
            currentBar.fillAmount = (float) storage.NextAmount / maxValue;
            nextTurnBar.fillAmount = (float) storage.CurrentAmount / maxValue;
            nextTurnBar.color = Color.red;
        }
        else
        {
            currentBar.fillAmount = (float) storage.CurrentAmount / maxValue;
            nextTurnBar.fillAmount = (float) storage.NextAmount / maxValue;
            nextTurnBar.color = Color.green;
        }
    }
}

