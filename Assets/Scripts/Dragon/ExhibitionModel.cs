using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class ExhibitionModel : MonoBehaviour
{
    public TextMeshPro [] manualTexts;
    public MeshRenderer [] manualIcons;
    public MeshRenderer [] manualModels;
    bool isManual = true;

    public void ShowManual(bool visible, float duration = 5)
    {
        isManual = visible;
        foreach (var item in manualTexts)
        {
            item.DOFade(visible ? 1 : 0, duration);
        }
        foreach (var item in manualIcons)
        {
            item.material.DOFade(visible ? 1 : 0, duration);
        }
        foreach (var item in manualModels)
        {
            item.material.DOFade(visible ? 1 : 0, duration);
        }
    }

    public void SwitchManual(float duration)
    {
        isManual = !isManual;
        ShowManual(isManual, duration);
    }
}
