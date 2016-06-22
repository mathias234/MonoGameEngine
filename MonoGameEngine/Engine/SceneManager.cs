﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Physics;

namespace MonoGameEngine.Engine {
    public static class SceneManager {
        public static void LoadScene(string name) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<GameObject>));
            FileStream fs = null;
            try {
                fs = new FileStream(@"Scenes\" + name, FileMode.OpenOrCreate);
                CoreEngine.instance.GameObjects = (List<GameObject>)serializer.Deserialize(fs);
                fs.Dispose();
            }
            catch (DirectoryNotFoundException) {
                Directory.CreateDirectory("Scenes");
                SaveScene(name);
            }
            catch (System.InvalidOperationException) {
                fs?.Close();
                CreateNewScene(name);
            }
            catch (Exception) {
                throw;
            }
            foreach (var gameObject in CoreEngine.instance.GameObjects) {
                gameObject.Initialize();
            }
        }

        public static void SaveScene(string name) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<GameObject>));
            FileStream fs = null;
            try {
                fs = new FileStream(@"Scenes\" + name, FileMode.OpenOrCreate);
                serializer.Serialize(fs, CoreEngine.instance.GameObjects);
                fs.Close();
            }
            catch (DirectoryNotFoundException) {
                Directory.CreateDirectory("Scenes");
                SaveScene(name);
            }
            catch (Exception) {
                throw;
            }
        }

        public static void CreateNewScene(string name) {
            CoreEngine.instance.GameObjects = new List<GameObject>();

            var camera = new GameObject(new Vector3(0, 20, -100));
            camera.AddComponent<Camera>();
            camera.name = "Camera";
            camera.Instantiate();

            var sampleCube = new GameObject(new Vector3(0, 30, 0));
            sampleCube.AddComponent<MeshRenderer>();
            sampleCube.GetComponent<MeshRenderer>().Mesh = Primitives.CreateCube();
            sampleCube.GetComponent<MeshRenderer>().Color = Color.LightGray;
            var sphereCollider = sampleCube.AddComponent<SphereCollider>();
            sphereCollider.Radius = 2;
            sphereCollider.Mass = 10;
            sphereCollider.IsStatic = false;
            sampleCube.name = "cube";
            sampleCube.Instantiate();

            var ground = new GameObject(new Vector3(0, 0, 0));
            ground.AddComponent<MeshRenderer>();
            ground.GetComponent<MeshRenderer>().Mesh = Primitives.CreateCube();
            ground.GetComponent<MeshRenderer>().Color = Color.LightGray;
            ground.Transform.Scale = new Vector3(200, 1, 200);
            var boxCollider = ground.AddComponent<BoxCollider>();
            boxCollider.Height = 2f;
            boxCollider.Width = 200 * 2f;
            boxCollider.Length = 200 * 2f;
            boxCollider.Mass = 0;
            boxCollider.IsStatic = true;
            ground.name = "ground";
            ground.Instantiate();
        }
    }
}