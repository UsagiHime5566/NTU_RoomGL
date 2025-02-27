int configuration = 2;
bool drawdual = false;

const float lwidth = 0.03;
const float pwidth = 0.12;
float ledge0 = 0.0, ledge1 = lwidth;
float pedge0 = 0.5*(lwidth+pwidth), pedge1 = pwidth;
const float scale = 2.5;
float zoom = 1.0;

const float PI =  3.141592654;
float eps = 1e-4;

const vec3 pcolor0 = vec3(1,0,0);
const vec3 pcolor1 = vec3(0,1,0);
const vec3 pcolor2 = vec3(1,1,0);
const vec3 pcolor3 = vec3(0,1,1);
const vec3 lcolor0 = pcolor0;
const vec3 lcolor1 = pcolor1;
const vec3 lcolor2 = pcolor2;
vec3 ccolor0 = vec3(1,1,1);
const vec3 ccolor1 = vec3(0,0,1);

// Distance from the conic
float dist(vec3 p, mat3 m) {
  return dot(p,m*p);
}

// The gradient uses the same matrix.
// Don't homegenize the result!
vec3 grad(vec3 p, mat3 m) {
  return m*p*2.0;
}

float conic(vec3 p, mat3 m) {
  float d = dist(p,m);
  vec3 dd = grad(p,m);
  d = abs(d/(p.z*length(dd.xy))); // Normalize for Euclidean distance
  return 1.0-smoothstep(ledge0,ledge1,d);
}

float point0(vec3 p, vec3 q) {
  if (abs(p.z) < eps) return 0.0;
  if (abs(q.z) < eps) return 0.0;
  p /= p.z; q /= q.z; // Normalize
  return 1.0-smoothstep(pedge0,pedge1,distance(p,q));
}

float line0(vec3 p, vec3 q) {
  // Just treat as a degenerate conic. Note factor of 2.
  // We could do this more efficiently of course.
  return conic(p,mat3(0,  0,  q.x,
                      0,  0,  q.y,
                      q.x,q.y,2.0*q.z));
}

float newpoint(vec3 p, vec3 q) {
  if (drawdual) return line0(p,q);
  else return point0(p,q);
}

float newline(vec3 p, vec3 q) {
  if (drawdual) return point0(p,q);
  else return line0(p,q);
}

vec3 join(vec3 p, vec3 q) {
  // Return either intersection of lines p and q
  // or line through points p and q, r = kp + jq
  return cross(p,q);
}

// Screen coords to P2 coords
vec3 map(vec2 p) {
  return vec3(scale*zoom*(2.0*p - iResolution.xy) / iResolution.y, 1);
}

//-------------------------------------------------
//From https://www.shadertoy.com/view/XtXGRS#
vec2 rotate(in vec2 p, in float t) {
  return p * cos(-t) + vec2(p.y, -p.x) * sin(-t);
}

vec3 transform(vec3 p) {
  float t = iTime;
  //if (keypress(CHAR_SPACE)) t = 0.0;
  p.x -= sin(0.08*t);
  p.xy = rotate(p.xy,0.2*t);
  p.yz = rotate(p.yz,0.1*t);
  return p;
}

vec3 cmix(vec3 color0, vec3 color1, float level) {
    return mix(color0,color1,level);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord ) {
  drawdual = true;
  configuration = 2;
  vec3 p = map(fragCoord.xy);
  vec3 p0,p1,p2,p3,p4; // p4 is the movable point
  if (configuration == 0) {
    p0 = vec3(1,0,0); p1 = vec3(0,1,0);
    p2 = vec3(0,0,1); p3 = vec3(1,1,1);
    p4 = vec3(0.5,-1,1);
  } else if (configuration == 1) {
    p0 = vec3(0,0,1); p1 = vec3(1,0,1);
    p2 = vec3(0,1,1); p3 = vec3(1,1,1);
    p4 = vec3(0.5,-1,1);
  } else if (configuration == 2) {
    p0 = vec3(0,0,1);
    p1 = vec3(0,1,1);
    p2 = vec3(0.866,-0.5,1);
    p3 = vec3(-0.866,-0.5,1);
    p4 = vec3(0,0.618,1);
  } else {
    p0 = vec3(1,0,1);  p1 = vec3(0,1,1);
    p2 = vec3(-1,0,1); p3 = vec3(0,-1,1);
    p4 = vec3(0.5,-1,1);
  }
  p0 = transform(p0); p1 = transform(p1);
  p2 = transform(p2); p3 = transform(p3);
  if (iMouse.x != 0.0) {
    p4 = map(iMouse.xy);
  }


  vec3 p01 = join(p0,p1);
  vec3 p02 = join(p0,p2);
  vec3 p03 = join(p0,p3);
  vec3 p12 = join(p1,p2);
  vec3 p13 = join(p1,p3);
  vec3 p23 = join(p2,p3);

  // The line through A,B intersects line L at
  // (A.L)B - (B.L)A, whose conjugate is (A.L)B + (B.L)A
  vec3 s01 = dot(p4,p0)*p1 + dot(p4,p1)*p0;
  vec3 t01 = dot(p4,p0)*p1 - dot(p4,p1)*p0;
  vec3 s12 = dot(p4,p1)*p2 + dot(p4,p2)*p1;
  vec3 t12 = dot(p4,p1)*p2 - dot(p4,p2)*p1;
  vec3 s23 = dot(p4,p2)*p3 + dot(p4,p3)*p2;
  vec3 t23 = dot(p4,p2)*p3 - dot(p4,p3)*p2;
  vec3 s03 = dot(p4,p3)*p0 + dot(p4,p0)*p3;
  vec3 t03 = dot(p4,p3)*p0 - dot(p4,p0)*p3;

  vec3 s02 = dot(p4,p0)*p2 + dot(p4,p2)*p0;
  vec3 t02 = dot(p4,p0)*p2 - dot(p4,p2)*p0;
  vec3 s13 = dot(p4,p1)*p3 + dot(p4,p3)*p1;
  vec3 t13 = dot(p4,p1)*p3 - dot(p4,p3)*p1;
  
  vec3 color = vec3(0);

  // The diagonal lines of the quadrangle
  color = cmix(color,lcolor2,newline(p,p01));
  color = cmix(color,lcolor2,newline(p,p02));
  color = cmix(color,lcolor2,newline(p,p03));
  color = cmix(color,lcolor2,newline(p,p12));
  color = cmix(color,lcolor2,newline(p,p13));
  color = cmix(color,lcolor2,newline(p,p23));
  
  // The moving line
  color = cmix(color,lcolor1,newline(p,p4));

  vec3 l0 = join(s01,s12);
  vec3 l1 = join(s23,s03);
  vec3 l2 = join(s12,s23);
  vec3 l3 = join(s03,s01);
  //vec3 l4 = join(s02,s23);
  //vec3 l5 = join(s01,s13);
  color = cmix(color,lcolor0,newline(p,l0));
  color = cmix(color,lcolor0,newline(p,l1));
  color = cmix(color,lcolor0,newline(p,l2));
  color = cmix(color,lcolor0,newline(p,l3));
  //color = cmix(color,lcolor0,newline(p,l4));
  //color = cmix(color,lcolor0,newline(p,l5));

  // The points of the quadrangle
  color = cmix(color,pcolor0,newpoint(p,p0));
  color = cmix(color,pcolor0,newpoint(p,p1));
  color = cmix(color,pcolor0,newpoint(p,p2));
  color = cmix(color,pcolor0,newpoint(p,p3));

  color = cmix(color,pcolor2,newpoint(p,s01));
  color = cmix(color,pcolor2,newpoint(p,t01));
  color = cmix(color,pcolor2,newpoint(p,s12));
  color = cmix(color,pcolor2,newpoint(p,t12));
  color = cmix(color,pcolor2,newpoint(p,s23));
  color = cmix(color,pcolor2,newpoint(p,t23));
  color = cmix(color,pcolor2,newpoint(p,s03));
  color = cmix(color,pcolor2,newpoint(p,t03));

  color = cmix(color,pcolor1,newpoint(p,join(l0,l1)));
  color = cmix(color,pcolor1,newpoint(p,join(l2,l3)));

  fragColor = vec4(pow(1.0*color,vec3(0.4545)),1);
}
