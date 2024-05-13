using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTestButton : MonoBehaviour
{
    public void OnClick(string name)
    {
        AkSoundEngine.PostEvent(name, gameObject);
        
        // 소리를 순간적으로 바꾸게 해주는 스위치 코드
        //AkSoundEngine.SetSwitch("Dirt", "PlayerFootStep_Dirt", gameObject);
    }
}
