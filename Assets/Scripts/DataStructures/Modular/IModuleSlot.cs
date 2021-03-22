using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IModuleSlot<TModular, TKeyword> : ScriptableObject, ISerializationCallbackReceiver
    where TModular : MonoBehaviour
{
    [HideInInspector]
    public string fullName = "";

    [SerializeField]
    public List<TKeyword> required_keywords;
    public HashSet<TKeyword> keywords_required;

    [SerializeField]
    public List<TKeyword> forbidden_keywords;
    public HashSet<TKeyword> keywords_forbidden;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        keywords_required = new HashSet<TKeyword>(required_keywords);
        keywords_forbidden = new HashSet<TKeyword>(forbidden_keywords);
    }

    public bool IsAllowed(IModule<TModular, TKeyword> module)
    {
        foreach(TKeyword keyword in keywords_required)
        {
            if(!module.keywords.Contains(keyword))
            {
                return false;
            }
        }

        foreach(TKeyword keyword in keywords_forbidden)
        {
            if(module.keywords.Contains(keyword))
            {
                return false;
            }
        }

        return true;
    }
}
