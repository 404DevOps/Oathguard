using System;
using UnityEngine;
using UnityEngine.VFX;

public class LootDropRotation : MonoBehaviour
{
    public float RotationSpeed;
    public float MaxRotationSpeed;
    public float MagnetizedAcelleration;

    private PickupBase _pickup;
    private float _currentAcelleration;
    private bool isMagnetized;
    private VisualEffect _vfx;

    private void OnEnable()
    {
        isMagnetized = false;
        _currentAcelleration = 1;
        _vfx = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMagnetized) //start counting up when being attracted
        {
            _currentAcelleration += MagnetizedAcelleration * Time.deltaTime;
            _currentAcelleration = Mathf.Clamp(_currentAcelleration, 1, MaxRotationSpeed);
        }
        this.transform.Rotate(new Vector3(0, 1, 0), RotationSpeed * _currentAcelleration * Time.deltaTime);
        
        //if has vfx rotate aswell.
        
        if (_vfx != null)
            _vfx.SetVector3("Angle", transform.rotation.eulerAngles);

        _pickup = GetComponentInChildren<PickupBase>();
        _pickup.OnMagnetized += OnMagnetized;
    }

    private void OnMagnetized()
    {
        isMagnetized = true;
    }
}
