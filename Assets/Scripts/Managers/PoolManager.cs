using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        //풀링된 오브젝트들의 루트
        public Transform Root { get; set; }
        //풀링된 오브젝트를 담는 스택
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        //풀 루트로 보내는 함수
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            //isUsing은 현재 사용 중인가에 대한 내용
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        //사용하기 위해 풀루트와 풀스택에서 꺼내옴
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    #region UIPool
    class UI_Pool
    {
        public GameObject Original { get; private set; }
        //풀링된 오브젝트들의 루트
        public RectTransform Root { get; set; }
        //풀링된 오브젝트를 담는 스택
        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            GameObject go = new GameObject();
            go.AddComponent<RectTransform>();
            go.TryGetComponent<RectTransform>(out RectTransform root);
            Root = root;

            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        //풀 루트로 보내는 함수
        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.gameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform);
            rectTransform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            //isUsing은 현재 사용 중인가에 대한 내용
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        //사용하기 위해 풀루트와 풀스택에서 꺼내옴
        public Poolable Pop(RectTransform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.SetParent(Managers.Scene.CurrentScene.transform);

            poolable.gameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform);
            rectTransform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Dictionary<string, UI_Pool> _ui_pool = new Dictionary<string, UI_Pool>();
    Transform _root;
    RectTransform _ui_root;
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root", tag = "PoolRoot" }.transform;
            
        }

        if(_ui_root == null)
        {
            GameObject go = new GameObject();
            go.AddComponent<RectTransform>();
            go.TryGetComponent<RectTransform>(out _ui_root);

            _ui_root.name = "@UI_Pool_Root";
            _ui_root.tag = "UIRoot";
        }
    }

    //오브젝트 풀을 만드는 함수
    public void CreatePool(GameObject original, int count = 5)
    {
        if(original.CompareTag("UI"))
        {
            UI_Pool ui_pool = new UI_Pool();
            ui_pool.Init(original, count);
            ui_pool.Root.SetParent(_ui_root);

            _ui_pool.Add(original.name, ui_pool);
        }
        else
        {
            Pool pool = new Pool();
            pool.Init(original, count);
            pool.Root.parent = _root;

            _pool.Add(original.name, pool);
        }
    }

    //poolable 스크립트가 있어야 오브젝트 풀링이 가능하다
    public void Push(Poolable poolable)
    {
        if(poolable.gameObject.CompareTag("UI"))
        {
            string name = poolable.gameObject.name;
            if (_ui_pool.ContainsKey(name) == false)
            {
                GameObject.Destroy(poolable.gameObject);
                return;
            }

            _ui_pool[name].Push(poolable);
        }
        else
        {
            string name = poolable.gameObject.name;
            if (_pool.ContainsKey(name) == false)
            {
                GameObject.Destroy(poolable.gameObject);
                return;
            }

            _pool[name].Push(poolable);
        }  
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {  
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);
        return _pool[original.name].Pop(parent);
    }

    public Poolable Pop(GameObject original, RectTransform parent = null)
    {
        if (_ui_pool.ContainsKey(original.name) == false)
            CreatePool(original);
        return _ui_pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
        {
            if (_ui_pool.ContainsKey(name) == false)
                return null;
            else
                return _ui_pool[name].Original;
        } 
        return _pool[name].Original;
    }

    //풀링된 내용을 싹 날려줌
    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
