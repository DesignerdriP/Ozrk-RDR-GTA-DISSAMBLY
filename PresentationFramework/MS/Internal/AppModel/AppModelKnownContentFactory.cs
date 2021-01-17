using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A1 RID: 1953
	internal static class AppModelKnownContentFactory
	{
		// Token: 0x06007A63 RID: 31331 RVA: 0x0022AD08 File Offset: 0x00228F08
		internal static object BamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (!BaseUriHelper.IsPackApplicationUri(baseUri))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			Uri partUri = PackUriHelper.GetPartUri(baseUri);
			string partName;
			string text;
			string text2;
			string text3;
			BaseUriHelper.GetAssemblyNameAndPart(partUri, out partName, out text, out text2, out text3);
			if (ContentFileHelper.IsContentFile(partName))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			return Application.LoadBamlStreamWithSyncInfo(stream, new ParserContext
			{
				BaseUri = baseUri,
				SkipJournaledProperties = isJournalNavigation
			});
		}

		// Token: 0x06007A64 RID: 31332 RVA: 0x0022AD80 File Offset: 0x00228F80
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static object XamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (sandboxExternalContent)
			{
				if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
				{
					baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
				}
				stream.Close();
				return new WebBrowser
				{
					Source = baseUri
				};
			}
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = baseUri;
			parserContext.SkipJournaledProperties = isJournalNavigation;
			if (allowAsync)
			{
				XamlReader xamlReader = new XamlReader();
				asyncObjectConverter = xamlReader;
				xamlReader.LoadCompleted += AppModelKnownContentFactory.OnParserComplete;
				return xamlReader.LoadAsync(stream, parserContext);
			}
			return XamlReader.Load(stream, parserContext);
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x0022AE0D File Offset: 0x0022900D
		private static void OnParserComplete(object sender, AsyncCompletedEventArgs args)
		{
			if (!args.Cancelled && args.Error != null)
			{
				throw args.Error;
			}
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x0022AE28 File Offset: 0x00229028
		internal static object HtmlXappConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (canUseTopLevelBrowser)
			{
				return null;
			}
			if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
			{
				baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
			}
			stream.Close();
			return new WebBrowser
			{
				Source = baseUri
			};
		}
	}
}
