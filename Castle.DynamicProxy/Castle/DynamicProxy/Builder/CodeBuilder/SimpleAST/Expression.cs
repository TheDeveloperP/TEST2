﻿namespace Castle.DynamicProxy.Builder.CodeBuilder.SimpleAST
{
    using Castle.DynamicProxy.Builder.CodeBuilder;
    using System;
    using System.Reflection.Emit;

    [CLSCompliant(false)]
    public abstract class Expression : IEmitter
    {
        public abstract void Emit(IEasyMember member, ILGenerator gen);
    }
}

