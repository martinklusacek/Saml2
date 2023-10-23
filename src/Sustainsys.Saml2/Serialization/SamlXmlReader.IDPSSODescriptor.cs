﻿using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Xml;
using static Sustainsys.Saml2.Constants;

namespace Sustainsys.Saml2.Serialization;

partial class SamlXmlReader
{
    /// <summary>
    /// Create an IDPSSODescriptor instance.
    /// </summary>
    protected virtual IDPSSODescriptor CreateIDPSSODescriptor() => new();

    /// <summary>
    /// Read the current node as an IDPSSODescriptor
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    protected virtual IDPSSODescriptor ReadIDPSSODescriptor(XmlTraverser source)
    {
        var result = CreateIDPSSODescriptor();

        ReadAttributes(source, result);

        ReadElements(source.GetChildren(), result);

        return result;
    }

    /// <summary>
    /// Read attributes of IDPSSODescriptor.
    /// </summary>
    /// <param name="source">Source</param>
    /// <param name="result">Result</param>
    protected virtual void ReadAttributes(XmlTraverser source, IDPSSODescriptor result)
    {
        result.WantAuthnRequestsSigned = source.GetBoolAttribute(AttributeNames.WantAuthnRequestsSigned) ?? false;

        ReadAttributes(source, (SSODescriptor)result);
    }

    /// <summary>
    /// Read child elements of IDPSSODescriptor
    /// </summary>
    /// <param name="source"></param>
    /// <param name="result"></param>
    protected virtual void ReadElements(XmlTraverser source, IDPSSODescriptor result)
    {
        ReadElements(source, (SSODescriptor)result);

        // We must have at least one SingleSignOnService in an IDPSSODescriptor and now we should be at it.
        if(!source.EnsureElement() &&             
            !source.EnsureName(Namespaces.MetadataUri, Elements.SingleSignOnService))
        {
            return;
        }

        do
        {
            result.SingleSignOnServices.Add(ReadEndpoint(source));
        } while (source.MoveNext(true) && source.HasName(Namespaces.MetadataUri, Elements.SingleSignOnService));

        source.Skip();
    }
}