namespace MXJ.Core.ValueInjector
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Collections;
    internal delegate void InjectMethod<TTarget, TSource>(TTarget target, TSource source);
    internal class InjectMethodFactory
    {
        private static MethodInfo _gim = null;
        private static Hashtable _injectors = Hashtable.Synchronized(new Hashtable());
        static InjectMethodFactory()
        {
            _gim = typeof(InjectMethodFactory).GetMethod("GetInjectMethod");
        }
        public static InjectMethod<TTarget, TSource> GetInjectMethod<TTarget, TSource>(InjectType injectType)
        {
            Type typeTarget = typeof(TTarget);
            Type typeSource = typeof(TSource);
            string key = typeTarget.Name + typeSource.Name;
            InjectMethod<TTarget, TSource> injector = _injectors[key] as InjectMethod<TTarget, TSource>;
            if (injector != null)
            {
                return injector;
            }
            DynamicMethod method = new DynamicMethod(key, null, new Type[] { typeTarget, typeSource }, true);
            var il = method.GetILGenerator();
            var tFields = typeTarget.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var sFields = typeSource.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var tField in tFields)
            {
                var sField = sFields.Where(f => f.Name == tField.Name).FirstOrDefault();
                if (sField == null)
                {
                    continue;
                }
                if (tField.FieldType.IsPrimitive || tField.FieldType.IsValueType || tField.FieldType == typeof(string))
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldfld, sField);
                    il.Emit(OpCodes.Stfld, tField);
                }
            }
            il.Emit(OpCodes.Ret);
            injector = (InjectMethod<TTarget, TSource>)method.CreateDelegate(typeof(InjectMethod<TTarget, TSource>));
            _injectors[key] = injector;
            return injector;
        }
    }
    internal enum InjectType
    {
        ShadowCopy
    }
    public static class Injector
    {
        public static TTarget Inject<TTarget, TSource>(this TTarget target, TSource source)
        {
            var method = InjectMethodFactory.GetInjectMethod<TTarget, TSource>(InjectType.ShadowCopy);
            method.Invoke(target, source);
            return target;
        }
    }
}
