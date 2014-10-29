﻿namespace Demo
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    using Demo.examples.cs.controls;

    using Examples;

    using ThreeCs.Extras.Helpers;
    using ThreeCs.Math;
    using ThreeCs.Cameras;
    using ThreeCs.Core;
    using ThreeCs.Extras;
    using ThreeCs.Extras.Geometries;
    using ThreeCs.Lights;
    using ThreeCs.Materials;
    using ThreeCs.Objects;
    using ThreeCs.Scenes;

    [Example("misc_controls_transform", ExampleCategory.Misc, "controls", 0.4f)]
    class misc_controls_transform : Example
    {
        private PerspectiveCamera camera;

        private Scene scene;

        private Object3D mesh;

        private TransformControls control;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public override void Load(Control openTKcontrol)
        {
            base.Load(openTKcontrol);

            camera = new PerspectiveCamera(70, openTKcontrol.Width / (float)openTKcontrol.Height, 1, 3000);
            camera.Position = new Vector3(1000, 500, 1000);
            camera.LookAt(new Vector3(0, 200, 0));

            scene = new Scene();
            scene.Add( new GridHelper( 500, 100 ) );

            var light = new DirectionalLight(Color.White, 2);
            light.Position = new Vector3(1, 1, 1);
            scene.Add(light);

            var texture = ImageUtils.LoadTexture(@"examples/textures/crate.gif");
            texture.Anisotropy = this.renderer.MaxAnisotropy;

            var geometry = new BoxGeometry(200, 200, 200);
            var material = new MeshLambertMaterial(null) { Map = texture };

            control = new TransformControls( camera/*, renderer.domElement*/ );

            //control.addEventListener( 'change', render );

            mesh = new Mesh(geometry, material);
            scene.Add(mesh);

            control.Attach( mesh );
            scene.Add( control );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSize"></param>
        public override void Resize(Size clientSize)
        {
            Debug.Assert(null != this.camera);
            Debug.Assert(null != this.renderer);

            this.camera.Aspect = clientSize.Width / (float)clientSize.Height;
            this.camera.UpdateProjectionMatrix();

            this.renderer.Size = clientSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Render()
        {
            control.Update();

            renderer.Render(scene, camera);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Unload()
        {
            this.scene.Dispose();
            this.camera.Dispose();

            base.Unload();
        }
    }
}