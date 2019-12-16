using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CrudMVc.Models
{
    public interface IDaoService
    {
        DataSet ejecutarProcedimiento(string procedimiento, params SqlParameter[] parameters);
        string Send<T>(string url, T objectRequest, string method = "GET");
    }
}
