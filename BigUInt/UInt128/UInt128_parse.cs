﻿using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BigUInt {
    public readonly partial struct UInt128 {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Regex parse_regex = new(@"^\d+$");

        public UInt128(string s) : this() {
            if (!parse_regex.IsMatch(s)) {
                throw new FormatException();
            }

            s = s.TrimStart('0');

            if (s == string.Empty) {
                return;
            }

            if (s.Length > MaxValueDigits) {
                throw new OverflowException();
            }

            const int decimals = UIntUtil.UInt32MaxDecimalDigits;

            s = new string('0', decimals * 5 - s.Length) + s;

            UInt32 dec4 = UInt32.Parse(s[..decimals], NumberStyles.Integer, CultureInfo.InvariantCulture);
            UInt32 dec3 = UInt32.Parse(s[decimals..(decimals * 2)], NumberStyles.Integer, CultureInfo.InvariantCulture);
            UInt32 dec2 = UInt32.Parse(s[(decimals * 2)..(decimals * 3)], NumberStyles.Integer, CultureInfo.InvariantCulture);
            UInt32 dec1 = UInt32.Parse(s[(decimals * 3)..(decimals * 4)], NumberStyles.Integer, CultureInfo.InvariantCulture);
            UInt32 dec0 = UInt32.Parse(s[(decimals * 4)..], NumberStyles.Integer, CultureInfo.InvariantCulture);

            UInt32 carry, bin0, bin1, bin2, bin3, bin4;

            (bin1, bin0) = UIntUtil.Unpack(UIntUtil.DecimalPack(dec4, dec3));

            (carry, bin0) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin0, dec2));
            (bin2, bin1) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin1, carry));

            (carry, bin0) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin0, dec1));
            (carry, bin1) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin1, carry));
            (bin3, bin2) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin2, carry));

            (carry, bin0) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin0, dec0));
            (carry, bin1) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin1, carry));
            (carry, bin2) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin2, carry));
            (bin4, bin3) = UIntUtil.Unpack(UIntUtil.DecimalPack(bin3, carry));

            if (bin4 > 0) {
                throw new OverflowException();
            }

            this.e3 = bin3;
            this.e2 = bin2;
            this.e1 = bin1;
            this.e0 = bin0;
        }

        public static UInt128 Parse(string s) {
            return new UInt128(s);
        }

        public static bool TryParse(string s, out UInt128 result) {
            try {
                result = Parse(s);
                return true;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException) {
                result = Zero;
                return false;
            }
        }
    }
}