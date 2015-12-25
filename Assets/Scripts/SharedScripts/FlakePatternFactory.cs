/**
 * Copyright (c) 2009 Alec McEachran
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 */
/**
 * Ported to Unity script by Kevin Bloom
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlakePatternFactory : Singleton<FlakePatternFactory> {
	
	static private float d60 = Mathf.PI / 3;
	private float COS60 = Mathf.Cos(d60);
	private float SIN60 = Mathf.Sin(d60);
	
	static private float d120 = d60 * 2;
	private float COS120 = Mathf.Cos(d120);
	private float SIN120 = Mathf.Sin(d120);
	
	
	public List<Vector3> generate(uint points, uint radius)
	{
		List<Vector3> pattern = randomPattern(points, radius);
		return generateFromPattern(pattern);
	}
	
	public List<Vector3> randomPattern(uint points, uint radius)
	{
		List<Vector3> pattern = new List<Vector3>((int)points);
		
		float angle = Mathf.PI / (points << 1);
		while (--points > 0)
		{
			uint len = (uint)(UnityEngine.Random.Range(0.0f,1.0f) * (float)radius);
			float ang = angle * points;
			pattern.Add(new Vector3(Mathf.Cos(ang) * len,0.0f, Mathf.Sin(ang) * len));
		}
		
		return pattern;
	}
	
	public List<Vector3> generateFromPattern(List<Vector3> points)
	{
		List<Vector3> a = convertPointsToSixtyDegrees(points);
		List<Vector3> b = reflectPointsInXAxis(a);
		List<Vector3> c = b.Concat(a).ToList();
		List<Vector3> c2 = rotatePointsAround60Degrees(c);
		List<Vector3> d = c2.Concat(c).ToList();
		List<Vector3> d2 = rotatePointsAround120Degrees(d);
		List<Vector3> e = d2.Concat(d).ToList();
		List<Vector3> e2 = rotatePointsAround120Degrees(e);
		
		return e2.Concat(c2).ToList().Concat(b).ToList().Concat(a).ToList();
	}
	
	private List<Vector3> convertPointsToSixtyDegrees(List<Vector3> points)
	{
		return transformPoints(points, convertPointToSixtyDegrees);
	}
	
	private Vector3 convertPointToSixtyDegrees(Vector3 point)
	{
		return new Vector3(point.x + point.z * SIN60, 0.0f, point.z * COS60);
	}
	
	private List<Vector3> reflectPointsInXAxis(List<Vector3> points)
	{
		List<Vector3> reflected = transformPoints(points, reflectPointInXAxis);
		reflected.Reverse ();
		return reflected;
	}
	
	private Vector3 reflectPointInXAxis(Vector3 point)
	{
		return new Vector3(point.x, 0.0f, -point.z);
	}
	
	private List<Vector3> rotatePointsAround60Degrees(List<Vector3> points)
	{
		return transformPoints(points, rotatePointAround60Degrees);
	}
	
	private Vector3 rotatePointAround60Degrees(Vector3 point)
	{
		float x = point.x;
		float y = point.z;
		return new Vector3(x * COS60 + y * SIN60, 0.0f, y * COS60 - x * SIN60);
	}
	
	private List<Vector3> rotatePointsAround120Degrees(List<Vector3> points)
	{
		return transformPoints(points, rotatePointAround120Degrees);
	}
	
	private Vector3 rotatePointAround120Degrees(Vector3 point)
	{
		float x = point.x;
		float y = point.z;
		return new Vector3(x * COS120 + y * SIN120, 0.0f, y * COS120 - x * SIN120);
	}
	
	private List<Vector3> transformPoints(List<Vector3> points, Func<Vector3,Vector3> method)
	{
		List<Vector3> transformed = new List<Vector3>(points.Count);
		
		int i = points.Count;
		while (i-- > 0)
		{
			Vector3 point = points[i];
			point = method(point);
			transformed.Add(point);
		}
		
		return transformed;
	}
}
