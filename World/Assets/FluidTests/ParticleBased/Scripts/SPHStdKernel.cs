using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPHStdKernel
{
    float h, h2, h3, h5;

    public SPHStdKernel(float h)
    {
        this.h = h;
        h2 = h * h;
        h3 = h2 * h;
        h5 = h2 * h3;
    }

    public float GetKernel(float distance)
    {
        float distanceSquared = distance * distance;
        if (distanceSquared >= h2) return 0;
        float x = 1 - distanceSquared / h2;
        return 315f / (64f * Mathf.PI * h3) * x * x * x;
    }

    public float GetFirstDerivative(float distance)
    {
        if (distance >= h)
        {
            return 0;
        }

        float x = 1 - distance * distance / h2;
        return -945f / (32f * Mathf.PI * h5) * distance * x * x;
    }

    public Vector3 Gradient(float distance, Vector3 directionToCenter)
    {
        return -GetFirstDerivative(distance) * directionToCenter;
    }

    public float GetSecondDerivative(float distance)
    {
        float distanceSquared = distance * distance;
        if (distanceSquared >= h2) return 0;
        float x = 1 - distanceSquared / h2;
        return 945f / (32f * Mathf.PI * h5) * (1-x) * (5*x - 1);
    }
}

public class SPHSpikyKernel
{
    float h, h2, h3, h4, h5;

    public SPHSpikyKernel(float h)
    {
        this.h = h;
        h2 = h * h;
        h3 = h2 * h;
        h4 = h2 * h2;
        h5 = h2 * h3;
    }

    public float GetKernel(float distance)
    {
        if (distance >= h) return 0;
        float x = 1 - distance / h;
        return 15f / (Mathf.PI * h3) * x * x * x;
    }

    public float GetFirstDerivative(float distance)
    {
        if (distance >= h)
        {
            return 0;
        }

        float x = 1 - distance / h;
        return -45f / (Mathf.PI * h4) * x * x;
    }

    public Vector3 Gradient(float distance, Vector3 directionToCenter)
    {
        return -GetFirstDerivative(distance) * directionToCenter;
    }
    
    public float GetSecondDerivative(float distance)
    {
        float distanceSquared = distance * distance;
        if (distanceSquared >= h2) return 0;
        float x = 1 - distanceSquared / h2;
        return 90 / (Mathf.PI * h5) * x;
    }
}