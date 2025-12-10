/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using System;
using System.Collections.Generic;
using System.IO;

namespace OXPd2ExamplesHost.Utilities
{
    public static class MultipartRequestHelper
    {
        public static bool IsMultipartContentType(string contentType)
        {
            if (false == string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string GetDirectiveValue(string segment, string directive)
        {
            string directiveValue = null;

            directiveValue = segment.Substring(directive.Length).Trim('"');

            return directiveValue;
        }
        public static string GetBoundary(string contentType, int lengthLimit)
        {
            // Content-Type: multipart/mixed; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
            string boundary = null;

            List<string> parts = new List<string>(contentType.Split(' ', StringSplitOptions.TrimEntries));

            var boundaryPart = parts.Find(o => o.StartsWith("boundary=", StringComparison.OrdinalIgnoreCase));
            if (null != boundaryPart)
            {
                boundary = GetDirectiveValue(boundaryPart, "boundary=");
            }

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }
    }
}
