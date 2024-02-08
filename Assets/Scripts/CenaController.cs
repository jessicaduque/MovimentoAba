using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CenaController : MonoBehaviour
{
    [SerializeField] CanvasGroup telaPretaCG;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        telaPretaCG.DOFade(0, 0.4f);
    }

}
