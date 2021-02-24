#pragma target 5.0

void ddx_fine_float(float In, out float OutX, out float OutY){
    OutX = ddx_fine(In);
    OutY = ddy_fine(In);
}