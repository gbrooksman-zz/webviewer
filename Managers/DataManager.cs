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
using Microsoft.Extensions.Caching.Memory;

namespace webviewer.Managers
{
    public class DataManager
    {
        string connString = string.Empty;        
		private DBHelpers db = new DBHelpers();
        private IMemoryCache _cache;
        private IConfiguration _iconfiguration;
        private MemoryCacheEntryOptions cacheEntryOptions; 
        public DataManager(IMemoryCache memoryCache, IConfiguration iconfiguration)
        {
             _cache = memoryCache;
            _iconfiguration = iconfiguration;
             connString =_iconfiguration.GetConnectionString("WVConn");
             cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove); 
        }   

        public List<Format> GetFormats()
        {
            List<Format> formats = new List<Format>();

            if (!_cache.TryGetValue("formats", out formats))
            { 
                formats = new List<Format>();

                using (SqlConnection con = new SqlConnection(connString)) 
                {
                    con.Open();
                    try 
                    {
                        using (SqlCommand command = new SqlCommand(@"SELECT F_FORMAT , F_FORMAT_DESC
                                                                    FROM T_FORMATS", con)) 
                        {
                            SqlDataReader reader  = command.ExecuteReader();

                            formats.Add(new Format(){Code ="%", Name="(All)"});

                            while (reader.Read())
                            {
                                formats.Add( new Format()
                                {
                                    Code = db.GetDBString(reader[0]),
                                    Name = db.GetDBString(reader[1])
                                });
                            }
                            reader.Close();
                        }

                        _cache.Set("formats",formats,cacheEntryOptions);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("GetFormats: " + ex.Message);
                    }
                }
            }         

            return formats;
        }

        public List<Subformat> GetSubFormats(string formatCode)
        {
            List<Subformat> subFormats = new List<Subformat>();

            if (!_cache.TryGetValue(formatCode + "_subformats", out subFormats))
            {
                subFormats = new List<Subformat>();

                using (SqlConnection con = new SqlConnection(connString)) 
                {                    
                    try 
                    {
                        using (SqlCommand command = new SqlCommand(@"SELECT F_MSDSTYPE, F_MSDSTYPE_DESC
                                                                    FROM T_MSDSTYPES
                                                                    WHERE F_FORMAT = @FMT 
                                                                    ORDER BY 2" , con)) 
                        {
                            SqlParameter param = new SqlParameter("@FMT",SqlDbType.VarChar,3);
                            param.Value = formatCode;
                            command.Parameters.Add(param);
                            con.Open();

                            subFormats.Add(new Subformat(){Code ="%", Name="(All)", FormatCode="%"});

                            SqlDataReader reader  = command.ExecuteReader();
                            while (reader.Read())
                            {
                                subFormats.Add( new Subformat()
                                {
                                    Code = db.GetDBString(reader[0]),                                    
                                    Name = db.GetDBString(reader[1]),
                                    FormatCode = formatCode
                                });
                            }
                            reader.Close();
                            con.Close();
                        }
                        _cache.Set(formatCode + "_subformats",subFormats,cacheEntryOptions);
                    }
                    catch (Exception ex)
                    {					
                        Log.Error("GetSubFormats: " + ex.Message);
                    }
                }
            }    
            return subFormats;
        }


        public List<Subformat> GetSubFormats()
        {
            List<Subformat> subFormats = new List<Subformat>();

            if (!_cache.TryGetValue("all_subformats", out subFormats))
            {
                subFormats = new List<Subformat>();

                using (SqlConnection con = new SqlConnection(connString)) 
                {                    
                    try 
                    {
                        using (SqlCommand command = new SqlCommand(@"SELECT F_MSDSTYPE, 
                                                                        F_MSDSTYPE_DESC, F_FORMAT
                                                                        FROM T_MSDSTYPES                                                                    
                                                                        ORDER BY 2" , con)) 
                        {                           
                            con.Open();

                            subFormats.Add(new Subformat(){Code ="%", Name="(All)", FormatCode="%"});

                            SqlDataReader reader  = command.ExecuteReader();
                            while (reader.Read())
                            {
                                subFormats.Add(new Subformat()
                                {
                                    Code = db.GetDBString(reader[0]),                                    
                                    Name = db.GetDBString(reader[1]),
                                    FormatCode = db.GetDBString(reader[2])
                                });
                            }
                            reader.Close();
                            con.Close();
                        }
                        _cache.Set("all_subformats",subFormats,cacheEntryOptions);
                    }
                    catch (Exception ex)
                    {					
                        Log.Error("GetSubFormats: (all) " + ex.Message);
                    }
                }
            }    
            return subFormats;
        }


        public List<Language> GetLanguages()
        {
            List<Language> languages = new List<Language>();

            if (!_cache.TryGetValue("languages", out languages))
            {
                languages = new List<Language>();

                using (SqlConnection con = new SqlConnection(connString)) 
                {                    
                    try 
                    {         
                     using (SqlCommand command = new SqlCommand(@"SELECT F_LANGUAGE , F_LANGUAGE_NAME
                                                                    FROM T_LANGUAGES
                                                                    WHERE F_LANGUAGE IN
                                                                    (SELECT DISTINCT(F_LANGUAGE) 
                                                                    FROM T_PDF_MSDS)
                                                                    ORDER BY 2",con))
                        {                            
                            

                            languages.Add(new Language(){Code ="%", Name="(All)"});

                            con.Open();                            
                            SqlDataReader reader  = command.ExecuteReader();
           
                            while (reader.Read())
                            {
                                    languages.Add( new Language()
                                    {
                                        Code = db.GetDBString(reader[0]),
                                        Name = db.GetDBString(reader[1])
                                    });
                            }
                            reader.Close();
                            con.Close();
                        }
                        _cache.Set("languages",languages,cacheEntryOptions);
                    }
                    catch (Exception ex)
                    {					
                        Log.Error("GetLanguages: " + ex.Message);
                    }
                }
            }    
            return languages;        
        }
      
        public List<Authorization> GetsAuthLevels()
        {
            List<Authorization> authLevels = new List<Authorization>();         
             
            if (!_cache.TryGetValue("authlevels", out authLevels))
            {
                authLevels = new List<Authorization>();  

                authLevels.Add(new Authorization{Description = "(All)", Level = -99});

                var authSection = _iconfiguration.GetSection("AuthorizationLevels:Levels");

                foreach (IConfigurationSection section in authSection.GetChildren())
                {
                    var key = section.GetValue<string>("Description");
                    var value = section.GetValue<string>("Level");
                    authLevels.Add(new Authorization{Description = key, Level = int.Parse(value)});
                }

             
                _cache.Set("authlevels",authLevels,cacheEntryOptions);
            }    
            return authLevels;        
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
                                               						WHERE F_GUID = @GUID ", con)) 
                    {
						SqlParameter param = new SqlParameter("@GUID",SqlDbType.UniqueIdentifier);
						param.Value = RecordGuid;						
						command.Parameters.Add(param);

						Log.Information(command.CommandText);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            doc = GetDocumentFromReader(reader);                           
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error in GetDocument: " + ex.Message);
                }
             }          

            return doc;
        }

        public List<Document> GetDocuments(SearchParameters searchParams)
        {
            List<Document> docs = new List<Document>();

 			using (SqlConnection con = new SqlConnection(connString)) 
            {
				SqlCommand command = new SqlCommand ();				
			
				command = BuildSQLForSearch(searchParams);
				command.Connection = con;
               
                try 
                {     
					con.Open();               
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
				finally
				{					
					con.Close();
				}
            }          

            return docs;
        }

        private SqlCommand BuildSQLForSearch(SearchParameters searchParams)
        {
            SqlCommand command = new SqlCommand();

			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT * FROM T_PDF_MSDS WHERE 1 = 1 ");
			
			try
			{		
				if (!string.IsNullOrEmpty(searchParams.ProductID))
				{
					SqlParameter param = new SqlParameter("@F_PRODUCT",SqlDbType.VarChar,50);
					sb.Append( GetSQLTextFilterFragment(param,searchParams.ProductID,
												searchParams.ProductIDFilter,"F_PRODUCT")) ; 
					command.Parameters.Add(param);
				}

				if (!string.IsNullOrEmpty(searchParams.ProductName))
				{
					SqlParameter param = new SqlParameter("@F_PRODUCT_NAME", SqlDbType.VarChar, 2000);
 					sb.Append(GetSQLTextFilterFragment(param,searchParams.ProductName,
												searchParams.ProductNameFilter,"F_PRODUCT_NAME"));            
					command.Parameters.Add(param);
				}

				if (searchParams.Authorization > -99 )
				{
					SqlParameter param = new SqlParameter("@AUTH",SqlDbType.Int);                          
					param.Value = searchParams.Authorization;
					sb.Append( " AND F_AUTHORIZED = @AUTH "); 
					command.Parameters.Add(param);
				}				

				if (searchParams.PublishedDate != DateTime.MinValue)
				{
					SqlParameter param = new SqlParameter("@F_PUBLISHED_DATE",SqlDbType.DateTime);
					sb.Append(GetSQLDateFilterFragment(param,searchParams.PublishedDate.ToString(),
							searchParams.PublishedDateFilter,"F_PUBLISHED_DATE"));   
					command.Parameters.Add(param);
				}

				if (!string.IsNullOrEmpty(searchParams.Language) && (searchParams.Language != "%"))
				{
					SqlParameter param = new SqlParameter("@LNG", SqlDbType.VarChar,2);
					sb.Append(" AND F_LANGUAGE = @LNG ");
					param.SqlValue = searchParams.Language;
					command.Parameters.Add(param);
				}

				if (!string.IsNullOrEmpty(searchParams.Format) && (searchParams.Format != "%"))
				{
					SqlParameter param = new SqlParameter("@FMT",SqlDbType.VarChar,3);
					sb.Append(" AND F_FORMAT = @FMT ");
					param.SqlValue = searchParams.Format;
					command.Parameters.Add(param);
				}

				if (!string.IsNullOrEmpty(searchParams.Subformat) &&  (searchParams.Subformat != "%"))
				{
					SqlParameter param = new SqlParameter("@SFMT",SqlDbType.VarChar,4);  
					sb.Append(" AND F_SUBFORMAT = @SFMT ");
					param.SqlValue = searchParams.Subformat;
					command.Parameters.Add(param);
				}

				if (searchParams.DisposalDate != DateTime.MinValue)
				{
					SqlParameter param = new SqlParameter("@F_DISPOSAL_DATE",SqlDbType.DateTime);
					sb.Append(GetSQLDateFilterFragment(param,searchParams.DisposalDate.ToString(),
							searchParams.DisposalDateFilter,"F_DISPOSAL_DATE")) ;   
					command.Parameters.Add(param);
				}
			}
			catch(Exception ex)
			{
				Log.Error("Error in BuildSQLForSearch: " + ex.Message);
			}

			command.CommandText = sb.ToString();
            return command;
        }

		private string GetSQLDateFilterFragment(SqlParameter param, string searchValue,
											string filterCondition , string fieldName)
		{
			StringBuilder sb = new StringBuilder();

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
                RecordID        = db.GetDBGuid(reader["F_GUID"]),
                ProductID       = db.GetDBString(reader["F_PRODUCT"]),
                Language        = db.GetDBString(reader["F_LANGUAGE"]),
                Format          = db.GetDBString(reader["F_FORMAT"]),
                Subformat       = db.GetDBString(reader["F_SUBFORMAT"]),
                DateStamp       = db.GetDBDate(reader["F_DATE_STAMP"]),
                Plant           = db.GetDBString(reader["F_PLANT"]),
                Supplier        = db.GetDBString(reader["F_SUPPLIER"]),
                ProductName     = db.GetDBString(reader["F_PRODUCT_NAME"]),
                RevisedDate     = db.GetDBDate(reader["F_DATE_REVISED"]),
                Content         = db.GetDBBlob(reader["F_PDF"]),
                Authorization   = db.GetDBInt(reader["F_AUTHORIZED"]),
                PublishedDate   = db.GetDBDate(reader["F_PUBLISHED_DATE"]),
                CasNumbers      = db.GetDBString(reader["F_CAS_NUMBERS"]),
                ComponentIDs    = db.GetDBString(reader["F_COMPONENT_IDS"]),
                IssueDate       = db.GetDBDate(reader["F_ISSUE_DATE"]),
                DisposalDate    = db.GetDBDate(reader["F_DISPOSAL_DATE"]),
                DocType         = db.GetDBInt(reader["F_DOC_TYPE"]),
                DocPath         = db.GetDBString(reader["F_DOC_PATH"]),
                Keywords        = db.GetDBString(reader["F_KEYWORDS"]),
                Custom1         = db.GetDBString(reader["F_CUSTOM1"]),
                Custom2         = db.GetDBString(reader["F_CUSTOM2"]),
                Custom3         = db.GetDBString(reader["F_CUSTOM3"]),
                Custom4         = db.GetDBString(reader["F_CUSTOM4"]),
                Custom5         = db.GetDBString(reader["F_CUSTOM5"]),
                UserUpdated     = db.GetDBString(reader["F_USER_UPDATED"]),
                RevisionNumber  = db.GetDBFloat(reader["F_REV_NUM"]),
                IsS3            = db.GetDBBool(reader["F_IS_S3"])
            };

            return doc;           
        }
    }
}