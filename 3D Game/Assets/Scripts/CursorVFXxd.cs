using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorVFXxd : MonoBehaviour
{
    [SerializeField] GameObject clickParticleEffect;
    [SerializeField] InputActionReference clickActionReference;

    private void Start()
    {
        clickActionReference.action.Enable();
        clickActionReference.action.performed += TheClickVFX;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Pointer.current.position.ReadValue();
    }

    public void TheClickVFX(InputAction.CallbackContext obj)
    {
        GameObject theVFX = Instantiate(clickParticleEffect);

        theVFX.transform.position = this.transform.position;
        theVFX.transform.SetParent(this.gameObject.transform);
        StartCoroutine(KilltheVFX(theVFX, 2f));
    }

    IEnumerator KilltheVFX(GameObject vfxInstance, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(vfxInstance);
        yield return null;
    }
}
