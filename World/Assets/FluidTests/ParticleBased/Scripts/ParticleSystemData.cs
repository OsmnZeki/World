using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemData
{
    public List<Vector3> positions;
    public List<Vector3> velocities;
    public List<Vector3> forces;
    public List<float> densities;
    public List<float> pressures;

    public PointNeighborSearcher neighborSearcher;
    public List<int>[] neighborList;

    public int defaultGridResolution;
    
    public const float KernelRadius = 1.0f;
    public const float Mass = 1f;
    public float targetDensity;

    public void BuildNeighborSearcher(float maxSearchRadius)
    {
        neighborSearcher = new PointNeighborSearcher(2 * maxSearchRadius,
            new Vector3Int(defaultGridResolution, defaultGridResolution, defaultGridResolution));

        neighborSearcher.Build(positions);
        neighborList = new List<int>[positions.Count];
    }

    public void BuildNeighborList(float maxSearchRadius)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 position = positions[i];
            neighborList[i].Clear();

            neighborSearcher.ForEachNearbyPoint(position, maxSearchRadius, (int pointIdx, Vector3 point) =>
            {
                if (pointIdx != i)
                {
                    neighborList[i].Add(pointIdx);
                }
            });
        }
    }

    public Vector3 Interpolate(Vector3 particlePosition, List<Vector3> values)
    {
        Vector3 sum = Vector3.zero;
        SPHStdKernel kernel = new SPHStdKernel(KernelRadius);

        neighborSearcher.ForEachNearbyPoint(particlePosition, KernelRadius, (int pointIdx, Vector3 point) =>
        {
            var distance = (point - particlePosition).magnitude;
            float weight = Mass / densities[pointIdx] * kernel.GetKernel(distance);
            sum += weight * values[pointIdx];
        });

        return sum;
    }

    public float SumOfKernelNearby(Vector3 particlePosition)
    {
        float sum = 0;
        SPHStdKernel kernel = new SPHStdKernel(KernelRadius);

        neighborSearcher.ForEachNearbyPoint(particlePosition, KernelRadius, (int pointIdx, Vector3 point) =>
        {
            var distance = (point - particlePosition).magnitude;
            sum += kernel.GetKernel(distance);
        });

        return sum;
    }

    public void UpdateDensities()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            float sum = SumOfKernelNearby(positions[i]);
            densities[i] = sum * Mass;
        }
    }

    public Vector3 GradientAt(int particleIdx, List<float> values)
    {
        Vector3 sum = Vector3.zero;
        SPHSpikyKernel kernel = new SPHSpikyKernel(KernelRadius);
        var neighbors = neighborList[particleIdx];
        var particleDensity = densities[particleIdx];

        foreach (var neighborIdx in neighbors)
        {
            var neighborPos = positions[neighborIdx];
            var dist = neighborPos - positions[particleIdx];
            var distance = dist.magnitude;
            if (distance > 0)
            {
                Vector3 dir = dist / distance;
                sum += particleDensity * Mass * (values[particleIdx] / Mathf.Pow(particleDensity, 2) + values[neighborIdx]
                    / Mathf.Pow(densities[neighborIdx], 2)) * kernel.Gradient(distance, dir);
            }
        }

        return sum;
    }

    public float LaplacianAt(int particleIdx, List<float> values)
    {
        float sum = 0;
        SPHSpikyKernel kernel = new SPHSpikyKernel(KernelRadius);
        var neighbors = neighborList[particleIdx];
        var particleDensity = densities[particleIdx];

        foreach (var neighborIdx in neighbors)
        {
            var neighborPos = positions[neighborIdx];
            var dist = neighborPos - positions[particleIdx];
            var distance = dist.magnitude;
            sum += Mass * (values[neighborIdx] - values[particleIdx]) / densities[neighborIdx] * kernel.GetSecondDerivative(distance);
        }

        return sum;
    }
}