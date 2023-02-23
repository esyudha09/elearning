<%@ Page ContentType="application/json" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>

<script language="C#" runat="server">
    private static readonly FilesDisposition FILES_DISPOSITION = FilesDisposition.ServerRoot;
    private static string FILES_PATH {
        get {
            string jenis = AI_ERP.Application_Libs.Libs.GetQueryString("jenis");
            string id = AI_ERP.Application_Libs.Libs.GetQueryString("id");
            string id2 = AI_ERP.Application_Libs.Libs.GetQueryString("id2");

            if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN)
            {
                return AI_ERP.Application_Libs.Libs.GetFolderElearningMateriPembelajaran(id, id2);
            }
            else if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL)
            {
                return AI_ERP.Application_Libs.Libs.GetFolderPendidikanNonFormal(id, id2);
            }
            else if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.FILE_PENDUKUNG)
            {
                return AI_ERP.Application_Libs.Libs.GetFolderFilePendukung(id, id2);
            }
            else if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN)
            {
                return AI_ERP.Application_Libs.Libs.GetFolderRiwayatKesehatan(id, id2);
            }
            else if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_MCU)
            {
                return AI_ERP.Application_Libs.Libs.GetFolderRiwayatMCU(id, id2);
            }
            else if (jenis == AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RAPOR || jenis == "file_rapor")
            {
                string[] arr_id2 = id2.Split(new string[] { ";" }, StringSplitOptions.None);
                if (arr_id2.Length == 3)
                {
                    string tahun_ajaran = arr_id2[0].Replace("_", "/");
                    string semester = arr_id2[1];
                    string rel_kelasdet = arr_id2[2];
                    AI_ERP.Application_Entities.KelasDet m_kelas_det = AI_ERP.Application_DAOs.DAO_KelasDet.GetByID_Entity(rel_kelasdet);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            AI_ERP.Application_Entities.Kelas m_kelas = AI_ERP.Application_DAOs.DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                            if (m_kelas != null)
                            {
                                if (m_kelas.Nama != null)
                                {
                                    AI_ERP.Application_Entities.Sekolah m_sekolah = AI_ERP.Application_DAOs.DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                                    if (m_sekolah != null)
                                    {
                                        if (m_sekolah.Nama != null)
                                        {
                                            return AI_ERP.Application_Libs.Libs.GetLokasiFolderFileRapor(
                                                id, tahun_ajaran, semester, rel_kelasdet,
                                                (AI_ERP.Application_Libs.Libs.UnitSekolah)m_sekolah.UrutanJenjang
                                            );
                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }

            return "";
        }
    }

    private static readonly string FILE_QUERY_VAR = "file";
    private static readonly string FILE_GET_CONTENT_TYPE = "application/octet-stream";

    private static readonly int ATTEMPTS_TO_WRITE = 3;
    private static readonly int ATTEMPT_WAIT = 100; //msec

    //private static readonly int BUFFER_SIZE = 15 * 1024 * 1024;
    private static readonly int BUFFER_SIZE = 1000 * 1024 * 1024;

    private enum FilesDisposition
    {
        ServerRoot,
        HandlerRoot,
        Absolute
    }

    private static class HttpMethods
    {
        public static readonly string GET = "GET";
        public static readonly string POST = "POST";
        public static readonly string DELETE = "DELETE";
    }

    protected string SerializeDictionary(Dictionary<string, object> dictionary)
    {
        StringBuilder Result = new StringBuilder();

        foreach (string Key in dictionary.Keys)
        {
            object Value = dictionary[Key];

            string FormatStr = String.Empty;

            switch (Value.GetType().Name)
            {
                case "String":
                case "string":
                    FormatStr = ",\"{0}\":\"{1}\"";
                    break;
                case "Boolean":
                case "bool":
                    FormatStr = ",\"{0}\":{1}";
                    Value = Value.ToString().ToLower();
                    break;
                case "Int64":
                case "Int32":
                case "int":
                    FormatStr = ",\"{0}\":{1}";
                    break;
                case "List`1":
                    FormatStr = ",\"{0}\":[{1}]";

                    Value = String.Empty;

                    foreach (Dictionary<string, object> SubDictionary in (dictionary[Key] as List<Dictionary<string, object>>))
                    {
                        Value += "," + SerializeDictionary(SubDictionary);
                    }

                    if (Value.ToString().Length > 0)
                    {
                        Value = Value.ToString().Substring(1);
                    }
                    break;
                case "Dictionary`2":
                    FormatStr = ",\"{0}\":[{1}]";
                    Value = SerializeDictionary(Value as Dictionary<string, object>);

                    break;
            }

            Result.Append(String.Format(FormatStr, Key, Value));
        }

        return "{" + (Result.Length > 0 ? Result.ToString().Substring(1) : String.Empty) + "}";
    }

    private string CreateFileUrl(string fileName, FilesDisposition filesDisposition)
    {
        switch (filesDisposition)
        {
            case FilesDisposition.ServerRoot:
                // 1. files directory lies in root directory catalog WRONG
                return String.Format("{0}{1}/{2}", Request.Url.GetLeftPart(UriPartial.Authority),
                    FILES_PATH, Path.GetFileName(fileName));

            case FilesDisposition.HandlerRoot:
                // 2. files directory lays in current page catalog WRONG
                return String.Format("{0}{1}{2}/{3}", Request.Url.GetLeftPart(UriPartial.Authority),
                    Path.GetDirectoryName(Request.CurrentExecutionFilePath).Replace(@"\", @"/"), FILES_PATH, Path.GetFileName(fileName));

            case FilesDisposition.Absolute:
                // 3. files directory lays anywhere YEAH
                return String.Format("{0}?{1}={2}", Request.Url.AbsoluteUri, FILE_QUERY_VAR, HttpUtility.UrlEncode(Path.GetFileName(fileName)));
            default:
                return String.Empty;
        }
    }

    private Dictionary<string, object> CreateFileDictionary(string fileName, long size, string error)
    {
        Dictionary<string, object> Result = new Dictionary<string, object>();

        Result.Add("name", Path.GetFileName(fileName));
        Result.Add("size", size.ToString());
        Result.Add("type", String.Empty);
        Result.Add("url", CreateFileUrl(fileName, FILES_DISPOSITION));
        Result.Add("error", error);
        Result.Add("deleteUrl", CreateFileUrl(fileName, FilesDisposition.Absolute));
        Result.Add("deleteType", HttpMethods.DELETE);

        return Result;
    }

    private void FromStreamToStream(Stream source, Stream destination)
    {
        int BufferSize = source.Length >= BUFFER_SIZE ? BUFFER_SIZE : (int)source.Length;
        long BytesLeft = source.Length;

        byte[] Buffer = new byte[BufferSize];

        int BytesRead = 0;

        while (BytesLeft > 0)
        {
            BytesRead = source.Read(Buffer, 0, BytesLeft > BufferSize ? BufferSize : (int)BytesLeft);

            destination.Write(Buffer, 0, BytesRead);

            BytesLeft -= BytesRead;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string RootElementName = "files";
        object ResponseObject;

        string FilesPath;

        switch (FILES_DISPOSITION)
        {
            case FilesDisposition.ServerRoot:
                FilesPath = Server.MapPath(FILES_PATH);
                break;
            case FilesDisposition.HandlerRoot:
                FilesPath = Server.MapPath(Path.GetDirectoryName(Request.CurrentExecutionFilePath) + FILES_PATH);
                break;
            case FilesDisposition.Absolute:
                FilesPath = FILES_PATH;
                break;
            default:
                Response.StatusCode = 500;
                Response.StatusDescription = "Configuration error (FILES_DISPOSITION)";
                return;
        }

        string jenis = AI_ERP.Application_Libs.Libs.GetQueryString("jenis");
        if (jenis == "foto")
        {
            if (Directory.Exists(FilesPath))
            {
                Directory.Delete(FilesPath, true);
            }
        }

        // prepare directory
        if (!Directory.Exists(FilesPath))
        {
            Directory.CreateDirectory(FilesPath);
        }

        string QueryFileName = Request[FILE_QUERY_VAR];
        string FullFileName = null;
        string FileShortName = null;

        //if (!String.IsNullOrEmpty(QueryFileName))
        if (QueryFileName != null) // param specified, but maybe in wrong format (empty). else user will download json with listed files
        {
            FileShortName = HttpUtility.UrlDecode(QueryFileName);
            FullFileName = String.Format(@"{0}\{1}", FilesPath, FileShortName);

            if (QueryFileName.Trim().Length == 0 || !File.Exists(FullFileName))
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "File not found";

                Response.End();
                return;
            }
        }

        if (Request.HttpMethod.ToUpper() == HttpMethods.GET)
        {
            if (FullFileName != null)
            {
                Response.ContentType = FILE_GET_CONTENT_TYPE;
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}{1}", Path.GetFileNameWithoutExtension(FileShortName), Path.GetExtension(FileShortName).ToUpper()));

                using (FileStream FileReader = new FileStream(FullFileName, FileMode.Open, FileAccess.Read))
                {
                    FromStreamToStream(FileReader, Response.OutputStream);

                    Response.OutputStream.Close();
                }

                Response.End();
                return;
            }
            else
            {
                List<Dictionary<string, object>> FileResponseList = new List<Dictionary<string, object>>();

                string[] FileNames = Directory.GetFiles(FilesPath);

                foreach (string FileName in FileNames)
                {
                    FileResponseList.Add(CreateFileDictionary(FileName, new FileInfo(FileName).Length, String.Empty));
                }

                ResponseObject = FileResponseList;
            }
        }
        else if (Request.HttpMethod.ToUpper() == HttpMethods.POST)
        {
            List<Dictionary<string, object>> FileResponseList = new List<Dictionary<string, object>>();

            for (int FileIndex = 0; FileIndex < Request.Files.Count; FileIndex++)
            {
                HttpPostedFile File = Request.Files[FileIndex];

                string FileName = String.Format(@"{0}\{1}", FilesPath, Path.GetFileName(File.FileName));
                if (jenis == "foto")
                {
                    FileName = String.Format(@"{0}\{1}", FilesPath, "Foto" + Path.GetExtension(File.FileName));
                }
                string ErrorMessage = String.Empty;
                bool valid = true;

                for (int Attempts = 0; Attempts < ATTEMPTS_TO_WRITE; Attempts++)
                {
                    ErrorMessage = String.Empty;

                    if (System.IO.File.Exists(FileName) && jenis != "foto")
                    {
                        FileName = String.Format(@"{0}\{1}_{2:yyyyMMddHHmmss.fff}{3}", FilesPath, Path.GetFileNameWithoutExtension(FileName), DateTime.Now, Path.GetExtension(FileName));
                    }

                    try
                    {
                        string file_ext = Path.GetExtension(FileName).Trim().ToUpper().Replace(".", "");
                        if (
                                !(
                                    file_ext == "JPG" ||
                                    file_ext == "JPEG" ||
                                    file_ext == "PNG" ||
                                    file_ext == "BMP" ||
                                    file_ext == "PDF" ||
                                    file_ext == "DOC" ||
                                    file_ext == "DOCX" ||
                                    file_ext == "PPT" ||
                                    file_ext == "PPTX" ||
                                    file_ext == "XLS" ||
                                    file_ext == "XLSX"
                                )
                            ){
                            ErrorMessage = "Format file tidak valid.";
                            valid = false;
                            break;
                        }
                        if (valid) File.SaveAs(FileName);
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.Message;
                        System.Threading.Thread.Sleep(ATTEMPT_WAIT);
                        continue;
                    }

                    break;
                }

                if (valid)
                {
                    if (jenis == "foto")
                    {
                        FileResponseList.Clear();
                        FileResponseList.Add(CreateFileDictionary(FileName, File.ContentLength, ErrorMessage));
                    }
                    else
                    {
                        FileResponseList.Add(CreateFileDictionary(FileName, File.ContentLength, ErrorMessage));
                    }
                }
                else
                {
                    Response.StatusCode = 405;
                    Response.StatusDescription = ErrorMessage;
                    Response.End();

                    return;
                }
            }

            ResponseObject = FileResponseList;
        }
        else if (Request.HttpMethod.ToUpper() == HttpMethods.DELETE)
        {
            RootElementName = FileShortName;
            ResponseObject = true;

            try
            {
                File.Delete(FullFileName);
            }
            catch
            {
                ResponseObject = false;
            }
        }
        else
        {
            Response.StatusCode = 405;
            Response.StatusDescription = "Method not allowed";
            Response.End();

            return;
        }

        Dictionary<string, object> ResultDictionary = new Dictionary<string, object>();
        ResultDictionary.Add(RootElementName, ResponseObject);
        Response.Write(SerializeDictionary(ResultDictionary));
        Response.End();
    }
</script>
