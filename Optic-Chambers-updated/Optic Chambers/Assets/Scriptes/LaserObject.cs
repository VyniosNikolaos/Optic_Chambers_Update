

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserObject
{
    // Object variables
    private List<Vector2> stepList = new List<Vector2>();
    
    // Laser Jumeling
    private LaserObject jumeledLaserObject = null;
    private Vector2 symetryAxis;
    private Vector2 symetryAxisPoint;
    
    // Line Look
    private LineRenderer lineRenderer;
    private Color laserColor = Color.red;

    public List<Vector2> getSteps()
    {
        return this.stepList;
    }
    
    public LaserObject(LineRenderer lineRenderer, Material laserMaterial, bool isTwin = false)
    {
        this.lineRenderer = lineRenderer;
        lineRenderer.material = laserMaterial;
        if (!isTwin)
        {
            addStep(lineRenderer.transform.position);
        }
    }

    public void setColor(Color color)
    {
        laserColor = color;
    }
    
    public Color getColor()
    {
        return laserColor;
    }
    
    public void addStep(Vector2 stepPosition)
    {
        stepList.Add(stepPosition);
    }

    public void setWeekTwin(Vector2 symetryAxisF, Vector2 symetryAxisPointF, LineRenderer twinLineRenderer)
    {
        jumeledLaserObject = new LaserObject(twinLineRenderer, lineRenderer.material, true);
        symetryAxis = -symetryAxisF.normalized;
        symetryAxisPoint = symetryAxisPointF;
        symetryAxis = new Vector2 (-symetryAxis.y, symetryAxis.x);
        // Set point for Twin
        int i = 1;
        foreach (Vector2 strongPoint in stepList)
        {
            float weakPointX = strongPoint.x; 
            
            float ySymmetryAxis = symetryAxisPointF.y;
            
            float distanceAxisStrong = strongPoint.y - ySymmetryAxis;
            
            float weakPointY = ySymmetryAxis-distanceAxisStrong;
            
            Vector2 weakPoint = new Vector2(weakPointX, weakPointY);

            if (i > 1)
            {
                jumeledLaserObject.addStep(Lazer.CheckCollision(jumeledLaserObject.stepList[jumeledLaserObject.stepList.Count-1], weakPoint));
                if (jumeledLaserObject.stepList[jumeledLaserObject.stepList.Count-1] != weakPoint)
                {
                    break;
                }
            }
            else
            {
                jumeledLaserObject.addStep(weakPoint);
            }
            i++;
            
            // Debug Grave
            // Debug.DrawLine(strongPoint, weakPoint, Color.green);
            // Debug.DrawLine(symetryAxisPoint, weakPoint, Color.cyan);
            // Debug.DrawLine(symetryAxisPoint, strongPoint, Color.magenta);
        }
        
    }
    
    public static Vector2 rotate(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    

    public void Draw(bool isTwin = false)
    {
        DefineColor(isTwin);
        
        lineRenderer.positionCount = stepList.Count;
        
        int iterator = 0;
        foreach (Vector2 point in stepList)
        {
            //Debug.Log(point);
            lineRenderer.SetPosition(iterator, point);
            iterator++;
        }

        if (jumeledLaserObject != null)
        {
            jumeledLaserObject.Draw(true);
        }
    }

    void DefineColor(bool isTwin)
    {
        if (isTwin)
        {
            laserColor *= 1.5f;
        }
        
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(laserColor, 0.0f), new GradientColorKey(laserColor, 1.0f) },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(alpha, 0.0f),
                new GradientAlphaKey(0.8f, 0.8f),
                new GradientAlphaKey(0f, 1.0f)
            }
        );
        lineRenderer.colorGradient = gradient;
        
    }
}
