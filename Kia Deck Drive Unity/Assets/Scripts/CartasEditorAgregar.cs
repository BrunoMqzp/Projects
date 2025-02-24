using System.Collections;
using System.Collections.Generic;
using KDDC;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ContainerPile))]
public class ContainerPileEditorAgregar : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ContainerPile containerPile = (ContainerPile)target;
        if (GUILayout.Button("Draw Next Card")){
            Mazo mazo = FindObjectOfType<Mazo>();
            if (mazo != null){
                containerPile.SorteoCarta(mazo);
            }
        }
    }
}
#endif
