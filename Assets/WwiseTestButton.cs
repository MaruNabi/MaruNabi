using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTestButton : MonoBehaviour
{
    public void OnClick(string name)
    {
        AkSoundEngine.PostEvent(name, gameObject);
        
        // �Ҹ��� ���������� �ٲٰ� ���ִ� ����ġ �ڵ�
        //AkSoundEngine.SetSwitch("Dirt", "PlayerFootStep_Dirt", gameObject);
    }
}
