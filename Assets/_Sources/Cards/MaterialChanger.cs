using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private List<Image> images; 
    private List<Material> _oldMaterials = new List<Material>();
    private bool _isSwiched = false;

    public void SwitchMaterials()
    {
        if(_isSwiched)
            return;
        _oldMaterials.Clear();
        
        foreach (var image in images)
        {
            _oldMaterials.Add(new Material(image.material));
            image.material = material;
        }

        _isSwiched = true;
    }

    public void ChangeBackMaterial()
    {
        if(!_isSwiched)
            return;
        for (int i = 0; i < images.Count; i++)
        {
            images[i].material = _oldMaterials[i];
        }
        _isSwiched = false;
    }
}
