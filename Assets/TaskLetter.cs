using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _about;
    [SerializeField] private Image _sealIcon;
    
    [SerializeField] private TaskLetterData _data;

    private void OnValidate()
    {
        _title.text = _data._taskName;
        _about.text = _data._taskDescription;
        _sealIcon.color = _data._sealColor;
    }
}

[System.Serializable]
public class TaskLetterData
{
    public string _taskName;
    
    [Space]
    [TextArea]
    public string _taskAbout;
    
    [Space]
    public string _taskPayment;
    public string _taskInsurance;
    public string _taskDistance;
    public string _taskReward;

    public Color _sealColor;
    
    public string _taskDescription => $"{_taskAbout}\n \nPagamento: {_taskPayment}\nSeguro: {_taskInsurance}\nDistancia: {_taskDistance}\nRecompensa: {_taskReward}";
}
