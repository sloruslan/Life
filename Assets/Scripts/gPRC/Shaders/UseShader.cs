using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseShader : MonoBehaviour
{
    public ComputeShader _computeShader;

    private void Start()
    {
        int testKernel = _computeShader.FindKernel("Test");
        int[] intValues = { 1, 2, 3, 4, 5, 6, 7, 8 };
        Debug.Log("Upload: " + intValues[0] + ", " + intValues[1] + ", " + intValues[2] + ", " + intValues[3] + ", " + intValues[4] + ", " + intValues[5] + ", " + intValues[6] + ", " + intValues[7]);
        ComputeBuffer intBuffer = new ComputeBuffer(intValues.Length, sizeof(int));
        _computeShader.SetBuffer(testKernel, "_IntBuffer", intBuffer);
        _computeShader.SetInts("_IntValues", intValues);
        _computeShader.Dispatch(testKernel, intValues.Length, 1, 1);
        intBuffer.GetData(intValues);
        Debug.Log("Download: " + intValues[0] + ", " + intValues[1] + ", " + intValues[2] + ", " + intValues[3] + ", " + intValues[4] + ", " + intValues[5] + ", " + intValues[6] + ", " + intValues[7]);
        intBuffer.Release();
    }
}
