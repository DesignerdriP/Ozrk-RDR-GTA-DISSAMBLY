using System;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using MS.Internal.AppModel;

namespace MS.Internal.Utility
{
	// Token: 0x020007EC RID: 2028
	internal static class BindUriHelper
	{
		// Token: 0x06007D26 RID: 32038 RVA: 0x00232D8D File Offset: 0x00230F8D
		internal static string UriToString(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return new StringBuilder(uri.GetComponents(uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString, UriFormat.SafeUnescaped), 2083).ToString();
		}

		// Token: 0x17001D17 RID: 7447
		// (get) Token: 0x06007D27 RID: 32039 RVA: 0x00232DCA File Offset: 0x00230FCA
		// (set) Token: 0x06007D28 RID: 32040 RVA: 0x00232DD1 File Offset: 0x00230FD1
		internal static Uri BaseUri
		{
			get
			{
				return BaseUriHelper.BaseUri;
			}
			[SecurityCritical]
			set
			{
				BaseUriHelper.BaseUri = BaseUriHelper.FixFileUri(value);
			}
		}

		// Token: 0x06007D29 RID: 32041 RVA: 0x00232DDE File Offset: 0x00230FDE
		internal static bool DoSchemeAndHostMatch(Uri first, Uri second)
		{
			return SecurityHelper.AreStringTypesEqual(first.Scheme, second.Scheme) && first.Host.Equals(second.Host);
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x00232E08 File Offset: 0x00231008
		internal static Uri GetResolvedUri(Uri baseUri, Uri orgUri)
		{
			Uri result;
			if (orgUri == null)
			{
				result = null;
			}
			else if (!orgUri.IsAbsoluteUri)
			{
				Uri baseUri2 = (baseUri == null) ? BindUriHelper.BaseUri : baseUri;
				result = new Uri(baseUri2, orgUri);
			}
			else
			{
				result = BaseUriHelper.FixFileUri(orgUri);
			}
			return result;
		}

		// Token: 0x06007D2B RID: 32043 RVA: 0x00232E50 File Offset: 0x00231050
		internal static string GetReferer(Uri destinationUri)
		{
			string result = null;
			Uri browserSource = SiteOfOriginContainer.BrowserSource;
			if (browserSource != null)
			{
				SecurityZone securityZone = CustomCredentialPolicy.MapUrlToZone(browserSource);
				SecurityZone securityZone2 = CustomCredentialPolicy.MapUrlToZone(destinationUri);
				if (securityZone == securityZone2 && SecurityHelper.AreStringTypesEqual(browserSource.Scheme, destinationUri.Scheme))
				{
					result = browserSource.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
				}
			}
			return result;
		}

		// Token: 0x06007D2C RID: 32044 RVA: 0x00232E9E File Offset: 0x0023109E
		internal static Uri GetResolvedUri(Uri originalUri)
		{
			return BindUriHelper.GetResolvedUri(null, originalUri);
		}

		// Token: 0x06007D2D RID: 32045 RVA: 0x00232EA8 File Offset: 0x002310A8
		internal static Uri GetUriToNavigate(DependencyObject element, Uri baseUri, Uri inputUri)
		{
			if (inputUri == null || inputUri.IsAbsoluteUri)
			{
				return inputUri;
			}
			if (BindUriHelper.StartWithFragment(inputUri))
			{
				baseUri = null;
			}
			Uri resolvedUri;
			if (baseUri != null)
			{
				if (!baseUri.IsAbsoluteUri)
				{
					resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, baseUri), inputUri);
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(baseUri, inputUri);
				}
			}
			else
			{
				Uri uri = null;
				if (element != null)
				{
					INavigator navigator = element as INavigator;
					if (navigator != null)
					{
						uri = navigator.CurrentSource;
					}
					else
					{
						NavigationService navigationService = element.GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
						uri = ((navigationService == null) ? null : navigationService.CurrentSource);
					}
				}
				if (uri != null)
				{
					if (uri.IsAbsoluteUri)
					{
						resolvedUri = BindUriHelper.GetResolvedUri(uri, inputUri);
					}
					else
					{
						resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, uri), inputUri);
					}
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(null, inputUri);
				}
			}
			return resolvedUri;
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x00232F6C File Offset: 0x0023116C
		internal static bool StartWithFragment(Uri uri)
		{
			return uri.OriginalString.StartsWith("#", StringComparison.Ordinal);
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x00232F80 File Offset: 0x00231180
		internal static string GetFragment(Uri uri)
		{
			Uri uri2 = uri;
			string result = string.Empty;
			if (!uri.IsAbsoluteUri)
			{
				uri2 = new Uri(BindUriHelper.placeboBase, uri);
			}
			string fragment = uri2.Fragment;
			if (fragment != null && fragment.Length > 0)
			{
				result = fragment.Substring(1);
			}
			return result;
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x00232FC8 File Offset: 0x002311C8
		internal static Uri GetUriRelativeToPackAppBase(Uri original)
		{
			if (original == null)
			{
				return null;
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(original);
			Uri packAppBaseUri = BaseUriHelper.PackAppBaseUri;
			return packAppBaseUri.MakeRelativeUri(resolvedUri);
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x00232FF6 File Offset: 0x002311F6
		internal static bool IsXamlMimeType(ContentType mimeType)
		{
			return MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentSequenceMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedPageMime.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x04003AE2 RID: 15074
		private const int MAX_PATH_LENGTH = 2048;

		// Token: 0x04003AE3 RID: 15075
		private const int MAX_SCHEME_LENGTH = 32;

		// Token: 0x04003AE4 RID: 15076
		public const int MAX_URL_LENGTH = 2083;

		// Token: 0x04003AE5 RID: 15077
		private const string PLACEBOURI = "http://microsoft.com/";

		// Token: 0x04003AE6 RID: 15078
		private static Uri placeboBase = new Uri("http://microsoft.com/");

		// Token: 0x04003AE7 RID: 15079
		private const string FRAGMENTMARKER = "#";
	}
}
