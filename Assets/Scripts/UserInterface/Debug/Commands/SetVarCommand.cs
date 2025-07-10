#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Reflection;

public class SetVarCommand : BaseCommand
{
    public override CommandReturnType Execute(object[] args)
    {
        if (args.Length != 4)
        {
            return new CommandReturnType(ReturnType.Error, "[ERROR] Invalid number of arguments (Expected 4)");;
        }
        
        string ObjectPath = args[0].ToString();
        string TargetClass = args[1].ToString();
        object NewVariable = args[2]; 
        object NewValue = args[3];
        
        GameObject TargetObject = GameObject.Find(ObjectPath);
        if (TargetObject == null) {  return new CommandReturnType(ReturnType.Error, "[ERROR] Couldn't find target object"); }
        
        Component TargetComponent = TargetObject.GetComponent(TargetClass);
        if (TargetComponent == null) {  return new CommandReturnType(ReturnType.Error,"[ERROR] Couldn't find target component"); }

        try
        {

            FieldInfo[] properties = TargetComponent.GetType().GetFields();
            foreach (FieldInfo property in properties)
            {
                Debug.Log(property.Name + " : " + (string)NewVariable);
                if (property.Name != (string)NewVariable) continue;

                var NextVal = Convert.ChangeType(NewValue, property.FieldType);
                property.SetValue(TargetComponent, NextVal);
                
                return new CommandReturnType(ReturnType.Success,$"[SUCCESS] Successfully changed variable {property.Name} to {NextVal}");
            }
        }
        catch (Exception e)
        {
            return new CommandReturnType(ReturnType.Error, $"[ERROR] {e.Message}");
        }
        
        return new CommandReturnType(ReturnType.None, $"[ERROR] Unknown error");
    }
}
#endif