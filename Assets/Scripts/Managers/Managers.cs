using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get {
        if (s_instance == null)
            Init();
        return s_instance;
    } }
    
    public static DataManager Data { get { return Instance.data; } }
    public static PoolManager Pool { get { return Instance.pool; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    public static SceneLoader Scene { get { return Instance.scene; } }
    public static InputManager Input { get { return Instance.input; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    // public static UIManager UI { get { return Instance._ui; } }
    
    DataManager data = new DataManager();
    PoolManager pool = new PoolManager();
    ResourceManager resource = new ResourceManager();
    SceneLoader scene = new SceneLoader();
    InputManager input = new InputManager();
    SoundManager _sound = new SoundManager();
    // UIManager _ui = new UIManager();
    
    // void Update()
    // {
    //     _input.OnUpdate();
    // }

    static void Init()
    {
        //@Managers라는 오브젝트가 없다면 자동으로 생성해준다.
        if(s_instance == null)
        {
            GameObject go = GameObject.FindWithTag("Manager");

            if (go == null)
            {
                go = new GameObject { name = "@Managers", tag = "Manager" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            
            s_instance.data.Init();
            s_instance.pool.Init();
            //s_instance._sound.Init();
        }   
    }

    public static void Clear()
    {
        //Input.Clear();
        //Sound.Clear();
        //UI.Clear();
        Scene.Clear();
        Pool.Clear();
    }

    void Update()
    {
        input.OnUpdate();
    }
}