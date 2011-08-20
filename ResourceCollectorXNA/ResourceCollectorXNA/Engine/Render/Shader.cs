﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine.Logic;

using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;
/*using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;*/

namespace ResourceCollectorXNA.Engine.Render
{
    public class Shader 
    {
        public static string AnimRenderNoSM = "AnimRenderNoSM";
        public static string NotAnimRenderNoSM = "NotAnimRenderNoSM";

        public static string AnimRenderSM = "AnimRenderSM";
        public static string NotAnimRenderSM = "NotAnimRenderSM";

        public static string AnimRenderSMSmooth = "AnimRenderSMSmooth";
        public static string NotAnimRenderSMSmooth = "NotAnimRenderSMSmooth";

        public static string CreateStaticShadowMap = "CreateStaticShadowMap";
        public static string CreateAnimShadowMap = "CreateAnimShadowMap";

        //ОТДЕЛЬНЫЕ ТЕХНИКИ НА ТРАВУ И ПУЛИ СО ВЗРЫВАМИ


     /*   protected class LoggerTrick : ContentBuildLogger
        {
            public override void LogMessage(string message, params object[] messageArgs)
            {

                Console.WriteLine(message);
            }

            public override void LogImportantMessage(string message, params object[] messageArgs)
            {
                Console.WriteLine(message);
            }

            public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
            {
                Console.WriteLine(message);
            }
        }

        protected class ContextTrick : ContentProcessorContext
        {
            public override TargetPlatform TargetPlatform { get { return TargetPlatform.Windows; } }
            public override GraphicsProfile TargetProfile { get { return GraphicsProfile.Reach; } }
            public override string BuildConfiguration { get { return string.Empty; } }
            public override string IntermediateDirectory { get { return string.Empty; } }
            public override string OutputDirectory { get { return string.Empty; } }
            public override string OutputFilename { get { return string.Empty; } }

            public override OpaqueDataDictionary Parameters { get { return parameters; } }
            OpaqueDataDictionary parameters = new OpaqueDataDictionary();

            public override ContentBuildLogger Logger { get { return logger; } }
            ContentBuildLogger logger = new LoggerTrick();

            public override void AddDependency(string filename) { }
            public override void AddOutputFile(string filename) { }

            public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters) { throw new NotImplementedException(); }
            public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName) { throw new NotImplementedException(); }
            public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName) { throw new NotImplementedException(); }
        }

        protected Shader(GraphicsDevice device, byte[] effecCode)
            : base(device, effecCode)
        { }

        public static Effect FromStream(Stream stream, GraphicsDevice device)
        {
            var content = new EffectContent();
            stream.Seek(0, SeekOrigin.Begin);
            content.EffectCode = (new StreamReader(stream)).ReadToEnd();
            byte[] ec = (new EffectProcessor()).Process(content, new ContextTrick()).GetEffectCode();
            BinaryWriter bw = new BinaryWriter(new FileStream("Effect.shader", FileMode.Create));
            bw.Write(ec);
            bw.Flush();
            bw.Close();
            return new Shader(device, ec);
        }*/
    }

    
    public class RenderArray
    {
        public string Tehniquename
        {
            get;
            private set;
        }
        public MyContainer<PivotObject> Objects
        {
            get;
            private set;
        }
        public RenderArray(string tehniquename)
        {
            Tehniquename = this.Tehniquename;
            this.Objects = new MyContainer<PivotObject>(100, 10);
        }
    }
    
}
