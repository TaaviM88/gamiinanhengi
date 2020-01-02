using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dissiController : MonoBehaviour
{
    public float maxXSize = 1, maxYSize, minXSize, minYSize;
    float orginalSizeX, orginalSizeY;
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        orginalSizeX = gameObject.transform.localScale.x;

        orginalSizeY = gameObject.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxXSize, maxYSize, 1), Time.deltaTime *speed);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(minXSize, minYSize, 1), Time.deltaTime * speed);
    }
}
