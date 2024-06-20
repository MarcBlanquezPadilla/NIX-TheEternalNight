if (_LifePercent < _LifePercentToStart)
{
    texColor = 0.0;
    float2 texelSize = 1.0 / _ScreenParams.xy;
    float weight = 1.0 / (2*_BlurSize * 2*_BlurSize);

    float lerpValue = 0;
    if (_LifePercent != _LifePercentToStart) lerpValue = (_LifePercentToStart - _LifePercent) / _LifePercentToStart;

    for (float x = -_BlurSize; x <= _BlurSize; x++) {
        for (float y = -_BlurSize; y <= _BlurSize; y++) {

            float2 offset = float2(x, y) * texelSize * lerp(_MinSeparation, _MaxSeparation, lerpValue);
            texColor += tex2D(_GrabbedTexture, IN.uv + offset) * weight;
        }
    }

    float4 vinyetaColor = tex2D(_Vinyeta, IN.uv) * _ColorVinyeta;

    texColor = lerp(texColor, vinyetaColor, lerpValue * vinyetaColor.a * _MagnitudVinyeta);
}