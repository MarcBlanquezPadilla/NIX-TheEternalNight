float3x3 conversionMatrix;
switch (_TipoDaltonismo) {
case 1:
    conversionMatrix = float3x3(
        0.567, 0.433, 0,
        0.558, 0.442, 0,
        0, 0.242, 0.758
        );
    texColor.rgb = mul(texColor.rgb, conversionMatrix);
    break;
case 2:
    conversionMatrix = float3x3(
        0.625, 0.375, 0,
        0.7, 0.3, 0,
        0, 0.3, 0.7
        );
    texColor.rgb = mul(texColor.rgb, conversionMatrix);
    break;
case 3:
    conversionMatrix = float3x3(
        0.95, 0.05, 0,
        0, 0.433, 0.567,
        0, 0.475, 0.525
        );
    texColor.rgb = mul(texColor.rgb, conversionMatrix);
    break;
}