// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;

namespace DanmakU {

	[System.Serializable]
	public abstract class DanmakuModifier : IEnumerable<DanmakuModifier> {

		[SerializeField]
		private DanmakuModifier subModifier;
		private FireData data;

		protected DynamicFloat Speed {
			get {
				return data.Speed;
			}
			set {
				data.Speed = value;
				if(subModifier != null)
					subModifier.Speed = value;
			}
		}

		protected DynamicFloat AngularSpeed {
			get {
				return data.AngularSpeed;
			}
			set {
				data.AngularSpeed = value;
				if(subModifier != null)
					subModifier.AngularSpeed = value;
			}
		}

		protected DanmakuField TargetField {
			get {
				return data.Field;
			}
			set {
				data.Field = value;
				if (subModifier != null)
					subModifier.TargetField = value;
			}
		}

		protected DanmakuController Controller {
			get {
				return data.Controller;
			}
			set {
				data.Controller = value;
				if (subModifier != null)
					subModifier.Controller = value;
			}
		}

		protected DanmakuPrefab BulletType {
			get {
				return data.Prefab;
			}
			set {
				data.Prefab = value;
				if (subModifier != null)
					subModifier.BulletType = value;
			}
		}

		protected DanmakuGroup Group {
			get {
				return data.Group;
			}
			set {
				data.Group = value;
				if (subModifier != null)
					subModifier.Group = value;
			}
		}

		public DanmakuModifier SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier == null)
					subModifier.Initialize(data);
			}
		}

		internal void Initialize(FireData data) {
			this.data = data;
			if (subModifier != null)
				subModifier.Initialize (data);
			OnInitialize ();
		}

		protected virtual void OnInitialize() {
		}

		public void Append(DanmakuModifier newModifier) {
			DanmakuModifier parent = this;
			DanmakuModifier current = subModifier;
			while (current != null) {
				current = current.subModifier;
			}
			parent.SubModifier = newModifier;
		}

		protected void FireSingle(Vector2 position,
		                          DynamicFloat rotation) {
			if (SubModifier == null) {
				data.Position = position;
				data.Rotation = rotation;
				data.Fire();
			} else {
				SubModifier.Fire (position, rotation);
			}
		}

		public abstract void Fire(Vector2 position, DynamicFloat rotation);

		#region IEnumerable implementation

		public IEnumerator<DanmakuModifier> GetEnumerator () {
			DanmakuModifier current = this;
			while (current != null) {
				yield return current;
				current = current.subModifier;
			}
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			DanmakuModifier current = this;
			while (current != null) {
				yield return current;
				current = current.subModifier;
			}
		}

		#endregion
	}

}