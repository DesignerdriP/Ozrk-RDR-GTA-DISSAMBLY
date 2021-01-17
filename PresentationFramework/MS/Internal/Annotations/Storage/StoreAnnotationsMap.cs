using System;
using System.Collections.Generic;
using System.Windows.Annotations;

namespace MS.Internal.Annotations.Storage
{
	// Token: 0x020007D0 RID: 2000
	internal class StoreAnnotationsMap
	{
		// Token: 0x06007BBA RID: 31674 RVA: 0x0022C0E2 File Offset: 0x0022A2E2
		internal StoreAnnotationsMap(AnnotationAuthorChangedEventHandler authorChanged, AnnotationResourceChangedEventHandler anchorChanged, AnnotationResourceChangedEventHandler cargoChanged)
		{
			this._authorChanged = authorChanged;
			this._anchorChanged = anchorChanged;
			this._cargoChanged = cargoChanged;
		}

		// Token: 0x06007BBB RID: 31675 RVA: 0x0022C10C File Offset: 0x0022A30C
		public void AddAnnotation(Annotation annotation, bool dirty)
		{
			annotation.AuthorChanged += this.OnAuthorChanged;
			annotation.AnchorChanged += this.OnAnchorChanged;
			annotation.CargoChanged += this.OnCargoChanged;
			this._currentAnnotations.Add(annotation.Id, new StoreAnnotationsMap.CachedAnnotation(annotation, dirty));
		}

		// Token: 0x06007BBC RID: 31676 RVA: 0x0022C168 File Offset: 0x0022A368
		public void RemoveAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				cachedAnnotation.Annotation.AuthorChanged -= this.OnAuthorChanged;
				cachedAnnotation.Annotation.AnchorChanged -= this.OnAnchorChanged;
				cachedAnnotation.Annotation.CargoChanged -= this.OnCargoChanged;
				this._currentAnnotations.Remove(id);
			}
		}

		// Token: 0x06007BBD RID: 31677 RVA: 0x0022C1DC File Offset: 0x0022A3DC
		public Dictionary<Guid, Annotation> FindAnnotations(ContentLocator anchorLocator)
		{
			if (anchorLocator == null)
			{
				throw new ArgumentNullException("locator");
			}
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (StoreAnnotationsMap.CachedAnnotation cachedAnnotation in this._currentAnnotations.Values)
			{
				Annotation annotation = cachedAnnotation.Annotation;
				bool flag = false;
				foreach (AnnotationResource annotationResource in annotation.Anchors)
				{
					foreach (ContentLocatorBase contentLocatorBase in annotationResource.ContentLocators)
					{
						ContentLocator contentLocator = contentLocatorBase as ContentLocator;
						if (contentLocator != null)
						{
							if (contentLocator.StartsWith(anchorLocator))
							{
								flag = true;
							}
						}
						else
						{
							ContentLocatorGroup contentLocatorGroup = contentLocatorBase as ContentLocatorGroup;
							if (contentLocatorGroup != null)
							{
								foreach (ContentLocator contentLocator2 in contentLocatorGroup.Locators)
								{
									if (contentLocator2.StartsWith(anchorLocator))
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (flag)
						{
							dictionary.Add(annotation.Id, annotation);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x0022C340 File Offset: 0x0022A540
		public Dictionary<Guid, Annotation> FindAnnotations()
		{
			Dictionary<Guid, Annotation> dictionary = new Dictionary<Guid, Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.Annotation);
			}
			return dictionary;
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x0022C3AC File Offset: 0x0022A5AC
		public Annotation FindAnnotation(Guid id)
		{
			StoreAnnotationsMap.CachedAnnotation cachedAnnotation = null;
			if (this._currentAnnotations.TryGetValue(id, out cachedAnnotation))
			{
				return cachedAnnotation.Annotation;
			}
			return null;
		}

		// Token: 0x06007BC0 RID: 31680 RVA: 0x0022C3D4 File Offset: 0x0022A5D4
		public List<Annotation> FindDirtyAnnotations()
		{
			List<Annotation> list = new List<Annotation>();
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					list.Add(keyValuePair.Value.Annotation);
				}
			}
			return list;
		}

		// Token: 0x06007BC1 RID: 31681 RVA: 0x0022C448 File Offset: 0x0022A648
		public void ValidateDirtyAnnotations()
		{
			foreach (KeyValuePair<Guid, StoreAnnotationsMap.CachedAnnotation> keyValuePair in this._currentAnnotations)
			{
				if (keyValuePair.Value.Dirty)
				{
					keyValuePair.Value.Dirty = false;
				}
			}
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x0022C4B0 File Offset: 0x0022A6B0
		private void OnAnchorChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._anchorChanged(sender, args);
		}

		// Token: 0x06007BC3 RID: 31683 RVA: 0x0022C4DB File Offset: 0x0022A6DB
		private void OnCargoChanged(object sender, AnnotationResourceChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._cargoChanged(sender, args);
		}

		// Token: 0x06007BC4 RID: 31684 RVA: 0x0022C506 File Offset: 0x0022A706
		private void OnAuthorChanged(object sender, AnnotationAuthorChangedEventArgs args)
		{
			this._currentAnnotations[args.Annotation.Id].Dirty = true;
			this._authorChanged(sender, args);
		}

		// Token: 0x04003A34 RID: 14900
		private Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation> _currentAnnotations = new Dictionary<Guid, StoreAnnotationsMap.CachedAnnotation>();

		// Token: 0x04003A35 RID: 14901
		private AnnotationAuthorChangedEventHandler _authorChanged;

		// Token: 0x04003A36 RID: 14902
		private AnnotationResourceChangedEventHandler _anchorChanged;

		// Token: 0x04003A37 RID: 14903
		private AnnotationResourceChangedEventHandler _cargoChanged;

		// Token: 0x02000B7C RID: 2940
		private class CachedAnnotation
		{
			// Token: 0x06008E33 RID: 36403 RVA: 0x0025B5F8 File Offset: 0x002597F8
			public CachedAnnotation(Annotation annotation, bool dirty)
			{
				this.Annotation = annotation;
				this.Dirty = dirty;
			}

			// Token: 0x17001F9F RID: 8095
			// (get) Token: 0x06008E34 RID: 36404 RVA: 0x0025B60E File Offset: 0x0025980E
			// (set) Token: 0x06008E35 RID: 36405 RVA: 0x0025B616 File Offset: 0x00259816
			public Annotation Annotation
			{
				get
				{
					return this._annotation;
				}
				set
				{
					this._annotation = value;
				}
			}

			// Token: 0x17001FA0 RID: 8096
			// (get) Token: 0x06008E36 RID: 36406 RVA: 0x0025B61F File Offset: 0x0025981F
			// (set) Token: 0x06008E37 RID: 36407 RVA: 0x0025B627 File Offset: 0x00259827
			public bool Dirty
			{
				get
				{
					return this._dirty;
				}
				set
				{
					this._dirty = value;
				}
			}

			// Token: 0x04004B74 RID: 19316
			private Annotation _annotation;

			// Token: 0x04004B75 RID: 19317
			private bool _dirty;
		}
	}
}
