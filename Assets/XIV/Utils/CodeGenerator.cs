namespace XIV.Utils
{
    using System.Text;
    
    public class ClassGenerator
    {
        class Builder
        {
            public StringBuilder builder = new StringBuilder();
            public int intendNumber;
        }

        Builder classBuilder;
        Builder methodBuilder;
        Builder attributeBuilder;
        Builder usingDirectiveBuilder;
        Builder namespaceBuilder;

        public readonly string className;
        public readonly string classModifier;
        public readonly string accessModifier;

        public ClassGenerator(string className, string classModifier = "", string accessModifier = "public", string inheritance = "")
        {
            classBuilder = new Builder();
            methodBuilder = new Builder();
            attributeBuilder = new Builder();
            usingDirectiveBuilder = new Builder();
            namespaceBuilder = new Builder();

            this.className = className;
            this.classModifier = classModifier;
            this.accessModifier = accessModifier;

            var line = Space(accessModifier) + Space(classModifier) + Space(className);
            if (IsNull(inheritance) == false)
            {
                line += ": " + inheritance;
            }

            WriteLine(line, classBuilder);
        }

        public void AddNamespace(string nameSpace)
        {
            nameSpace = "namespace " + nameSpace;
            WriteLine(nameSpace, namespaceBuilder);
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

        public void EndMethod()
        {
            CloseBrackets(methodBuilder);
            WriteLine("", methodBuilder);
        }

        public string EndClass()
        {
            var code = "";
            code += usingDirectiveBuilder.builder.ToString() + "\n";
            code += attributeBuilder.builder.ToString();

            WriteMethodsToClass();

            if (namespaceBuilder.builder.Length > 0)
            {
                WriteClassInsideNamespace();

                code += namespaceBuilder.builder.ToString();
            }
            else
            {
                code += classBuilder.builder.ToString();
            }

            classBuilder = null;
            methodBuilder = null;
            attributeBuilder = null;
            usingDirectiveBuilder = null;
            namespaceBuilder = null;

            return code;
        }

        void WriteClassInsideNamespace()
        {
            string[] namespaceLines = namespaceBuilder.builder.ToString().Split('\n');

            namespaceBuilder.builder.Clear();

            int length = namespaceLines.Length - 1; // ignore last since its empty
            for (int i = 0; i < length; i++)
            {
                WriteLine(namespaceLines[i], namespaceBuilder);
                OpenBrackets(namespaceBuilder);
            }

            string[] classLines = classBuilder.builder.ToString().Split('\n');
            for (int i = 0; i < classLines.Length; i++)
            {
                WriteLine(classLines[i], namespaceBuilder);
            }

            for (int i = 0; i < length; i++)
            {
                CloseBrackets(namespaceBuilder);
            }
        }

        void WriteMethodsToClass()
        {
            OpenBrackets(classBuilder);
            string[] methodLines = methodBuilder.builder.ToString().Split("\n");
            for (int i = 0; i < methodLines.Length; i++)
            {
                WriteLine(methodLines[i], classBuilder);
            }
            CloseBrackets(classBuilder);
        }

        void WriteLine(string line, Builder builder)
        {
            string intend = GetIntend(builder);
            builder.builder.AppendLine(intend + line);
        }

        void OpenBrackets(Builder builder)
        {
            WriteLine("{", builder);
            builder.intendNumber++;
        }

        void CloseBrackets(Builder builder)
        {
            builder.intendNumber--;
            WriteLine("}", builder);
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