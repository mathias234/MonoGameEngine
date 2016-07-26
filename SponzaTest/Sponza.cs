﻿using System.IO;
using NewEngine.Engine.components;
using NewEngine.Engine.Core;
using NewEngine.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SponzaTest {
    public class Sponza : Game {
        private GameObject _directionalLightObj;

        public override void Start() {
            AddObject(
                new GameObject().AddComponent(new FreeLook())
                    .AddComponent(new FreeMove())
                    .AddComponent(
                        new Camera(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70.0f),
                            CoreEngine.GetWidth()/CoreEngine.GetHeight(), 0.1f, 1000))));

            _directionalLightObj = new GameObject();
            var directionalLight = new DirectionalLight(new Vector3(1), 0.0f);
            _directionalLightObj.AddComponent(directionalLight);
            _directionalLightObj.Transform.Rotate(new Vector3(1, 0, 0), MathHelper.RadiansToDegrees(90));
            _directionalLightObj.Transform.Position = new Vector3(0, 0, 0);
            AddObject(_directionalLightObj);


            string[] sponzaModels = {
                "blue_flags", "red_flags", "green_flags", "floor_plants", "columns", "arch", "leaves", "lion", "ceiling",
                "bricks", "floor",
                "blue_fabric", "green_fabric", "red_fabric", "vase_hanging", "chains", "columns1", "roof", "flagpole",
                "details", "columns2", "background", "floor_plants_plant"
            };

            foreach (var sponzaModel in sponzaModels) {
                float displacementOffset = 0;
                float displacementScale = 0;

                if (File.Exists("sponza/" + sponzaModel + "_disp.png")) {
                    displacementScale = 0.02f;
                    displacementOffset = -0.2f;
                }

                var material = new Material(new Texture("sponza/" + sponzaModel + ".png"), 0.5f, 32,
                    new Texture("sponza/" + sponzaModel + "_nrm.png"),
                    new Texture("sponza/" + sponzaModel + "_disp.png"), displacementScale, displacementOffset);
                material.SetTexture("cutoutMask",
                    new Texture("sponza/" + sponzaModel + "_mask.png", TextureTarget.Texture2D, TextureFilter.Point));
                var sponza =
                    new GameObject().AddComponent(new MeshRenderer(new Mesh("sponza/" + sponzaModel + "/model.obj"),
                        material));
                AddObject(sponza);
            }

            var flameColor = new Vector3(226/255.0f, 88/255.0f, 34/255.0f);

            var p1 = new GameObject().AddComponent(new PointLight(flameColor, 10f, new Attenuation(0, 0, 1f)));
            p1.Transform.Position = new Vector3(-32.09187f, 7.249063f, 7.12112f);
            var p2 = new GameObject().AddComponent(new PointLight(flameColor, 10f, new Attenuation(0, 0, 1f)));
            p2.Transform.Position = new Vector3(25.13826f, 7.249063f, 7.12112f);

            var p3 = new GameObject().AddComponent(new PointLight(flameColor, 10f, new Attenuation(0, 0, 1f)));
            p3.Transform.Position = new Vector3(25.13826f, 7.249063f, -11.12112f);
            var p4 = new GameObject().AddComponent(new PointLight(flameColor, 10f, new Attenuation(0, 0, 1f)));
            p4.Transform.Position = new Vector3(-32.09187f, 7.249063f, -11.12112f);

            AddObject(p1);
            AddObject(p2);
            AddObject(p3);
            AddObject(p4);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            //directionalLightObj.Transform.Rotation *= Quaternion.FromAxisAngle(new Vector3(1, 0, 0), -0.004f);
            var flameColor = new Vector3(226/255.0f, 88/255.0f, 34/255.0f);

            if (!Input.GetKeyDown(Key.P)) return;
            var newPointLight =
                new GameObject().AddComponent(new PointLight(flameColor, 10f, new Attenuation(0, 0, 1f)));
            newPointLight.Transform.Position =
                new Vector3(CoreEngine.GetCoreEngine.RenderingEngine.MainCamera.Transform.GetTransformedPosition());
            AddObject(newPointLight);
        }
    }
}