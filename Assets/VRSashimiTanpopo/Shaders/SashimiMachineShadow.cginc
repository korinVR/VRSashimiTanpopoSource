#define SHADOW_0_X 1.34
#define SHADOW_1_X 1.64
    
float clampedMap(float value, float s0, float s1, float d0, float d1)
{
    float t = (value - s0) / (s1 - s0);
    return d0 + clamp(t, 0, 1) * (d1 - d0);
}

float getSashimiMachineAttenuation(const float3 worldPos)
{
    if (worldPos.y > 0.8)
    {
        return 1;
    }
    return pow(clampedMap(abs(worldPos.x), SHADOW_0_X, SHADOW_1_X, 1, 0), 4);
}
