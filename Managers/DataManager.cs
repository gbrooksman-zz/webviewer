using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.SqlTypes;
using Serilog;


namespace webviewer.Managers
{

    public class DataManager
    {
        string connString = string.Empty;        

        public DataManager()
        {
           // connString = @"Server=10.34.4.146;Initial Catalog=QA_P;User Id=wercs;Password=wercs;";
			connString = @"Server=10.0.2.2;Initial Catalog=SVT_DEV;User Id=wercs;Password=wercs;";
        }   

        public List<Format> GetFormats()
        {
            List<Format> formats = new List<Format>();

             using (SqlConnection con = new SqlConnection(connString)) 
             {
                con.Open();
                try 
                {
                    using (SqlCommand command = new SqlCommand(@"SELECT F_FORMAT , F_FORMAT_DESC
                                                                 FROM T_FORMATS", con)) 
                    {
                        SqlDataReader reader  = command.ExecuteReader();

                        while (reader.Read())
                        {
                            formats.Add( new Format()
                            {
                                Code = reader[0].ToString(),
                                Name = reader[1].ToString()
                            });
                        }
                        reader.Close();
                    }
                }
                catch 
                {
                    Console.WriteLine("Something went wrong");
                }
             }           

            return formats;
        }

        public List<Subformat> GetSubFormats(string formatCode)
        {
            List<Subformat> subFormats = new List<Subformat>();

             using (SqlConnection con = new SqlConnection(connString)) 
             {
                con.Open();
                try 
                {
                    using (SqlCommand command = new SqlCommand(@"SELECT F_MSDSTYPE, F_MSDSTYPE_DESC
                                                                 FROM T_MSDSTYPES
																 WHERE F_FORMAT = @FMT ", con)) 
					{
						SqlParameter param = new SqlParameter("@FMT",SqlDbType.VarChar,3);
						param.Value = formatCode;
						command.Parameters.Add(param);

                        SqlDataReader reader  = command.ExecuteReader();

                        while (reader.Read())
                        {
                            subFormats.Add( new Subformat()
                            {
                                Code = reader[0].ToString(),
                                Name = reader[1].ToString()
                            });
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
					
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }
             }          

            return subFormats;
        }

        public Document GetDocument(Guid RecordGuid)
        {
            Document doc = new Document();

             using (SqlConnection con = new SqlConnection(connString)) 
             {
                con.Open();
                try 
                {
                    using (SqlCommand command = new SqlCommand(@"	SELECT *
                                                                	FROM T_PDF_MSDS
                                               						WHERE F_GUID = ? ", con)) 
                    {

						Log.Information(command.CommandText);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            doc = GetDocumentFromReader(reader);                           
                        }
                        reader.Close();
                    }
                }
                catch 
                {
                    Console.WriteLine("Something went wrong");
                }
             }          

            return doc;
        }


        public List<Document> GetDocuments(SearchParameters searchParams)
        {
            List<Document> docs = new List<Document>();

            //string sql = "SELECT * FROM T_PDF_MSDS WHERE 1 = 1 ";

            SqlCommand command = BuildSQLForSearch(searchParams);

			using (SqlConnection con = new SqlConnection(connString)) 
            {
                con.Open();
                try 
                {                    
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        docs.Add(GetDocumentFromReader(reader));                           
                    }
                    reader.Close();                    
                }
                catch (Exception ex)
                {
					
                    Log.Error("Error in DataManager.GetDocuments: " + ex.Message);
                }
            }          

            return docs;
        }

		// public List<Filter> GetFilterCollection(SearchParameters searchParams)
		// {
		// 	List<Filter> filters = new List<Filter>();			

		// 	if (!string.IsNullOrEmpty(searchParams.ProductID))
		// 	{
		// 		Filter filter = new Filter();
		// 		filter.FieldName = "F_PRODUCT";
		// 		filter.SearchType = searchParams.ProductIDFilter;
		// 		filter.SearchText = searchParams.ProductID;	
		// 		filters.Add(filter);		
		// 	}

		// 	if (!string.IsNullOrEmpty(searchParams.ProductName))
		// 	{
		// 		Filter filter = new Filter();
		// 		filter.FieldName = "F_PRODUCT_NAME";
		// 		filter.SearchType = searchParams.ProductNameFilter;
		// 		filter.SearchText = searchParams.ProductID;	
		// 		filters.Add(filter);		
		// 	}

		// 	if (searchParams.PublishedDate != DateTime.MinValue)
		// 	{
		// 		Filter filter = new Filter();
		// 		filter.FieldName = "F_PUBLISHED_DATE";
		// 		filter.SearchType = searchParams.PublishedDateFilter;
		// 		filter.SearchText = searchParams.PublishedDate.ToString();	
		// 		filters.Add(filter);		
		// 	}			

		// 	if (!string.IsNullOrEmpty(searchParams.Format))
		// 	{
		// 		Filter filter = new Filter();
		// 		filter.FieldName = "F_FORMAT";
		// 		filter.SearchType = "eq";
		// 		filter.SearchText = searchParams.Format;	
		// 		filters.Add(filter);		
		// 	}

		// 	if (!string.IsNullOrEmpty(searchParams.Subformat))
		// 	{
		// 		Filter filter = new Filter();
		// 		filter.FieldName = "F_SUBFORMAT";
		// 		filter.SearchType = "eq";
		// 		filter.SearchText = searchParams.Subformat;	
		// 		filters.Add(filter);		
		// 	}

		// 	return filters;
		// }


        private SqlCommand BuildSQLForSearch(SearchParameters searchParams)
        {
            SqlCommand command = new SqlCommand();

			string sql = "SELECT * FROM T_PDF_MSDS WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(searchParams.ProductID))
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 50; 

				sql += GetSQLTextFilterFragment(param,searchParams.ProductID,
											searchParams.ProductIDFilter,"F_PRDDUCT") ; 
               
                command.Parameters.Add(param);
            }


            if (!string.IsNullOrEmpty(searchParams.ProductName))
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 2000;                

               	sql += GetSQLTextFilterFragment(param,searchParams.ProductName,
											searchParams.ProductNameFilter,"F_PRDDUCT_NAME") ;            
               
                command.Parameters.Add(param);
            }

			if (searchParams.Authorization > 0)
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.Int;                             
				param.Value = searchParams.Authorization;
               	sql += " AND F_AUTHORIZATION = ? ";         
               
                command.Parameters.Add(param);
            }

			if (searchParams.PublishedDate != DateTime.MinValue)
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.DateTime;                             

				sql += GetSQLDateFilterFragment(param,searchParams.PublishedDate.ToString(),
						searchParams.PublishedDateFilter,"F_PUBLISHED_DATE")  ;   
               
                command.Parameters.Add(param);
            }

            if (!string.IsNullOrEmpty(searchParams.Language))
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 2;    
                sql += " AND F_LANGUAGE = ? ";
                param.SqlValue = searchParams.Language;
                command.Parameters.Add(param);
            }

			if (!string.IsNullOrEmpty(searchParams.Format))
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 3;    
                sql += " AND F_FORMAT = ? ";
                param.SqlValue = searchParams.Format;
                command.Parameters.Add(param);
            }

			if (!string.IsNullOrEmpty(searchParams.Subformat))
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 4;    
                sql += " AND F_SUBFORMAT = ? ";
                param.SqlValue = searchParams.Subformat;
                command.Parameters.Add(param);
            }

			if (searchParams.DisposalDate != DateTime.MinValue)
            {
                SqlParameter param = new SqlParameter();
                param.SqlDbType = SqlDbType.DateTime;                             

				sql += GetSQLDateFilterFragment(param,searchParams.DisposalDate.ToString(),
						searchParams.DisposalDateFilter,"F_DISPOSAL_DATE")  ;   
               
                command.Parameters.Add(param);
            }

            return command;
        }

		private string GetSQLDateFilterFragment(SqlParameter param, string searchValue,
											string filterCondition , string fieldName)
		{
			StringBuilder sb = new StringBuilder();;

			switch(filterCondition)                
			{
				case "on":
					sb.AppendFormat(" AND {0} = @[1] ", fieldName,fieldName);
					break;
				case "ob":
					sb.AppendFormat(" AND {0} <=  @{1}", fieldName,fieldName);					
					break;
				case "oa":
						sb.AppendFormat(" AND {0} >= @{1}", fieldName,fieldName);
					break;
			}
			param.SqlValue = searchValue;

			return sb.ToString();
		}

		private string GetSQLTextFilterFragment(SqlParameter param, string searchValue,
											string filterCondition , string fieldName)
		{
			StringBuilder sb = new StringBuilder();;

			switch(filterCondition)                
			{
				case "eq":
					sb.AppendFormat(" AND {0} = @{1} ", fieldName,fieldName);
					param.SqlValue = searchValue;
					break;
				case "sw":
					sb.AppendFormat(" AND {0} LIKE @{1} ", fieldName,fieldName);
					param.SqlValue = "%" + searchValue;
					break;
				case "ew":
						sb.AppendFormat(" AND {0} LIKE @{1} ", fieldName, fieldName);
					param.SqlValue = searchValue + "%";
					break;
				case "ct":
					sb.AppendFormat(" AND {0} LIKE @{1} ", fieldName,fieldName);
					param.SqlValue = "%" + searchValue + "%";
					break;
			}

			return sb.ToString();
		}


        private Document GetDocumentFromReader(SqlDataReader reader)
        {
            Document doc = new Document()
            {
                RecordID        = GetDBGuid(reader["F_GUID"]),
                ProductID       = GetDBString(reader["F_PRODUCT"]),
                Language        = GetDBString(reader["F_LANGUAGE"]),
                Format          = GetDBString(reader["F_FORMAT"]),
                Subformat       = GetDBString(reader["F_SUBFORMAT"]),
                DateStamp       = GetDBDate(reader["F_DATE_STAMP"]),
                Plant           = GetDBString(reader["F_PLANT"]),
                Supplier        = GetDBString(reader["F_SUPPLIER"]),
                ProductName     = GetDBString(reader["F_PRODUCT_NAME"]),
                RevisedDate     = GetDBDate(reader["F_DATE_REVISED"]),
                Content         = GetDBBlob(reader["F_PDF"]),
                Authorization   = GetDBInt(reader["F_AUTHORIZED"]),
                PublishedDate   = GetDBDate(reader["F_PUBLISHED_DATE"]),
                CasNumbers      = GetDBString(reader["F_CAS_NUMBERS"]),
                ComponentIDs    = GetDBString(reader["F_COMPONENT_IDS"]),
                IssueDate       = GetDBDate(reader["F_ISSUE_DATE"]),
                DisposalDate    = GetDBDate(reader["F_DISPOSAL_DATE"]),
                DocType         = GetDBInt(reader["F_DOC_TYPE"]),
                DocPath         = GetDBString(reader["F_DOC_PATH"]),
                Keywords        = GetDBString(reader["F_KEYWORDS"]),
                Custom1         = GetDBString(reader["F_CUSTOM1"]),
                Custom2         = GetDBString(reader["F_CUSTOM2"]),
                Custom3         = GetDBString(reader["F_CUSTOM3"]),
                Custom4         = GetDBString(reader["F_CUSTOM4"]),
                Custom5         = GetDBString(reader["F_CUSTOM5"]),
                UserUpdated     = GetDBString(reader["F_USER_UPDATED"]),
                RevisionNumber  = GetDBFloat(reader["F_REV_NUM"]),
                IsS3            = GetDBBool(reader["F_IS_S3"])
            };

            return doc;           
        }


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