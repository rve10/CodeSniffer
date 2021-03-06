﻿using CodeSniffer.Interfaces;
using CodeSniffer.Models.CodeSmells;
using CodeSniffer.Models.Metrics;
using CodeSniffer.Models.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeSniffer.Models
{
    public class Class : ICodeFragment
    {
        public IList<Method> Methods { get; private set; }
        public IList<Class> Classes { get; private set; }

        public string Name { get; private set; }

        public string Content { get; private set; }

        public IList<ICodeFragment> Children
        {
            get
            {
                return Methods.Cast<ICodeFragment>().ToList();
            }
            set
            {
                //not impl. setter is needed for json Serialization / Deserialization :-(
            }
        }

        public IList<IMetric> Metrics { get; set; }

        public IList<ICodeSmell> CodeSmells { get; set; }

        public IList<string> MemberDeclarartions { get; private set; }
        public IList<string> InstanceVariables { get; private set; }


        private Object lockObj = new Object();
        private string _additionalCasesDir;
        private string _additionalCasesFile;

        public int NumberOfMethods
        {
            get
            {
                return Methods.Count;
            }
        }

        public void AddInstanceVariable(string text)
        {
            InstanceVariables.Add(text);
        }

        public Class(string name, string text)
        {
            Name = name;
            Content = LineEndingConverter.ConvertToCRLF(text);
            Methods = new List<Method>();
            Classes = new List<Class>();
            MemberDeclarartions = new List<string>();
            InstanceVariables = new List<string>();

            Metrics = new List<IMetric>();
            Metrics.Add(new LinesOfCode(Content));
            Metrics.Add(new NumberOfMembers(MemberDeclarartions));
            Metrics.Add(new TCC(Methods));
            Metrics.Add(new WMC(Methods));
            Metrics.Add(new ATFD(Methods));

            CodeSmells = new List<ICodeSmell>();
            CodeSmells.Add(new LargeClass(Metrics[2], Metrics[3], Metrics[4], Metrics[0])); //TODO: REFACTOR THIS TEMP SOLUTION

            var p = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _additionalCasesDir = p + "\\TrainingsData\\";
            _additionalCasesFile = "AdditionalData.csv";


            foreach (var codesmell in CodeSmells)
            {
                codesmell.Updated += OnCodeSmellUpdated;
            }
        }

        private void OnCodeSmellUpdated()
        {
            WriteToTrainingSet(_additionalCasesFile, _additionalCasesDir);
        }

        public void AddMethod(Method method)
        {
            lock (lockObj)
            {
                Methods.Add(method);
            }
        }

        public void AddClass(Class classToAdd)
        {
            lock (lockObj)
            {
                Classes.Add(classToAdd);
            }
        }

        public void AddMemberDecleration(string member)
        {
            lock (lockObj)
            {
                MemberDeclarartions.Add(member);
            }
        }

        public void FindClassRelations(List<Class> totalClassOverView)
        {
            Methods.ToList().ForEach(x => x.FindRelatedClassForOutboundInvocation(totalClassOverView));
        }

        public void WriteToTrainingSet(string filename, string dir = "")
        {
            filename = dir + "Class" + filename;

            StringBuilder sb = new StringBuilder();
            StringBuilder headers = new StringBuilder();

            headers.Append("Name,");
            sb.Append(Name + ",");

            for (int i = 0; i < Metrics.Count; i++)
            {
                var metric = Metrics[i];
                sb.Append(metric.Value);
                headers.Append(metric.Name);

                sb.Append(",");
                headers.Append(",");
            }

            for (int i = 0; i < CodeSmells.Count; i++)
            {
                var codeSmell = CodeSmells[i];
                sb.Append(codeSmell.IsDetected.ToString());
                sb.Append("," + codeSmell.Confidence.ToString());
                headers.Append(codeSmell.Name);
                headers.Append("," + codeSmell.Name + "Score");

                if (i < CodeSmells.Count - 1)
                {
                    sb.Append(",");
                    headers.Append(",");
                }
            }

            if (!File.Exists(filename))
            {
                WriteLine(headers.ToString(), filename);
            }

            WriteLine(sb.ToString(), filename);
        }

        private void WriteLine(string line, string filename)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(filename, FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine(line);
            }
        }
    }
}
