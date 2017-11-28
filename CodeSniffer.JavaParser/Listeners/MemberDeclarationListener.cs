﻿using Antlr4.Runtime.Misc;
using CodeSniffer.Listeners;
using CodeSniffer.Models;
using NLog;

namespace CodeSniffer.Listeners
{
    public class MemberDeclarationListener : BaseListener
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private Class _currentClass;

        public void setCurrentClass(Class currentClass)
        {
            _currentClass = currentClass;
        }

        public void resetCurrentClass()
        {
            _currentClass = null;
        }

        public override void EnterMemberDeclaration([NotNull] JavaParser.MemberDeclarationContext context)
        {
            Logger.Debug("parsing member declaration");

            var inputStream = context.Start.InputStream;

            var interval = new Interval(context.Start.StartIndex, context.Stop.StopIndex);

            var text = inputStream.GetText(interval);

            if(text.Contains("{")) //this is a method decl, strip until the open curly brace
            {
                text = text.Substring(0, text.IndexOf('{'));
            }

            if (_currentClass != null)
                _currentClass.AddMemberDecleration(text);
        }
    }
}
