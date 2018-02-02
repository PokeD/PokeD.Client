using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Components.Input
{
    public abstract class InputListenerComponent : Component
    {
        public abstract class BaseEventHandler<TEventArgs> : IDisposable where TEventArgs : EventArgs
        {
            // I just wanted to check if this will works. It worked. I lol'd, I kept it.

            /// <summary>
            /// Copies behaviour of <see cref="BaseEventHandler{T1}.Subscribe(ValueTuple{Component, EventHandler{TEventArgs}})"/>
            /// </summary>
            public static BaseEventHandler<TEventArgs> operator +(BaseEventHandler<TEventArgs> eventHandler, (GameComponent, EventHandler<TEventArgs>) tuple) => eventHandler.Subscribe(tuple);
            /// <summary>
            /// Copies behaviour of <see cref="BaseEventHandler{T1}.Subscribe(EventHandler{TEventArgs})"/>
            /// </summary>
            public static BaseEventHandler<TEventArgs> operator +(BaseEventHandler<TEventArgs> eventHandler, EventHandler<TEventArgs> @delegate) => eventHandler.Subscribe(@delegate);
            /// <summary>
            /// Copies behaviour of <see cref="BaseEventHandler{T1}.Unsubscribe"/>
            /// </summary>
            public static BaseEventHandler<TEventArgs> operator -(BaseEventHandler<TEventArgs> eventHandler, EventHandler<TEventArgs> @delegate) => eventHandler.Unsubscribe(@delegate);

            public abstract BaseEventHandler<TEventArgs> Subscribe(GameComponent component, EventHandler<TEventArgs> @delegate);
            public abstract BaseEventHandler<TEventArgs> Subscribe((GameComponent Component, EventHandler<TEventArgs> Delegate) tuple);
            public abstract BaseEventHandler<TEventArgs> Subscribe(EventHandler<TEventArgs> @delegate);
            public abstract BaseEventHandler<TEventArgs> Unsubscribe(EventHandler<TEventArgs> action);

            protected BaseEventHandler()
            {
                if (!IsSubclassOf(GetType().GetGenericTypeDefinition(), typeof(BaseEventHandlerWithInvoke<>)))
                    throw new InvalidCastException($"Do not create custom implementations of {nameof(BaseEventHandler<TEventArgs>)}");
            }
            // TODO: Optimize
            private static bool IsSubclassOf(Type type, Type baseType)
            {
                if (type == null || baseType == null || type == baseType)
                    return false;

                var typeTypeInfo = type.GetTypeInfo();
                var baseTypeTypeInfo = baseType.GetTypeInfo();

                if (!baseTypeTypeInfo.IsGenericType)
                {
                    if (!typeTypeInfo.IsGenericType)
                        return typeTypeInfo.IsSubclassOf(baseType);
                }
                else
                {
                    baseType = baseType.GetGenericTypeDefinition();
                    baseTypeTypeInfo = baseType.GetTypeInfo();
                }

                type = typeTypeInfo.BaseType;
                typeTypeInfo = type.GetTypeInfo();

                var objectType = typeof(object);
                while (type != objectType && type != null)
                {
                    var curentType = typeTypeInfo.IsGenericType ? type.GetGenericTypeDefinition() : type;
                    if (curentType == baseType)
                        return true;

                    type = typeTypeInfo.BaseType;
                    typeTypeInfo = type.GetTypeInfo();
                }

                return false;
            }


            public abstract void Dispose();
        }
        protected abstract class BaseEventHandlerWithInvoke<TEventArgs> : BaseEventHandler<TEventArgs> where TEventArgs : EventArgs
        {
            protected internal abstract void Invoke(object sender, TEventArgs e);
        }

        protected sealed class CustomEventHandler<TEventArgs> : BaseEventHandlerWithInvoke<TEventArgs> where TEventArgs : EventArgs
        {
            // I would have used ValueTuple, but the comparison should be done only using EventHandler<TEventArgs> Action.
            private class Storage : IEqualityComparer<Storage>
            {
                public readonly GameComponent Component;
                public readonly EventHandler<TEventArgs> Delegate;

                public Storage(GameComponent component, EventHandler<TEventArgs> @delegate) { Component = component; Delegate = @delegate; }
                public Storage((GameComponent Component, EventHandler<TEventArgs> Delegate) tuple) { Component = tuple.Component; Delegate = tuple.Delegate; }


                public bool Equals(Storage x, Storage y) => ((Delegate) x.Delegate).Equals((Delegate) y.Delegate);
                public int GetHashCode(Storage obj) => obj.Component.GetHashCode() ^ ((Delegate) obj.Delegate).GetHashCode();

                public override bool Equals(object obj)
                {
                    var storage = obj as Storage;
                    return !ReferenceEquals(storage, null) && Equals(this, storage);
                }
                public override int GetHashCode() => GetHashCode(this);
            }

            private List<Storage> Subscribers { get; } = new List<Storage>();

            public override BaseEventHandler<TEventArgs> Subscribe(GameComponent component, EventHandler<TEventArgs> @delegate) { lock (Subscribers) { Subscribers.Add(new Storage(component, @delegate)); return this; } }
            public override BaseEventHandler<TEventArgs> Subscribe((GameComponent Component, EventHandler<TEventArgs> Delegate) tuple) { lock (Subscribers) { Subscribers.Add(new Storage(tuple)); return this; } }
            public override BaseEventHandler<TEventArgs> Subscribe(EventHandler<TEventArgs> @delegate) { lock (Subscribers) { Subscribers.Add(new Storage(null, @delegate)); return this; } }
            public override BaseEventHandler<TEventArgs> Unsubscribe(EventHandler<TEventArgs> @delegate) { lock (Subscribers) { Subscribers.Remove(new Storage(null, @delegate)); return this; } }

            public override void Dispose()
            {
                if (Subscribers.Any())
                {
#if DEBUG
                    throw new Exception("Leaked events!");
#endif
                }
            }

            protected internal override void Invoke(object sender, TEventArgs e)
            {
                lock (Subscribers)
                {
                    var tempList = Subscribers.ToList();
                    foreach (var subscriber in tempList)
                    {
                        if (subscriber != null)
                        {
                            if (subscriber.Component != null)
                            {
                                if (subscriber.Component.Enabled)
                                    subscriber?.Delegate?.Invoke(sender, e);
                            }
                            else
                                subscriber.Delegate?.Invoke(sender, e);
                        }
                    }
                }
            }
        }
        protected sealed class CustomEventHandlerOld<TEventArgs> : BaseEventHandlerWithInvoke<TEventArgs> where TEventArgs : EventArgs
        {
            private event EventHandler<TEventArgs> EventHandler; 

            public override BaseEventHandler<TEventArgs> Subscribe(GameComponent component, EventHandler<TEventArgs> @delegate) { EventHandler += @delegate; return this; }
            public override BaseEventHandler<TEventArgs> Subscribe((GameComponent Component, EventHandler<TEventArgs> Delegate) tuple) { EventHandler += tuple.Delegate; return this; }
            public override BaseEventHandler<TEventArgs> Subscribe(EventHandler<TEventArgs> @delegate) { EventHandler += @delegate; return this; }
            public override BaseEventHandler<TEventArgs> Unsubscribe(EventHandler<TEventArgs> @delegate) { EventHandler -= @delegate; return this; }

            protected internal override void Invoke(object sender, TEventArgs e) { EventHandler?.Invoke(sender, e); }

            public override void Dispose()
            {
                if (EventHandler.GetInvocationList().Any())
                {
#if DEBUG
                    throw new Exception("Leaked events!");
#endif
                }
            }
        }


        //public ViewportAdapter ViewportAdapter { get; set; }

        protected InputListenerComponent(PortableGame game) : base(game) { }


        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose every event to check if somthing is leaking.

                    var eventFields = GetType().GetTypeInfo().DeclaredFields
                        .Where(fieldInfo =>
                        {
                            var typeInfo = fieldInfo.FieldType.GetTypeInfo();
                            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(BaseEventHandler<>);
                        });
                    foreach (var fieldInfo in eventFields)
                        (fieldInfo.GetValue(this) as IDisposable)?.Dispose();

                    var eventProperties = GetType().GetTypeInfo().DeclaredProperties
                        .Where(propertyInfo =>
                        {
                            var typeInfo = propertyInfo.PropertyType.GetTypeInfo();
                            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(BaseEventHandler<>);
                        });
                    foreach (var propertyInfo in eventProperties)
                        (propertyInfo.GetValue(this) as IDisposable)?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}