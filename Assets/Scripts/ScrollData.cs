using System;

[Serializable]
public class ScrollData
{
    public float leftPosX = 0f;
    public float rightPosX = 0f;
    public float rightPosY = 0f;
    public float xScreenHalfSize;
    public float yScreenHalfSize;
    
    public ScrollData(float _xScreenHalfSize, float _yScreenHalfSize, int _length)
    {
        xScreenHalfSize = _xScreenHalfSize;
        yScreenHalfSize = _yScreenHalfSize;
        
        leftPosX = -(xScreenHalfSize * 2)/2;
        rightPosX = xScreenHalfSize * 2 * _length;
        rightPosY = -8f;
    }
}
