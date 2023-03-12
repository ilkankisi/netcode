using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Manager").GetComponent<LevelManager>().CoinIncrease();

        StartCoroutine(DestroyCoin());
    }

    private IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
