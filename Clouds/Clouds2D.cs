﻿using EveManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Utils;

namespace Atmosphere
{
    public class Clouds2DMaterial : MaterialManager
    {
        [Persistent] 
        Color _Color = new Color(1,1,1,1);
        [Persistent]
        String _MainTex = "";
        [Persistent]
        String _DetailTex = "";
        [Persistent]
        float _FalloffPow = 2f;
        [Persistent]
        float _FalloffScale = 3f;
        [Persistent]
        float _DetailScale = 100f;
        [Persistent]
        Vector3 _DetailOffset = new Vector3(0, 0, 0);
        [Persistent]
        float _DetailDist = 0.00875f;
        [Persistent]
        float _MinLight = .5f;
        [Persistent]
        float _FadeDist = 10f;
        [Persistent]
        float _FadeScale = .002f;
        [Persistent]
        float _RimDist = 1f;
    }

    class Clouds2D
    {
        GameObject CloudMesh;
        [Persistent]
        Clouds2DMaterial cloudMaterial;
        private static Shader cloudShader = null;
        private static Shader CloudShader
        {
            get
            {
                if (cloudShader == null)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    StreamReader shaderStreamReader = new StreamReader(assembly.GetManifestResourceStream("Atmosphere.Shaders.Compiled-SphereCloud.shader"));
                    String shaderTxt = shaderStreamReader.ReadToEnd();
                    cloudShader = new Material(shaderTxt).shader;
                } return cloudShader;
            }
        }
        public void Apply(float radius, Transform parent)
        {
            Material newmat = new Material(CloudShader);
            cloudMaterial.ApplyMaterialProperties(newmat);
            HalfSphere hp = new HalfSphere(radius, newmat);
            CloudMesh = hp.GameObject;
            CloudMesh.transform.parent = parent;
            CloudMesh.transform.localPosition = Vector3.zero;
            CloudMesh.transform.localScale = Vector3.one;
            CloudMesh.layer = EVEManager.MACRO_LAYER;
        }

        internal void UpdateRotation(Quaternion rotation)
        {
            CloudMesh.transform.rotation = rotation;
            Matrix4x4 mtrx = Matrix4x4.TRS(Vector3.zero, rotation, new Vector3(1, 1, 1));
		    // Update the rotation matrix.
            //mtrx = Matrix4x4.identity;
            CloudMesh.GetComponent<MeshRenderer>().material.SetMatrix("_Rotation", mtrx);

        }
    }
}
