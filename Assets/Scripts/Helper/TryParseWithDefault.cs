using System;

// Thank you, MYNAMEISCOFFEY
// http://mynameiscoffey.com/2011/04/02/tryparse-with-default-values/

public static class TryParseWithDefault {
	public static Int16 ToInt16(string valueToParse, Int16 defaultValue) {
		Int16 returnValue;
		if (!Int16.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Int32 ToInt32(string valueToParse, Int32 defaultValue) {
		Int32 returnValue;
		if (!Int32.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Int64 ToInt64(string valueToParse, Int64 defaultValue) {
		Int64 returnValue;
		if (!Int64.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static UInt16 ToUInt16(string valueToParse, UInt16 defaultValue) {
		UInt16 returnValue;
		if (!UInt16.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static UInt32 ToUInt32(string valueToParse, UInt32 defaultValue) {
		UInt32 returnValue;
		if (!UInt32.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static UInt64 ToUInt64(string valueToParse, UInt64 defaultValue) {
		UInt64 returnValue;
		if (!UInt64.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Single ToSingle(string valueToParse, Single defaultValue) {
		Single returnValue;
		if (!Single.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Double ToDouble(string valueToParse, Double defaultValue) {
		Double returnValue;
		if (!Double.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Decimal ToDecimal(string valueToParse, Decimal defaultValue) {
		Decimal returnValue;
		if (!Decimal.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static Byte ToByte(string valueToParse, Byte defaultValue) {
		Byte returnValue;
		if (!Byte.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
	
	public static SByte ToSByte(string valueToParse, SByte defaultValue) {
		SByte returnValue;
		if (!SByte.TryParse(valueToParse, out returnValue))
			returnValue = defaultValue;
		return returnValue;
	}
}