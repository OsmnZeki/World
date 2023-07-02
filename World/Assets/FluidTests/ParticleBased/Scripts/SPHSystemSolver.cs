using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPHSystemSolver
{
    ParticleSystemData particleSystemData;
    public float speedOfSound;
    public float _eosExponent;
    public float viscosityCoefficient;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    public float dragCoefficient;
    
    public float ComputePressureFromEOS(float density, float targetDensity, float eosScale, float eosExponent, float negativePressureScale = 0)
    {
        float p = eosScale / eosExponent * (Mathf.Pow((density / targetDensity), eosExponent) - 1);
        if (p < 0) p *= negativePressureScale;
        return p;
    }

    public void ComputePressure()
    {
        var densities = particleSystemData.densities;
        var pressures = particleSystemData.pressures;
        var targetDensity = particleSystemData.targetDensity;
        float eosScale = targetDensity * Mathf.Pow(speedOfSound, 2) / _eosExponent;

        for (int i = 0; i < densities.Count; i++)
        {
            pressures[i] = ComputePressureFromEOS(densities[i], targetDensity, eosScale, _eosExponent);
        }
    }

    public void AccumulatePressureFore()
    {
        var densities = particleSystemData.densities;
        var pressures = particleSystemData.pressures;
        var positions = particleSystemData.positions;
        var forces = particleSystemData.forces;
        
        var massSquared = Mathf.Pow(ParticleSystemData.Mass, 2);
        var kernel = new SPHSpikyKernel(ParticleSystemData.KernelRadius);

        for (int i = 0; i < positions.Count; i++)
        {
            var neighbors = particleSystemData.neighborList[i];
            var position = positions[i];
            var density = densities[i];
            var pressure = pressures[i];
            
            foreach (var neighbor in neighbors)
            {
                var neighborPosition = positions[neighbor];
                var neighborDensity = densities[neighbor];
                var neighborPressure = pressures[neighbor];
                
                var distance = (neighborPosition - position).magnitude;
                if (distance > 0)
                {
                    var direction = (neighborPosition - position) / distance;
                    forces[i] -= massSquared * (pressure / Mathf.Pow(density, 2) + neighborPressure / Mathf.Pow(neighborDensity, 2)) * kernel.Gradient(distance, direction);
                }
               
            }
        }
    }
    
    public void AccumulateViscosityForce()
    {
        var densities = particleSystemData.densities;
        var positions = particleSystemData.positions;
        var forces = particleSystemData.forces;
        
        var massSquared = Mathf.Pow(ParticleSystemData.Mass, 2);
        var kernel = new SPHSpikyKernel(ParticleSystemData.KernelRadius);

        for (int i = 0; i < positions.Count; i++)
        {
            var neighbors = particleSystemData.neighborList[i];
            var position = positions[i];
            
            foreach (var neighbor in neighbors)
            {
                var neighborPosition = positions[neighbor];
                var neighborDensity = densities[neighbor];
                
                var distance = (neighborPosition - position).magnitude;
               forces[i] += viscosityCoefficient * massSquared * (neighborPosition -position)/neighborDensity * kernel.GetSecondDerivative(distance);
               
            }
        }
    }
    
    public void AccumulateExternalForce()
    {
        var forces = particleSystemData.forces;
        var positions = particleSystemData.positions;
        
        for (int i = 0; i < positions.Count; i++)
        {
            forces[i] += gravity * ParticleSystemData.Mass;
            forces[i] += -dragCoefficient * particleSystemData.velocities[i];
        }
    }
    
    public void TimeIntegration(float timeStep)
    {
        var positions = particleSystemData.positions;
        var velocities = particleSystemData.velocities;
        var forces = particleSystemData.forces;
        var densities = particleSystemData.densities;
        
        for (int i = 0; i < positions.Count; i++)
        {
            velocities[i] += timeStep * forces[i] / densities[i];
            positions[i] += timeStep * velocities[i];
        }
    }
}