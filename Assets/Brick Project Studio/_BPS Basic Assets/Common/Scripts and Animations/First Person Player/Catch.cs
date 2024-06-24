using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{
    public int distance = 15;
    public LayerMask layer;
    public List<string> tagsObjects;
    public Transform handPosition;
    public bool catch_ = false;
    private Transform objectHand;
    private GameObject objectInstance;
    public List<GameObject> objectPrefabs;
    public LayerMask floor;
    public float throwForce = 10f;

    void Update()
    {
        if (catch_)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReleaseObject();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                ThrowObject();
            }
        }
        else
        {
            CheckForObject();
        }

        // Garantir que o objeto instanciado siga a mão
        if (objectInstance != null)
        {
            objectInstance.transform.position = handPosition.position;
            objectInstance.transform.rotation = handPosition.rotation;
        }
    }

    void CheckForObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layer, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetKeyDown(KeyCode.E) && !catch_)
            {
                string tag = tagsObjects.Find(tag => hit.transform.gameObject.tag == tag);
                if (tag != null)
                {
                    if (hit.transform.GetComponent<CanBeCaught>() == null)
                    {
                        GameObject prefab = objectPrefabs.Find(obj => obj.tag == tag);
                        if (prefab != null)
                        {
                            // Instanciar o prefab na posição e rotação do handPosition
                            objectInstance = Instantiate(prefab, handPosition.position, handPosition.rotation);
                            // Definir handPosition como pai do objeto instanciado
                            objectInstance.transform.SetParent(handPosition, true);
                            objectInstance.transform.localPosition = Vector3.zero;
                            objectInstance.transform.localRotation = Quaternion.identity;
                            objectInstance.AddComponent<CanBeCaught>();
                            catch_ = true;
                            GetComponent<AudioSource>().Play();
                        }
                    }
                    else
                    {
                        objectInstance = hit.transform.gameObject;
                        objectInstance.transform.SetParent(handPosition, true);
                        objectInstance.transform.localPosition = Vector3.zero;
                        objectInstance.transform.localRotation = Quaternion.identity;
                        catch_ = true;
                    }
                }
            }
        }
    }

    void ReleaseObject()
    {
        catch_ = false;
        if (objectInstance != null)
        {
            // Desassociar o objeto da mão
            objectInstance.transform.SetParent(null);
            if (!objectInstance.GetComponent<DestroyOnFloor>())
            {
                DestroyOnFloor destroyOnFloor = objectInstance.AddComponent<DestroyOnFloor>();
                destroyOnFloor.floor = floor;

            }
            objectInstance = null;
        }
        objectHand = null;
    }

    void ThrowObject()
    {
        catch_ = false;
        if (objectInstance != null)
        {
            // Desassociar o objeto da mão
            objectInstance.transform.SetParent(null);

            Rigidbody rb = objectInstance.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = objectInstance.AddComponent<Rigidbody>();
            }

            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

            if (!objectInstance.GetComponent<DestroyOnFloor>())
            {
                DestroyOnFloor destroyOnFloor = objectInstance.AddComponent<DestroyOnFloor>();
                destroyOnFloor.floor = floor;
            }
            objectInstance = null;
        }
        objectHand = null;
    }
}

public class CanBeCaught : MonoBehaviour
{
    // Este script serve para conseguir pegar no objeto novamente.
}
