﻿using System;
using Newtonsoft.Json.Linq;
using Shintio.Json.Nodes;

namespace Shintio.Json.Newtonsoft.Nodes
{
	public abstract class NewtonsoftJsonNode<TNode> : NewtonsoftJsonNode, IJsonNode where TNode : JToken
	{
		public NewtonsoftJsonNode(TNode node)
		{
			Node = node;
		}

		public TNode Node { get; }

		// public abstract IJsonNode? this[string propertyName] { get; set; }

		public IJsonNode? this[string propertyName]
		{
			get => Create(Node[propertyName]);
			set => Node[propertyName] = (value as NewtonsoftJsonNode<TNode>)?.Node;
		}

		public object GetRealNode()
		{
			return Node;
		}

		public T ToObject<T>()
		{
			return Node.ToObject<T>();
		}

		public object? ToObject(Type type)
		{
			return Node.ToObject(type);
		}

		public override string ToString()
		{
			return Node.ToString();
		}
	}

	public abstract class NewtonsoftJsonNode
	{
		public static IJsonNode? Create(JToken? node) => node switch
		{
			JArray n => new NewtonsoftJsonArray(n),
			JObject n => new NewtonsoftJsonObject(n),
			JValue n => new NewtonsoftJsonValue(n),
			_ => null,
		};
	}
}