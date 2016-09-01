// FontFamily.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// A font family.
    /// </summary>
    public struct FontFamily : IDataType, IFormattable
    {
        private string _family;

        /// <summary />
        public FontFamily(string family)
        {
            _family = family;
        }

#pragma warning disable 1591
        public static FontFamily Arial = new FontFamily("Arial");
        public static FontFamily ComicSans = new FontFamily("Comic Sans");
        public static FontFamily TimesNewRoman = new FontFamily("Times New Roman");
        public static FontFamily Helvetica = new FontFamily("Helvetica");
        public static FontFamily SourceSans = new FontFamily("Source Sans");
        public static FontFamily Roboto = new FontFamily("Roboto");
#pragma warning restore 1591

        /// <summary />
        public override string ToString()
        {
            return _family ?? Arial;
        }

        /// <summary />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        /// <summary />
        public static implicit operator FontFamily(string family)
        {
            return new FontFamily(family);
        }

        /// <summary />
        public static implicit operator string(FontFamily family)
        {
            return family.ToString();
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            writer.Write(ToString());
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            _family = reader.ReadString();
        }
    }
}