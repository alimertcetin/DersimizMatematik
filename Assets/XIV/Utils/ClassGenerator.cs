﻿namespace XIV.Utils
{
    using System.Text;
    using System;
    
    public class ClassGenerator
    {
        class Builder
        {
            public StringBuilder builder = new StringBuilder();
            public int intendNumber;
        }

        Builder classBuilder;
        Builder memberBuilder;
        Builder methodBuilder;
        Builder attributeBuilder;
        Builder usingDirectiveBuilder;
        Builder namespaceBuilder;
        Builder innerClassBuilder;

        public readonly string className;
        public readonly string classModifier;
        public readonly string accessModifier;
        public readonly bool isInnerClass;

        string GENERATION_TEXT => "/*" + Environment.NewLine + 
                                  "* Generated by " + typeof(ClassGenerator).Namespace + "." + nameof(ClassGenerator) + Environment.NewLine +
                                  "*/";

        public ClassGenerator(string className, string classModifier = "", string accessModifier = "public", string inheritance = "", bool isInnerClass = false)
        {
            classBuilder = new Builder();
            memberBuilder = new Builder();
            methodBuilder = new Builder();
            attributeBuilder = new Builder();
            usingDirectiveBuilder = new Builder();
            namespaceBuilder = new Builder();
            innerClassBuilder = new Builder();

            this.className = className;
            this.classModifier = classModifier;
            this.accessModifier = accessModifier;
            this.isInnerClass = isInnerClass;

            var line = Space(accessModifier) + Space(classModifier) + Space("class " + className);
            if (IsNull(inheritance) == false)
            {
                line += ": " + inheritance;
            }

            WriteLine(line, classBuilder);
        }

        public void AddNamespace(string @namespace)
        {
            @namespace = "namespace " + @namespace;
            WriteLine(@namespace, namespaceBuilder);
        }

        public void Use(string libName)
        {
            libName = "using " + libName + ";";
            WriteLine(libName, usingDirectiveBuilder);
        }

        public void AddAttribute(string attribute)
        {
            attribute = "[" + attribute + "]";

            WriteLine(attribute, attributeBuilder);
        }

        public void AddInnerClass(ClassGenerator classGenerator)
        {
            AddInnerClass(classGenerator.EndClass());
        }

        public void AddInnerClass(string innerClass)
        {
            var lines = innerClass.Split(Environment.NewLine.ToCharArray());
            for (int i = 0; i < lines.Length; i++)
            {
                WriteLine(lines[i], innerClassBuilder);
            }
        }

        public void StartMethod(string methodName, string accessModifier = "public", string modifier = "", string returnType = "void", params string[] paramaters)
        {
            if (methodBuilder.intendNumber > 0)
            {
                accessModifier = "";
            }

            var line = Space(accessModifier) + Space(modifier) + Space(returnType) + Space(methodName);

            line += "(";
            if (paramaters.Length > 0)
            {
                line += paramaters[0];
                for (int i = 1; i < paramaters.Length; i++)
                {
                    line += Space("," + paramaters[i], false);
                }
            }
            line += ")";

            WriteLine(line, methodBuilder);
            OpenBrackets(methodBuilder);
        }

        public void WriteMethodLine(string line)
        {
            line = line.TrimEnd(';');
            line += ";";
            WriteLine(line, methodBuilder);
        }

        public void WriteCommentInsideMethod(string comment, bool oneLine = true)
        {
            AddComment(comment, methodBuilder, oneLine);
        }

        public void EndMethod()
        {
            CloseBrackets(methodBuilder);
            WriteLine("", methodBuilder);
        }
        
        public void AddField(string fieldName, string fieldValue, string returnType, string modifier = "", string accessModifier = "public")
        {
            var line = Space(accessModifier) + Space(modifier) + Space(returnType) + Space(fieldName) + "=" + Space(fieldValue, false);
            line += ";";

            WriteLine(line, memberBuilder);
        }
        
        public void AddGetOnlyProperty(string propertyName, string returnType, string getBlockContent, string modifier = "", string accessModifier = "public")
        {
            var line = Space(accessModifier) + Space(modifier) + Space(returnType) + Space(propertyName);
            WriteLine(line, memberBuilder);
            
            OpenBrackets(memberBuilder);
            WritePropertyBlock("get", getBlockContent);
            CloseBrackets(memberBuilder);
        }
        
        public void AddGetSetProperty(string propertyName, string returnType, string getBlockContent, string setBlockContent, string modifier = "", string accessModifier = "public")
        {
            var line = Space(accessModifier) + Space(modifier) + Space(returnType) + Space(propertyName);
            WriteLine(line, memberBuilder);
            
            OpenBrackets(memberBuilder);
            WritePropertyBlock("get", getBlockContent);
            WritePropertyBlock("set", setBlockContent);
            CloseBrackets(memberBuilder);
        }

        void WritePropertyBlock(string blockType, string getBlock)
        {
            WriteLine(blockType, memberBuilder);
            OpenBrackets(memberBuilder);
            WriteLine(getBlock, memberBuilder);
            CloseBrackets(memberBuilder);
        }

        public string EndClass()
        {
            StringBuilder code = new StringBuilder(512);

            if (isInnerClass == false) code.AppendLine(GENERATION_TEXT);
            
            if (usingDirectiveBuilder.builder.Length > 0) code.AppendLine(usingDirectiveBuilder.builder.ToString());
            if (attributeBuilder.builder.Length > 0) code.AppendLine(attributeBuilder.builder.ToString());

            FillClassBody();

            if (namespaceBuilder.builder.Length > 0)
            {
                WriteClassInsideNamespace();

                code.AppendLine(namespaceBuilder.builder.ToString());
            }
            else
            {
                code.AppendLine(classBuilder.builder.ToString());
            }

            classBuilder = null;
            methodBuilder = null;
            attributeBuilder = null;
            usingDirectiveBuilder = null;
            namespaceBuilder = null;

            return code.ToString();
        }

        void FillClassBody()
        {
            OpenBrackets(classBuilder);

            if (innerClassBuilder.builder.Length > 0) AddComment("Inner Classes", classBuilder, true);

            string[] innerClassLines = innerClassBuilder.builder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < innerClassLines.Length; i++)
            {
                WriteLine(innerClassLines[i], classBuilder);
            }
            if (innerClassLines.Length > 0) WriteLine("", classBuilder);

            if (memberBuilder.builder.Length > 0) AddComment("Members", classBuilder, true);
            string[] memberLines = memberBuilder.builder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < memberLines.Length; i++)
            {
                WriteLine(memberLines[i], classBuilder);
            }
            if (memberLines.Length > 0) WriteLine("", classBuilder);

            if (methodBuilder.builder.Length > 0) AddComment("Functions", classBuilder, true);
            string[] methodLines = methodBuilder.builder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < methodLines.Length; i++)
            {
                WriteLine(methodLines[i], classBuilder);
            }
            if (methodLines.Length > 0) WriteLine("", classBuilder);
            CloseBrackets(classBuilder, "class " + className);
            WriteLine(" ", classBuilder);
        }

        void WriteClassInsideNamespace()
        {
            string[] namespaceLines = namespaceBuilder.builder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            namespaceBuilder.builder.Clear();

            int length = namespaceLines.Length;
            for (int i = 0; i < length; i++)
            {
                WriteLine(namespaceLines[i], namespaceBuilder);
                OpenBrackets(namespaceBuilder);
            }

            string[] classLines = classBuilder.builder.ToString().Split(Environment.NewLine.ToCharArray());
            for (int i = 0; i < classLines.Length; i++)
            {
                WriteLine(classLines[i], namespaceBuilder);
            }

            WriteLine("", namespaceBuilder);
            for (int i = 0; i < length; i++)
            {
                CloseBrackets(namespaceBuilder, namespaceLines[i]);
            }
        }

        void AddComment(string comment, Builder builder, bool oneLine)
        {
            if (oneLine)
            {
                WriteLine("// " + comment, builder);
                return;
            }

            WriteLine("/*", builder);
            WriteLine("* " + comment, builder);
            WriteLine("*/", builder);
        }

        void WriteLine(string line, Builder builder)
        {
            string intend = GetIntend(builder);
            builder.builder.AppendLine(intend + line);
        }

        void OpenBrackets(Builder builder, string bracesLineComment = "")
        {
            var braceLine = "{";
            if (IsNull(bracesLineComment) == false)
            {
                braceLine += "// " + bracesLineComment;
            }
            WriteLine(braceLine, builder);
            builder.intendNumber++;
        }

        void CloseBrackets(Builder builder, string bracesLineComment = "")
        {
            builder.intendNumber--;
            var braceLine = "}";
            if (IsNull(bracesLineComment) == false)
            {
                braceLine += " // " + bracesLineComment;
            }
            WriteLine(braceLine, builder);
        }

        static string Space(string value, bool after = true)
        {
            if (string.IsNullOrEmpty(value)) return "";

            return after ? value + " " : " " + value;
        }

        static bool IsNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value);
        }

        string GetIntend(Builder builder)
        {
            string intend = "";
            for (int i = 0; i < builder.intendNumber; i++)
            {
                intend += "\t";
            }

            return intend;
        }

        public override string ToString()
        {
            return EndClass();
        }
    }
}