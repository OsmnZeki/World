using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNeighborSearcher
{
    public List<Vector3> points;
    public List<int>[] buckets;

    public float gridSpacing;
    public Vector3Int resolution;

    public delegate void GetNearbyFunc(int pointIdx, Vector3 nearbyPoint);

    public PointNeighborSearcher(float gridSpacing, Vector3Int resolution)
    {
        this.gridSpacing = gridSpacing;
        this.resolution = resolution;

        points = new List<Vector3>();
    }

    public void Build(List<Vector3> targetPoints)
    {
        points.Clear();
        buckets = null;

        if (targetPoints.Count == 0) return;

        buckets = new List<int>[resolution.x * resolution.y * resolution.z];

        for (int i = 0; i < targetPoints.Count; i++)
        {
            points.Add(targetPoints[i]);
            int key = GetHasKeyFromPosition(targetPoints[i]);
            buckets[key].Add(i);;
        }
    }

    Vector3Int GetBucketIdx(Vector3 targetPoint)
    {
        Vector3Int bucketIdx = Vector3Int.zero;

        bucketIdx.x = Mathf.FloorToInt(targetPoint.x / resolution.x);
        bucketIdx.y = Mathf.FloorToInt(targetPoint.y / resolution.y);
        bucketIdx.z = Mathf.FloorToInt(targetPoint.z / resolution.z);
        return bucketIdx;
    }

    int GetHashKeyFromBucketIdx(Vector3Int bucketIdx)
    {
        bucketIdx.x = bucketIdx.x % resolution.x;
        bucketIdx.y = bucketIdx.y % resolution.y;
        bucketIdx.z = bucketIdx.z % resolution.z;

        if (bucketIdx.x < 0) bucketIdx.x += resolution.x;
        if (bucketIdx.y < 0) bucketIdx.y += resolution.y;
        if (bucketIdx.z < 0) bucketIdx.z += resolution.z;

        var hashPos = (bucketIdx.z * resolution.y + bucketIdx.y) * resolution.x + bucketIdx.x;
        return hashPos;
    }

    int GetHasKeyFromPosition(Vector3 targetPoint)
    {
        var bucketIdx = GetBucketIdx(targetPoint);
        return GetHashKeyFromBucketIdx(bucketIdx);
    }

    public void ForEachNearbyPoint(Vector3 targetPoint,float radius, GetNearbyFunc callback)
    {
        if(buckets.Length ==0) return;

        int[] nearbyKeys = new int[8];
        GetNearbyKeys(targetPoint,nearbyKeys);

        var queryRadiusSquared = radius * radius;
        for (int i = 0; i < 8; i++)
        {
            var currentBucket = buckets[nearbyKeys[i]];
            for (int j = 0; j < currentBucket.Count; j++)
            {
                var pointIdx = currentBucket[j];
                var point = points[pointIdx];

                var distance = (point - targetPoint).sqrMagnitude;
                if (distance < queryRadiusSquared)
                {
                    callback(pointIdx, point);
                }
            }
        }
    }

    void GetNearbyKeys(Vector3 targetPoint, int[] nearbyKeys)
    {
        var originIdx = GetBucketIdx(targetPoint);
        Vector3Int[] nearbyBucketIndices = new Vector3Int[8];

        for (int i = 0; i < nearbyBucketIndices.Length; i++)
        {
            nearbyBucketIndices[i] = originIdx;
        }

        if ((originIdx.x + .5f) * gridSpacing <= targetPoint.x)
        {
            nearbyBucketIndices[4].x += 1;
            nearbyBucketIndices[5].x += 1;
            nearbyBucketIndices[6].x += 1;
            nearbyBucketIndices[7].x += 1;
        }
        else
        {
            nearbyBucketIndices[4].x -= 1;
            nearbyBucketIndices[5].x -= 1;
            nearbyBucketIndices[6].x -= 1;
            nearbyBucketIndices[7].x -= 1;
        }
        
        if ((originIdx.y + .5f) * gridSpacing <= targetPoint.y)
        {
            nearbyBucketIndices[2].y += 1;
            nearbyBucketIndices[3].y += 1;
            nearbyBucketIndices[6].y += 1;
            nearbyBucketIndices[7].y += 1;
        }
        else
        {
            nearbyBucketIndices[2].y -= 1;
            nearbyBucketIndices[3].y -= 1;
            nearbyBucketIndices[6].y -= 1;
            nearbyBucketIndices[7].y -= 1;
        }
        
        if ((originIdx.z + .5f) * gridSpacing <= targetPoint.z)
        {
            nearbyBucketIndices[1].z += 1;
            nearbyBucketIndices[3].z += 1;
            nearbyBucketIndices[5].z += 1;
            nearbyBucketIndices[7].z += 1;
        }
        else
        {
            nearbyBucketIndices[1].z -= 1;
            nearbyBucketIndices[3].z -= 1;
            nearbyBucketIndices[5].z -= 1;
            nearbyBucketIndices[7].z -= 1;
        }

        for (int i = 0; i < nearbyKeys.Length; i++)
        {
            nearbyKeys[i] = GetHashKeyFromBucketIdx(nearbyBucketIndices[i]);
        }
    }
}