using System.Collections;
using System.Collections.Generic;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
