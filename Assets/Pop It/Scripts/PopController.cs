using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace PopIt
{
public class PopController : MonoBehaviour
{
    public List<Popable> popables;
    public UnityEvent onPopAllPopables;
    private void OnEnable() 
    {
        popables.ForEach(f=>f.onPop.AddListener(OnPopPopable));
        ResetAllPopables();
    }
    private void OnDisable() 
    {
        popables.ForEach(f=>f.onPop.RemoveListener(OnPopPopable));
    }
    public void ResetAllPopables()
    {
        popables.ForEach(f=>f.ResetPopable());
    }
    public void ActivateAllPopables()
    {
        popables.ForEach(f=>f.Activate());
    }
    public void OnPopPopable()
    {
        if(HasPopedAll())
        {
            onPopAllPopables.Invoke();
        }
    }
    public bool HasPopedAll()
    {
        foreach (var item in popables)
        {
            if(!item.hasPoped)
            {
                return false;
            }
        }
        return true;
    }
    
}
}