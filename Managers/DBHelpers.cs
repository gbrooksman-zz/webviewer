using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace webviewer.Managers
{
    public  class DBHelpers
    {
  
        public DBHelpers(){}

        #region Get helpers

		public Guid GetDBGuid(object obj)
		{
			if (!(obj is DBNull) && obj != null)
			{
				return new Guid(System.Convert.ToString(obj));
			}
			else
			{
				return Guid.Empty;
			}
		}

		public string GetDBString(object obj, string defaultValue)
		{
			string strValue = string.Empty;
			if (!(obj is DBNull) && obj != null)
			{
				strValue = System.Convert.ToString(obj);
			}
			if (strValue == string.Empty)
			{
				return defaultValue;
			}
			else
			{
				return strValue;
			}
		}

		public string GetDBString(object obj)
		{
			if (!(obj is DBNull) && obj != null)
			{
				return System.Convert.ToString(obj);
			}
			else
			{
				return string.Empty;
			}
		}

		public char GetDBChar(object obj, char defaultValue)
		{
			char chValue = '\0';
			if (!(obj is DBNull) && obj != null)
			{
				try
				{
					chValue = System.Convert.ToChar(obj);
				}
				catch
				{
					return defaultValue;
				}
			}
			if (chValue == '\0')
			{
				return defaultValue;
			}
			else
			{
				return chValue;
			}
		}

		public string GetDBNVarChar2String(object obj, string languageId, string defaultValue)
		{
			string strValue = string.Empty;
			if (!(obj is DBNull) && obj != null)
			{
				strValue = System.Convert.ToString(obj);
			}
			if (strValue == string.Empty)
			{
				return defaultValue;
			}
			else
			{
				return strValue;
			}
		}

		public string GetDBNVarChar2String(object obj, string languageId)
		{
			string strValue = string.Empty;
			if (!(obj is DBNull) && obj != null)
			{
				strValue = System.Convert.ToString(obj);
				return strValue;
			}
			else
			{
				return string.Empty;
			}
		}

		public DateTime GetDBDate(object obj)
		{
			DateTime dtmResult = new DateTime(0);
			if (!(obj is DBNull))
			{
				dtmResult = System.Convert.ToDateTime(obj);
			}
			return dtmResult;
		}

		public int GetDBInt(object value, int defaultValue)
		{
			if ((value is DBNull))
			{
				return defaultValue;
			}

			int intRetValue = 0;
			try
			{
				intRetValue = System.Convert.ToInt32(value);
			}
			catch
			{
				intRetValue = defaultValue;
			}

			return intRetValue;
		}

		public int GetDBInt(object value)
		{
			if (value == DBNull.Value)
				return 0;

			int intRetValue = 0;

			try
			{
				intRetValue = System.Convert.ToInt32(value);
			}
			catch
			{
			}

			return intRetValue;
		}

		public long GetDBLong(object value)
		{
			long longRetValue = 0;
			long.TryParse(GetDBString(value), out longRetValue);

			return longRetValue;
		}

		public byte GetDBByte(object value, byte defaultValue)
		{
			if ((value is DBNull))
			{
				return defaultValue;
			}
			return System.Convert.ToByte(value);
		}

		public byte[] GetDBBlob(object value)
		{
			if ((value is DBNull))
			{
				return null;
			}
			return (byte[])value;
		}

		public short GetDBInt16(object value, short defaultValue)
		{
			if ((value is DBNull))
			{
				return defaultValue;
			}
			return System.Convert.ToInt16(value);
		}

		public short GetDBInt16(object value)
		{
			if ((value is DBNull))
			{
				return 0;
			}
			return System.Convert.ToInt16(value);
		}

		public float GetDBFloat(object value, float defaultValue)
		{
			if ((value is DBNull))
			{
				return defaultValue;
			}

			float sglRetVal = 0;
			try
			{
				sglRetVal = System.Convert.ToSingle(value);
			}
			catch
			{
			}
			return (float)sglRetVal;
		}

		public float GetDBFloat(object value)
		{
			if ((value is DBNull))
			{
				return 0;
			}

			if (string.Empty == value.ToString())
			{
				return 0;  
			}

			float sglRetVal = 0;
			try
			{
				sglRetVal = System.Convert.ToSingle(value);
			}
			catch
			{
			}

			return sglRetVal;
		}

		public decimal GetDBDecimal(object value, decimal defaultValue = 0)
		{
			if ((value is DBNull))
			{
				return defaultValue;
			}

			decimal sglRetVal = 0;
			try
			{
				sglRetVal = System.Convert.ToDecimal(value);
			}
			catch
			{
			}
			return (decimal)sglRetVal;
		}

		public bool GetDBBool(object value, bool defaultValue)
		{
			if (value is DBNull)
			{
				return defaultValue;
			}
			else
			{
				if (value is bool)
				{
					return (bool)value;
				}
				else
				{
					return (System.Convert.ToInt16(value)) != 0;
				}
			}
		}

		public bool GetDBBool(object value)
		{
			try
			{
				if (value is DBNull)
				{
					return false;
				}
				else
				{
					if (value is bool)
					{
						return (bool)value;
					}
					else
					{
						return (System.Convert.ToInt16(value)) != 0;
					}
				}
			}
			catch (Exception)
			{
				try
				{
					if (value != null)
					{
						if (value.ToString().Length > 0)
						{
							bool blnResult = false;
							bool tryRet = bool.TryParse(value.ToString(), out blnResult);

							if (tryRet)
								return blnResult;
							else
								return false;
						}
					}
					return false;
				}
				catch (Exception) { }
			}
			return false;
		}

		public short ToDBBit(bool value)
		{
			if (value == true)
				return System.Convert.ToInt16(1);
			else
				return System.Convert.ToInt16(0);
		}

		public string GetDBShortDateString(System.DateTime date)
		{
			DateTime dt = DateTime.MinValue;
			if (date == dt)	return null;
			string strDate = date.ToString("MM/dd/yyyy");
			return strDate;
		}

        #endregion
    }

}