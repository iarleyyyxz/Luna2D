#version 330 core

layout(location = 0) in vec2 aPosition;   // world position (já transformada no CPU)
layout(location = 1) in vec2 aUV;
layout(location = 2) in vec4 aColor;
layout(location = 3) in float aTexIndex;
layout(location = 4) in float aLayer;     // opcional, pode ser 0

uniform mat4 projection;

out vec2 vUV;
out vec4 vColor;
flat out int vTexIndex;

void main()
{
    vUV = aUV;
    vColor = aColor;
    vTexIndex = int(aTexIndex);

    gl_Position = projection * vec4(aPosition, 0.0, 1.0);
}
